/********************************************************
*                                                       *
*   Copyright (C) Microsoft. All rights reserved.       *
*                                                       *
********************************************************/

// Authors: Matteo Venanzi and John Guiver

/* Community-Based Bayesian Aggregation for Crowdsoucing
* 
* Software to run the experiment presented in the paper "Community-Based Bayesian Aggregation Models for Crowdsourcing" by Venanzi et. al, WWW14
* To run it, you must create csv file with your data with the format <Worker id, Task id, worker's label, (optional) task's gold label>:
* 
* Example: {842,79185673,0,0
1258,79185673,0,0
1467,79185673,0,0
1674,79185673,0,0
662,79185673,0,0
708,79185673,0,0
1507,79185673,3,0
1701,79185724,4
38,79185724,3
703,79185724,1
353,79185724,1
165,79185724,0
1025,79185724,4
1638,79185724,4
782,79185900,1
1480,79185900,1}
* 
* You can download the original CF data set used in the paper from www.crowdscale.org
*/


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
    using VectorGaussianArray = DistributionRefArray<VectorGaussian, Vector>;
    using VectorGaussianArrayArray = DistributionRefArray<DistributionRefArray<VectorGaussian, Vector>, Vector[]>;
    using DiscreteArray = DistributionRefArray<Discrete, int>;
    using System.IO;

    /// <summary>
    /// Options for which model to run.
    /// </summary>
    public enum RunType
    {
        /// <summary>
        /// The true label distribution
        /// as given by the normalised workers' label counts.
        /// </summary>
        VoteDistribution = 0,

        /// <summary>
        /// The true label is the majority label.
        /// </summary>
        MajorityVote = 1,

        /// <summary>
        /// The Dawid-Skene model.
        /// </summary>
        DawidSkene = 2,

        /// <summary>
        /// The BCC model.
        /// </summary>
        BCC = 3,

        /// <summary>
        /// The CBCC model.
        /// </summary>
        CBCC = 4


    }

    /// <summary>
    /// Metrics for selecting tasks.
    /// </summary>
    public enum TaskSelectionMethod
    {
        RandomTask,
        EntropyTask,
        UniformTask,
    }

    /// <summary>
    /// Metrics for selecting workers
    /// </summary>
    public enum WorkerSelectionMethod
    {
        RandomWorker,
        BestWorker,
    }


    /// <summary>
    /// The different modes in which the model can be run.
    /// </summary>
    public enum RunMode
    {
        /// <summary>
        /// Clears all posteriors
        /// </summary>
        ClearResults,
        /// <summary>
        /// Training from a batch of data - uses initial priors.
        /// </summary>
        BatchTraining,
        /// <summary>
        /// Online training from a batch of data - uses previous posteriors as priors.
        /// </summary>
        OnlineTraining,
        /// <summary>
        /// Online training where we don't update the posteriors
        /// </summary>
        LookAheadExperiment,
        /// <summary>
        /// Use communities as workers in a BCC
        /// </summary>
        LoadAndUseCommunityPriors,
        /// <summary>
        /// Prediction of worker labels
        /// </summary>
        Prediction,
    };
}