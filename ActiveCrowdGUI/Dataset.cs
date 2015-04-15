using CrowdsourcingModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcriveCrowdGUI
{
    /// <summary>
    /// The dataset object
    /// </summary>
    public class Dataset
    {
        public string datasetPath
        {
            get;
            private set;
        }

        public string datasetName 
        {
            get;
            private set;

        }

        public int datasetIndex
        {
            get;
            private set;
        }
        /// <summary>
        /// The maximum number of labelling rounds of the dataset
        /// </summary>
        public int maximumOfLabellingRound
        {
            get;
            private set;
        }

        public int totalNumberOfLabellingRows
        {
            get;
            private set;
        }

        public int totalNumberOfTasks
        {
            get;
            private set;
        }

        public int numberOfWorkerLabels
        {
            get;
            private set;
        }

        public Dataset(string datasetPath) 
        {
            this.datasetPath = datasetPath;
            this.datasetName = "";
            SetDatasetValues();
        }

        public Dataset(string datasetPath, string datasetName)
            : this(datasetPath)
        {
            this.datasetName = datasetName;

        }



        public void SetDatasetValues()
        {
            Debug.WriteLine("set datasetValues:" +datasetPath);
            IList<Datum> data = Datum.LoadData(datasetPath);
            totalNumberOfLabellingRows = data.Count;
            totalNumberOfTasks = data.Select(d => d.TaskId).Distinct().Count();
            numberOfWorkerLabels = data.Select(d => d.WorkerLabel).Distinct().Count();
            //set maximumOfLabellingRound

            //maximumOfLabellingRound = (int)totalNumberOfLabellingRows / totalNumberOfTasks;
            var minRow = data.GroupBy(d => d.TaskId).OrderBy(d => d.Count()).First();
            this.maximumOfLabellingRound = (int)minRow.Count();
        }


        /// <summary>
        /// Load Data from ActiveLearning
        /// </summary>
        /// <returns></returns>
        public IList<Datum> LoadData()
        {
            return Datum.LoadData(datasetPath);
        }



        /// <summary>
        /// Get the dataset name without extension, 
        /// used for running RunParallelActiveLearning in the ActiveLearning Class
        /// </summary>
        /// <returns></returns>
        public string GetDataSetNameWithoutExtension()
        {
            string dataSetNameWithoutExtension = "";
            int fileExtPos = datasetName.LastIndexOf(".");
            if (fileExtPos >= 0)
                dataSetNameWithoutExtension = datasetName.Substring(0, fileExtPos);

            return dataSetNameWithoutExtension;
        }




        /// <summary>
        /// Get a labelStartingPoints for initialing the starting point of the ExperimentModel
        /// </summary>
        /// <returns></returns>
        public int[] GetLabelStartingPoints()
        {
            int[] labelStartingPoints = new int[maximumOfLabellingRound];
            //add the label starting points into the array
            for (int i = 0; i < maximumOfLabellingRound; i++)
            { 
                labelStartingPoints[i] =  totalNumberOfTasks * (i+1) + 1;
            
            }
            return labelStartingPoints;
        }


        //overide ToString

        public override string ToString() 
        {
            return datasetName;
        }
    }
}
