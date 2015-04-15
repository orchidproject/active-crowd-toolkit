//#define usePvlScore

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
    public class BCCTimeTaskPropensity : BCC
    {
        // Additional variables
        protected VariableArray<VariableArray<double>, double[][]> WorkerTimeSpent; 
        protected Variable<Vector> SpammerLabelProb;
        protected VariableArray<double> TaskShortTime;
        protected VariableArray<double> TaskLongTime;
        
#if usePvlScore
        protected VariableArray<double> pvlScore;
        protected Variable<double> BackgroundPvlScore;
#else
        protected VariableArray<double> PropensityForValidLabelling;
#endif

        // Additional priors
        protected Variable<Dirichlet> SpammerLabelProbPrior;

        protected override void DefineVariablesAndRanges(int taskCount, int labelCount)
        {
            base.DefineVariablesAndRanges(taskCount, labelCount);

            // The spammer probabilities of generating labels
            SpammerLabelProbPrior = Variable.New<Dirichlet>().Named("SpammerLabelProbPrior");
            SpammerLabelProb = Variable<Vector>.Random(SpammerLabelProbPrior).Named("SpammerLabelProb");

            TaskShortTime = Variable.Array<double>(n).Named("TaskShortTime");
            TaskLongTime = Variable.Array<double>(n).Named("TaskLongTime");

            WorkerTimeSpent = Variable.Array(Variable.Array<double>(kn), k).Named("WorkerTimeSpent");

            if (Program.UseRealData)
            {
                TaskShortTime[n] = Variable.GaussianFromMeanAndPrecision(10, 0.01).ForEach(n);
                TaskLongTime[n] = Variable.GaussianFromMeanAndPrecision(50, 0.01).ForEach(n);
                //TaskLongTime[n] = Variable.GaussianFromMeanAndPrecision(200, 0.01).ForEach(n);
                //this.SpammerLabelProb.ObservedValue = Vector.FromArray(0.5, 0.5);
            }
            else
            {
                TaskShortTime[n] = Variable.GaussianFromMeanAndPrecision(20, 2).ForEach(n);
                TaskLongTime[n] = Variable.GaussianFromMeanAndPrecision(100, 2).ForEach(n);
            }


#if usePvlScore
            BackgroundPvlScore = Variable.GaussianFromMeanAndPrecision(1, 10);
            pvlScore = Variable.Array<double>(k);
            pvlScore[k] = Variable.GaussianFromMeanAndPrecision(BackgroundPvlScore, 1.0).ForEach(k);
#else
            PropensityForValidLabelling = Variable.Array<double>(k);
            PropensityForValidLabelling[k] = Variable.Beta(taskCount * 0.7, taskCount * 0.3).ForEach(k);
            //PropensityForValidLabelling[k] = Variable.Beta(500, 100).ForEach(k);
            
#endif

            // Positive constraints for the time variables
            Variable.ConstrainPositive(TaskShortTime[n]);
            Variable.ConstrainPositive(TaskLongTime[n] - TaskShortTime[n]);

        }

        protected override void DefineGenerativeProcess()
        {

            // The process that generates the worker's label
            using (Variable.ForEach(k))
            {
                var trueLabel = Variable.Subarray(TrueLabel, WorkerTaskIndex[k]);
                trueLabel.SetValueRange(c);
                var shortTime = Variable.Subarray(TaskShortTime, WorkerTaskIndex[k]);
                var longTime = Variable.Subarray(TaskLongTime, WorkerTaskIndex[k]);

                using (Variable.ForEach(kn))
                {
#if usePvlScore
                    var isValidLabelAttempt = Variable.GaussianFromMeanAndPrecision(pvlScore[k], 1.0) > 0;
#else

                    var isValidLabelAttempt = Variable.Bernoulli(PropensityForValidLabelling[k]);
#endif
                    using (Variable.If(isValidLabelAttempt))
                    {
                        Variable.ConstrainBetween(WorkerTimeSpent[k][kn], shortTime[kn], longTime[kn]);
                        using (Variable.Switch(trueLabel[kn]))
                        {
                            WorkerLabel[k][kn] = Variable.Discrete(WorkerConfusionMatrix[k][trueLabel[kn]]);
                        }
                    }
                    using (Variable.IfNot(isValidLabelAttempt))
                    {
                        //var tooShort = (shortTime[kn] > WorkerTimeSpent[k][kn]);
                        //var tooLong = (longTime[kn] < WorkerTimeSpent[k][kn]);
                        //var outOfLimits = tooShort | tooLong;
                        //Variable.ConstrainTrue(outOfLimits);

                        WorkerLabel[k][kn] = Variable.Discrete(SpammerLabelProb);
                    }
                }
            }
        }


        protected override void DefineInferenceEngine()
        {
            base.DefineInferenceEngine();
            Engine.Compiler.UseSerialSchedules = true;
            //k.AddAttribute(new Sequential());
            Engine.ShowProgress = true;
        }

        protected void AttachData(int[][] taskIndices, int[][] workerLabels, double[][] workerTimeSpent)
        {
            base.AttachData(taskIndices, workerLabels);

            // Prediction mode is indicated by none of the workers having a time.
            // We can just look at the first one
            if (workerTimeSpent[0] != null)
            {
                WorkerTimeSpent.ObservedValue = workerTimeSpent;
            }
            else
            {
                WorkerTimeSpent.ClearObservedValue();
            }
        }

        protected void SetPriors(int workerCount, int taskCount)
        {
            base.SetPriors(workerCount, null);

            int numClasses = c.SizeAsInt;

            SpammerLabelProbPrior.ObservedValue = Dirichlet.Symmetric(numClasses, 10);
            //PropensityForValidLabelling.ObservedValue = Util.ArrayInit(workerCount, i => 0.8);

        }

        public virtual BCCTimeTaskPropensityPosteriors Infer(int[][] taskIndices, int[][] workerLabels, double[][] workerTimeSpent, int taskCount, bool monitorConvergence = false)
        {
            if (monitorConvergence)
            {
                int numIterations = NumberOfIterations;
                BCCTimeTaskPropensityPosteriors result = null;
                for (int i = 1; i <= numIterations; i++)
                {
                    NumberOfIterations = i;
                    result = Infer(taskIndices, workerLabels, workerTimeSpent, null, taskCount);
                    Console.WriteLine("\t{0}", result.WorkerConfusionMatrix[0][0]);
                }
                return Infer(taskIndices, workerLabels, workerTimeSpent, null, taskCount);
            }
            else
            {
                return Infer(taskIndices, workerLabels, workerTimeSpent, null, taskCount);
            }
        }


        public virtual BCCTimeTaskPropensityPosteriors Infer(int[][] taskIndices, int[][] workerLabels, double[][] workerTimeSpent, BCCPosteriors priors, int taskCount)
        {
            int workerCount = workerLabels.Length;
            if (!WorkerTimeSpent.IsObserved)
            {
                SetPriors(workerCount, taskCount);
                AttachData(taskIndices, workerLabels, workerTimeSpent);
            }
            var result = new BCCTimeTaskPropensityPosteriors();
            Engine.NumberOfIterations = NumberOfIterations;
            result.Evidence = Engine.Infer<Bernoulli>(Evidence);
            result.BackgroundLabelProb = Engine.Infer<Dirichlet>(BackgroundLabelProb);
            result.WorkerConfusionMatrix = Engine.Infer<Dirichlet[][]>(WorkerConfusionMatrix);
            result.TrueLabel = Engine.Infer<Discrete[]>(TrueLabel);
            result.TrueLabelConstraint = Engine.Infer<Discrete[]>(TrueLabel, QueryTypes.MarginalDividedByPrior);

            // Prediction mode is indicated by none of the workers having a label.
            // We can just look at the first one
            if (workerLabels[0] == null)
            {
                result.WorkerPrediction = Engine.Infer<Discrete[][]>(WorkerLabel);
            }

            // Additional inference results for the spammer variables
            result.SpammerLabelProbPosterior = Engine.Infer<Dirichlet>(SpammerLabelProb);
            result.TaskShortTimePosterior = Engine.Infer<Gaussian[]>(TaskShortTime);
            result.TaskLongTimePosterior = Engine.Infer<Gaussian[]>(TaskLongTime);

            if (workerTimeSpent[0] == null)
            {
                result.WorkerTimeSpentGaussianPrediction = Engine.Infer<Gaussian[][]>(WorkerTimeSpent);
            }


#if usePvlScore
            result.PropensityForValidLabellingScorePosterior = Engine.Infer<Gaussian[]>(pvlScore);
#else
            result.PropensityForValidLabellingPosterior = Engine.Infer<Beta[]>(PropensityForValidLabelling);
            //result.PropensityForValidLabellingPosterior = Util.ArrayInit(workerCount, w => Beta.PointMass(1.0));
#endif
            return result;
        }
    }

    // Results class
    [Serializable]
    public class BCCTimeTaskPropensityPosteriors : BCCPosteriors
    {
        public Dirichlet SpammerLabelProbPosterior;
        public Gaussian[] TaskShortTimePosterior;
        public Gaussian[] TaskLongTimePosterior;
        public Gaussian[][] WorkerTimeSpentGaussianPrediction;
        public Beta[] PropensityForValidLabellingPosterior;
        public Gaussian[] PropensityForValidLabellingScorePosterior;
    }
}
