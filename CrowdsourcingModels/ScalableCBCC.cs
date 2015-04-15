using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrosoftResearch.Infer;
using MicrosoftResearch.Infer.Distributions;
using MicrosoftResearch.Infer.Factors;
using MicrosoftResearch.Infer.Maths;
using MicrosoftResearch.Infer.Models;
using MicrosoftResearch.Infer.Utils;

namespace CrowdsourcingModels
{
    using VectorGaussianArray = DistributionRefArray<VectorGaussian, Vector>;
    using VectorGaussianArrayArray = DistributionRefArray<DistributionRefArray<VectorGaussian, Vector>, Vector[]>;
    using DiscreteArray = DistributionRefArray<Discrete, int>;

    public class ScalableCBCC : CBCC
    {
        public Model TrueLabelModel;
        public Model WorkerModel;
        public Model CommunityModel;
        private SharedVariableArray<int> TrueLabelShared;
        private VariableArray<int> TrueLabelCopy;
        private ISharedVariableArray<VariableArray<Vector>, Vector[][]> CommunityScoreMatrixShared;
        private VariableArray<VariableArray<Vector>, Vector[][]> CommunityScoreMatrixCopy;
        public InferenceEngine EngineForTrueLabelModel;
        private InferenceEngine EngineForCommunityModel;

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
        public ScalableCBCC()
            : base()
        {
            BatchCount = 10;
            NumberOfPasses = 5;
            NumberOfIterations = 30;
        }

        protected override void DefineVariablesAndRanges(int taskCount, int labelCount)
        {
            base.DefineVariablesAndRanges(taskCount, labelCount);
            TrueLabelModel = new Model(1).Named("DefiningModel");
            WorkerModel = new Model(BatchCount).Named("WorkerModel");
            CommunityModel = new Model(1).Named("CommunityModel");

            // Shared variable defined by true label model
            var trueLabelUniform = (DiscreteArray)(Distribution<int>.Array(Util.ArrayInit(taskCount, t => Discrete.Uniform(labelCount))));
            TrueLabelShared = SharedVariable<int>.Random(n, trueLabelUniform).Named("TrueLabelShared");
            TrueLabelShared.SetDefinitionTo(TrueLabelModel, TrueLabel);

            // Shared variable defined by community model
            var communityScoreMatrixUniform = (VectorGaussianArrayArray)Distribution<Vector>.Array(Util.ArrayInit(CommunityCount, m1 => Util.ArrayInit(labelCount, c1 => VectorGaussian.Uniform(labelCount))));
            CommunityScoreMatrixShared = SharedVariable<Vector>.Random(Variable.Array<Vector>(c), m, communityScoreMatrixUniform).Named("CommunityScoreMatrixShared");
            CommunityScoreMatrixShared.SetDefinitionTo(CommunityModel, CommunityScoreMatrix);

            // Copies of the shared variables for the worker model
            TrueLabelCopy = TrueLabelShared.GetCopyFor(WorkerModel).Named("TrueLabelCopy");
            TrueLabelCopy.SetValueRange(c);
            CommunityScoreMatrixCopy = CommunityScoreMatrixShared.GetCopyFor(WorkerModel).Named("CommunityScoreMatrixCopy");
            CommunityScoreMatrixCopy.SetValueRange(c);
        }

        protected override void DefineGenerativeProcess()
        {
            // The process that generates the worker's label
            using (Variable.ForEach(k))
            {
                using (Variable.Switch(Community[k]))
                {
                    ScoreMatrix[k][c] = Variable.VectorGaussianFromMeanAndPrecision(CommunityScoreMatrixCopy[Community[k]][c], NoiseMatrix);
                }

                Variable.ConstrainEqualRandom(ScoreMatrix[k][c], ScoreMatrixConstraint[k][c]);
                WorkerConfusionMatrix[k][c] = Variable.Softmax(ScoreMatrix[k][c]);
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
            Engine.OptimiseForVariables = new IVariable[] { ScoreMatrix, WorkerConfusionMatrix, TrueLabelCopy, CommunityScoreMatrixCopy, CommunityProb, Community };
            Engine.ShowProgress = false;

            EngineForTrueLabelModel = new InferenceEngine()
            {
                ShowProgress = false
            };

            EngineForTrueLabelModel.OptimiseForVariables = new IVariable[] { TrueLabel, BackgroundLabelProb };

            EngineForCommunityModel = new InferenceEngine(new VariationalMessagePassing())
            {
                ShowProgress = false
            };

            EngineForCommunityModel.Compiler.GivePriorityTo(typeof(SoftmaxOp_BL06));
        }

        public override BCCPosteriors Infer(int[][] taskIndices, int[][] workerLabels, BCCPosteriors priors)
        {
            var result = new CBCCPosteriors();
            int workerCount = workerLabels.Length;
            var cbccPriors = (CBCCPosteriors)priors;
            VectorGaussian[][] scoreConstraint = (cbccPriors == null ? null : cbccPriors.WorkerScoreMatrixConstraint);
            Discrete[] communityConstraint = (cbccPriors == null ? null : cbccPriors.WorkerCommunityConstraint);
            SetPriors(workerCount, priors);
            AttachData(taskIndices, workerLabels, scoreConstraint, communityConstraint);
            result.WorkerConfusionMatrix = new Dirichlet[workerCount][];
            result.Community = new Discrete[workerCount];
            result.WorkerScoreMatrixConstraint = new VectorGaussian[workerCount][];
            result.WorkerCommunityConstraint = new Discrete[workerCount];
            int[] boundaries = ScalableBCC.GetBatchBoundaries(workerCount, BatchCount);
            VectorGaussian[][] batchedScoreConstraint = null;
            Discrete[] batchedCommunityConstraint = null;
            for (int pass = 0; pass < NumberOfPasses; pass++)
            {
                TrueLabelModel.InferShared(EngineForTrueLabelModel, 0);
                result.BackgroundLabelProb = EngineForTrueLabelModel.Infer<Dirichlet>(BackgroundLabelProb);
                result.TrueLabel = EngineForTrueLabelModel.Infer<Discrete[]>(TrueLabel);
                result.TrueLabelConstraint = EngineForTrueLabelModel.Infer<Discrete[]>(TrueLabel, QueryTypes.MarginalDividedByPrior);
                CommunityModel.InferShared(EngineForCommunityModel, 0);
                result.CommunityScoreMatrix = EngineForCommunityModel.Infer<VectorGaussian[][]>(CommunityScoreMatrix);
                result.CommunityConfusionMatrix = EngineForCommunityModel.Infer<Dirichlet[][]>(CommunityConfusionMatrix);
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
                    if (scoreConstraint != null)
                        batchedScoreConstraint = scoreConstraint.Where((v, i) => i >= workerStart && i < workerEnd).ToArray();
                    if (communityConstraint != null)
                        batchedCommunityConstraint = communityConstraint.Where((v, i) => i >= workerStart && i < workerEnd).ToArray();

                    AttachData(batchOfTaskIndices, batchOfWorkerLabels, batchedScoreConstraint, batchedCommunityConstraint);
                    WorkerModel.InferShared(Engine, batch);
                    result.CommunityProb = Engine.Infer<Dirichlet>(CommunityProb);
                    var confusionMatrixPosterior = Engine.Infer<Dirichlet[][]>(WorkerConfusionMatrix);
                    var communityPosterior = Engine.Infer<Discrete[]>(Community);
                    var workerCommunityConstraint = Engine.Infer<Discrete[]>(Community, QueryTypes.MarginalDividedByPrior);
                    var workerScoreMatrixConstraint = Engine.Infer<VectorGaussian[][]>(ScoreMatrix, QueryTypes.MarginalDividedByPrior);
                    for (int w = workerStart, i = 0; w < workerEnd; w++, i++)
                    {
                        result.WorkerConfusionMatrix[w] = confusionMatrixPosterior[i];
                        result.Community[w] = communityPosterior[i];
                        result.WorkerScoreMatrixConstraint[w] = workerScoreMatrixConstraint[i];
                        result.WorkerCommunityConstraint[w] = workerCommunityConstraint[i];
                    }

                    ////Console.WriteLine("Pass {0} Batch {1}:\t{2:0.0000}", pass+ + 1, batch +1, confusionMatrixPosterior[0][0]);
                }
            }

            return result;
        }
    }
}
