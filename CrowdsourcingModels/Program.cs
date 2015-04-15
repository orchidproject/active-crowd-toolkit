using MicrosoftResearch.Infer;
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
using CrowdsourcingProject.Statistics;


namespace CrowdsourcingModels
{
    /// <summary>
    /// The class for the main program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The datasets.
        /// </summary>
        //public static string[] GoldDatasets = new string[] { "CF_amt", "SP_amt", "SP_amt_short_time"};
        public static string[] GoldDatasets = new string[] { "ZenCrowd_all", "ZenCrowd_us", "ZenCrowd_in" };
        public static bool UseRealData;

        /// <summary>
        /// The number of communities of CBCC.
        /// </summary>
        public static int[] NumCommunities = new int[] { 4 };



        /// <summary>
        /// Flag to redirect the console output to txt file
        /// </summary>
        public static bool isRecordedToTxtFile = false;

        public static int startIndex = 0;
        public static int endIndex = 0;
        public static int whichModel;
        public static int clusterIter;

        /*
        /// <summary>
        /// The initial number labels per task.
        /// </summary>
        public static int projectInitialNumLabelsPerTask = 1;
        */
        /// <summary>
        /// Main method to run the crowdsourcing experiments presented in Venanzi et.al (WWW14).
        /// </summary>
        /// <param name="args[0]">agrs for cluster development</param>
        /// <param name="args[1]">dataset] selected</param>
        /// <param name="args[2]">model selected</param>
        /// <param name="args[3]">value of constant c</param>
        static void Main(string[] args)
        {


            
        }

        public static void RunOnRealData(string dataSet, RunType runType, int numCommunities = 3)
        {
            // Load real data
            UseRealData = true;
            var data = Datum.LoadData(@".\Data\" + dataSet + ".csv");
            int totalLabels = data.Count();
            Console.WriteLine("Original dataset: {1}, {0} labels", totalLabels, dataSet);

            RunGold(dataSet, data, RunType.VoteDistribution, null);
            RunGold(dataSet, data, RunType.MajorityVote, null);
            RunGold(dataSet, data, RunType.BCC, new BCC());
            RunGold(dataSet, data, RunType.DawidSkene, null);

            //RunGold(dataSet, spammerData, RunType.BCC, new BCC());
            //RunGold(dataSet, data, RunType.BCCTime, new BCCTimeSpammer());
            //RunGold(dataSet, data, RunType.BCCTime, new BCCTimeSpammerMultimode());
            RunGold(dataSet, data, RunType.BCCTime, new BCCTimeTaskPropensity());

        }

        /// <summary>
        /// Runs all the models in batch
        /// </summary>
        public static void RunBatch()
        {
            for (int ds = startIndex; ds <= endIndex; ds++)
            {
                var data = Datum.LoadData(@".\Data\" + GoldDatasets[ds] + ".csv");

                //RunGold(GoldDatasets[ds], data, RunType.VoteDistribution, null);
                //RunGold(GoldDatasets[ds], data, RunType.MajorityVote, null);
                //RunGold(GoldDatasets[ds], data, RunType.DawidSkene, null);
                //RunGold(GoldDatasets[ds], data, RunType.BCC, new BCC());
                //RunGold(GoldDatasets[ds], data, RunType.CBCC, new CBCC(), NumCommunities[ds]);
                RunGold(GoldDatasets[ds], data, RunType.BCCTime, new BCCTimeSpammer());
            }
        }

        /// <summary>
        /// Runs a model with the full gold set.
        /// </summary>
        /// <param name="dataSet">The data.</param>
        /// <param name="currentTaskSelectionMethod">Current Task Selection Method</param>
        /// <param name="runType">The model run type.</param>
        /// <param name="model">The model instance.</param>
        /// <param name="numCommunities">The number of communities (only for CBCC).</param>
        /// <returns>The inference results</returns>
        public static Results RunGold(string dataSet, IList<Datum> data, RunType runType, BCC model, int numCommunities = 3)
        {

            string modelName = Program.GetModelName(dataSet, runType);
            ResultsTime results = new ResultsTime();

            switch (runType)
            {
                case RunType.VoteDistribution:
                    results.RunMajorityVote(data, data, true, true);
                    break;
                case RunType.MajorityVote:
                    results.RunMajorityVote(data, data, true, false);
                    break;
                case RunType.DawidSkene:
                    results.RunDawidSkene(data, data, true);
                    break;
                default:
                    results.RunBCC(modelName, data, data, model, Results.RunMode.ClearResults, true, numCommunities, false, false);
                    break;
            }
            //bool isExportedToCSV = true;
            //if (isExportedToCSV)
            //{
            //    using (StreamWriter writer = new StreamWriter(Program.ResultsDir + modelName +  "_endpoints.csv"))
            //    {
            //        writer.WriteLine("{0}", modelName);
            //        results.WriteAccuracy(writer);
            //        results.WriteResults(writer, false, true, false);
            //    }
            //}

            using (StreamWriter writer = new StreamWriter(Console.OpenStandardOutput()))
            {
                results.WriteAccuracy(writer);
                //results.WriteResults(writer, false, false, false);
            }

            
            return results;
        }

         public static Results RunGold(string dataSet, RunType runType, BCC model, int numCommunities = 3)
        {
            var data = Datum.LoadData(@".\Data\" + dataSet + ".csv");
            return RunGold(dataSet, data, runType, model, numCommunities);
        }

        /// <summary>
        /// Returns the model name as a string.
        /// </summary>
        /// <param name="dataset">The name of the data set.</param>
        /// <param name="runType">The model run type.</param>
        /// <param name="taskSelectionMethod">The method for selecting tasks (Random / Entropy).</param>
        /// <param name="workerSelectionMethod">The method for selecting workers (only Random is implemented).</param>
        /// <param name="numCommunities">The number of communities (only for CBCC).</param>
        /// <returns>The model name</returns>
        public static string GetModelName(string dataset, RunType runType, TaskSelectionMethod taskSelectionMethod, WorkerSelectionMethod workerSelectionMethod, int numCommunities = -1, double lipschitzConstant = -1)
        {
            return dataset + "_" + Enum.GetName(typeof(RunType), runType)
                + "_" + (!taskSelectionMethod.Equals("") ? Enum.GetName(typeof(TaskSelectionMethod), taskSelectionMethod) : "")
                + (lipschitzConstant>0 ? lipschitzConstant.ToString().Replace(".", "") : "");
        }

        /// <summary>
        /// Returns the model name as a string.
        /// </summary>
        /// <param name="dataset">The name of the data set.</param>
        /// <param name="runType">The model run type.</param>
        /// <returns>The model name</returns>
        public static string GetModelName(string dataset, RunType runType)
        {
            return dataset + "_" + Enum.GetName(typeof(RunType), runType);
        }


    }

}
