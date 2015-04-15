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
    public class BCCTimeTaskMultimode : BCC
    {
        // Additional variables
        protected VariableArray<VariableArray<double>, double[][]> WorkerTimeSpent; 
        protected Variable<Vector> SpammerLabelProb;
        protected VariableArray<double> TaskShortTime;
        protected VariableArray<double> TaskLongTime;

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
            TaskShortTime[n] = Variable.GaussianFromMeanAndPrecision(0, 1).ForEach(n);
            TaskLongTime[n] = Variable.GaussianFromMeanAndPrecision(100, 1).ForEach(n);

            WorkerTimeSpent = Variable.Array(Variable.Array<double>(kn), k).Named("WorkerTimeSpent");
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
                    var isValidLabelAttempt = Variable.IsBetween(WorkerTimeSpent[k][kn], shortTime[kn], longTime[kn]);
                    using (Variable.If(isValidLabelAttempt))
                    {
                        using (Variable.Switch(trueLabel[kn]))
                        {
                            WorkerLabel[k][kn] = Variable.Discrete(WorkerConfusionMatrix[k][trueLabel[kn]]);
                        }
                    }
                    using (Variable.IfNot(isValidLabelAttempt))
                    {
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

            SpammerLabelProbPrior.ObservedValue = Dirichlet.Uniform(numClasses);

        }

        public virtual BCCTimeTaskMultimodePosteriors Infer(int[][] taskIndices, int[][] workerLabels, double[][] workerTimeSpent, int taskCount, bool monitorConvergence = false)
        {
            if (monitorConvergence)
            {
                int numIterations = NumberOfIterations;
                BCCTimeTaskMultimodePosteriors result = null;
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


        public virtual BCCTimeTaskMultimodePosteriors Infer(int[][] taskIndices, int[][] workerLabels, double[][] workerTimeSpent, BCCPosteriors priors, int taskCount)
        {
            int workerCount = workerLabels.Length;
            if (!WorkerTimeSpent.IsObserved)
            {
                SetPriors(workerCount, taskCount);
                AttachData(taskIndices, workerLabels, workerTimeSpent);
            }
            var result = new BCCTimeTaskMultimodePosteriors();
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

            return result;
        }
    }

    // Results class
    [Serializable]
    public class BCCTimeTaskMultimodePosteriors : BCCPosteriors
    {
        public Dirichlet SpammerLabelProbPosterior;
        public Gaussian[] TaskShortTimePosterior;
        public Gaussian[] TaskLongTimePosterior;
        public Gaussian[][] WorkerTimeSpentGaussianPrediction;
    }
}
