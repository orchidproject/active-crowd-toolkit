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
    public class BCCTimeSpammerGamma : BCC
    {
        // Additional variables
        protected VariableArray<VariableArray<double>, double[][]> WorkerTimeSpent;
        protected VariableArray<Vector> SpammerLabelProb;
        protected Variable<double> SpammerTimeShape;
        protected Variable<double> SpammerTimeRate;
        protected Variable<double> NonSpammerTimeShape;
        protected Variable<double> NonSpammerTimeRate;
        protected VariableArray<bool> IsSpammer;
        protected Variable<double> IsSpammerProb;

        // Additional priors
        protected VariableArray<Dirichlet> SpammerLabelProbPrior;
        protected Variable<Gamma> SpammerTimeShapePrior;
        protected Variable<Gamma> SpammerTimeRatePrior;
        protected Variable<Gamma> NonSpammerTimeShapePrior;
        protected Variable<Gamma> NonSpammerTimeRatePrior;
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
            SpammerTimeShapePrior = Variable.New<Gamma>().Named("SpammerTimeShapePrior");
            SpammerTimeShape = Variable<double>.Random(SpammerTimeShapePrior).Named("SpammerTimeShape");
            SpammerTimeRatePrior = Variable.New<Gamma>().Named("SpammerTimeRatePrior");
            SpammerTimeRate = Variable<double>.Random(SpammerTimeRatePrior).Named("SpammerTimeRate");
            NonSpammerTimeShapePrior = Variable.New<Gamma>().Named("NonSpammerTimeShapePrior");
            NonSpammerTimeShape = Variable<double>.Random(NonSpammerTimeShapePrior).Named("NonSpammerTimeShape");
            NonSpammerTimeRatePrior = Variable.New<Gamma>().Named("NonSpammerTimeRatePrior");
            NonSpammerTimeRate = Variable<double>.Random(NonSpammerTimeRatePrior).Named("NonSpammerTimeRate");
            WorkerTimeSpent = Variable.Array(Variable.Array<double>(kn), k).Named("WorkerTimeSpent");
        }

        protected override void DefineInferenceEngine()
        {
            Engine = new InferenceEngine(new VariationalMessagePassing());
            //Engine = new InferenceEngine(new ExpectationPropagation());
            //Engine = new InferenceEngine(new GibbsSampling());
            Engine.Compiler.UseParallelForLoops = true;
            Engine.ShowProgress = true;
            Engine.ShowFactorGraph = false;
            Engine.Compiler.WriteSourceFiles = true;

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
                        using (Variable.Switch(trueLabel[kn]))
                        {
                            WorkerLabel[k][kn] = Variable.Discrete(WorkerConfusionMatrix[k][trueLabel[kn]]);
                        }

                        WorkerTimeSpent[k][kn] = Variable.GammaFromShapeAndRate(NonSpammerTimeShape, NonSpammerTimeRate);
                    }
                }

                // The spammer generative process
                using (Variable.If(IsSpammer[k]))
                {
                    using (Variable.ForEach(kn))
                    {
                        WorkerLabel[k][kn] = Variable.Discrete(SpammerLabelProb[k]);
                        WorkerTimeSpent[k][kn] = Variable.GammaFromShapeAndRate(SpammerTimeShape, SpammerTimeRate);
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

        protected override void SetPriors(int workerCount, BCCPosteriors priors)
        {
            base.SetPriors(workerCount, priors);

            BCCTimeSpammerGammaPosteriors priorsForSpamModel = priors as BCCTimeSpammerGammaPosteriors;
            int numClasses = c.SizeAsInt;

            if (priorsForSpamModel == null)
            {
                // Additional priors for spammers
                IsSpammerProbPrior.ObservedValue = Beta.Uniform();
                SpammerTimeShapePrior.ObservedValue = Gamma.FromShapeAndRate(1, 1);
                SpammerTimeRatePrior.ObservedValue = Gamma.FromShapeAndScale(1, 1);
                NonSpammerTimeShapePrior.ObservedValue = Gamma.FromShapeAndRate(1, 1);
                NonSpammerTimeRatePrior.ObservedValue = Gamma.FromShapeAndScale(1, 1);
                SpammerLabelProbPrior.ObservedValue = Util.ArrayInit(workerCount, i => Dirichlet.Uniform(numClasses));
            }
            else
            {
                // Additional priors for spammers
                IsSpammerProbPrior.ObservedValue = priorsForSpamModel.BackgroundIsSpammerPosterior;
                SpammerTimeShapePrior.ObservedValue = priorsForSpamModel.SpammerTimeShapePosterior;
                SpammerTimeRatePrior.ObservedValue = priorsForSpamModel.SpammerTimeRatePosterior;
                NonSpammerTimeShapePrior.ObservedValue = priorsForSpamModel.NonSpammerTimeShapePosterior;
                NonSpammerTimeRatePrior.ObservedValue = priorsForSpamModel.NonSpammerTimeRatePosterior;
                SpammerLabelProbPrior.ObservedValue = priorsForSpamModel.SpammerLabelProbPosterior;
            }
        }

        public virtual BCCTimeSpammerGammaPosteriors Infer(int[][] taskIndices, int[][] workerLabels, double[][] workerTimeSpent)
        {
            return Infer(taskIndices, workerLabels, workerTimeSpent, null);
        }

        public virtual BCCTimeSpammerGammaPosteriors Infer(int[][] taskIndices, int[][] workerLabels, double[][] workerTimeSpent, BCCPosteriors priors)
        {

            int workerCount = workerLabels.Length;
            SetPriors(workerCount, priors);
            AttachData(taskIndices, workerLabels, workerTimeSpent);
            var result = new BCCTimeSpammerGammaPosteriors();
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
            result.NonSpammerTimeShapePosterior = Engine.Infer<Gamma>(NonSpammerTimeShape);
            result.NonSpammerTimeRatePosterior = Engine.Infer<Gamma>(NonSpammerTimeRate);
            result.SpammerTimeShapePosterior = Engine.Infer<Gamma>(SpammerTimeShape);
            result.SpammerTimeRatePosterior = Engine.Infer<Gamma>(SpammerTimeRate);
            result.SpammerLabelProbPosterior = Engine.Infer<Dirichlet[]>(SpammerLabelProb);

            if (workerTimeSpent[0] == null)
            {
                result.WorkerTimeSpentGammaPrediction = Engine.Infer<Gamma[][]>(WorkerTimeSpent);
            }

            return result;
        }
    }

    // Results class
    [Serializable]
    public class BCCTimeSpammerGammaPosteriors : BCCPosteriors
    {
        public Beta BackgroundIsSpammerPosterior;
        public Bernoulli[] IsSpammerPosterior;
        public Gamma SpammerTimeShapePosterior;
        public Gamma SpammerTimeRatePosterior;
        public Gamma NonSpammerTimeShapePosterior;
        public Gamma NonSpammerTimeRatePosterior;
        public Dirichlet[] SpammerLabelProbPosterior;
        public Gamma[][] WorkerTimeSpentGammaPrediction;
    }
}
