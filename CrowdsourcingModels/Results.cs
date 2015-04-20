using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrosoftResearch.Infer.Distributions;
using MicrosoftResearch.Infer.Maths;
using MicrosoftResearch.Infer.Utils;
using GetAnotherLabel;
using CrowdsourcingProject.Statistics;

namespace CrowdsourcingModels
{

    /// Results class containing posteriors and predictions.
    /// </summary>
    public class Results
    {
        /// <summary>
        /// The posterior of the true label for each task.
        /// </summary>
        public Dictionary<string, Discrete> TrueLabel
        {
            get;
            protected set;
        }

        /// <summary>
        /// The predicted label for each task when doing simulations from the current
        /// model state. It avoids overwriting the true label posterior.
        /// </summary>
        public Dictionary<string, Discrete> LookAheadTrueLabel
        {
            get;
            protected set;
        }

        /// <summary>
        /// The posterior for the constraint that allows online learning for the true label variable.
        /// </summary>
        public Dictionary<string, Discrete> TrueLabelConstraint
        {
            get;
            protected set;
        }

        /// <summary>
        /// The predicted label for each task
        /// </summary>
        public Dictionary<string, int?> PredictedLabel
        {
            get;
            protected set;
        }

        /// <summary>
        /// The probabilities that generate the true label of all the tasks.
        /// </summary>
        public Dirichlet BackgroundLabelProb
        {
            get;
            protected set;
        }

        /// <summary>
        /// The posterior of the confusion matrix of each worker.
        /// </summary>
        public Dictionary<string, Dirichlet[]> WorkerConfusionMatrix
        {
            get;
            protected set;
        }

        /// <summary>
        /// The look-ahead posterior of the confusion matrix of each worker obtained after simulating
        /// a new label in look-ahead run mode.
        /// </summary>
        public Dictionary<string, Dirichlet[]> LookAheadWorkerConfusionMatrix
        {
            get;
            protected set;
        }


        /// <summary>
        /// The predictive probabilities of the labels produced by each worker.
        /// </summary>
        public Dictionary<string, Dictionary<string, Discrete>> WorkerPrediction
        {
            get;
            protected set;
        }

        /// <summary>
        /// The community membership probabilities of each worker.
        /// </summary>
        public Dictionary<string, Discrete> WorkerCommunity
        {
            get;
            protected set;
        }

        /// <summary>
        /// The confusion matrix of each community.
        /// </summary>
        public Dirichlet[][] CommunityConfusionMatrix
        {
            get;
            protected set;
        }

        /// <summary>
        /// The score matrix of each community.
        /// </summary>
        public VectorGaussian[][] CommunityScoreMatrix
        {
            get;
            protected set;
        }

        /// <summary>
        /// The posterior for the constraint that allows online learning for worker confusion matrices
        /// int the community model.
        /// </summary>
        public Dictionary<string, VectorGaussian[]> WorkerScoreMatrixConstraint
        {
            get;
            protected set;
        }

        /// <summary>
        /// The probabilities that generate the community memberships of all the workers.
        /// </summary>
        public Dirichlet CommunityProb
        {
            get;
            protected set;
        }

        /// <summary>
        /// The posterior for the constraint that allows online learning for community membership.
        /// int the community model.
        /// </summary>
        public Dictionary<string, Discrete> CommunityConstraint
        {
            get;
            protected set;
        }

        /// <summary>
        /// Model evidence.
        /// </summary>
        public Bernoulli ModelEvidence
        {
            get;
            protected set;
        }

        /// <summary>
        /// Memory used for model training (in MB)
        /// </summary>
        public double Memory
        {
            get;
            set;
        }

        /// <summary>
        /// Execution time of model training (in seconds)
        /// </summary>
        public double ExecutionTime
        {
            get;
            set;
        }

        /// <summary>
        /// The data mapping.
        /// </summary>
        public DataMapping Mapping
        {
            get;
            set;
        }

        /// <summary>
        /// The full data mapping.
        /// </summary>
        public DataMapping FullMapping
        {
            get;
            set;
        }

        /// <summary>
        /// The gold labels of each task. The gold label type is nullable to
        /// support the (usual) situation where the is no labels.
        /// </summary>
        public Dictionary<string, int?> GoldLabels
        {
            get;
            protected set;
        }

        /// <summary>
        /// The accuracy of the current true label predictions.
        /// </summary>
        public double Accuracy
        {
            get;
            private set;
        }

        /// <summary>
        /// The accuracy of the worker labels.
        /// </summary>
        public double WorkerLabelAccuracy
        {
            get;
            protected set;
        }

        /// <summary>
        /// The negative log probability density (NLPD) scores of the current true label predictions.
        /// </summary>
        public double NegativeLogProb
        {
            get;
            private set;
        }

        /// <summary>
        /// The average recall of the current true label predictions.
        /// </summary>
        public double AvgRecall
        {
            get;
            private set;
        }

        /// <summary>
        /// The confusion matrix of the predicted true labels against the gold labels
        /// The rows are the gold labels and the columns are the predicted labels.
        /// </summary>
        public double[,] ModelConfusionMatrix
        {
            get;
            private set;
        }

        /// <summary>
        /// Flags whether the model instance is CBCC (true) or BCC (false).
        /// </summary>
        public bool IsCommunityModel
        {
            get;
            private set;
        }

        /// <summary>
        /// Flags whether the model instance is a BCC time model (true) or not (false).
        /// </summary>
        public bool IsTimeModel
        {
            get;
            private set;
        }

        /// <summary>
        /// Flags whether the model instance is a BCC time model (true) or not (false).
        /// </summary>
        public bool IsTimeMultimodeModel
        {
            get;
            private set;
        }

        public bool IsTimeTaskPropensityModel
        {
            get;
            private set;
        }

        /// <summary>
        /// The number of communities.
        /// </summary>
        public int CommunityCount
        {
            get;
            private set;
        }

        public ConfusionMatrix BynaryConfusionMatrix
        {
            get;
            private set;
        }

        public ReceiverOperatingCharacteristic RocCurve
        {
            get;
            private set;
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
        public Results RunMajorityVote(IList<Datum> data, IList<Datum> fullData, bool calculateAccuracy, bool useVoteDistribution)
        {
            //if (useVoteDistribution)
            //    Console.WriteLine("\n--- Vote distribution ---");
            //else
            //    Console.WriteLine("\n--- Majority Vote ---");

            PredictedLabel = new Dictionary<string, int?>();
            var dataMapping = new DataMapping(data);
            Mapping = dataMapping;

            var fullDataMapping = new DataMapping(fullData);
            GoldLabels = fullDataMapping.GetGoldLabelsPerTaskId();

            var inferredLabels = useVoteDistribution ? dataMapping.GetVoteDistribPerTaskIndex() : dataMapping.GetMajorityVotesPerTaskIndex().Select(mv => mv == null ? (Discrete)null : Discrete.PointMass(mv.Value, dataMapping.LabelCount)).ToArray();
            TrueLabel = inferredLabels.Select((lab, i) => new
            {
                key = dataMapping.TaskIndexToId[i],
                val = lab
            }).ToDictionary(a => a.key, a => a.val);

            if (calculateAccuracy)
            {
                UpdateAccuracy();
            }
            return this;
        }

        /// <summary>
        /// Run Dawid-Skene on the data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="calculateAccuracy">Whether to calculate accuracy</param>
        /// <returns>A results instance</returns>
        public Results RunDawidSkene(IList<Datum> data, IList<Datum> fullData, bool calculateAccuracy)
        {
            // If you want to run Dawid-Skene code, download his code, integrate it into
            // the project, and change false to true below.
            Console.WriteLine("--- Dawid Skene ---");
            PredictedLabel = new Dictionary<string, int?>();
            Mapping = new DataMapping(data);
            var fullDataMapping = new DataMapping(fullData);
            var dataMapping = new DataMapping(data);
            Mapping = dataMapping;
            var labelings = data.Select(d => new Labeling(d.WorkerId, d.TaskId, d.WorkerLabel.ToString(), d.GoldLabel.ToString())).ToList();
            DawidSkene ds = new DawidSkene(labelings, null, null);

            // The labels may be in a different order from our data labeling - we need to create a map.
            int[] labelIndexMap = new int[Mapping.LabelCount];
            var dwLabels = ds.classes.Keys.ToArray();
            for (int i = 0; i < Mapping.LabelCount; i++)
            {
                labelIndexMap[i] = Array.IndexOf(dwLabels, (i + Mapping.LabelMin).ToString());
            }

            GoldLabels = fullDataMapping.GetGoldLabelsPerTaskId().
                ToDictionary(kvp => kvp.Key, kvp => kvp.Value == null ? (int?)null : (int?)labelIndexMap[kvp.Value.Value]);

            ds.Estimate(10);

            var inferredLabels = ds.GetObjectClassProbabilities().Select(r => new Discrete(r)).ToArray();
            TrueLabel = inferredLabels.Select((lab, i) => new
            {
                key = Mapping.TaskIndexToId[i],
                val = lab
            }).ToDictionary(a => a.key, a => a.val);

            if (calculateAccuracy)
            {
                UpdateAccuracy();
            }

            return this;
        }

        public enum RunMode
        {
            ClearResults,
            BatchTraining,
            IncrementalExperiment,
            OnlineExperiment,
            LookAheadExperiment,
            LoadAndUseCommunityPriors,
            Prediction,
            //OnlineProduction,
            //LookAheadProduction,
        };

        [Serializable]
        public struct NonTaskWorkerParameters
        {
            public Dirichlet BackgroundLabelProb;
            public Dirichlet CommunityProb;
            public VectorGaussian[][] CommunityScoreMatrix;
        }
        

        public void RunBCC(string modelName,
            IList<Datum> data,
            IList<Datum> fullData,
            BCC model,
            RunMode mode,
            bool calculateAccuracy,
            int numCommunities = -1,
            bool serialize = false,
            bool serializeCommunityPosteriors = false)
        {
            CBCC communityModel = model as CBCC;
            IsCommunityModel = communityModel != null;


            bool IsBCC = !(IsCommunityModel || IsTimeModel || IsTimeMultimodeModel || IsTimeTaskPropensityModel);

            if (this.Mapping == null)
            {
                this.Mapping = new DataMapping(fullData, numCommunities);
                this.GoldLabels = this.Mapping.GetGoldLabelsPerTaskId();
            }
                
            bool createModel = (Mapping.LabelCount != model.LabelCount) || (Mapping.TaskCount != model.TaskCount);

            if (IsCommunityModel)
            {
                Console.WriteLine("--- CBCC ---");
                CommunityCount = numCommunities;
                createModel = createModel || (numCommunities != communityModel.CommunityCount);

                if (createModel)
                {
                    communityModel.CreateModel(Mapping.TaskCount, Mapping.LabelCount, numCommunities);
                }
            }
            else if (createModel)
            {

                model.CreateModel(Mapping.TaskCount, Mapping.LabelCount);
            }

            BCCPosteriors priors = null;
            switch (mode)
            {
                case RunMode.IncrementalExperiment:
                case RunMode.LookAheadExperiment:
                case RunMode.OnlineExperiment:
                case RunMode.Prediction:
                    priors = ToPriors();
                    break;
                default:
                    ClearResults();

                    if (mode == RunMode.LoadAndUseCommunityPriors && IsCommunityModel)
                    {
                        priors = DeserializeCommunityPosteriors(modelName, numCommunities);
                    }
                    break;
            }

            // Get data structures
            int[][] taskIndices = Mapping.GetTaskIndicesPerWorkerIndex(data);
            int[][] workerLabels = Mapping.GetLabelsPerWorkerIndex(data);
            double[][] workerTimeSpent = (IsTimeModel || IsTimeMultimodeModel || IsTimeTaskPropensityModel) ? Mapping.GetTimeSpentPerWorkerIndex(data) : null;

            if (mode == RunMode.Prediction)
            {
                // Signal prediction mode by setting all labels to null
                workerLabels = workerLabels.Select(arr => (int[])null).ToArray();
            }

            // Call inference
            BCCPosteriors posteriors = null;
            if (IsBCC)
            {
                Console.WriteLine("--- BCC ---");
                posteriors = model.Infer(
                    taskIndices,
                    workerLabels,
                    priors);
            }

            UpdateResults(posteriors, mode);

            if (calculateAccuracy)
            {
                UpdateAccuracy();
            }
            
            if (serialize)
            {
                using (FileStream stream = new FileStream(modelName + ".xml", FileMode.Create))
                {
                    var serializer = new System.Xml.Serialization.XmlSerializer(IsCommunityModel ? typeof(CBCCPosteriors) : typeof(BCCPosteriors));
                    serializer.Serialize(stream, posteriors);
                }
            }

            if (serializeCommunityPosteriors && IsCommunityModel)
            {
                SerializeCommunityPosteriors(modelName);
            }
        }



        void SerializeCommunityPosteriors(string modelName)
        {
            NonTaskWorkerParameters ntwp = new NonTaskWorkerParameters();
            ntwp.BackgroundLabelProb = BackgroundLabelProb;
            ntwp.CommunityProb = CommunityProb;
            ntwp.CommunityScoreMatrix = CommunityScoreMatrix;
            using (FileStream stream = new FileStream(modelName + "CommunityPriors.xml", FileMode.Create))
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(NonTaskWorkerParameters));
                serializer.Serialize(stream, ntwp);
            }
        }

        CBCCPosteriors DeserializeCommunityPosteriors(string modelName, int numCommunities)
        {
            CBCCPosteriors cbccPriors = new CBCCPosteriors();
            using (FileStream stream = new FileStream(modelName + "CommunityPriors.xml", FileMode.Open))
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(NonTaskWorkerParameters));
                var ntwp = (NonTaskWorkerParameters)serializer.Deserialize(stream);

                if (ntwp.BackgroundLabelProb.Dimension != Mapping.LabelCount)
                {
                    throw new ApplicationException("Unexpected number of labels");
                }

                BackgroundLabelProb = ntwp.BackgroundLabelProb;
                cbccPriors.BackgroundLabelProb = ntwp.BackgroundLabelProb;
                if (ntwp.CommunityScoreMatrix.Length != numCommunities)
                {
                    throw new ApplicationException("Unexpected number of communities");
                }

                if (ntwp.CommunityScoreMatrix[0][0].Dimension != Mapping.LabelCount)
                {
                    throw new ApplicationException("Unexpected number of labels");
                }

                CommunityScoreMatrix = ntwp.CommunityScoreMatrix;
                cbccPriors.CommunityScoreMatrix = ntwp.CommunityScoreMatrix;

                if (ntwp.CommunityProb.Dimension != numCommunities)
                {
                    throw new ApplicationException("Unexpected number of communities");
                }

                CommunityProb = ntwp.CommunityProb;
                cbccPriors.CommunityProb = ntwp.CommunityProb;
            }

            return cbccPriors;
        }

        protected virtual void ClearResults()
        {
            BackgroundLabelProb = Dirichlet.Uniform(Mapping.LabelCount);
            WorkerConfusionMatrix = new Dictionary<string, Dirichlet[]>();
            WorkerPrediction = new Dictionary<string, Dictionary<String, Discrete>>();
            WorkerCommunity = new Dictionary<string, Discrete>();
            TrueLabel = new Dictionary<string, Discrete>();
            TrueLabelConstraint = new Dictionary<string,Discrete>();
            CommunityConfusionMatrix = null;
            WorkerScoreMatrixConstraint = new Dictionary<string,VectorGaussian[]>();
            CommunityProb = null;
            CommunityScoreMatrix = null;
            CommunityConstraint = new Dictionary<string, Discrete>();
            LookAheadTrueLabel = new Dictionary<string, Discrete>();
            LookAheadWorkerConfusionMatrix = new Dictionary<string, Dirichlet[]>();
            ModelEvidence = new Bernoulli(0.5);
            PredictedLabel = new Dictionary<string, int?>();
        }

        protected virtual void UpdateResults(BCCPosteriors posteriors, RunMode mode)
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
                
                this.ModelEvidence = posteriors.Evidence;
            }
        }

        BCCPosteriors ToPriors()
        {
            int numClasses = Mapping.LabelCount;
            int numTasks = Mapping.TaskCount;
            int numWorkers = Mapping.WorkerCount;
            CBCCPosteriors cbccPriors = new CBCCPosteriors();
            BCCPosteriors priors = IsCommunityModel ? cbccPriors : new BCCPosteriors();
            priors.BackgroundLabelProb = BackgroundLabelProb;
            priors.WorkerConfusionMatrix = Util.ArrayInit(numWorkers, 
                w => 
                    {
                        string wid = Mapping.WorkerIndexToId[w];
                        if (WorkerConfusionMatrix.ContainsKey(wid))
                            return Util.ArrayInit(numClasses, c => WorkerConfusionMatrix[wid][c]);
                        else
                            return Util.ArrayInit(numClasses, c => Dirichlet.Uniform(numClasses));
                    });

            priors.TrueLabelConstraint = Util.ArrayInit(numTasks,
                t => 
                    {
                        string tid = Mapping.TaskIndexToId[t];
                        if (TrueLabelConstraint.ContainsKey(tid))
                            return TrueLabelConstraint[Mapping.TaskIndexToId[t]];
                        else
                            return Discrete.Uniform(numClasses);
                    });

            if (IsCommunityModel)
            {
                cbccPriors.CommunityConfusionMatrix = CommunityConfusionMatrix;
                cbccPriors.WorkerScoreMatrixConstraint = Util.ArrayInit(numWorkers,
                w =>
                {
                    string wid = Mapping.WorkerIndexToId[w];
                    if (WorkerScoreMatrixConstraint.ContainsKey(wid))
                        return Util.ArrayInit(numClasses, c => WorkerScoreMatrixConstraint[wid][c]);
                    else
                        return Util.ArrayInit(numClasses, c => VectorGaussian.Uniform(numClasses));
                });
                cbccPriors.CommunityProb = CommunityProb;
                cbccPriors.CommunityScoreMatrix = CommunityScoreMatrix;
                cbccPriors.WorkerCommunityConstraint = Util.ArrayInit(numWorkers,
                w =>
                {
                    string wid = Mapping.WorkerIndexToId[w];
                    if (CommunityConstraint.ContainsKey(wid))
                        return CommunityConstraint[wid];
                    else
                        return Discrete.Uniform(CommunityCount);
                });
            }

            priors.Evidence = ModelEvidence;
            
            return priors;
        }
        /// <summary>
        /// Updates the accuracy using the current results.
        /// </summary>
        protected virtual void UpdateAccuracy()
        {
            double nlpdThreshold = -Math.Log(0.001);
            int labelCount = TrueLabel.Where(kvp => kvp.Value != null).First().Value.Dimension;
            var confusionMatrix = Util.ArrayInit(labelCount, labelCount, (i, j) => 0.0);
            int correct = 0;
            double logProb = 0.0;

            int goldX = 0;

            List<double> trueBinaryLabelList = null;
            List<double> probTrueLabelList = null;

            // Only for binary labels
            if (Mapping.LabelCount == 2) 
            {
                trueBinaryLabelList = new List<double>();
                probTrueLabelList = new List<double>();
            }

            foreach (var kvp in GoldLabels)
            {
                if (kvp.Value == null)
                    continue;

                // We have a gold label
                goldX++;

                Discrete trueLabel = null;
                if (TrueLabel.ContainsKey(kvp.Key))
                    trueLabel = TrueLabel[kvp.Key];

                if (trueLabel == null)
                {
                    trueLabel = Discrete.Uniform(Mapping.LabelCount);
                    //continue;  // No inferred label
                }

                var probs = trueLabel.GetProbs();
                double max = probs.Max();
                var predictedLabels = probs.Select((p, i) => new
                {
                    prob = p,
                    idx = i
                }).Where(a => a.prob == max).Select(a => a.idx).ToArray();

                int predictedLabel = predictedLabels.Length == 1 ? predictedLabels[0] : predictedLabels[Rand.Int(predictedLabels.Length)];

                this.PredictedLabel[kvp.Key] = predictedLabel;

                int goldLabel = kvp.Value.Value;

                if (goldLabel == predictedLabel)
                    correct++;

                confusionMatrix[goldLabel, predictedLabel] = confusionMatrix[goldLabel, predictedLabel] + 1.0;

                var nlp = -trueLabel.GetLogProb(goldLabel);
                if (nlp > nlpdThreshold)
                    nlp = nlpdThreshold;
                logProb += nlp;

                if (trueBinaryLabelList != null)
                {
                    trueBinaryLabelList.Add(goldLabel);
                    probTrueLabelList.Add(probs[goldLabel]);
                }
            }

            Accuracy = correct / (double)goldX;
            NegativeLogProb = logProb / (double)goldX;
            ModelConfusionMatrix = confusionMatrix;

            // Average recall
            double sumRec = 0;
            for (int i = 0; i < labelCount; i++)
            {
                double classSum = 0;
                for (int j = 0; j < labelCount; j++)
                {
                    classSum += confusionMatrix[i, j];
                }

                sumRec += confusionMatrix[i, i] / classSum;
            }
            AvgRecall = sumRec / labelCount;

            // WorkerLabelAccuracy: Perc. agreement between worker label and gold label
            int sumAcc = 0;
            var LabelSet = Mapping.DataWithGold;
            int numLabels = LabelSet.Count();
            foreach (var datum in LabelSet)
            {
                sumAcc += datum.WorkerLabel == datum.GoldLabel ? 1 : 0;
            }
            WorkerLabelAccuracy = (double) sumAcc / (double) numLabels;

            if (trueBinaryLabelList != null)
            {
                RocCurve = new ReceiverOperatingCharacteristic(trueBinaryLabelList.ToArray(), probTrueLabelList.ToArray());
                RocCurve.Compute(10000);
                BynaryConfusionMatrix = new ConfusionMatrix((int)confusionMatrix[1, 1], (int)confusionMatrix[0, 0], (int)confusionMatrix[0, 1], (int)confusionMatrix[1, 0]);
            }
        }
        
        public void WriteBasicStatistics(StreamWriter writer)
        {
            int numHITs = Mapping.TaskCount;
            int numJudgments = Mapping.LabelCount;
            int numJudges = Mapping.WorkerCount;
            var goldLabels = Mapping.GetGoldLabelsPerTaskIndex();
            int numGoldHITs = goldLabels.Where(lab => lab != null).Count();
            double avgJ = ((double)numJudgments) / ((double)numJudges);
            double avgH = ((double)numJudgments) / ((double)numHITs);

            writer.WriteLine("HITs\t{0}", numHITs);
            writer.WriteLine("Judgments\t{0}", numJudgments);
            writer.WriteLine("Judges\t{0}", numJudges);
            writer.WriteLine("Avg-J\t{0:0.00}", avgJ);
            writer.WriteLine("Avg-H\t{0:0.00}", avgH);
            writer.WriteLine("Gold HITs\t{0}", numGoldHITs);
        }


        public static void WriteConfusionMatrix(StreamWriter writer, string worker, Dirichlet[] confusionMatrix)
        {
            int labelCount = confusionMatrix.Length;
            var meanConfusionMatrix = confusionMatrix.Select(cm => cm.GetMean()).ToArray();
            var printableConfusionMatrix = Util.ArrayInit(labelCount, labelCount, (i, j) => meanConfusionMatrix[i][j]);
            WriteWorkerConfusionMatrix(writer, worker, printableConfusionMatrix);
        }
        
        public static void WriteWorkerConfusionMatrix(StreamWriter writer, string worker, double[,] confusionMatrix)
        {
            int labelCount = confusionMatrix.GetLength(0);
            writer.WriteLine(worker);
            for (int j = 0; j < labelCount; j++)
                writer.Write(",{0}", j);
            writer.WriteLine();

            for (int i = 0; i < labelCount; i++)
            {
                writer.Write(i);
                for (int j = 0; j < labelCount; j++)
                    writer.Write(",{0:0.0000}", confusionMatrix[i, j]);

                writer.WriteLine();
            }
        }

        public static void WriteWorkerConfusionMatrix(StreamWriter writer, string worker, Vector[] confusionMatrix)
        {
            int labelCount = confusionMatrix.Length;
            writer.WriteLine(worker);
            for (int j = 0; j < labelCount; j++)
                writer.Write(",{0}", j);
            writer.WriteLine();

            for (int i = 0; i < labelCount; i++)
            {
                writer.Write(i);
                for (int j = 0; j < labelCount; j++)
                    writer.Write(",{0:0.0000}", confusionMatrix[i][j]);

                writer.WriteLine();
            }
        }

        public virtual void WriteResults(StreamWriter writer, bool writeCommunityParameters, bool writeWorkerParameters, bool writeWorkerCommunities)
        {
            if (writeCommunityParameters && this.CommunityConfusionMatrix != null)
            {
                for (int communityIndex = 0; communityIndex < this.CommunityConfusionMatrix.Length; communityIndex++)
                {
                    WriteConfusionMatrix(writer, "Community" + communityIndex, this.CommunityConfusionMatrix[communityIndex]);
                }
            }

            if (writeWorkerParameters && this.WorkerConfusionMatrix != null)
            {
                foreach (var kvp in this.WorkerConfusionMatrix)
                {
                    WriteConfusionMatrix(writer, kvp.Key, kvp.Value);
                }
            }

            if (writeWorkerCommunities && this.WorkerCommunity != null)
            {
                foreach (var kvp in this.WorkerCommunity)
                {
                    writer.WriteLine(string.Format("{0}:\t{1}", kvp.Key, kvp.Value));
                }
            }

        }
 
        public virtual void WriteAccuracy(StreamWriter writer)
        {
            WriteBasicStatistics(writer);
            if (Mapping.LabelCount == 2)
            {
                writer.WriteLine("True positive = {0:0.000}", BynaryConfusionMatrix.TruePositives);
                writer.WriteLine("True negative = {0:0.000}", BynaryConfusionMatrix.TrueNegatives);
                writer.WriteLine("False positive = {0:0.000}", BynaryConfusionMatrix.FalsePositives);
                writer.WriteLine("False negative = {0:0.000}", BynaryConfusionMatrix.FalseNegatives);

                writer.WriteLine("Precision = {0:0.000}", BynaryConfusionMatrix.PositivePredictiveValue);
                writer.WriteLine("Recall = {0:0.000}", BynaryConfusionMatrix.Sensitivity);
                writer.WriteLine("Accuracy = {0:0.000}", BynaryConfusionMatrix.Accuracy);
                writer.WriteLine("AUC = {0:0.000}", RocCurve.Area);

                WriteWorkerConfusionMatrix(writer, "Model confusion matrix", this.ModelConfusionMatrix);
            }
            else
            {
                writer.WriteLine("Accuracy = {0:0.000}", this.Accuracy);
                writer.WriteLine("Worker label accuracy = {0:0.00000}", this.WorkerLabelAccuracy);
                writer.WriteLine("Average recall = {0:0.000}", this.AvgRecall);
                writer.WriteLine("Mean negative log prob density = {0:0.000}", this.NegativeLogProb);
                WriteWorkerConfusionMatrix(writer, "Model confusion matrix", this.ModelConfusionMatrix);
                writer.WriteLine("Log Evidence = {0:0.000}", this.ModelEvidence.LogOdds);
            }

        }

        public static string GetModelName(string dataset, RunType runType, TaskSelectionMethod taskSelectionMetric, WorkerSelectionMethod workerSelectionMetric, bool online, int taskSamples = -1, int workerSamples = -1, int numCommunities = -1)
        {
            return dataset + "_" + Enum.GetName(typeof(RunType), runType)
                + "_" + Enum.GetName(typeof(TaskSelectionMethod), taskSelectionMetric)
                + "_" + Enum.GetName(typeof(WorkerSelectionMethod), workerSelectionMetric)
                + (online ? "Online" : "") + (taskSamples > 0 ? "_T" + taskSamples.ToString() : "")
                + (workerSamples > 0 ? "_W" + workerSamples.ToString() : "")
                + (numCommunities > 0 ? "_Comm" + numCommunities.ToString() : "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workerId">if the workerId is null and communityIndex > -1, return the community confusion matrix; 
        ///                             otherwise return the worker confusion matrix</param>

        /// <param name="communityIndex"></param>
        /// <returns></returns>
        public double[,] GetConfusionMatrices(string workerId = "", int communityIndex = -1)
        {
            Dirichlet[] confusionMatrix;
            bool isWorker = true;
            if (communityIndex != -1)
            {
                isWorker = false;
            }

            if (isWorker)
            {
                //wait until the matrix is ready

                while (WorkerConfusionMatrix.Count == 0)
                {

                }

                //check if the workerConfusionMatrix is ready
                bool isKeyExisted = this.WorkerConfusionMatrix.ContainsKey(workerId);
                while (!isKeyExisted)
                {
                    try
                    {
                        isKeyExisted = this.WorkerConfusionMatrix.ContainsKey(workerId);
                    }
                    catch (Exception)
                    {
                    }
                }

                confusionMatrix = this.WorkerConfusionMatrix[workerId];

            }
            else //community confusion matrix
            {
                while (CommunityConfusionMatrix == null || CommunityConfusionMatrix.Length < communityIndex)
                {

                }
                confusionMatrix = CommunityConfusionMatrix[communityIndex];

            }


            int labelCount = confusionMatrix.Length;
            var meanConfusionMatrix = confusionMatrix.Select(cm => cm.GetMean()).ToArray();
            double[,] printableConfusionMatrix = Util.ArrayInit(labelCount, labelCount, (i, j) => meanConfusionMatrix[i][j]);
            return printableConfusionMatrix;

        }//end getConfusionMatrices

        /// <summary>
        /// Get True Label values of the given taskId
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public Discrete getTaskTrueLabel(string taskId)
        {
            while (TrueLabel.Count == 0)
            {

            }
            bool isKeyExisted = false;
            //wait until the key is existed
            while (!isKeyExisted)
            {
                try
                {
                    //check if the workerConfusionMatrix is ready
                    isKeyExisted = this.TrueLabel.ContainsKey(taskId);
                }
                catch (Exception)
                {
                }
            }
            return this.TrueLabel[taskId];

        } //End GetTaskTrueLabel
    }
}
