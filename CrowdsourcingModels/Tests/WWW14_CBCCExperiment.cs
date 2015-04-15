using MicrosoftResearch.Infer;
using MicrosoftResearch.Infer.Factors;
using MicrosoftResearch.Infer.Distributions;
using MicrosoftResearch.Infer.Maths;
using MicrosoftResearch.Infer.Models;
using MicrosoftResearch.Infer.Utils;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrowdsourcingModels;

namespace CrowdsourcingModels
{
    public class WWW14_CBCCExperiment
    {

        /// <summary>
        /// The results directory.
        /// </summary>
        public static string ResultsDir = @"ResultsCBCC/";

        /// <summary>
        /// Method for the original WWW14 experiment
        /// </summary>
        public static void OriginalExperiment()
        {

            // Experiment of Figure 5 and Table 2
            RunFullGold(Program.startIndex, Program.endIndex);

            /*
            // Experiment of Figure 4
            RunWWWExperiments(startIndex, endIndex, whichModel);

            // Experiment to find the number of communities
            FindNumCommunities(startIndex, endIndex, 10);
            */

        }

        /// <summary>
        /// Runs all the models on an array of full gold sets.
        /// </summary>
        /// <param name="startIndex">The first index of the gold set array.</param>
        /// <param name="endIndex">The fast index of the gold set array.</param>
        public static void RunFullGold(int startIndex, int endIndex)
        {
            Console.Write("RunFullGolds: Running models");
            for (int ds = startIndex; ds <= endIndex; ds++)
            {
                var data = Datum.LoadData(@".\Data\" + Program.GoldDatasets[ds] + ".csv");
                Program.RunGold(Program.GoldDatasets[ds], data, RunType.MajorityVote, null); Console.Write(".");
                Program.RunGold(Program.GoldDatasets[ds], data, RunType.DawidSkene, null); Console.Write(".");
                Program.RunGold(Program.GoldDatasets[ds], data, RunType.BCC, new BCC()); Console.Write(".");
                //RunGold(GoldDatasets[ds], RunType.CBCC, new CBCC(), numCommunities:NumCommunities[ds]); Console.Write(".");
            }
            Console.Write("done\n");
        }

        /// <summary>
        /// Finds the optimal number of communities
        /// </summary>
        /// <param name="startIndex">The first index of the gold set array.</param>
        /// <param name="endIndex">The fast index of the gold set array.</param>
        /// <param name="communityUpperBound">The maximum number of communities</param>
        /// <param name="currentTaskSelectionMethod">Current Task Selection Method</param>
        /// ///
        static void FindNumCommunities(int startIndex, int endIndex, int communityUpperBound = 10)
        {
            Console.WriteLine("Find community count: Running models");
            var modelEvidence = Util.ArrayInit<double>(communityUpperBound, endIndex + 1, (i, j) => 0.0);
            for (int ds = startIndex; ds <= endIndex; ds++)
            {
                Console.WriteLine("Dataset: " + Program.GoldDatasets[ds]);
                var data = Datum.LoadData(@".\Data\" + Program.GoldDatasets[ds] + ".csv");
                for (int communityCount = 1; communityCount <= communityUpperBound; communityCount++)
                {

                    Results results = Program.RunGold(Program.GoldDatasets[ds], data, RunType.CBCC, new CBCC(), communityCount);
                    modelEvidence[communityCount - 1, ds] = results.ModelEvidence.LogOdds;
                    Console.WriteLine("Community {0}: {1:0.0000}", communityCount, modelEvidence[communityCount - 1, ds]);
                }
            }
        }


        /// <summary>
        /// Runs the active learning experiment presented in Venanzi et.al (WWW14) on a single data set.
        /// </summary>
        /// <param name="dataSet">The data.</param>
        /// <param name="runType">The model run type.</param>
        /// <param name="taskSelectionMethod">The method for selecting tasks (Random / Entropy).</param>
        /// <param name="model">The model instance.</param>
        /// <param name="communityCount">The number of communities (only for CBCC).</param>
        public static void RunWWWActiveLearning(string dataSet, RunType runType, TaskSelectionMethod taskSelectionMethod, int projectInitialNumLabelsPerTask, BCC model, int communityCount = 4, double lipschitzConstant = 1)
        {
            // Reset the random seed so results can be duplicated for the paper
            Rand.Restart(12347);
            var workerSelectionMetric = WorkerSelectionMethod.RandomWorker;
            var data = Datum.LoadData(@"Data/" + dataSet + ".csv");
            string modelName = Program.GetModelName(dataSet, runType, taskSelectionMethod, workerSelectionMetric, communityCount, lipschitzConstant);

            //initial Number of Label Per Task
            //int initialNumLabelsPerTask = 1;
            int initialNumLabelsPerTask = projectInitialNumLabelsPerTask;
            ActiveLearning.RunActiveLearning(data, modelName, runType, model, taskSelectionMethod, workerSelectionMetric, ResultsDir, communityCount, initialNumLabelsPerTask, lipschitzConstant: lipschitzConstant);
        }

        /// <summary>
        /// Runs the active learning experiment presented in Venanzi et.al (WWW14)
        /// for all the models with an array of data sets.
        /// </summary>
        /// <param name="startIndex">First instance of the data set array.</param>
        /// <param name="endIndex">Last instance of the data set array.</param>
        /// <param name="whichModel">Model to run.</param>
        public static void RunWWWExperiments(int startIndex, int endIndex, int whichModel, TaskSelectionMethod currentTaskSelectionMethod, int InitialNumLabelsPerTask, double lipschitzConstant = 1)
        {
            //Select current task selection method(Entropy/Random) 
            //TaskSelectionMethod currentTaskSelectionMethod = TaskSelectionMethod.EntropyTask;

            for (int ds = startIndex; ds <= endIndex; ds++)
            {
                switch (whichModel)
                {
                    case 1: RunWWWActiveLearning(Program.GoldDatasets[ds], RunType.MajorityVote, currentTaskSelectionMethod, InitialNumLabelsPerTask, null); break;
                    case 2: RunWWWActiveLearning(Program.GoldDatasets[ds], RunType.DawidSkene, currentTaskSelectionMethod, InitialNumLabelsPerTask, null); break;
                    case 3: RunWWWActiveLearning(Program.GoldDatasets[ds], RunType.BCC, currentTaskSelectionMethod, InitialNumLabelsPerTask, new BCC(), lipschitzConstant: lipschitzConstant); break;
                    case 4: RunWWWActiveLearning(Program.GoldDatasets[ds], RunType.CBCC, currentTaskSelectionMethod, InitialNumLabelsPerTask, new CBCC(), Program.NumCommunities[ds], lipschitzConstant: lipschitzConstant); break;
                    default: // Run all
                        RunWWWActiveLearning(Program.GoldDatasets[ds], RunType.MajorityVote, currentTaskSelectionMethod, InitialNumLabelsPerTask, null);
                        RunWWWActiveLearning(Program.GoldDatasets[ds], RunType.DawidSkene, currentTaskSelectionMethod, InitialNumLabelsPerTask, null);
                        RunWWWActiveLearning(Program.GoldDatasets[ds], RunType.BCC, currentTaskSelectionMethod, InitialNumLabelsPerTask, new BCC(), lipschitzConstant: lipschitzConstant);
                        RunWWWActiveLearning(Program.GoldDatasets[ds], RunType.CBCC, currentTaskSelectionMethod, InitialNumLabelsPerTask, new CBCC(), Program.NumCommunities[ds], lipschitzConstant: lipschitzConstant);
                        RunWWWActiveLearning(Program.GoldDatasets[ds], RunType.BCC, currentTaskSelectionMethod, InitialNumLabelsPerTask, new CBCC(), lipschitzConstant: lipschitzConstant);
                        break;
                }
            }
        }
    }
}
