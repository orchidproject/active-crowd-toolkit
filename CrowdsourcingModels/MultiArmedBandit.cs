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
//
namespace CrowdsourcingModels
{
    /// <summary>
    /// Class of Multi Armed Bandit
    /// </summary>
    public class MultiArmedBandit
    {
        //Dictionary<string, int> currentCounts;

        double lipschitzConstant = 0.1;

        /// <summary>
        /// Constructor with currentCounts Dictionary
        /// </summary>
        /// <param name="currentCounts"></param>
        public MultiArmedBandit(Dictionary<string, int> currentCounts) 
        {
            //this.currentCounts = currentCounts;
        }

        /// <summary>
        /// Overloading Constructor with currentCounts Dictionary and lipschitzConstant
        /// </summary>
        /// <param name="currentCounts"></param>
        public MultiArmedBandit(Dictionary<string, int> currentCounts, double lipschitzConstant)
        {
            this.lipschitzConstant = lipschitzConstant;
        }
        /// <summary>
        /// Add UCB and turn the task Value
        /// </summary>
        /// <param name="TaskValue"></param>
        /// <param name="currentCounts"></param>
        /// <param name="totalCounts"></param>
        /// <returns>The TaskValue</returns>
        public Dictionary<string, ActiveLearningResult> AddUCB(Dictionary<string, ActiveLearningResult> TaskValue, Dictionary<string, int> currentCounts, int timeT)
        {
            
            //implement the algorithm

            //UCBCrowd chooses a task with the highest UCB index

            //add confidence intrival value to taskValue
            foreach(var taskValueItem in TaskValue)
            { 
                //The Lipschitz Constant
                double value = taskValueItem.Value.TotalTaskValue;
                taskValueItem.Value.TaskValue = value;
                String taskId = taskValueItem.Value.TaskId;
                taskValueItem.Value.UcbValue = GetConfidenceIntrivalValue2(currentCounts[taskId], lipschitzConstant, timeT);
                taskValueItem.Value.TotalTaskValue = value + taskValueItem.Value.UcbValue;
              
            }
            return TaskValue;
        }

        /// <summary>
        /// Get UCB confidence interval value
        /// </summary>
        /// <param name="currentNumLabels"> The current label count of the task</param>
        /// <param name="lipschitzConstant">The Lipschitx constant of the UCB algorithm</param>
        ///  <param name="currentLabelingRound">The current labeling rounds</param>
        /// <returns>The confidenceIntrivalValue</returns
        public double GetConfidenceIntrivalValue(int currentLabelCount, double lipschitzConstant, int currentLabelingRound) 
        {
            double confidenceIntrivalValue = 0;
            double t4 = Math.Pow(currentLabelingRound,-4);
            double numerator = -1.0 * Math.Log(1.0 - Math.Sqrt(1.0 - t4));
            double denominator = (2.0 * currentLabelCount);
            double squarePart = Math.Sqrt(numerator / denominator);
            confidenceIntrivalValue = lipschitzConstant * squarePart;
            return confidenceIntrivalValue;
        
        }

        /// <summary>
        /// version 2 of Get confidence intrival value 
        /// </summary>
        /// <param name="numberOfLabelRequiredUntilTimeT"></param>
        /// <param name="LipschitzConstant"></param>
        ///  <param name="timeT"></param>
        /// <returns>The confidenceIntrivalValue</returns

        public double GetConfidenceIntrivalValue2(int numberOfLabelRequiredUntilTimeT, double lipschitzConstant, int timeT)
        {
            double confidenceIntrivalValue = 0;
            double numerator = 40.0 * Math.Log(timeT / 16.0);
            double denominator = (numberOfLabelRequiredUntilTimeT);
            double squarePart = Math.Sqrt(numerator / denominator);
            confidenceIntrivalValue = lipschitzConstant * squarePart;
            return confidenceIntrivalValue;

        }

    }
}
