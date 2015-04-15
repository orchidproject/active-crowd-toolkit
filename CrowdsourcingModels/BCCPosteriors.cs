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

    /// <summary>
    /// The BCC posteriors class.
    /// </summary>
    [Serializable]
    public class BCCPosteriors
    {
        /// <summary>
        /// The probabilities that generate the true labels of all the tasks.
        /// </summary>
        public Dirichlet BackgroundLabelProb;

        /// <summary>
        /// The probabilities of the true label of each task.
        /// </summary>
        public Discrete[] TrueLabel;

        /// <summary>
        /// The Dirichlet parameters of the confusion matrix of each worker.
        /// </summary>
        public Dirichlet[][] WorkerConfusionMatrix;

        /// <summary>
        /// The predictive probabilities of the worker's labels.
        /// </summary>
        public Discrete[][] WorkerPrediction;

        /// <summary>
        /// The true label constraint used in online training.
        /// </summary>
        public Discrete[] TrueLabelConstraint;

        /// <summary>
        /// The model evidence.
        /// </summary>
        public Bernoulli Evidence;
    }



}
