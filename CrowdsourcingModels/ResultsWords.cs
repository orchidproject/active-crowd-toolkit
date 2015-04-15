﻿using MicrosoftResearch.Infer;
using MicrosoftResearch.Infer.Factors;
using MicrosoftResearch.Infer.Distributions;
using MicrosoftResearch.Infer.Maths;
using MicrosoftResearch.Infer.Models;
using MicrosoftResearch.Infer.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GetAnotherLabel;
using TFIDF;


namespace CrowdsourcingModels
{

    /// <summary>
    /// Results class containing posteriors and predictions of BCCWords.
    /// </summary>
    public class ResultsWords : Results
    {
        /// <summary>
        /// The posterior of the word probabilities for each true label.
        /// </summary>
        public Dirichlet[] ProbWords
        {
            get;
            private set;
        }

        /// <summary>
        /// The vocabulary
        /// </summary>
        public List<String> Vocabulary
        {
            get;
            set;
        }

        public ResultsWords(IList<Datum> data, List<string> vocabulary)
        {

            if (vocabulary==null)
            {
                // Build vocabulary
                Console.Write("Building vocabulary...");
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                string[] corpus = data.Select(d => d.BodyText).Distinct().ToArray();
                Vocabulary = BuildVocabularyFromCorpus(corpus);
                Console.WriteLine("done. Elapsed time: {0}", stopwatch.Elapsed);
            }

            // Build data mapping
            Vocabulary = vocabulary;
            this.Mapping = new DataMappingWords(data, vocabulary);
            this.GoldLabels = Mapping.GetGoldLabelsPerTaskId();
        }

        /// <summary>
        /// Runs the majority vote method on the data.
        /// </summary>
        /// <param name="data">The data</param>
        /// <param name="calculateAccuracy">Compute the accuracy (true).</param>
        /// <param name="useVoteDistribution">The true label is sampled from the vote distribution (true) or it is
        /// taken as the mode of the vote counts (false).
        /// In the latter case, ties are broken by sampling from the most voted classes.</param>
        /// <returns>The updated results</returns>
        /// 
        public void RunBCCWords(string modelName, IList<Datum> data, IList<Datum> fullData, BCCWords model, RunMode mode, bool calculateAccuracy, bool useMajorityVote = false, bool useRandomLabel = false)
        {
            DataMappingWords MappingWords = null;
            if (FullMapping == null)
                FullMapping = new DataMapping(fullData);

            if(Mapping==null)
            {
                // Build vocabulary
                Console.Write("Building vocabulary...");
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                string[] corpus = data.Select(d => d.BodyText).Distinct().ToArray();
                Vocabulary = BuildVocabularyFromCorpus(corpus);
                Console.WriteLine("done. Elapsed time: {0}", stopwatch.Elapsed);

                // Build data mapping
                this.Mapping = new DataMappingWords(data, MappingWords.Vocabulary);
                MappingWords = Mapping as DataMappingWords;
                this.GoldLabels = MappingWords.GetGoldLabelsPerTaskId();
            }

            MappingWords = Mapping as DataMappingWords;
            int[] trueLabels = null;
            if (useMajorityVote)
            {
                var majorityLabel = MappingWords.GetMajorityVotesPerTaskId(data);
                trueLabels = Util.ArrayInit(FullMapping.TaskCount, i => majorityLabel.ContainsKey(Mapping.TaskIndexToId[i]) ? (int)majorityLabel[Mapping.TaskIndexToId[i]] : Rand.Int(Mapping.LabelMin, Mapping.LabelMax + 1));

                var majorityData = MappingWords.BuildDataFromAssignedLabels(majorityLabel, data);
                if (MappingWords == null)
                    MappingWords = new DataMappingWords(fullData, MappingWords.Vocabulary, MappingWords.WordCountsPerTaskIndex, MappingWords.WordIndicesPerTaskIndex, true);
                data = majorityData;
            }

            if (useRandomLabel)
            {
                Rand.Restart(12347);
                var randomLabels = MappingWords.GetRandomLabelPerTaskId(data);
                var randomData = MappingWords.BuildDataFromAssignedLabels(randomLabels, data);
                if (MappingWords == null)
                    MappingWords = new DataMappingWords(fullData, MappingWords.Vocabulary, MappingWords.WordCountsPerTaskIndex, MappingWords.WordIndicesPerTaskIndex, true);
                data = randomData;
            }

            
            var labelsPerWorkerIndex = MappingWords.GetLabelsPerWorkerIndex(data);
            var taskIndicesPerWorkerIndex = MappingWords.GetTaskIndicesPerWorkerIndex(data);

            // Create model
            ClearResults();
            model.CreateModel(MappingWords.TaskCount, MappingWords.LabelCount, MappingWords.WordCount);

            // Run model inference
            BCCWordsPosteriors posteriors = model.InferPosteriors(labelsPerWorkerIndex, taskIndicesPerWorkerIndex, MappingWords.WordIndicesPerTaskIndex, MappingWords.WordCountsPerTaskIndex, trueLabels);

            // Update results
            UpdateResults(posteriors, mode);

            /// Compute accuracy
            if (calculateAccuracy)
            {
                UpdateAccuracy();
            }
        }


        public void SetVocabulary(List<string> vocabulary)
        {
            DataMappingWords MappingWords = Mapping as DataMappingWords;
            this.Vocabulary = vocabulary;
            MappingWords.Vocabulary = vocabulary;
        }

        public void RunBCCWordsGoldData(string modelName, IList<Datum> goldData, BCCWords model, RunMode mode, bool calculateAccuracy, int numCommunities = -1, bool serialize = false, bool serializeCommunityPosteriors = false)
        {
            DataMappingWords MappingWords = null;
            if (Mapping == null)
            {
                // Build vocabulary
                Console.Write("Building ResultsWords object. Building corpus...");
                Stopwatch stopwatch = new Stopwatch();
                string[] corpus = goldData.Select(d => d.BodyText).Distinct().ToArray();
                MappingWords = Mapping as DataMappingWords;
                MappingWords.Vocabulary = BuildVocabularyFromCorpus(corpus);
                Console.WriteLine("done. Elapsed time: {0}", stopwatch.Elapsed);

                // Build data mapping
                MappingWords = new DataMappingWords(goldData, MappingWords.Vocabulary);
                this.GoldLabels = MappingWords.GetGoldLabelsPerTaskId();
            }

            int[][] wordIdicesPerTaskIndex = null;
            int[] wordCountsPerTaskIndex = null;
            MappingWords.GetWordIndicesAndCountsPerTaskIndex(goldData, out wordIdicesPerTaskIndex, out wordCountsPerTaskIndex);
            var labelsPerWorkerIndex = MappingWords.GetLabelsPerWorkerIndex(goldData);
            var taskIndicesPerWorkerIndex = MappingWords.GetTaskIndicesPerWorkerIndex(goldData);

            // Create model
            model.CreateModel(MappingWords.WorkerCount, MappingWords.TaskCount, MappingWords.LabelCount, MappingWords.WordCount);

            // Run model inference
            BCCWordsPosteriors posteriors = model.InferPosteriors(labelsPerWorkerIndex, taskIndicesPerWorkerIndex, wordIdicesPerTaskIndex, wordCountsPerTaskIndex);

            // Update results
            ClearResults();
            UpdateResults(posteriors, mode);

            /// Compute accuracy
            if (calculateAccuracy)
            {
                UpdateAccuracy();
            }
        }

        private static List<string> BuildVocabularyFromCorpus(string[] corpus, double tfidf_threshold = 0.8)
        {
            List<string> vocabulary;
            double[][] inputs = TFIDFClass.Transform(corpus, out vocabulary, 0);
            inputs = TFIDFClass.Normalize(inputs);

            // Select high TF_IDF terms
            List<string> vocabulary_tfidf = new List<string>();
            for (int index = 0; index < inputs.Length; index++)
            {
                var sortedTerms = inputs[index].Select((x, i) => new KeyValuePair<string, double>(vocabulary[(int)i], x)).OrderByDescending(x => x.Value).ToList();
                vocabulary_tfidf.AddRange(sortedTerms.Where(entry => entry.Value > tfidf_threshold).Select(k => k.Key).ToList());
            }
            return vocabulary.Distinct().ToList();
        }


        /// <summary>
        /// Updates the results of with the new posteriors.
        /// </summary>
        /// <param name="posteriors">The posteriors.</param>
        /// <param name="mode">The mode (for example training, prediction, etc.).</param>
        void UpdateResults(BCCWordsPosteriors posteriors, RunMode mode)
        {
            this.BackgroundLabelProb = posteriors.BackgroundLabelProb;
            if (posteriors.WorkerConfusionMatrix.Length == Mapping.WorkerCount)
            {
                for (int w = 0; w < posteriors.WorkerConfusionMatrix.Length; w++)
                {
                    WorkerConfusionMatrix[Mapping.WorkerIndexToId[w]] = posteriors.WorkerConfusionMatrix[w];
                }
            }
            for (int t = 0; t < posteriors.TrueLabel.Length; t++)
            {
                TrueLabel[Mapping.TaskIndexToId[t]] = posteriors.TrueLabel[t];
            }
            this.ProbWords = posteriors.ProbWordPosterior;
        }


        protected override void ClearResults()
        {
            BackgroundLabelProb = Dirichlet.Uniform(Mapping.LabelCount);
            WorkerConfusionMatrix = new Dictionary<string, Dirichlet[]>();
            WorkerPrediction = new Dictionary<string, Dictionary<String, Discrete>>();
            WorkerCommunity = new Dictionary<string, Discrete>();
            TrueLabel = new Dictionary<string, Discrete>();
            PredictedLabel = new Dictionary<string, int?>();
            TrueLabelConstraint = new Dictionary<string, Discrete>();
            CommunityConfusionMatrix = null;
            WorkerScoreMatrixConstraint = new Dictionary<string, VectorGaussian[]>();
            CommunityProb = null;
            CommunityScoreMatrix = null;
            CommunityConstraint = new Dictionary<string, Discrete>();
            LookAheadTrueLabel = new Dictionary<string, Discrete>();
            LookAheadWorkerConfusionMatrix = new Dictionary<string, Dirichlet[]>();
            ModelEvidence = new Bernoulli(0.5);


            ProbWords = null;

        }

        /// <summary>
        /// Writes various results to a StreamWriter.
        /// </summary>
        /// <param name="writer">A StreamWriter instance.</param>
        /// <param name="writeCommunityParameters">Set true to write community parameters.</param>
        /// <param name="writeWorkerParameters">Set true to write worker parameters.</param>
        /// <param name="writeWorkerCommunities">Set true to write worker communities.</param>
        public void WriteResults(StreamWriter writer, bool writeCommunityParameters, bool writeWorkerParameters, bool writeWorkerCommunities, bool writeProbWords, int topWords = 30)
        {
            base.WriteResults(writer, writeCommunityParameters, writeWorkerCommunities, writeWorkerCommunities);
            DataMappingWords MappingWords = Mapping as DataMappingWords;
            if(writeProbWords && this.ProbWords != null)
            {
                int NumClasses = ProbWords.Length;
                for (int c = 0; c < NumClasses; c++)
                {
                    if (MappingWords.WorkerCount > 300) // Assume it's CF
                        writer.WriteLine("Class {0}", MappingWords.CFLabelName[c]);
                    else
                        writer.WriteLine("Class {0}", MappingWords.SPLabelName[c]);
                    Vector probs = ProbWords[c].GetMean();
                    var probsDictionary = probs.Select((value, index) => new KeyValuePair<string, double>(MappingWords.Vocabulary[index], Math.Log(value))).OrderByDescending(x => x.Value).ToArray();
                    for (int w = 0; w < topWords; w++)
                    {
                        writer.WriteLine(string.Format("\t{0}: \t{1:0.000}", probsDictionary[w].Key, probsDictionary[w].Value));
                    }       
                } 
            }
        }


        public static List<string> BuildVocabularyOnSubdata(List<Datum> data)
        {
            // Build vocabulary on sub data
            Console.Write("Building vocabulary...");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var subData = data.Where((k, i) => i < 20000).ToList();
            string[] corpus = subData.Select(d => d.BodyText).Distinct().ToArray();
            var vocabularyOnSubData = BuildVocabularyFromCorpus(corpus);
            Console.WriteLine("done. Elapsed time: {0}", stopwatch.Elapsed);
            return vocabularyOnSubData.GetRange(0, 300);
            
        }

    } //end of class Results
}