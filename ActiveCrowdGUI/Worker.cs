using CrowdsourcingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcriveCrowdGUI
{
    public class Worker
    {
        /// <summary>
        /// The worker id.
        /// </summary>
        public string WorkerId
        {
            get;
            set;
        }

        /// <summary>
        /// The worker's number of label
        /// </summary>
        public double numberOfLabels
        {
            get;
            set;
        }

        public List<ActiveLearningResult> getTasksList()
        {
            return null; 
        }

        public Worker(string workerId) 
        {
            this.WorkerId = workerId;
        }
        public Worker()
        { 
        }
    }
}
