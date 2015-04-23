using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using MicrosoftResearch.Infer;
using MicrosoftResearch.Infer.Factors;
using MicrosoftResearch.Infer.Distributions;
using MicrosoftResearch.Infer.Maths;
using MicrosoftResearch.Infer.Models;
using MicrosoftResearch.Infer.Utils;

namespace CrowdsourcingModels
{
    class HCOMP15_ActiveCrowdExperimentExperiment
    {

        public const string dataDirectory = "Data/";

        public static int endClusterRun = 100;

        public static int startClusterRun = 1;

        public static string[] Datasets = new string[] { "CF", "MS", "SP" };


        /// <summary>
        /// The path to the results directory.
        /// </summary>
        public static string ResultsPath = @"ResultsActiveCrowdToolkit/";

        /// <summary>
        /// The path to the results directory including the cluster run (e.g., Results/Run1).
        /// </summary>
        public static string ResultsDir = "";

        /// <summary>
        /// Method for the HCOMP15-ActiveCrowd experiment
        /// </summary>
        public static void Run(string[] args)
        {

            //Directory.Delete(ResultsPath, true);

            if (args.Length>1)
            {
                startClusterRun = int.Parse(args[3]);
                endClusterRun = int.Parse(args[4]);
                Datasets = new string[] { args[0] }; 
            }

            // Param2: task selection method
            TaskSelectionMethod whichTaskSelection = 0;
            if (args.Length > 3)
            {
                switch (args[3])
                {
                    case "ET": whichTaskSelection = TaskSelectionMethod.EntropyTask; break;
                    case "UT": whichTaskSelection = TaskSelectionMethod.UniformTask; break;
                    case "RT": whichTaskSelection = TaskSelectionMethod.RandomTask; break;
                }
            }

            // Param2: task selection method
            WorkerSelectionMethod whichWorkerSelection = 0;
            if (args.Length > 3)
            {
                switch (args[3])
                {
                    case "RW": whichWorkerSelection = WorkerSelectionMethod.RandomWorker; break;
                    case "BW": whichWorkerSelection = WorkerSelectionMethod.BestWorker; break;
                }
            }

            // Experiment
            RunHCOMPExperiments(0, 0, whichTaskSelection, whichWorkerSelection, 1);

            // Aggregate results
            //AggregateResults("CF");

        }

        /// <summary>
        /// Runs the active learning experiment on a single data set.
        /// </summary>
        /// <param name="dataSet">The data.</param>
        /// <param name="runType">The model run type.</param>
        /// <param name="taskSelectionMethod">The method for selecting tasks (Random / Entropy).</param>
        /// <param name="model">The model instance.</param>
        /// <param name="communityCount">The number of communities (only for CBCC).</param>
        public static void RunHCOMPActiveLearning(string dataSet, RunType runType, TaskSelectionMethod taskSelectionMethod, WorkerSelectionMethod workerSelectionMethod, int InitialNumLabelsPerTask, BCC model, int communityCount = 4)
        {
            var data = Datum.LoadData(dataDirectory + dataSet + ".csv");
            string modelName = Program.GetModelName(dataSet, runType, taskSelectionMethod, workerSelectionMethod, communityCount);
            ActiveLearning.RunActiveLearning(data, modelName, runType, model, taskSelectionMethod, workerSelectionMethod, ResultsDir, communityCount, InitialNumLabelsPerTask);
        }

        /// <summary>
        /// Runs the active learning experiment presented in Venanzi et.al (WWW14)
        /// for all the models with an array of data sets.
        /// </summary>
        /// <param name="startIndex">First instance of the data set array.</param>
        /// <param name="endIndex">Last instance of the data set array.</param>
        /// <param name="whichModel">Model to run.</param>
        public static void RunHCOMPExperiments(int startIndex, int endIndex, TaskSelectionMethod TaskSelectionMethod, WorkerSelectionMethod WorkerSelectionMethod, int InitialNumLabelsPerTask)
        {
            for (int run = startClusterRun; run <= endClusterRun; run++)
            {
                // Create run directory
                ResultsDir = String.Format(ResultsPath + "Run{0}/", run);
                Directory.CreateDirectory(ResultsDir);

                // Reset the random seed with run index to aid reproducibility
                Rand.Restart(run);

                Console.WriteLine("\nCluster run {0} / {1}", run, endClusterRun);
                for (int ds = startIndex; ds <= endIndex; ds++)
                {
                    RunHCOMPActiveLearning(Datasets[ds], RunType.VoteDistribution, TaskSelectionMethod, WorkerSelectionMethod, InitialNumLabelsPerTask, null);
                    RunHCOMPActiveLearning(Datasets[ds], RunType.MajorityVote, TaskSelectionMethod, WorkerSelectionMethod, InitialNumLabelsPerTask, null);
                    //RunHCOMPActiveLearning(Datasets[ds], RunType.DawidSkene, TaskSelectionMethod, WorkerSelectionMethod, InitialNumLabelsPerTask, null);
                    RunHCOMPActiveLearning(Datasets[ds], RunType.BCC, TaskSelectionMethod, WorkerSelectionMethod, InitialNumLabelsPerTask, new BCC());
                    RunHCOMPActiveLearning(Datasets[ds], RunType.CBCC, TaskSelectionMethod, WorkerSelectionMethod, InitialNumLabelsPerTask, new CBCC(), Program.NumCommunities[ds]);
                }
            }
        }

        public static void AggregateResults(string dataset)
        {
            ///
            /// The results matrix is a dictionary with 1-level key = filename, 2-level key is the accuracy name, 3-level key is the labelling round and the value is the array of accuracies at that round
            /// 
            var resultsMatrix = new Dictionary<string, Dictionary<string, List<List<double>>>>();
            string[] Headers = null;
            for (int run = 1; run <= endClusterRun; run++)
            {
                string resultsDir = String.Format(ResultsPath + "Run{0}/", run);
                var graphFiles = new DirectoryInfo(resultsDir).GetFiles("*graph*");

                foreach (var file in graphFiles)
                {
                    string filename = file.Name;
                    if (!resultsMatrix.ContainsKey(filename))
                        resultsMatrix[filename] = new Dictionary<string, List<List<double>>>();

                    
                    using (var reader = new StreamReader(file.FullName))
                    {

                        string line = reader.ReadLine();
                        if (Headers == null)
                            Headers = line.Split(',');

                        if (!resultsMatrix[filename].ContainsKey(Headers[0]))
                            resultsMatrix[filename] = Headers.ToDictionary(k => k, v => new List<List<double>>());

                        int lineNumber = 0;
                        while ((line = reader.ReadLine()) != null)
                        {

                            var strarr = line.Split(',');
                            int length = strarr.Length;

                            if (Headers.Length != strarr.Length)
                                throw new ApplicationException("The number of headers is different from the number of results");

                            for (int j = 0; j < strarr.Length; j++)
                            {
                                if (resultsMatrix[filename][Headers[j]].Count < (lineNumber+1))
                                    resultsMatrix[filename][Headers[j]].Add(new List<double>());

                                resultsMatrix[filename][Headers[j]][lineNumber].Add(double.Parse(strarr[j]));
                            }
                            lineNumber++;
                        }
                    }
                }
            }

            ///
            /// Compute averages
            /// 
            foreach(var kvp in resultsMatrix)
            {
                string method = kvp.Key;
                var aggregatedMeanAccuracies = kvp.Value.ToDictionary(k => k.Key, v => v.Value.Select(r => r.Average()).ToArray());
                var aggregatedStdAccuracies = kvp.Value.ToDictionary(k => k.Key, v => v.Value.Select(r => getStandardDeviation(r)).ToArray());
                string[] metrics = aggregatedMeanAccuracies.Keys.ToArray();
                int numRows = aggregatedMeanAccuracies[metrics[0]].Length;

                using (StreamWriter writer = new StreamWriter(ResultsPath+"aggregated_"+method))
                {
                    var headers = kvp.Value.Keys.Select(h => String.Format("{0},{0}_std,", h));
                    writer.WriteLine(string.Join(",", headers));

                    for (int r = 0; r < numRows; r++)
                    {
                        List<string> line = new List<string>();
                        for (int m = 0; m < metrics.Length; m++)
                        {
                            line.Add(String.Format("{0:0.0000},{1:0.0000}", aggregatedMeanAccuracies[metrics[m]][r], aggregatedStdAccuracies[metrics[m]][r]));
                        }
                        writer.WriteLine(string.Join(",", line));
                    }
                }
            }
        }

        /// <summary>
        /// Compute the standard deviation for a list of doubles
        /// </summary>
        /// <param name="doubleList">The list of doubles</param>
        /// <returns>The standard deviation</returns>
        private static double getStandardDeviation(List<double> doubleList)
        {
            double ret = 0;
            if (doubleList.Count() > 0)
            {
                //Compute the Average      
                double avg = doubleList.Average();
                //Perform the Sum of (value-avg)_2_2      
                double sum = doubleList.Sum(d => Math.Pow(d - avg, 2));
                //Put it all together      
                ret = Math.Sqrt((sum) / (doubleList.Count() - 1));
            }
            return ret;
        }
    }
}
