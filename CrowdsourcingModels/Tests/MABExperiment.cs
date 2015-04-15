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
using CrowdsourcingModels;

namespace CrowdsourcinSoton.Tests
{
    class MABExperiment
    {

        /// <summary>
        /// The results directory.
        /// </summary>
        public static string ResultsDir = @"ResultsMAB/";

        public static void MABTest(string[] args)
        {
            // Redirect output streams
            FileStream ostrm = null;
            StreamWriter writer = null;
            TextWriter oldOut = null;
            if (Program.isRecordedToTxtFile)
            {
                oldOut = Console.Out;
                try
                {
                    ostrm = new FileStream("./Redirect.txt", FileMode.OpenOrCreate, FileAccess.Write);
                    writer = new StreamWriter(ostrm);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Cannot open Redirect.txt for writing");
                    Console.WriteLine(e.Message);
                    return;
                }
                Console.SetOut(writer);
                Console.WriteLine("Console output ");
            }

            // Parse args for cluster deployment
            if (args.Length > 0)
                Program.clusterIter = Int16.Parse(args[0]);

            //Select dataset
            if (args.Length > 1)
            {
                switch (args[1])
                {
                    case "CF": Program.startIndex = 0; Program.endIndex = 0; break;
                    case "MS": Program.startIndex = 1; Program.endIndex = 1; break;
                    case "SA": Program.startIndex = 2; Program.endIndex = 2; break;
                    default:
                        Program.startIndex = 0;
                        Program.endIndex = Program.GoldDatasets.Length - 1;
                        break;
                }
            }

            if (args.Length > 2)
            {
                switch (args[2])
                {
                    case "MV": Program.whichModel = 1; break;
                    case "BCC": Program.whichModel = 3; break;
                    case "CBCC": Program.whichModel = 4; break;
                    default: Program.whichModel = Int16.MaxValue; break;
                }
            }


            //

            //set constant C 
            double c = 0.009;
            if (args.Length > 3) {
                c = Double.Parse(args[3]);
            }

            ResultsDir = ResultsDir + "Run" + Program.clusterIter + "//";
            Directory.CreateDirectory(ResultsDir);


           TestParallelActiveLearning("CF", 1);

            ////run simulation according to input parameters
            //RunWWWExperiments(startIndex, endIndex, whichModel, TaskSelectionMethod.EntropyMABTask, 1, c);
            //RunWWWExperiments(startIndex, endIndex, whichModel, TaskSelectionMethod.EntropyTask, 1, c);
            //RunWWWExperiments(startIndex, endIndex, whichModel, TaskSelectionMethod.EntropyTask, 2, c);
            //RunWWWExperiments(startIndex, endIndex, whichModel, TaskSelectionMethod.EntropyTask, 3, c);
            //RunWWWExperiments(startIndex, endIndex, whichModel, TaskSelectionMethod.EntropyTask, 4, c);
            //RunWWWExperiments(startIndex, endIndex, whichModel, TaskSelectionMethod.EntropyTask, 5, c);
            //RunWWWExperiments(startIndex, endIndex, whichModel, TaskSelectionMethod.RandomTask, 1, c);
            //RunWWWExperiments(startIndex, endIndex, whichModel, TaskSelectionMethod.UniformTask, 1, c);



            //code for running multiple results 
            Program.clusterIter = 1;
            Program.whichModel = 3;
            Program.startIndex = 0;
            Program.endIndex = 0;

            for (int i = 16; i <= 18; i++) {

                Program.clusterIter = i;
                ResultsDir = @"Results/";
                ResultsDir = ResultsDir + "Run" + Program.clusterIter + "//";
                Directory.CreateDirectory(ResultsDir);

                //run simulation according to input parameters
                WWW14_CBCCExperiment.RunWWWExperiments(Program.startIndex, Program.endIndex, Program.whichModel, TaskSelectionMethod.EntropyMABTask, 1, c);
                WWW14_CBCCExperiment.RunWWWExperiments(Program.startIndex, Program.endIndex, Program.whichModel, TaskSelectionMethod.EntropyTask, 1, c);
                WWW14_CBCCExperiment.RunWWWExperiments(Program.startIndex, Program.endIndex, Program.whichModel, TaskSelectionMethod.EntropyTask, 2, c);
                WWW14_CBCCExperiment.RunWWWExperiments(Program.startIndex, Program.endIndex, Program.whichModel, TaskSelectionMethod.EntropyTask, 3, c);
                WWW14_CBCCExperiment.RunWWWExperiments(Program.startIndex, Program.endIndex, Program.whichModel, TaskSelectionMethod.EntropyTask, 4, c);
                WWW14_CBCCExperiment.RunWWWExperiments(Program.startIndex, Program.endIndex, Program.whichModel, TaskSelectionMethod.EntropyTask, 5, c);
                WWW14_CBCCExperiment.RunWWWExperiments(Program.startIndex, Program.endIndex, Program.whichModel, TaskSelectionMethod.RandomTask, 1, c);
                WWW14_CBCCExperiment.RunWWWExperiments(Program.startIndex, Program.endIndex, Program.whichModel, TaskSelectionMethod.UniformTask, 1, c);


            }


            ///end for running multiple results

            /// 
            /// Experiment 1: Original experiment on the microsoft blog
            ///

            //OriginalExperiment(); 


            ///
            /// Experiment 2: Test different initial number of labels with all task selection methods
            /// 

            //TestTaskSelectionWithDifferentInitialLabels(TaskSelectionMethod.UniformTask, 1);
            //TestTaskSelectionWithDifferentInitialLabels(TaskSelectionMethod.EntropyTask, 5);
            //TestTaskSelectionWithDifferentInitialLabels(TaskSelectionMethod.RandomTask, 5);
            //TestTaskSelectionWithDifferentInitialLabels(TaskSelectionMethod.EntropyMABTask, 1);  //Entropy MAB Task


            ///Experiment 3: Run different values of LipschitzConstant with using EntropyMABTask selection method
            //runDifferentLipschitzConstant(TaskSelectionMethod.EntropyMABTask, 1, 0.001, 0.01, 0.001);

            //runDifferentLipschitzConstant(TaskSelectionMethod.EntropyMABTask, 1, 2, 2, 0.1);



            if (Program.isRecordedToTxtFile)
            {
                Console.SetOut(oldOut);
                writer.Close();
                ostrm.Close();
            }

            Console.Write("===End of the Main Program===");
        }

        /// <summary>
        /// Test for different initial number of labels
        /// </summary>
        public static void TestDifferentInitialNumLabelsAllMethods()
        {

            TaskSelectionMethod[] taskSelectionMethodOptions = { TaskSelectionMethod.EntropyTask, TaskSelectionMethod.RandomTask, TaskSelectionMethod.UniformTask };
            int[] maxNumberOfLabelsArray = { 5, 5, 1 };
            //int maxNumberOfLabels = 5;

            //for each task selection method 
            for (int i = 0; i < taskSelectionMethodOptions.Length; i++)
            {
                //select taskSelectionMethodOptions
                TaskSelectionMethod currentTaskSelectionMethod = taskSelectionMethodOptions[i];
                int maxNumberOfLabels = maxNumberOfLabelsArray[i];

                //call method TestTaskSelectionWithDifferentInitialLabels
                TestTaskSelectionWithDifferentInitialLabels(taskSelectionMethodOptions[i], maxNumberOfLabels);
            }
        }


        /// <summary>
        /// Test a task selection method with different initial number of labels
        /// </summary>
        public static void TestTaskSelectionWithDifferentInitialLabels(TaskSelectionMethod taskSelectionMethod, int maxNumberOfLabels, double lipschitzConstant = 1)
        {

            //Directory.CreateDirectory(ResultsDir);

            //run through different numbers of initial labels
            for (int j = 1; j <= maxNumberOfLabels; j++)
            {
                //select the initial number of labels
                int projectInitialNumLabelsPerTask = j;

                // Experiment of Figure 4
                WWW14_CBCCExperiment.RunWWWExperiments(Program.startIndex, Program.endIndex, Program.whichModel, taskSelectionMethod, projectInitialNumLabelsPerTask, lipschitzConstant);

                // Experiment to find the number of communities
                //FindNumCommunities(startIndex, endIndex, 10);

            } //end for each number of label
        }

        /// <summary>
        /// Runs different values Lipschiz constant with using MAB task selection method(s)
        /// <param name="taskSelectionMethod">The selected task selection method </param>
        /// <param name="maxNumberOfLabels">The maximum number of labels used </param>
        /// <param name="minLipschitzConstant">The minimum value of Lipschitz constant</param>
        /// <param name="maxLipschitzConstant">The maximum value of Lipschitz constant</param>
        /// <param name="stepForLipschitzConstant">The step value for LipschitzConstant</param>
        /// </summary>
        static void runDifferentLipschitzConstant(TaskSelectionMethod taskSelectionMethod, int maxNumberOfLabels, double minLipschitzConstant, double maxLipschitzConstant, double stepForLipschitzConstant)
        {
            //initial the current Lipschitz constant 
            double currentLipschitzConstant = minLipschitzConstant;

            //continue to run active learning experiment until the Lipschitz constant reach its maximum value
            while (currentLipschitzConstant <= maxLipschitzConstant + stepForLipschitzConstant)
            {

                TestTaskSelectionWithDifferentInitialLabels(taskSelectionMethod, maxNumberOfLabels, currentLipschitzConstant);

                //increase the constant with step value
                currentLipschitzConstant += stepForLipschitzConstant;
            }//end while 
        }

        public static void TestParallelActiveLearning(string dataSet, int projectInitialNumLabelsPerTask)
        {

            var data = Datum.LoadData(@"Data/" + dataSet + ".csv");
            int numModels = 2;
            RunType[] runTypeArray = new RunType[numModels];
            string[] modelNameArray = new string[numModels];
            TaskSelectionMethod[] taskSelectionMethodArray = new TaskSelectionMethod[numModels];
            WorkerSelectionMethod[] workerSelectionMethodArray = new WorkerSelectionMethod[numModels];
            double lipschitzConstant = 1;
            BCC[] modelArray = new BCC[numModels];

            // Majority vote
            runTypeArray[0] = RunType.MajorityVote;

            // BCC
            runTypeArray[1] = RunType.BCC;
            taskSelectionMethodArray[1] = TaskSelectionMethod.RandomTask;
            workerSelectionMethodArray[1] = WorkerSelectionMethod.RandomWorker;
            modelArray[1] = new BCC();

            modelNameArray = modelNameArray.Select((s, i) => Program.GetModelName(dataSet, runTypeArray[i])).ToArray();
            ActiveLearning.RunParallelActiveLearning(data, modelNameArray, runTypeArray, modelArray, taskSelectionMethodArray, workerSelectionMethodArray, -1, projectInitialNumLabelsPerTask, lipschitzConstant);
        }
    }
}
