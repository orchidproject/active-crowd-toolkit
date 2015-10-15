using MicrosoftResearch.Infer.Distributions;
using MicrosoftResearch.Infer.Maths;

namespace CrowdsourcingModels
{

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
        /// <summary>
        /// Select random tasks.
        /// </summary>
        RandomTask,

        /// <summary>
        /// Select tasks with the highest entropy.
        /// </summary>
        EntropyTask,

        /// <summary>
        /// Select tasks uniformly.
        /// </summary>
        UniformTask,
    }

    /// <summary>
    /// Metrics for selecting workers
    /// </summary>
    public enum WorkerSelectionMethod
    {
        /// <summary>
        /// Select random workers.
        /// </summary>
        RandomWorker,

        /// <summary>
        /// Select the worker with the highest accuracy.
        /// </summary>
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