using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrosoftResearch.Infer;
using MicrosoftResearch.Infer.Distributions;
using MicrosoftResearch.Infer.Maths;
using MicrosoftResearch.Infer.Models;
using MicrosoftResearch.Infer.Utils;

namespace CrowdsourcingModels
{
    using DiscreteArray = DistributionRefArray<Discrete, int>;
    public class ScalableBCC : BCC
    {
        public Model TrueLabelModel;
        public Model WorkerModel;
        private SharedVariableArray<int> TrueLabelShared;
        private VariableArray<int> TrueLabelCopy;
        public InferenceEngine EngineForTrueLabelModel;

        public int BatchCount
        {
            get;
            set;
        }
        public int NumberOfPasses
        {
            get;
            set;
        }

        public ScalableBCC()
            : base()
        {
            BatchCount = 10;
            NumberOfPasses = 5;
            NumberOfIterations = 7;
        }

        protected override void DefineVariablesAndRanges(int taskCount, int labelCount)
        {
            base.DefineVariablesAndRanges(taskCount, labelCount);
            TrueLabelShared = SharedVariable<int>.Random(n, (DiscreteArray)(Distribution<int>.Array(Util.ArrayInit(taskCount, t => Discrete.Uniform(labelCount))))).Named("TrueLabelShared");
            TrueLabelModel = new Model(1);
            TrueLabelShared.SetDefinitionTo(TrueLabelModel, TrueLabel);
            WorkerModel = new Model(BatchCount);
            TrueLabelCopy = TrueLabelShared.GetCopyFor(WorkerModel).Named("TrueLabelCopy");
            TrueLabelCopy.SetValueRange(c);
        }

        protected override void DefineGenerativeProcess()
        {
            // The process that generates the worker's label
            using (Variable.ForEach(k))
            {
                var trueLabel = Variable.Subarray(TrueLabelCopy, WorkerTaskIndex[k]);
                trueLabel.SetValueRange(c);
                using (Variable.ForEach(kn))
                {
                    using (Variable.Switch(trueLabel[kn]))
                    {
                        WorkerLabel[k][kn] = Variable.Discrete(WorkerConfusionMatrix[k][trueLabel[kn]]);
                    }
                }
            }
        }

        protected override void DefineInferenceEngine()
        {
            base.DefineInferenceEngine();
            Engine.ShowProgress = false;
            Engine.OptimiseForVariables = new IVariable[] { WorkerConfusionMatrix, TrueLabelCopy };
            
            EngineForTrueLabelModel = new InferenceEngine()
            {
                ShowProgress = false
            };

            EngineForTrueLabelModel.OptimiseForVariables = new IVariable[] { TrueLabel, BackgroundLabelProb };
        }

        public override BCCPosteriors Infer(int[][] taskIndices, int[][] workerLabels, BCCPosteriors priors)
        {
            var result = new BCCPosteriors();
            int workerCount = workerLabels.Length;
            SetPriors(workerCount, priors);
            var confusionMatrixPriors = ConfusionMatrixPrior.ObservedValue;
            AttachData(taskIndices, workerLabels);
            result.WorkerConfusionMatrix = new Dirichlet[workerCount][];
            int[] boundaries = GetBatchBoundaries(workerCount, BatchCount);
            for (int pass = 0; pass < NumberOfPasses; pass++)
            {
                //Console.WriteLine("Pass {0}", pass + 1);
                TrueLabelModel.InferShared(EngineForTrueLabelModel, 0);
                result.BackgroundLabelProb = EngineForTrueLabelModel.Infer<Dirichlet>(BackgroundLabelProb);
                result.TrueLabel = EngineForTrueLabelModel.Infer<Discrete[]>(TrueLabel);
                result.TrueLabelConstraint = EngineForTrueLabelModel.Infer<Discrete[]>(TrueLabel, QueryTypes.MarginalDividedByPrior);
                Engine.NumberOfIterations = NumberOfIterations;
                for (int batch = 0; batch < BatchCount; batch++)
                {
                    //Console.Write("Batch {0} ", batch + 1);
                    int workerStart = boundaries[batch];
                    int workerEnd = boundaries[batch + 1];
                    if (workerStart >= workerCount)
                        break;
                    int batchedWorkerCount = workerEnd - workerStart;

                    var batchOfTaskIndices = taskIndices.Where((v, i) => i >= workerStart && i < workerEnd).ToArray();
                    var batchOfWorkerLabels = workerLabels.Where((v, i) => i >= workerStart && i < workerEnd).ToArray();
                    var batchOfConfusionMatrixPriors = confusionMatrixPriors.Where((v, i) => i >= workerStart && i < workerEnd).ToArray();
                    AttachData(batchOfTaskIndices, batchOfWorkerLabels, batchOfConfusionMatrixPriors);

                    WorkerModel.InferShared(Engine, batch);
                    var confusionMatrixPosterior = Engine.Infer<Dirichlet[][]>(WorkerConfusionMatrix);
                    for (int w = workerStart, i = 0; w < workerEnd; w++, i++)
                        result.WorkerConfusionMatrix[w] = confusionMatrixPosterior[i];
                    //Console.WriteLine("Pass {0} Batch {1}:\t{2:0.0000}\t{3:0.0000}", pass, batch, result.TrueLabel[0], confusionMatrixPosterior[0][0]);
                }

                //Console.WriteLine();
            }

            return result;
        }

        public static int[] GetBatchBoundaries(int workerCount, int batchCount)
        {
            double numUsersPerBatch = ((double)workerCount) / batchCount;
            if (numUsersPerBatch == 0)
                numUsersPerBatch = 1;
            int[] boundary = new int[batchCount + 1];
            boundary[0] = 0;
            double currBoundary = 0.0;
            for (int batch = 1; batch <= batchCount; batch++)
            {
                currBoundary += numUsersPerBatch;
                int bnd = (int)currBoundary;
                if (bnd > workerCount)
                    bnd = workerCount;
                boundary[batch] = bnd;
            }
            boundary[batchCount] = workerCount;

            return boundary;
        }
    }
}
