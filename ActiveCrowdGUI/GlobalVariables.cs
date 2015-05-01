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
        public const string dataDirectory = @"C:\Users\Matteo\OneDrive\ActiveCrowdToolkit\Datasets\";
        public static string[] preLoadedDatasetsPath = {"WS-AMT", "SP-2015" };
        public static int[] communityCounts = { 2, 2 };
        public static TaskSelectionMethod mvDefaultTaskSelectionMethod = TaskSelectionMethod.EntropyTask;
        public static List<Dataset> loadedDatasets = null;

        /// <summary>
        /// PreLoad
        /// </summary>
        public static void LoadDatasets() 
        {
            loadedDatasets = new List<Dataset>();

            //load preloadedDatasts
            for(int i = 0; i < preLoadedDatasetsPath.Length; i++)
            {
                loadedDatasets.Add(new Dataset(dataDirectory, preLoadedDatasetsPath[i]));
            }
            
        }


        public static string[] getAllDatasetNames()
        {

            String[] allDatasetNames = new String[loadedDatasets.Count];
            for (int i = 0; i < loadedDatasets.Count; i++)
            {
                allDatasetNames[i] = loadedDatasets[i].DatasetName;
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
