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
        /// The utility of the task
        /// </summary>
        double _TaskValue = 0;
        public double TaskValue
        {
            set { _TaskValue = value; }
            get
            {
                //return 3 decimal places of the taskValue
                return Math.Round(_TaskValue, 3);
            }
        }

        /// <summary>
        /// The utility of the worker
        /// </summary>
        double _WorkerValue = 0;
        public double WorkerValue
        {
            set { _WorkerValue = value; }
            get
            {
                //return 3 decimal places of the taskValue
                return Math.Round(_WorkerValue, 3);
            }
        }
    } // end class of ActiveLearningResult

}
