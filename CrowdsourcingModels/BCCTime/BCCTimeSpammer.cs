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
    public class BCCTimeSpammer : BCC
    {
        // Additional variables
        protected VariableArray<VariableArray<double>, double[][]> WorkerTimeSpent; 
        protected VariableArray<Vector> SpammerLabelProb;
        protected Variable<double> SpammerTimeMean;
        protected Variable<double> SpammerTimePrecision;

        protected VariableArray<double> NonSpammerTimeMean;
        protected Variable<double> NonSpammerTimePrecision;
        protected VariableArray<bool> IsSpammer;
        protected Variable<double> IsSpammerProb;


        // Additional priors
        protected VariableArray<Dirichlet> SpammerLabelProbPrior;
        protected Variable<Gaussian> SpammerTimeMeanPrior;
        protected Variable<Gamma> SpammerTimePrecisionPrior;
        protected VariableArray<Gaussian> NonSpammerTimeMeanPrior;
        protected Variable<Gamma> NonSpammerTimePrecisionPrior;
        protected Variable<Beta> IsSpammerProbPrior;

        protected override void DefineVariablesAndRanges(int taskCount, int labelCount)
        {
            base.DefineVariablesAndRanges(taskCount, labelCount);

            // The spammer variables
            IsSpammerProbPrior = Variable.New<Beta>().Named("IsSpammerProbPrior");
            IsSpammerProb = Variable<double>.Random(IsSpammerProbPrior).Named("IsSpammerProb");
            IsSpammer = Variable.Array<bool>(k).Named("IsSpammer");
            IsSpammer[k] = Variable.Bernoulli(IsSpammerProb).ForEach(k);


            SpammerLabelProbPrior = Variable.Array<Dirichlet>(k).Named("SpammerLabelProbPrior");
            SpammerLabelProb = Variable.Array<Vector>(k).Named("SpammerLabelProb");
            SpammerLabelProb[k] = Variable<Vector>.Random(SpammerLabelProbPrior[k]);

            // The time spent on judgment variables
            SpammerTimeMeanPrior = Variable.New<Gaussian>().Named("SpammerTimeMeanPrior");
            SpammerTimeMean = Variable<double>.Random(SpammerTimeMeanPrior).Named("SpammerTimeMean");
            SpammerTimePrecisionPrior = Variable.New<Gamma>().Named("SpammerTimePrecisionPrior");
            SpammerTimePrecision = Variable<double>.Random(SpammerTimePrecisionPrior).Named("SpammerTimePrecision");

            NonSpammerTimeMeanPrior = Variable<Gaussian>.Array(n).Named("NonSpammerTimeMeanPrior");
            NonSpammerTimeMean = Variable<double>.Array(n).Named("NonSpammerTimeMean");
            NonSpammerTimeMean[n] = Variable<double>.Random(NonSpammerTimeMeanPrior[n]);
            NonSpammerTimePrecisionPrior = Variable.New<Gamma>().Named("NonSpammerTimePrecisionPrior");
            NonSpammerTimePrecision = Variable<double>.Random(NonSpammerTimePrecisionPrior).Named("NonSpammerTimePrecision");
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
				using (Variable.IfNot(IsSpammer[k]))
				{
					using (Variable.ForEach(kn))
					{
                        var workerTaskIndex = WorkerTaskIndex[k][kn];
						using (Variable.Switch(trueLabel[kn]))
						{
                            WorkerLabel[k][kn] = Variable.Discrete(WorkerConfusionMatrix[k][trueLabel[kn]]);
						}

						WorkerTimeSpent[k][kn] = Variable.GaussianFromMeanAndPrecision(NonSpammerTimeMean[workerTaskIndex], NonSpammerTimePrecision);
					}
				}

		        // The spammer generative process
				using (Variable.If(IsSpammer[k]))
				{
                        using (Variable.ForEach(kn))
                        {
                            WorkerLabel[k][kn] = Variable.Discrete(SpammerLabelProb[k]);
                            WorkerTimeSpent[k][kn] = Variable.GaussianFromMeanAndPrecision(SpammerTimeMean, SpammerTimePrecision);
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

            BCCTimeSpammerPosteriors priorsForSpamModel = priors as BCCTimeSpammerPosteriors;
            int numClasses = c.SizeAsInt;

            if (priorsForSpamModel == null)
            {
                IsSpammerProbPrior.ObservedValue = Beta.Uniform();
                SpammerLabelProbPrior.ObservedValue = Util.ArrayInit(workerCount, i => Dirichlet.Uniform(numClasses));

                // Additional priors for spammers - for CF
                //SpammerTimeMeanPrior.ObservedValue = Gaussian.FromMeanAndPrecision(0.5, 1);
                //SpammerTimePrecisionPrior.ObservedValue = Gamma.PointMass(10);
                //NonSpammerTimeMeanPrior.ObservedValue = Util.ArrayInit(taskCount, t => Gaussian.FromMeanAndPrecision(20, 0.1));
                //NonSpammerTimePrecisionPrior.ObservedValue = Gamma.PointMass(0.1);

                // Additional priors for spammers - for SP
                SpammerTimeMeanPrior.ObservedValue = Gaussian.FromMeanAndPrecision(5, 1);
                SpammerTimePrecisionPrior.ObservedValue = Gamma.PointMass(10);
                NonSpammerTimeMeanPrior.ObservedValue = Util.ArrayInit(taskCount, t => Gaussian.FromMeanAndPrecision(100, 0.1));
                NonSpammerTimePrecisionPrior.ObservedValue = Gamma.PointMass(0.1);
            }
            else
            {
                // Additional priors for spammers
                IsSpammerProbPrior.ObservedValue = priorsForSpamModel.BackgroundIsSpammerPosterior;
                SpammerTimeMeanPrior.ObservedValue = priorsForSpamModel.SpammerTimeMeanPosterior;
                SpammerTimePrecisionPrior.ObservedValue = priorsForSpamModel.SpammerTimePrecisionPosterior;
                NonSpammerTimeMeanPrior.ObservedValue = priorsForSpamModel.TaskNonSpammerTimeMeanPosterior;
                NonSpammerTimePrecisionPrior.ObservedValue = priorsForSpamModel.NonSpammerTimePrecisionPosterior;
                SpammerLabelProbPrior.ObservedValue = priorsForSpamModel.SpammerLabelProbPosterior;
            }
        }

        public virtual BCCTimeSpammerPosteriors Infer(int[][] taskIndices, int[][] workerLabels, double[][] workerTimeSpent, int taskCount)
        {
            return Infer(taskIndices, workerLabels, workerTimeSpent, null, taskCount);
        }

        public virtual BCCTimeSpammerPosteriors Infer(int[][] taskIndices, int[][] workerLabels, double[][] workerTimeSpent, BCCPosteriors priors, int taskCount)
        {

            int workerCount = workerLabels.Length;
            SetPriors(workerCount, taskCount, priors);
            AttachData(taskIndices, workerLabels, workerTimeSpent);
            var result = new BCCTimeSpammerPosteriors();
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
            result.IsSpammerPosterior = Engine.Infer<Bernoulli[]>(IsSpammer);
            result.TaskNonSpammerTimeMeanPosterior = Engine.Infer<Gaussian[]>(NonSpammerTimeMean);
            result.NonSpammerTimePrecisionPosterior = Engine.Infer<Gamma>(NonSpammerTimePrecision);
            result.SpammerTimeMeanPosterior = Engine.Infer<Gaussian>(SpammerTimeMean);
            result.SpammerTimePrecisionPosterior = Engine.Infer<Gamma>(SpammerTimePrecision);
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
    public class BCCTimeSpammerPosteriors : BCCPosteriors
    {
        public Beta BackgroundIsSpammerPosterior;
        public Bernoulli[] IsSpammerPosterior;
        public Gaussian SpammerTimeMeanPosterior;
        public Gamma SpammerTimePrecisionPosterior;
        public Gaussian[] TaskNonSpammerTimeMeanPosterior;
        public Gamma NonSpammerTimePrecisionPosterior;
        public Dirichlet[] SpammerLabelProbPosterior;
        public Gaussian[][] WorkerTimeSpentGaussianPrediction;
    }
}
