using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrosoftResearch.Infer.Distributions;
using MicrosoftResearch.Infer.Maths;
using MicrosoftResearch.Infer.Utils;

namespace CrowdsourcingModels
{
    public class ResultsTime : Results
    {
        // Additional variables for spammer model
        public Beta BackgroundIsSpammer
        {
            get;
            private set;
        }

        public Dictionary<string, Bernoulli> IsSpammer
        {
            get;
            private set;
        }

        public Gaussian SpammerTimeMean
        {
            get;
            private set;
        }

        public Gamma SpammerTimePrecision
        {
            get;
            private set;
        }

        public Dictionary<string, Gaussian> NonSpammerTimeMean
        {
            get;
            private set;
        }

        public Gamma NonSpammerTimePrecision
        {
            get;
            private set;
        }

        public Dictionary<string, Dirichlet> SpammerLabelProb
        {
            get;
            private set;
        }

        public Dirichlet BackgroundSpammerLabelProb
        {
            get;
            private set;
        }

        public Dictionary<string, Dictionary<string, Gaussian>> WorkerTimeSpent
        {
            get;
            private set;
        }

        public Dictionary<string, Discrete> SpammerType
        {
            get;
            private set;
        }

        public Dictionary<string, Gaussian> TaskShortTime
        {
            get;
            private set;
        }

        public Dictionary<string, Gaussian> TaskLongTime
        {
            get;
            private set;
        }

        public Dictionary<string, Beta> WorkerPropensityForValidLabelling
        {
            get;
            private set;
        }

        public Dictionary<string, Gaussian> WorkerPropensityForValidLabellingScore
        {
            get;
            private set;
        }

        protected override void ClearResults()
        {
            base.ClearResults();
            IsSpammer = new Dictionary<string, Bernoulli>();
            SpammerType = new Dictionary<string, Discrete>();
            SpammerLabelProb = new Dictionary<string, Dirichlet>();
            WorkerTimeSpent = new Dictionary<string, Dictionary<string, Gaussian>>();
            BackgroundIsSpammer = Beta.Uniform();
            SpammerTimeMean = Gaussian.Uniform();
            SpammerTimePrecision = Gamma.Uniform();
            NonSpammerTimeMean = new Dictionary<string,Gaussian>();
            NonSpammerTimePrecision = Gamma.Uniform();
            BackgroundSpammerLabelProb = Dirichlet.Uniform(Mapping.LabelCount);
            WorkerTimeSpent = new Dictionary<string, Dictionary<string, Gaussian>>();
            BackgroundIsSpammer = Beta.Uniform();
            SpammerTimeMean = Gaussian.Uniform();
            TaskShortTime = new Dictionary<string, Gaussian>();
            TaskLongTime = new Dictionary<string, Gaussian>();
            WorkerPropensityForValidLabelling = new Dictionary<string, Beta>();
            WorkerPropensityForValidLabellingScore = new Dictionary<string, Gaussian>();
        }

        protected override void UpdateAccuracy()
        {
            base.UpdateAccuracy();

            if (IsTimeModel)
            {
                // WorkerLabelAccuracy: Perc. agreement between worker label and gold label for non spammers
                int sumAcc = 0;
                var LabelSet = Mapping.DataWithGold;
                int numNonSpammerLabels = 0;
                foreach (var datum in LabelSet)
                {
                    if (IsSpammer[datum.WorkerId].GetProbFalse() > 0.9)
                    {
                        numNonSpammerLabels++;
                        sumAcc += datum.WorkerLabel == datum.GoldLabel ? 1 : 0;
                    }

                }
                this.WorkerLabelAccuracy = (double)sumAcc / (double)numNonSpammerLabels;
            }
        }

        public override void WriteResults(StreamWriter writer, bool writeCommunityParameters, bool writeWorkerParameters, bool writeWorkerCommunities)
        {
            base.WriteResults(writer, writeCommunityParameters, writeWorkerParameters, writeWorkerCommunities);

            if (IsTimeModel)
            {
                writer.WriteLine("Spammer time mean: {0}, precision: {1}", SpammerTimeMean, SpammerTimePrecision);
                double avgTimeTaken = NonSpammerTimeMean.Average(t => t.Value.GetMean());
                writer.WriteLine("Average non-spammer task time: {0}", avgTimeTaken);

                foreach (string t in NonSpammerTimeMean.Keys)
                {
                    //writer.WriteLine("Task {1}: Expected time: {0}", NonSpammerTimeMean[t].GetMean(), t);
                }
                foreach (string w in IsSpammer.Keys)
                {
                    writer.WriteLine("Worker {0}: IsSpammer {1:0.000}", w, IsSpammer[w].GetMode());
                }
            }

            if (IsTimeMultimodeModel)
            {
                foreach (string t in NonSpammerTimeMean.Keys)
                {
                    //writer.WriteLine("Task {1}: Expected time: {0}", NonSpammerTimeMean[t].GetMean(), t);
                }
                foreach (string w in SpammerType.Keys)
                {
                    writer.WriteLine("Worker {0}: SpammerType {1}", w, SpammerType[w]);
                }
            }

            if (IsTimeTaskPropensityModel)
            {
                var avgTime = GetAverageTaskTime();
                //foreach (string t in TaskShortTime.Keys.Take(20))
                foreach (string t in TaskShortTime.Keys)
                {
                    //writer.WriteLine("Task {1}: Short time: {0:}, Long time: {2}, Avg time: {3:0.000}", TaskShortTime[t], t.Substring(0, 20), TaskLongTime[t], avgTime[t]);
                    writer.WriteLine("Task {1}: Short time: {0:}, Long time: {2}, Avg time: {3:0.000}", TaskShortTime[t], t, TaskLongTime[t], avgTime[t]);
                }
                //foreach (string w in WorkerPropensityForValidLabelling.Keys.Take(20))
                foreach (string w in WorkerPropensityForValidLabelling.Keys)
                {
                    writer.WriteLine("Worker {0}: Propensity {1}", w, WorkerPropensityForValidLabelling[w]);
                    for (int i = 0; i < this.Mapping.LabelCount; i++)
                    {
                        writer.WriteLine(this.WorkerConfusionMatrix[w][i].GetMean());
                    }
                }

                var a = WorkerPropensityForValidLabelling.Count(w => w.Value.GetMean() > 0.5);
                double percNonSpammers = WorkerPropensityForValidLabelling.Count(w => w.Value.GetMean() > 0.5) / (double)WorkerPropensityForValidLabelling.Count();
                writer.WriteLine("% non spammers: {0:0.000}", percNonSpammers);

                writer.WriteLine("\nBackground spammer label prob: {0}", BackgroundSpammerLabelProb.GetMean());
            }

        }

        protected override void UpdateResults(BCCPosteriors posteriors, RunMode mode)
        {
            if (mode == RunMode.LookAheadExperiment)
            {
                for (int t = 0; t < posteriors.TrueLabel.Length; t++)
                {
                    LookAheadTrueLabel[Mapping.TaskIndexToId[t]] = posteriors.TrueLabel[t];
                }
                for (int w = 0; w < posteriors.WorkerConfusionMatrix.Length; w++)
                {
                    LookAheadWorkerConfusionMatrix[Mapping.WorkerIndexToId[w]] = posteriors.WorkerConfusionMatrix[w];
                }
            }
            else if (mode == RunMode.Prediction)
            {
                for (int w = 0; w < posteriors.WorkerConfusionMatrix.Length; w++)
                {
                    WorkerPrediction[Mapping.WorkerIndexToId[w]] = new Dictionary<string, Discrete>();
                    for (int tw = 0; tw < posteriors.WorkerPrediction[w].Length; tw++)
                    {
                        WorkerPrediction[Mapping.WorkerIndexToId[w]][Mapping.TaskIndexToId[tw]] = posteriors.WorkerPrediction[w][tw];
                    }
                }

                if (IsTimeModel)
                {
                    BCCTimeSpammerPosteriors bccTimeSpammerPosteriors = posteriors as BCCTimeSpammerPosteriors;
                    for (int w = 0; w < bccTimeSpammerPosteriors.WorkerConfusionMatrix.Length; w++)
                    {
                        WorkerTimeSpent[Mapping.WorkerIndexToId[w]] = new Dictionary<string, Gaussian>();
                        for (int tw = 0; tw < bccTimeSpammerPosteriors.WorkerTimeSpentGaussianPrediction[w].Length; tw++)
                        {
                            WorkerTimeSpent[Mapping.WorkerIndexToId[w]][Mapping.TaskIndexToId[tw]] = bccTimeSpammerPosteriors.WorkerTimeSpentGaussianPrediction[w][tw];
                        }
                    }
                }

            }
            else
            {
                // Update results for BCC
                BackgroundLabelProb = posteriors.BackgroundLabelProb;
                for (int w = 0; w < posteriors.WorkerConfusionMatrix.Length; w++)
                {
                    WorkerConfusionMatrix[Mapping.WorkerIndexToId[w]] = posteriors.WorkerConfusionMatrix[w];
                }
                for (int t = 0; t < posteriors.TrueLabel.Length; t++)
                {
                    TrueLabel[Mapping.TaskIndexToId[t]] = posteriors.TrueLabel[t];
                }
                for (int t = 0; t < posteriors.TrueLabelConstraint.Length; t++)
                {
                    TrueLabelConstraint[Mapping.TaskIndexToId[t]] = posteriors.TrueLabelConstraint[t];
                }

                // Update results for CBCC
                if (IsCommunityModel)
                {
                    CBCCPosteriors cbccPosteriors = posteriors as CBCCPosteriors;
                    CommunityConfusionMatrix = cbccPosteriors.CommunityConfusionMatrix;
                    for (int w = 0; w < cbccPosteriors.WorkerScoreMatrixConstraint.Length; w++)
                    {
                        WorkerScoreMatrixConstraint[Mapping.WorkerIndexToId[w]] = cbccPosteriors.WorkerScoreMatrixConstraint[w];
                        CommunityConstraint[Mapping.WorkerIndexToId[w]] = cbccPosteriors.WorkerCommunityConstraint[w];
                        WorkerCommunity[Mapping.WorkerIndexToId[w]] = cbccPosteriors.Community[w];
                    }

                    CommunityProb = cbccPosteriors.CommunityProb;
                    CommunityScoreMatrix = cbccPosteriors.CommunityScoreMatrix;
                }

                // Update results for BCCTimeSpammer
                if (IsTimeModel)
                {
                    BCCTimeSpammerPosteriors bccTimeSpammerPosteriors = posteriors as BCCTimeSpammerPosteriors;                    
                    BackgroundIsSpammer = bccTimeSpammerPosteriors.BackgroundIsSpammerPosterior;
                    SpammerTimeMean = bccTimeSpammerPosteriors.SpammerTimeMeanPosterior;
                    SpammerTimePrecision = bccTimeSpammerPosteriors.SpammerTimePrecisionPosterior;

                    for (int t = 0; t < posteriors.TrueLabelConstraint.Length; t++)
                    {
                        NonSpammerTimeMean[Mapping.TaskIndexToId[t]] = bccTimeSpammerPosteriors.TaskNonSpammerTimeMeanPosterior[t];
                    }

                    NonSpammerTimePrecision = bccTimeSpammerPosteriors.NonSpammerTimePrecisionPosterior;
                    for (int w = 0; w < bccTimeSpammerPosteriors.IsSpammerPosterior.Length; w++)
                    {
                        IsSpammer[Mapping.WorkerIndexToId[w]] = bccTimeSpammerPosteriors.IsSpammerPosterior[w];
                        SpammerLabelProb[Mapping.WorkerIndexToId[w]] = bccTimeSpammerPosteriors.SpammerLabelProbPosterior[w];
                    }

                }

                // Update results for BCCTimeSpammerMultimode
                if (IsTimeMultimodeModel)
                {
                    BCCTimeSpammerMultimodePosteriors bccTimeSpammerPosteriors = posteriors as BCCTimeSpammerMultimodePosteriors;

                    //NonSpammerTimePrecision = bccTimeSpammerPosteriors.NonSpammerTimePrecisionPosterior;
                    for (int w = 0; w < bccTimeSpammerPosteriors.SpammerTypePosterior.Length; w++)
                    {
                        SpammerType[Mapping.WorkerIndexToId[w]] = bccTimeSpammerPosteriors.SpammerTypePosterior[w];
                        SpammerLabelProb[Mapping.WorkerIndexToId[w]] = bccTimeSpammerPosteriors.SpammerLabelProbPosterior[w];
                    }
                }

                if (IsTimeTaskPropensityModel)
                {

                    // Update results for BCCTime
                    BCCTimeTaskPropensityPosteriors bccTimePosteriors = posteriors as BCCTimeTaskPropensityPosteriors;
                    BackgroundSpammerLabelProb = bccTimePosteriors.SpammerLabelProbPosterior;
                    for (int t = 0; t < posteriors.TrueLabelConstraint.Length; t++)
                    {
                        TaskShortTime[Mapping.TaskIndexToId[t]] = bccTimePosteriors.TaskShortTimePosterior[t];
                        TaskLongTime[Mapping.TaskIndexToId[t]] = bccTimePosteriors.TaskLongTimePosterior[t];
                    }

                    for (int w = 0; w < bccTimePosteriors.WorkerConfusionMatrix.Length; w++)
                    {
                        WorkerPropensityForValidLabelling[Mapping.WorkerIndexToId[w]] = bccTimePosteriors.PropensityForValidLabellingPosterior[w];
                    }
                }

                this.ModelEvidence = posteriors.Evidence;
            }
        }

        protected Dictionary<string, double> GetAverageTaskTime()
        {
            return TaskShortTime.ToDictionary(kvp => kvp.Key, kvp =>
            {
                var lt = TaskLongTime[kvp.Key].GetMean();
                var st = TaskShortTime[kvp.Key].GetMean();
                return (lt - st) / 2;
            });
        }
    }
}
