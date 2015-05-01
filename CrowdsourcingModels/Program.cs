using System;
using System.Collections.Generic;
using System.IO;

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
        public static string[] Datasets = new string[] { "CF", "ZenCrowd_us", "ZenCrowd_in" };
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

        /// <summary>
        /// Main method to run the crowdsourcing experiments presented in Venanzi et.al (WWW14).
        /// </summary>
        /// <param name="args[0]">agrs for cluster development</param>
        /// <param name="args[1]">dataset] selected</param>
        /// <param name="args[2]">model selected</param>
        /// <param name="args[3]">value of constant c</param>
        static void Main(string[] args)
        {

            HCOMP15_ActiveCrowdExperimentExperiment.Run(args);
            
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
        public static Results RunGold(string dataSet, IList<Datum> data, RunType runType, BCC model, int numCommunities = 2)
        {

            string modelName = Program.GetModelName(dataSet, runType);
            Results results = new Results();

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
                    results.RunBCC(modelName, data, data, model, RunMode.ClearResults, true, numCommunities, false, false);
                    break;
            }
            
            return results;
        }

         public static Results RunGold(string dataSetPath, RunType runType, BCC model, int numCommunities = 3)
        {
            var data = Datum.LoadData(dataSetPath);
            return RunGold(dataSetPath, data, runType, model, numCommunities);
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
        public static string GetModelName(string dataset, RunType runType, TaskSelectionMethod taskSelectionMethod, WorkerSelectionMethod workerSelectionMethod, int NumCommunities)
        {
            return dataset + "_" + Enum.GetName(typeof(RunType), runType)
                + "_" + (!taskSelectionMethod.Equals("") ? Enum.GetName(typeof(TaskSelectionMethod), taskSelectionMethod) : "")
                + "_" + (!workerSelectionMethod.Equals("") ? Enum.GetName(typeof(WorkerSelectionMethod), workerSelectionMethod) : "");
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
