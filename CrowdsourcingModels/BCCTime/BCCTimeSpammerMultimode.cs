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
    public class BCCTimeSpammerMultimode : BCC
    {
        // Additional variables
        protected VariableArray<VariableArray<double>, double[][]> WorkerTimeSpent; 
        protected VariableArray<Vector> SpammerLabelProb;
        protected Variable<double> SpammerShortTimeMean;
        protected Variable<double> SpammerShortTimePrecision;
        protected Variable<double> SpammerLongTimeMean;
        protected Variable<double> SpammerLongTimePrecision;
        protected VariableArray<double> TaskTimeMean;
        protected Variable<double> TaskTimePrecision;
        protected VariableArray<int> SpammerType;
        protected Variable<Vector> BackgroundSpammerTypeProb;


        // Additional priors
        protected VariableArray<Dirichlet> SpammerLabelProbPrior;
        protected Variable<Gaussian> SpammerShortTimeMeanPrior;
        protected Variable<Gamma> SpammerShortTimePrecisionPrior;
        protected Variable<Gaussian> SpammerLongTimeMeanPrior;
        protected Variable<Gamma> SpammerLongTimePrecisionPrior;
        protected VariableArray<Gaussian> TaskTimeMeanPrior;
        protected Variable<Gamma> TaskTimePrecisionPrior;
        protected Variable<Dirichlet> BackgroundSpammerTypeProbPrior;

        protected override void DefineVariablesAndRanges(int taskCount, int labelCount)
        {
            base.DefineVariablesAndRanges(taskCount, labelCount);

            // The spammer type
            BackgroundSpammerTypeProbPrior = Variable.New<Dirichlet>().Named("BackgroundSpammerProbPrior");
            BackgroundSpammerTypeProb = Variable<Vector>.Random(BackgroundSpammerTypeProbPrior).Named("BackgroundSpammerProb");
            SpammerType = Variable.Array<int>(k).Named("SpammerType");
            SpammerType[k] = Variable.Discrete(BackgroundSpammerTypeProb).ForEach(k);

            // The spammer probabilities of generating labels
            SpammerLabelProbPrior = Variable.Array<Dirichlet>(k).Named("SpammerLabelProbPrior");
            SpammerLabelProb = Variable.Array<Vector>(k).Named("SpammerLabelProb");
            SpammerLabelProb[k] = Variable<Vector>.Random(SpammerLabelProbPrior[k]);

            // The variables of time spent on judgment for long spammers
            SpammerLongTimeMeanPrior = Variable.New<Gaussian>().Named("SpammerLongTimeMeanPrior");
            SpammerLongTimeMean = Variable<double>.Random(SpammerLongTimeMeanPrior).Named("SpammerLongTimeMean");
            SpammerLongTimePrecisionPrior = Variable.New<Gamma>().Named("SpammerLongTimePrecisionPrior");
            SpammerLongTimePrecision = Variable<double>.Random(SpammerLongTimePrecisionPrior).Named("SpammerLongTimePrecision");

            // The variables of time spent on judgment  for short spammers
            SpammerShortTimeMeanPrior = Variable.New<Gaussian>().Named("SpammerShortTimeMeanPrior");
            SpammerShortTimeMean = Variable<double>.Random(SpammerShortTimeMeanPrior).Named("SpammerShortTimeMean");
            SpammerShortTimePrecisionPrior = Variable.New<Gamma>().Named("SpammerShortTimePrecisionPrior");
            SpammerShortTimePrecision = Variable<double>.Random(SpammerShortTimePrecisionPrior).Named("SpammerShortTimePrecision");

            // The variable of time spent on judgment  for non-spammers
            TaskTimeMeanPrior = Variable<Gaussian>.Array(n).Named("TaskTimeMeanPrior");
            TaskTimeMean = Variable<double>.Array(n).Named("TaskTimeMean");
            TaskTimeMean[n] = Variable<double>.Random(TaskTimeMeanPrior[n]);
            TaskTimePrecisionPrior = Variable.New<Gamma>().Named("TaskTimePrecisionPrior");
            TaskTimePrecision = Variable<double>.Random(TaskTimePrecisionPrior).Named("TaskTimePrecision");
            WorkerTimeSpent = Variable.Array(Variable.Array<double>(kn), k).Named("WorkerTimeSpent");
        }

		protected override void DefineGenerativeProcess()
		{
#if false
            // The process that generates the worker's label
            using (Variable.ForEach(k))
            {
                var trueLabel = Variable.Subarray(TrueLabel, WorkerTaskIndex[k]);
                trueLabel.SetValueRange(c);

                // The generative process for non spammers - BCC
                using (Variable.If(SpammerType[k] == 0))
                {
                    using (Variable.ForEach(kn))
                    {
                        var workerTaskIndex = WorkerTaskIndex[k][kn];
                        using (Variable.Switch(trueLabel[kn]))
                        {
                            WorkerLabel[k][kn] = Variable.Discrete(WorkerConfusionMatrix[k][trueLabel[kn]]);
                        }
                        WorkerTimeSpent[k][kn] = Variable.GaussianFromMeanAndPrecision(TaskTimeMean[workerTaskIndex], TaskTimePrecision);
                    }
                }

                // The  generative process for short spammers
                using (Variable.If(SpammerType[k] == 1))
                {
                    using (Variable.ForEach(kn))
                    {
                        WorkerLabel[k][kn] = Variable.Discrete(SpammerLabelProb[k]);
                        WorkerTimeSpent[k][kn] = Variable.GaussianFromMeanAndPrecision(SpammerShortTimeMean, SpammerShortTimePrecision);
                    }
                }

                // The  generative process for short spammers -- the inference fails if uncommented
                using (Variable.If(SpammerType[k] == 2))
                {
                    using (Variable.ForEach(kn))
                    {
                        WorkerLabel[k][kn] = Variable.Discrete(SpammerLabelProb[k]);
                        WorkerTimeSpent[k][kn] = Variable.GaussianFromMeanAndPrecision(SpammerLongTimeMean, SpammerLongTimePrecision);
                    }
                }
            }
#endif
#if false
            // The process that generates the worker's label
            using (Variable.ForEach(k))
            {
                var trueLabel = Variable.Subarray(TrueLabel, WorkerTaskIndex[k]);
                trueLabel.SetValueRange(c);
                using (Variable.ForEach(kn))
                {
                    var workerTaskIndex = WorkerTaskIndex[k][kn];

                    // The generative process for non spammers - BCC
                    using (Variable.If(0 == SpammerType[k]))
                    {
                        using (Variable.Switch(trueLabel[kn]))
                        {
                            WorkerLabel[k][kn] = Variable.Discrete(WorkerConfusionMatrix[k][trueLabel[kn]]);
                        }
                        WorkerTimeSpent[k][kn] = Variable.GaussianFromMeanAndPrecision(TaskTimeMean[workerTaskIndex], TaskTimePrecision);
                    }

                    // The  generative process for short spammers
                    using (Variable.If(1 == SpammerType[k]))
                    {
                        WorkerLabel[k][kn] = Variable.Discrete(SpammerLabelProb[k]);
                        WorkerTimeSpent[k][kn] = Variable.GaussianFromMeanAndPrecision(SpammerShortTimeMean, SpammerShortTimePrecision);
                    }

                    // The  generative process for short spammers -- the inference fails if uncommented
                    using (Variable.If(2 == SpammerType[k]))
                    {
                        WorkerLabel[k][kn] = Variable.Discrete(SpammerLabelProb[k]);
                        WorkerTimeSpent[k][kn] = Variable.GaussianFromMeanAndPrecision(SpammerLongTimeMean, SpammerLongTimePrecision);
                    }
                }
            }
#endif
#if true
            // The process that generates the worker's label
            using (Variable.ForEach(k))
            {
                var trueLabel = Variable.Subarray(TrueLabel, WorkerTaskIndex[k]);
                trueLabel.SetValueRange(c);

                using (Variable.Case(SpammerType[k], 0))
                {
                    using (Variable.ForEach(kn))
                    {
                        var workerTaskIndex = WorkerTaskIndex[k][kn];
                        using (Variable.Switch(trueLabel[kn]))
                        {
                            WorkerLabel[k][kn] = Variable.Discrete(WorkerConfusionMatrix[k][trueLabel[kn]]);
                        }
                        WorkerTimeSpent[k][kn] = Variable.GaussianFromMeanAndPrecision(TaskTimeMean[workerTaskIndex], TaskTimePrecision);
                    }
                }

                // The  generative process for short spammers
                using (Variable.Case(SpammerType[k], 1))
                {
                    using (Variable.ForEach(kn))
                    {
                        WorkerLabel[k][kn] = Variable.Discrete(SpammerLabelProb[k]);
                        WorkerTimeSpent[k][kn] = Variable.GaussianFromMeanAndPrecision(SpammerShortTimeMean, SpammerShortTimePrecision);
                    }
                }

                // The  generative process for short spammers -- the inference fails if uncommented
                using (Variable.Case(SpammerType[k], 2))
                {
                    using (Variable.ForEach(kn))
                    {
                        WorkerLabel[k][kn] = Variable.Discrete(SpammerLabelProb[k]);
                        WorkerTimeSpent[k][kn] = Variable.GaussianFromMeanAndPrecision(SpammerLongTimeMean, SpammerLongTimePrecision);
                    }
                }
            }
#endif
        }

        protected override void DefineInferenceEngine()
        {
            base.DefineInferenceEngine();
            Engine.Compiler.UseSerialSchedules = true;
            k.AddAttribute(new Sequential());
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

        protected void SetPriors(int workerCount, int taskCount, BCCPosteriors priors)
        {
            base.SetPriors(workerCount, priors);

            BCCTimeSpammerMultimodePosteriors priorsForTimeModel = priors as BCCTimeSpammerMultimodePosteriors;
            int numClasses = c.SizeAsInt;

            if (priorsForTimeModel == null)
            {

                if(Program.UseRealData)
                {
                    BackgroundSpammerTypeProbPrior.ObservedValue = Dirichlet.Uniform(3);
                    SpammerLabelProbPrior.ObservedValue = Util.ArrayInit(workerCount, i => Dirichlet.Uniform(numClasses));
                    SpammerLongTimeMeanPrior.ObservedValue = Gaussian.FromMeanAndPrecision(100, 0.1);
                    SpammerLongTimePrecisionPrior.ObservedValue = Gamma.PointMass(1);
                    SpammerShortTimeMeanPrior.ObservedValue = Gaussian.FromMeanAndPrecision(10, 0.1);
                    SpammerShortTimePrecisionPrior.ObservedValue = Gamma.PointMass(1);
                    TaskTimeMeanPrior.ObservedValue = Util.ArrayInit(taskCount, t => Gaussian.FromMeanAndPrecision(100, 0.1));
                    TaskTimePrecisionPrior.ObservedValue = Gamma.PointMass(1);
                }
                else
                {
                    //BackgroundSpammerTypeProbPrior.ObservedValue = Dirichlet.PointMass(.8, 0.1, 0.1);
                    //SpammerLabelProbPrior.ObservedValue = Util.ArrayInit(workerCount, i => Dirichlet.Uniform(numClasses));
                    //SpammerLongTimeMeanPrior.ObservedValue = Gaussian.FromMeanAndPrecision(50, 0.1);
                    //SpammerLongTimePrecisionPrior.ObservedValue = Gamma.PointMass(1);
                    //SpammerShortTimeMeanPrior.ObservedValue = Gaussian.FromMeanAndPrecision(10, 0.1);
                    //SpammerShortTimePrecisionPrior.ObservedValue = Gamma.PointMass(1);
                    //TaskTimeMeanPrior.ObservedValue = Util.ArrayInit(taskCount, t => Gaussian.FromMeanAndPrecision(30, 0.1));
                    //TaskTimePrecisionPrior.ObservedValue = Gamma.PointMass(1);
                }

            }
            else
            {
                BackgroundSpammerTypeProbPrior.ObservedValue = priorsForTimeModel.BackgroundSpammerTypePosterior;
                SpammerLabelProbPrior.ObservedValue = priorsForTimeModel.SpammerLabelProbPosterior;
                SpammerLongTimeMeanPrior.ObservedValue = priorsForTimeModel.SpammerLongTimeMeanPosterior;
                SpammerLongTimePrecisionPrior.ObservedValue = priorsForTimeModel.SpammerLongTimePrecisionPosterior;
                SpammerShortTimeMeanPrior.ObservedValue = priorsForTimeModel.SpammerShortTimeMeanPosterior;
                SpammerShortTimePrecisionPrior.ObservedValue = priorsForTimeModel.SpammerShortTimePrecisionPosterior;
                TaskTimeMeanPrior.ObservedValue = priorsForTimeModel.TaskTimeMeanPosterior;
                TaskTimePrecisionPrior.ObservedValue = priorsForTimeModel.TaskTimePrecisionPosterior;

            }
        }

        public virtual BCCTimeSpammerMultimodePosteriors Infer(int[][] taskIndices, int[][] workerLabels, double[][] workerTimeSpent, int taskCount, bool monitorConvergence = false)
        {
            if (monitorConvergence)
            {
                int numIterations = NumberOfIterations;
                BCCTimeSpammerMultimodePosteriors result = null;
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


        public virtual BCCTimeSpammerMultimodePosteriors Infer(int[][] taskIndices, int[][] workerLabels, double[][] workerTimeSpent, BCCPosteriors priors, int taskCount)
        {
            int workerCount = workerLabels.Length;
            if (!WorkerTimeSpent.IsObserved)
            {
                SetPriors(workerCount, taskCount, priors);
                AttachData(taskIndices, workerLabels, workerTimeSpent);
            }
            var result = new BCCTimeSpammerMultimodePosteriors();
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
            result.BackgroundSpammerTypePosterior = Engine.Infer<Dirichlet>(BackgroundSpammerTypeProb);
            result.SpammerLabelProbPosterior = Engine.Infer<Dirichlet[]>(SpammerLabelProb);
            result.TaskTimeMeanPosterior = Engine.Infer<Gaussian[]>(TaskTimeMean);
            result.TaskTimePrecisionPosterior = Engine.Infer<Gamma>(TaskTimePrecision);
            result.SpammerLongTimeMeanPosterior = Engine.Infer<Gaussian>(SpammerLongTimeMean);
            result.SpammerLongTimePrecisionPosterior = Engine.Infer<Gamma>(SpammerLongTimePrecision);
            result.SpammerShortTimeMeanPosterior = Engine.Infer<Gaussian>(SpammerShortTimeMean);
            result.SpammerShortTimePrecisionPosterior = Engine.Infer<Gamma>(SpammerShortTimePrecision);
            result.SpammerTypePosterior = Engine.Infer<Discrete[]>(SpammerType);

            if (workerTimeSpent[0] == null)
            {
                result.WorkerTimeSpentGaussianPrediction = Engine.Infer<Gaussian[][]>(WorkerTimeSpent);
            }

            return result;
        }
    }

    // Results class
    [Serializable]
    public class BCCTimeSpammerMultimodePosteriors : BCCPosteriors
    {
        public Dirichlet BackgroundSpammerTypePosterior;
        public Discrete[] SpammerTypePosterior;
        public Gaussian SpammerLongTimeMeanPosterior;
        public Gamma SpammerLongTimePrecisionPosterior;
        public Gaussian SpammerShortTimeMeanPosterior;
        public Gamma SpammerShortTimePrecisionPosterior;
        public Gaussian[] TaskTimeMeanPosterior;
        public Gamma TaskTimePrecisionPosterior;
        public Dirichlet[] SpammerLabelProbPosterior;
        public Gaussian[][] WorkerTimeSpentGaussianPrediction;
    }
}
