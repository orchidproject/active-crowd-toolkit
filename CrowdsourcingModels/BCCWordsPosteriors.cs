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
    /// CBCC posterior object.
    /// </summary>
    [Serializable]
    public class BCCWordsPosteriors : BCCPosteriors
    {
        /// <summary>
        /// The Dirichlet posteriors of the word probabilities for each true label value.
        /// </summary>
        public Dirichlet[] ProbWordPosterior;

    }

    //
}
