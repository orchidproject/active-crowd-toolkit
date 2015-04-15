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

namespace CrowdsourcingSoton
{
    public class BCCTimeSpammerV2 : BCC
    {
        // Additional variables
        protected VariableArray<VariableArray<double>, double[][]> WorkerTimeSpent; 
        protected VariableArray<Vector> SpammerLabelProb;

        // The per-task time taken by non spammer workers
        protected VariableArray<double> TaskNonSpammerTimeMean;
        protected Variable<double> TaskNonSpammerTimePrecision;

        // The two time modes (short - long time) of spammers
        protected Variable<double> SpammerShortTimeMean;
        protected Variable<double> SpammerLongTimeMean;
        protected Variable<double> SpammerShortTimePrecision;
        protected Variable<double> SpammerLongTimePrecision;

        // The spammer flag and probability of each worker
        protected VariableArray<bool> IsSpammer;
        protected Variable<double> IsSpammerProb;

        // The time behaviour of each spammer
        protected VariableArray<bool> IsLongSpammer;
        protected Variable<double> IsLongSpammerProb;

        // Additional priors
        protected VariableArray<Dirichlet> SpammerLabelProbPrior;
        protected VariableArray<Gaussian> TaskNonSpammerTimeMeanPrior;
        protected Variable<Beta> IsSpammerProbPrior;
        protected Variable<Beta> IsLongSpammerProbPrior;
        protected Variable<Gaussian> SpammerShortTimeMeanPrior;
        protected Variable<Gaussian> SpammerLongTimeMeanPrior;

        protected override void DefineVariablesAndRanges(int taskCount, int labelCount)
        {
            base.DefineVariablesAndRanges(taskCount, labelCount);

            // The variables generating the spammer type
            IsSpammerProbPrior = Variable.New<Beta>().Named("IsSpammerProbPrior");
            IsSpammerProb = Variable<double>.Random(IsSpammerProbPrior).Named("IsSpammerProb");
            IsSpammer = Variable.Array<bool>(k).Named("IsSpammer");
            IsSpammer[k] = Variable.Bernoulli(IsSpammerProb).ForEach(k);
            //IsSpammer[k].InitialiseTo(Bernoulli.PointMass(false));

            IsLongSpammerProbPrior = Variable.New<Beta>().Named("IsLongSpammerProbPrior");
            IsLongSpammerProb = Variable<double>.Random(IsLongSpammerProbPrior).Named("IsLongSpammerProb");
            IsLongSpammer = Variable.Array<bool>(k).Named("IsLongSpammer");
            IsLongSpammer[k] = Variable.Bernoulli(IsLongSpammerProb).ForEach(k);

            // The variables generating the spammer labels
            SpammerLabelProbPrior = Variable.Array<Dirichlet>(k).Named("SpammerLabelProbPrior");
            SpammerLabelProb = Variable.Array<Vector>(k).Named("SpammerLabelProb");
            SpammerLabelProb[k] = Variable<Vector>.Random(SpammerLabelProbPrior[k]);

            // The variables generating the time taken
            SpammerShortTimeMeanPrior = Variable.New<Gaussian>().Named("SpammerShortTimeMeanPrior");
            SpammerShortTimeMean = Variable<double>.Random(SpammerShortTimeMeanPrior).Named("SpammerShortTimeMean");
            SpammerLongTimeMeanPrior = Variable.New<Gaussian>().Named("SpammerLongTimeMeanPrior");
            SpammerLongTimeMean = Variable<double>.Random(SpammerLongTimeMeanPrior).Named("SpammerLongTimeMean");
            SpammerShortTimePrecision = Variable.New<double>().Named("SpammerShortTimePrecisionPrior");
            SpammerLongTimePrecision = Variable.New<double>().Named("SpammerLongTimePrecisionPrior");

            TaskNonSpammerTimeMeanPrior = Variable<Gaussian>.Array(n).Named("TaskNonSpammerTimeMeanPrior");
            TaskNonSpammerTimeMean = Variable<double>.Array(n).Named("TaskNonSpammerTimeMean");
            TaskNonSpammerTimeMean[n] = Variable<double>.Random(TaskNonSpammerTimeMeanPrior[n]);
            TaskNonSpammerTimePrecision = Variable.New<double>().Named("TaskNonSpammerTimePrecision");

            WorkerTimeSpent = Variable.Array(Variable.Array<double>(kn), k).Named("WorkerTimeSpent");
        }

		protected override void DefineGenerativeProcess()
		{
			// The process that generates the worker's label
			using (Variable.ForEach(k))
			{
				var trueLabel = Variable.Subarray(TrueLabel, WorkerTaskIndex[k]);
				trueLabel.SetValueRange(c);

				// The non spammer generative process
				using (Variable.If(IsSpammer[k]))
				{
					using (Variable.ForEach(kn))
					{
                        var workerTaskIndex = WorkerTaskIndex[k][kn];
						using (Variable.Switch(trueLabel[kn]))
						{
                            WorkerLabel[k][kn] = Variable.Discrete(WorkerConfusionMatrix[k][trueLabel[kn]]);
						}

						WorkerTimeSpent[k][kn] = Variable.GaussianFromMeanAndPrecision(TaskNonSpammerTimeMean[workerTaskIndex], TaskNonSpammerTimePrecision);
					}
				}

		        // The spammer generative process
				using (Variable.IfNot(IsSpammer[k]))
				{
                    using (Variable.If(IsLongSpammer[k]))
                    {

				        using (Variable.ForEach(kn))
					    {
						    WorkerLabel[k][kn] = Variable.Discrete(SpammerLabelProb[k]);
                            WorkerTimeSpent[k][kn] = Variable.GaussianFromMeanAndPrecision(SpammerLongTimeMean, SpammerLongTimePrecision);
                        }
                    }

                    using (Variable.IfNot(IsLongSpammer[k]))
                    {
                        using (Variable.ForEach(kn))
                        {
                            WorkerLabel[k][kn] = Variable.Discrete(SpammerLabelProb[k]);
                            WorkerTimeSpent[k][kn] = Variable.GaussianFromMeanAndPrecision(SpammerShortTimeMean, SpammerShortTimePrecision);
                        }
                    }
				}
			}
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

            BCCTimeSpammerPosteriorsV2 priorsForSpamModel = priors as BCCTimeSpammerPosteriorsV2;
            int numClasses = c.SizeAsInt;

            if (priorsForSpamModel == null)
            {
                // Additional priors for spammers
                IsSpammerProbPrior.ObservedValue = Beta.Uniform();
                IsLongSpammerProbPrior.ObservedValue = Beta.Uniform();
                SpammerShortTimeMeanPrior.ObservedValue = Gaussian.FromMeanAndPrecision(1, 1);
                SpammerLongTimeMeanPrior.ObservedValue = Gaussian.FromMeanAndPrecision(100, 1);
                TaskNonSpammerTimeMeanPrior.ObservedValue = Util.ArrayInit(taskCount, t => Gaussian.FromMeanAndPrecision(20, 1));
                SpammerLabelProbPrior.ObservedValue = Util.ArrayInit(workerCount, i => Dirichlet.Uniform(numClasses));
            }
            else
            {
                // Additional priors for spammers
                IsSpammerProbPrior.ObservedValue = priorsForSpamModel.BackgroundIsSpammerPosterior;
                SpammerLongTimeMeanPrior.ObservedValue = priorsForSpamModel.SpammerLongTimeMeanPosterior;
                SpammerShortTimeMeanPrior.ObservedValue = priorsForSpamModel.SpammerShortTimeMeanPosterior;
                TaskNonSpammerTimeMeanPrior.ObservedValue = priorsForSpamModel.TaskNonSpammerTimeMeanPosterior;
                SpammerLabelProbPrior.ObservedValue = priorsForSpamModel.SpammerLabelProbPosterior;
            }

            SpammerShortTimePrecision.ObservedValue = 100;
            SpammerLongTimePrecision.ObservedValue = 100;
            TaskNonSpammerTimePrecision.ObservedValue = 1;

        }

        public virtual BCCTimeSpammerPosteriorsV2 Infer(int[][] taskIndices, int[][] workerLabels, double[][] workerTimeSpent, int taskCount)
        {
            return Infer(taskIndices, workerLabels, workerTimeSpent, null, taskCount);
        }

        public virtual BCCTimeSpammerPosteriorsV2 Infer(int[][] taskIndices, int[][] workerLabels, double[][] workerTimeSpent, BCCPosteriors priors, int taskCount)
        {

            int workerCount = workerLabels.Length;
            SetPriors(workerCount, taskCount, priors);
            AttachData(taskIndices, workerLabels, workerTimeSpent);
            var result = new BCCTimeSpammerPosteriorsV2();
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
            result.BackgroundIsSpammerPosterior = Engine.Infer<Beta>(IsSpammerProb);
            result.BackgroundIsLongSpammerPosterior = Engine.Infer<Beta>(IsLongSpammerProb);
            result.IsSpammerPosterior = Engine.Infer<Bernoulli[]>(IsSpammer);
            result.IsLongSpammerPosterior = Engine.Infer<Bernoulli[]>(IsLongSpammer);
            result.TaskNonSpammerTimeMeanPosterior = Engine.Infer<Gaussian[]>(TaskNonSpammerTimeMean);
            result.SpammerLongTimeMeanPosterior = Engine.Infer<Gaussian>(SpammerLongTimeMean);
            result.SpammerShortTimeMeanPosterior = Engine.Infer<Gaussian>(SpammerShortTimeMean);
            result.SpammerLabelProbPosterior = Engine.Infer<Dirichlet[]>(SpammerLabelProb);

            if (workerTimeSpent[0] == null)
            {
                result.WorkerTimeSpentGaussianPrediction = Engine.Infer<Gaussian[][]>(WorkerTimeSpent);
            }

            return result;
        }
    }

    // Results class
    [Serializable]
    public class BCCTimeSpammerPosteriorsV2 : BCCPosteriors
    {
        public Beta BackgroundIsSpammerPosterior;
        public Beta BackgroundIsLongSpammerPosterior;
        public Bernoulli[] IsSpammerPosterior;
        public Bernoulli[] IsLongSpammerPosterior;
        public Gaussian SpammerLongTimeMeanPosterior;
        public Gaussian SpammerShortTimeMeanPosterior;
        public Gaussian[] TaskNonSpammerTimeMeanPosterior;
        public Dirichlet[] SpammerLabelProbPosterior;
        public Gaussian[][] WorkerTimeSpentGaussianPrediction;
    }
}
