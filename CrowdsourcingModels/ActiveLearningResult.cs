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
namespace CrowdsourcingModels
{
    public class ActiveLearningResult
    {
        /// <summary>
        /// The task id.
        /// </summary>
        public string TaskId
        {
            get;
            set;
        }

        /// <summary>
        /// The worker id.
        /// </summary>
        public string WorkerId
        {
            get;
            set;
        }

        /// <summary>
        /// The total utility of a label provided by the worker for the task.
        /// </summary>
        double _totalTaskValue = 0;
        public double TotalTaskValue
        {
            set { _totalTaskValue = value; }
            get
            {
                //return 3 decimal places of the taskValue
                return Math.Round(_totalTaskValue, 3);
            }
        }


        /// <summary>
        /// The utility of a label provided by the worker for the task.
        /// </summary>
        double _taskValue = 0;
        public double TaskValue
        {
            set { _taskValue = value; }
            get
            {
                //return 3 decimal places of the taskValue
                return Math.Round(_taskValue, 3);
            }
        }
       
    
        /// <summary>
        /// The upper confidence bound value for Multi Armed Bandit 
        /// </summary>
        public double UcbValue
        {
            get;
            set;
        }
    } // end class of ActiveLearningResult

}
