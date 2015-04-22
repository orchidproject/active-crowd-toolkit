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
using System.Runtime.Serialization.Formatters.Binary;
namespace CrowdsourcingModels
{
    /// <summary>
    /// This class represents a single datum, and has methods to read in data.
    /// </summary>
    [Serializable()]
    public class Datum
    {
        /// <summary>
        /// The worker id.
        /// </summary>
        public string WorkerId;

        /// <summary>
        /// The task id.
        /// </summary>
        public string TaskId;

        /// <summary>
        /// The worker's label.
        /// </summary>
        public int WorkerLabel;

        /// <summary>
        /// The task's gold label (optional).
        /// </summary>
        public int? GoldLabel;

        /// <summary>
        /// The time spent by the worker to produce the label (optional)
        /// </summary>
        public double? TimeSpent;

        /// <summary>
        /// The body text of the document (optional - only for text sentiment labelling tasks).
        /// </summary>
        public string BodyText;

        /// <summary>
        /// Loads the data file in the format (worker id, task id, worker label, ?gold label).
        /// </summary>
        /// <param name="filename">The data file.</param>
        /// <returns>The list of parsed data.</returns>
        public static IList<Datum> LoadData(string filename)
        {
            var result = new List<Datum>();
            using (var reader = new StreamReader(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var strarr = line.Split(',');
                    int length = strarr.Length;
                    //if (length < 3 || length > 4)
                    //{
                    //    //continue;
                    //}

                    int workerLabel = int.Parse(strarr[2]);
                    //if (workerLabel < -4 || workerLabel > 4)
                    //{
                    //    continue;
                    //}

                    var datum = new Datum()
                    {
                        WorkerId = strarr[0],
                        TaskId = strarr[1],
                        WorkerLabel = workerLabel,
                    };

                    if (length >= 4 && !strarr[3].Equals("NaN"))
                        datum.GoldLabel = int.Parse(strarr[3]);
                    else
                        datum.GoldLabel = null;

                    if (length == 5)
                        datum.TimeSpent = double.Parse(strarr[4]);
                    else
                        datum.TimeSpent = null;


                    result.Add(datum);
                }
            }

            return result;
        }
    }
}