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
            using (
                var reader = new StreamReader(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var strarr = line.Split(',');
                    int length = strarr.Length;
                    if (length < 3 || length > 4)
                    {
                        //continue;
                    }

                    int workerLabel = int.Parse(strarr[2]);
                    if (workerLabel < -4 || workerLabel > 4)
                    {
                        continue;
                    }

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

        public static IList<Datum> LoadDataCF()
        {
            var result = new List<Datum>();
            using (var reader = new StreamReader(@".\Data\CF_tweets.csv"))
            {
                string line = reader.ReadLine();
                int count = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    count++;
                    var strarr = line.Split(',');
                    int length = strarr.Length;
                    //if (length < 3 || length > 4) //Filter bad entries!!
                    //    continue;

                    int workerLabel;
                    try
                    {
                        workerLabel = int.Parse(strarr[2]);
                    }
                    catch //Handler for documents in bad format: Concatenate string to the body text of the previous document
                    {
                        result.Last().BodyText += strarr[0];
                        continue;
                    }
                    //if (workerLabel < -4 || workerLabel > 4) //Filter bad entries!!
                    //    continue;

                    var datum = new Datum()
                    {
                        WorkerId = strarr[1],
                        TaskId = strarr[0],
                        WorkerLabel = workerLabel,
                        BodyText = strarr[3]
                    };

                    result.Add(datum);

                    if (count % 10000 == 0)
                        Console.WriteLine("\t{0} / {1}", count, 590000);

                    //if (count == 285000)
                    //    return result;
                }
            }



            using (var reader = new StreamReader(@".\Data\CF_gold.csv"))
            {
                string line = reader.ReadLine();
                while ((line = reader.ReadLine()) != null)
                {
                    var strarr = line.Split(',');
                    int length = strarr.Length;

                    string taskId = strarr[0];
                    int goldLabel = int.Parse(strarr[1]);
                    result.Where(d => d.TaskId.Equals(taskId)).Select(d => d.GoldLabel = goldLabel).ToList();

                    var a = result.Where(d => d.TaskId.Equals(taskId)).ToArray();
                }
            }

            // Save CF in the right format
            //using (var writer = new StreamWriter(@".\Data\CF_with_text.csv"))
            //{
            //    foreach (Datum datum in result)
            //    {
            //        writer.WriteLine(String.Format("{0},{1},{2},{3}", datum.WorkerId, datum.TaskId, datum.WorkerLabel, datum.BodyText) + datum.GoldLabel==null ? "" : datum.GoldLabel.ToString());
            //    }
            //}

            // Serialize object
            using (Stream stream = File.Open("CFdata.bin", FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, result);
            }


            return result;
        }

        public static Dictionary<string, Dictionary<string, bool>> buildGroundTruthEntityData()
        {
            var filenames = Directory.GetFiles(@"C:\Users\Matteo\Desktop\ZenCrowdDataset\ConvertedData");
            Dictionary<string, Dictionary<string, bool>> groundTruthEntities = new Dictionary<string, Dictionary<string, bool>>();
            foreach (var file in filenames)
            {
                if (file.Contains("GT"))
                {
                    using (var reader = new StreamReader(file))
                    {
                        string line = "";
                        while ((line = reader.ReadLine()) != null)
                        {
                            var strarr = line.Split(',');
                            int length = strarr.Length;

                            string entityName = strarr[0];
                            var linkarr = strarr[3].Split(' ');

                            if (groundTruthEntities.ContainsKey(entityName))
                            {
                                foreach (var link in linkarr)
                                {

                                    if (!link.Contains("http") || link.Equals("None") || link.Contains("Not"))
                                        continue;

                                    if (!groundTruthEntities[entityName].ContainsKey(link))
                                        groundTruthEntities[entityName][link] = true;
                                }
                            }
                            else
                            {
                                groundTruthEntities[entityName] = new Dictionary<string, bool>();
                                foreach (var link in linkarr)
                                {
                                    if (!link.Contains("http") || link.Equals("None") || link.Contains("Not"))
                                        continue;

                                    groundTruthEntities[entityName][link] = true;
                                }
                            }
                        }
                    }
                }
            }

            foreach (var file in filenames)
            {
                if (!file.Contains("GT"))
                {
                    using (var reader = new StreamReader(file))
                    {
                        string line = "";
                        while ((line = reader.ReadLine()) != null)
                        {
                            var strarr = line.Split(',');
                            int length = strarr.Length;

                            string entityName = strarr[0];

                            if (!groundTruthEntities.ContainsKey(entityName))
                                continue;

                            var linkarr = strarr[3].Split(' ');
                            foreach (var link in linkarr)
                            {
                                if (!link.Contains("http"))
                                    continue;

                                if (!groundTruthEntities[entityName].ContainsKey(link))
                                    groundTruthEntities[entityName][link] = false;
                            }
                        }
                    }
                }
            }

            // Remove bad data
            var entities = groundTruthEntities.Keys.ToList();
            foreach (var ent in entities)
            {
                if (groundTruthEntities[ent].Count > 5)
                    groundTruthEntities.Remove(ent);
            }

            // Fill missing links
            foreach (var kvp in groundTruthEntities)
            {
                if (kvp.Value.Count < 5)
                {
                    int missingLinks = 5 - kvp.Value.Count;
                    string missingLinkSuffix = kvp.Key + "_undisclosedLink";
                    for (int i = 0; i < missingLinks; i++)
                    {
                        kvp.Value[missingLinkSuffix + i] = false;
                    }
                }
            }

            // Print out file for testing
            using (var writer = new StreamWriter(@"Testfile_EntityData.txt"))
            {
                    foreach (var kvp in groundTruthEntities)
                    {
                        foreach (var kvp2 in kvp.Value)
                            writer.WriteLine("{0},{1},{2},{3}", kvp.Key, kvp2.Key, kvp2.Value, kvp.Value.Count);
                    }
            }

            return groundTruthEntities;
        }

        public static void SaveZenCrowdData()
        {
            var groundTruthEntities = buildGroundTruthEntityData();
            var filenames = Directory.GetFiles(@"C:\Users\Matteo\Desktop\ZenCrowdDataset\ConvertedData");
            List<Datum> data = new List<Datum>();
            foreach (var file in filenames)
            {

                if (!file.Contains("GT"))
                {
                    using (var reader = new StreamReader(file))
                    {
                        string line = "";
                        while ((line = reader.ReadLine()) != null)
                        {
                            var strarr = line.Split(',');
                            int length = strarr.Length;

                            string entityName = strarr[0];
                            string workerId = strarr[1];
                            int time = int.Parse(strarr[2]);
                            var workerLinks = strarr[3].Split(' ');

                            if (!groundTruthEntities.ContainsKey(entityName))
                                continue;

                            foreach (var gtLink in groundTruthEntities[entityName])
                            {
                                var datum = new Datum();
                                datum.WorkerId = workerId;
                                datum.TimeSpent = time;
                                datum.TaskId = entityName + "-"+gtLink.Key;
                                datum.GoldLabel = gtLink.Value? 1: 0;
                                if (workerLinks.Contains(gtLink.Key))
                                    datum.WorkerLabel = 1;
                                else
                                    datum.WorkerLabel = 0;
                                data.Add(datum);
                            }
                        }
                    }
                }
            }

            WriteDatasetToFile(data, @"ZenCrowd_all.csv");
        }

        public static void WriteDatasetToFile(IList<Datum> data, string filename)
        {
            using (var writer = new StreamWriter(filename))
            {
                foreach (var datum in data)
                {
                    writer.WriteLine("{0},{1},{2},{3},{4}", datum.WorkerId, datum.TaskId, datum.WorkerLabel, datum.GoldLabel, datum.TimeSpent);
                }
            }
        }

        public static IList<Datum> LoadDataCFFromBinaryFile()
        {
            List<Datum> result = null;
            // Deserialize object
            using (Stream stream = File.Open("CFdata.bin", FileMode.Open))
            {
                BinaryFormatter bin = new BinaryFormatter();
                result = (List<Datum>)bin.Deserialize(stream);
            }
            return result;
        }

        public static IList<Datum> LoadDataSP()
        {
            var result = new List<Datum>();
            using (var reader = new StreamReader(@".\Data\SP_tweets.txt"))
            {
                string line = reader.ReadLine();
                int count = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    count++;
                    var strarr = line.Split('\t');
                    int length = strarr.Length;
                    //if (length < 3 || length > 4) //Filter bad entries!!
                    //    continue;

                    int workerLabel;
                    try
                    {
                        workerLabel = int.Parse(strarr[2]);
                    }
                    catch //Handler for documents in bad format: Concatenate string to the body text of the previous document
                    {
                        result.Last().BodyText += strarr[0];
                        continue;
                    }
                    //if (workerLabel < -4 || workerLabel > 4) //Filter bad entries!!
                    //    continue;

                    var datum = new Datum()
                    {
                        WorkerId = strarr[0],
                        TaskId = strarr[1],
                        WorkerLabel = workerLabel,
                        BodyText = strarr[3],
                        GoldLabel = int.Parse(strarr[4])
                    };

                    result.Add(datum);

                    if (count % 10000 == 0)
                        Console.WriteLine("\t{0} / {1}", count, 590000);
                }
            }

            return result;
        }
    }

    //end class of Datum

}