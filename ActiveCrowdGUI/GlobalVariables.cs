using CrowdsourcingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcriveCrowdGUI
{
    public class GlobalVariables
    {
        public const string dataDirectory = "@../../../../../CrowdsourcingProject/Data/";
        public static string[] preLoadedDatasetsPath = {"CF.csv", "MS.csv", "SP.csv" };
        public static int[] communityCounts = { 4, 2, 2 };
        public static double[] mabConstants = {0.009, 0.0018, 0.0036,0.009};
        public static int[] labelStartingPoints = {301,601,901,1201,1501 };
        public static TaskSelectionMethod[] taskSelectionMethodOptions = { TaskSelectionMethod.EntropyTask, TaskSelectionMethod.RandomTask, TaskSelectionMethod.UniformTask, TaskSelectionMethod.EntropyMABTask };

        public static TaskSelectionMethod mvDefaultTaskSelectionMethod = TaskSelectionMethod.EntropyTask;

        public static List<Dataset> loadedDatasets = null;
        public static void LoadDatasets() 
        {
            loadedDatasets = new List<Dataset>();

            //load preloadedDatasts
            for(int i = 0; i < preLoadedDatasetsPath.Length; i++)
            {
                loadedDatasets.Add(new Dataset(dataDirectory + preLoadedDatasetsPath[i], preLoadedDatasetsPath[i]));
            }
            
        }


        public static string[] getAllDatasetNames()
        {

            String[] allDatasetNames = new String[loadedDatasets.Count];
            for (int i = 0; i < loadedDatasets.Count; i++)
            {
                allDatasetNames[i] = loadedDatasets[i].datasetName;
            }
           
            return allDatasetNames;
        
        }

        public static int getDatasetIndex(Dataset currentItem)
        {
            return loadedDatasets.IndexOf(currentItem);
        }

    

    }

    //Enum for the experiment type 
    public enum ExperimentType
    {
        BatchRunning,
        ActiveLearning
    }
}
