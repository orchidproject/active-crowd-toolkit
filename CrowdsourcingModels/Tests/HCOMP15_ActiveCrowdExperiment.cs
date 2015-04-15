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

        public static string[] GoldDatasets = new string[] { "CF.csv", "MS.csv", "SP.csv" };


        /// <summary>
        /// The results directory.
        /// </summary>
        public static string ResultsDir = @"ResultsActiveCrowdToolkit/";

        /// <summary>
        /// Method for the HCOMP15-ActiveCrowd experiment
        /// </summary>
        public static void Run()
        {

            // Reset the random seed so results can be duplicated for the paper
            Rand.Restart(12347);
            Directory.CreateDirectory(ResultsDir);

            // Experiment
            RunHCOMPExperiments(0, 0, -1, TaskSelectionMethod.RandomTask, 1);

        }


        /// <summary>
        /// Runs the active learning experiment presented in Venanzi et.al (WWW14) on a single data set.
        /// </summary>
        /// <param name="dataSet">The data.</param>
        /// <param name="runType">The model run type.</param>
        /// <param name="taskSelectionMethod">The method for selecting tasks (Random / Entropy).</param>
        /// <param name="model">The model instance.</param>
        /// <param name="communityCount">The number of communities (only for CBCC).</param>
        public static void RunHCOMPActiveLearning(string dataSet, RunType runType, TaskSelectionMethod taskSelectionMethod, int InitialNumLabelsPerTask, BCC model, int communityCount = 4, double lipschitzConstant = 1)
        {
            var data = Datum.LoadData(@"Data/" + dataSet + ".csv");
            string modelName = Program.GetModelName(dataSet, runType, taskSelectionMethod, WorkerSelectionMethod.RandomWorker, communityCount, lipschitzConstant);

            //initial Number of Label Per Task
            //int initialNumLabelsPerTask = 1;
            int initialNumLabelsPerTask = InitialNumLabelsPerTask;
            ActiveLearning.RunActiveLearning(data, modelName, runType, model, taskSelectionMethod, WorkerSelectionMethod.RandomWorker, ResultsDir, communityCount, initialNumLabelsPerTask, lipschitzConstant: lipschitzConstant);
        }

        /// <summary>
        /// Runs the active learning experiment presented in Venanzi et.al (WWW14)
        /// for all the models with an array of data sets.
        /// </summary>
        /// <param name="startIndex">First instance of the data set array.</param>
        /// <param name="endIndex">Last instance of the data set array.</param>
        /// <param name="whichModel">Model to run.</param>
        public static void RunHCOMPExperiments(int startIndex, int endIndex, int whichModel, TaskSelectionMethod currentTaskSelectionMethod, int InitialNumLabelsPerTask, double lipschitzConstant = 1)
        {
            //Select current task selection method(Entropy/Random) 
            //TaskSelectionMethod currentTaskSelectionMethod = TaskSelectionMethod.EntropyTask;

            for (int ds = startIndex; ds <= endIndex; ds++)
            {
                switch (whichModel)
                {
                    case 1: RunHCOMPActiveLearning(Program.GoldDatasets[ds], RunType.MajorityVote, currentTaskSelectionMethod, InitialNumLabelsPerTask, null); break;
                    case 2: RunHCOMPActiveLearning(Program.GoldDatasets[ds], RunType.DawidSkene, currentTaskSelectionMethod, InitialNumLabelsPerTask, null); break;
                    case 3: RunHCOMPActiveLearning(Program.GoldDatasets[ds], RunType.BCC, currentTaskSelectionMethod, InitialNumLabelsPerTask, new BCC(), lipschitzConstant: lipschitzConstant); break;
                    case 4: RunHCOMPActiveLearning(Program.GoldDatasets[ds], RunType.CBCC, currentTaskSelectionMethod, InitialNumLabelsPerTask, new CBCC(), Program.NumCommunities[ds], lipschitzConstant: lipschitzConstant); break;
                    default: // Run all
                        RunHCOMPActiveLearning(Program.GoldDatasets[ds], RunType.MajorityVote, currentTaskSelectionMethod, InitialNumLabelsPerTask, null);
                        RunHCOMPActiveLearning(Program.GoldDatasets[ds], RunType.DawidSkene, currentTaskSelectionMethod, InitialNumLabelsPerTask, null);
                        RunHCOMPActiveLearning(Program.GoldDatasets[ds], RunType.BCC, currentTaskSelectionMethod, InitialNumLabelsPerTask, new BCC(), lipschitzConstant: lipschitzConstant);
                        RunHCOMPActiveLearning(Program.GoldDatasets[ds], RunType.CBCC, currentTaskSelectionMethod, InitialNumLabelsPerTask, new CBCC(), Program.NumCommunities[ds], lipschitzConstant: lipschitzConstant);
                        RunHCOMPActiveLearning(Program.GoldDatasets[ds], RunType.BCC, currentTaskSelectionMethod, InitialNumLabelsPerTask, new CBCC(), lipschitzConstant: lipschitzConstant);
                        break;
                }
            }
        }
    }
}
