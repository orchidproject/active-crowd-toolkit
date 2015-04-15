using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrosoftResearch.Infer.Distributions;
using MicrosoftResearch.Infer.Maths;
using MicrosoftResearch.Infer.Utils;

namespace CrowdsourcingSoton
{
    public class ResultsTimePropensity : Results
    {
        // Additional variables for spammer model
        public Beta BackgroundIsSpammer
        {
            get;
            private set;
        }

        public Gaussian SpammerTimeMean
        {
            get;
            private set;
        }

        public Dirichlet SpammerLabelProb
        {
            get;
            private set;
        }

        public Dictionary<string, Dictionary<string, Gaussian>> WorkerTimeSpent
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

        protected override void ClearResults()
        {
            base.ClearResults();
            SpammerLabelProb = Dirichlet.Uniform(Mapping.LabelCount);
            WorkerTimeSpent = new Dictionary<string, Dictionary<string, Gaussian>>();
            BackgroundIsSpammer = Beta.Uniform();
            SpammerTimeMean = Gaussian.Uniform();
            TaskShortTime = new Dictionary<string, Gaussian>();
            TaskLongTime = new Dictionary<string, Gaussian>();
            WorkerPropensityForValidLabelling = new Dictionary<string, Beta>();
        }

        public override void WriteResults(StreamWriter writer, bool writeCommunityParameters, bool writeWorkerParameters, bool writeWorkerCommunities)
        {
            base.WriteResults(writer, writeCommunityParameters, writeWorkerParameters, writeWorkerCommunities);

            var avgTime = GetAverageTaskTime();
            foreach (string t in TaskShortTime.Keys)
            {
                writer.WriteLine("Task {1}: Short time: {0}, Long time: {2}, Avg time: {3}", TaskShortTime[t], t, TaskLongTime[t], avgTime[t]);
            }
            foreach (string w in WorkerPropensityForValidLabelling.Keys)
            {
                writer.WriteLine("Worker {0}: Propensity {1}", w, WorkerPropensityForValidLabelling[w]);
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

                // Update results for BCCTime
                BCCTimeTaskPropensityPosteriors bccTimePosteriors = posteriors as BCCTimeTaskPropensityPosteriors;
                SpammerLabelProb = bccTimePosteriors.SpammerLabelProbPosterior;
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
}
