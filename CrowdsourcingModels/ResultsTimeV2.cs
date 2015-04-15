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
    public class ResultsTimeV2 : Results
    {
        // Additional variables for spammer model
        public Beta BackgroundIsSpammer
        {
            get;
            private set;
        }

        public Beta BackgroundIsLongSpammer
        {
            get;
            private set;
        }

        public Dictionary<string, Bernoulli> IsSpammer
        {
            get;
            private set;
        }

        public Dictionary<string, Bernoulli> IsLongSpammer
        {
            get;
            private set;
        }

        public Gaussian SpammerLongTimeMean
        {
            get;
            private set;
        }

        public Gaussian SpammerShortTimeMean
        {
            get;
            private set;
        }


        public Dictionary<string, Gaussian> TaskNonSpammerTimeMean
        {
            get;
            private set;
        }


        public Dictionary<string, Dirichlet> SpammerLabelProb
        {
            get;
            private set;
        }

        public Dictionary<string, Dictionary<string, Gaussian>> WorkerTimeSpent
        {
            get;
            private set;
        }

        protected override void ClearResults()
        {
            base.ClearResults();
            IsSpammer = new Dictionary<string, Bernoulli>();
            IsLongSpammer = new Dictionary<string, Bernoulli>();
            SpammerLabelProb = new Dictionary<string, Dirichlet>();
            WorkerTimeSpent = new Dictionary<string, Dictionary<string, Gaussian>>();
            BackgroundIsSpammer = Beta.Uniform();
            BackgroundIsLongSpammer = Beta.Uniform();
            SpammerLongTimeMean = Gaussian.Uniform();
            SpammerShortTimeMean = Gaussian.Uniform();
            TaskNonSpammerTimeMean = new Dictionary<string,Gaussian>();
        }

        public override void WriteResults(StreamWriter writer, bool writeCommunityParameters, bool writeWorkerParameters, bool writeWorkerCommunities)
        {
            base.WriteResults(writer, writeCommunityParameters, writeWorkerParameters, writeWorkerCommunities);

            if (IsTimeModel)
            {
                double avgTimeTaken = TaskNonSpammerTimeMean.Average(t => t.Value.GetMean());
                writer.WriteLine("Average non-spammer time taken: {0}", avgTimeTaken);

                foreach (string t in TaskNonSpammerTimeMean.Keys)
                {
                    //writer.WriteLine("Task {1}: Expected time: {0}", TaskNonSpammerTimeMean[t].GetMean(), t);
                }
                foreach (string w in IsSpammer.Keys)
                {
                    var probSpammer = IsSpammer[w].GetProbTrue();
                    if (probSpammer > 0.5)
                        writer.WriteLine("Worker {0}: IsSpammer {1:0.000} IsLongSpammer {2:0.000} LB {3:0.000} UB {4:0.000}", w, probSpammer, IsLongSpammer[w].GetProbTrue(), SpammerShortTimeMean.GetMean(), SpammerLongTimeMean.GetMean());

                    else
                        writer.WriteLine("Worker {0}: IsSpammer {1:0.000}", w, probSpammer);
                }
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
                    BCCTimeSpammerPosteriorsV2 bccTimeSpammerPosteriors = posteriors as BCCTimeSpammerPosteriorsV2;
                    BackgroundIsSpammer = bccTimeSpammerPosteriors.BackgroundIsSpammerPosterior;

                    for (int t = 0; t < posteriors.TrueLabelConstraint.Length; t++)
                    {
                        TaskNonSpammerTimeMean[Mapping.TaskIndexToId[t]] = bccTimeSpammerPosteriors.TaskNonSpammerTimeMeanPosterior[t];
                    }

                    SpammerShortTimeMean = bccTimeSpammerPosteriors.SpammerShortTimeMeanPosterior;
                    SpammerLongTimeMean = bccTimeSpammerPosteriors.SpammerLongTimeMeanPosterior;
                    for (int w = 0; w < bccTimeSpammerPosteriors.IsSpammerPosterior.Length; w++)
                    {
                        IsSpammer[Mapping.WorkerIndexToId[w]] = bccTimeSpammerPosteriors.IsSpammerPosterior[w];
                        IsLongSpammer[Mapping.WorkerIndexToId[w]] = bccTimeSpammerPosteriors.IsLongSpammerPosterior[w];
                        SpammerLabelProb[Mapping.WorkerIndexToId[w]] = bccTimeSpammerPosteriors.SpammerLabelProbPosterior[w];

                    }

                }

                this.ModelEvidence = posteriors.Evidence;
            }
        }
    }
}
