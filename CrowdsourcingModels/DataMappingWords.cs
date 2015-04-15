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
using TFIDF;

//
namespace CrowdsourcingModels
{
    /// <summary>
    /// Data mapping class. This class manages the mapping between the data (which is
    /// in the form of task, worker ids, and labels) and the model data (which is in term of indices).
    /// </summary>
    public class DataMappingWords : DataMapping
    {
        /// <summary>
        /// The vocabulary
        /// </summary>
        public List<string> Vocabulary;

        /// <summary>
        /// The size of the vocabulary.
        /// </summary>
        public int WordCount
        {
            get
            {
                return Vocabulary.Count();
            }
        }

        public int[] WordCountsPerTaskIndex;

        public int[][] WordIndicesPerTaskIndex;

        public string[] CFLabelName = { "Negative", "Neutral", "Positive", "NotRelated", "Unknown"  };
        public string[] SPLabelName = { "Negative", "Positive" };

        public DataMappingWords(
            IEnumerable<Datum> data,
            List<string> vocab,
            int[] wordCountPerTaskIndex = null,
            int[][] wordIndicesPerTaskIndex = null,
            bool buildFullMapping = false)
            : base(data)
        {
                Vocabulary = vocab;
                if (wordCountPerTaskIndex == null)
                    GetWordIndicesAndCountsPerTaskIndex(data, out WordIndicesPerTaskIndex, out WordCountsPerTaskIndex);
                else
                {
                    WordCountsPerTaskIndex = wordCountPerTaskIndex;
                    WordIndicesPerTaskIndex = wordIndicesPerTaskIndex;
                }

            if (buildFullMapping) // Use task ids as worker ids
            {
                TaskIndexToId = data.Select(d => d.TaskId).Distinct().ToArray();
                TaskIdToIndex = TaskIndexToId.Select((id, idx) => new KeyValuePair<string, int>(id, idx)).ToDictionary(x => x.Key, y => y.Value);
            }
        }

        /// <summary>
        /// Returns the matrix of the task indices (columns) of each worker (rows).
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>The matrix of the word indices (columns) of each task (rows).</returns>
        public void GetWordIndicesAndCountsPerTaskIndex(IEnumerable<Datum> data, out int[][] wordIndicesPerTaskIndex, out int[] wordCountsPerTaskIndex)
        {
            wordIndicesPerTaskIndex = new int[TaskCount][];
            wordCountsPerTaskIndex = new int[TaskCount];
            string[] corpus = new string[TaskCount];

            // Dictionary keyed by task Id, with randomly order labelings
            var groupedRandomisedData =
                data.GroupBy(d => d.TaskId).
                Select(g =>
                {
                    var arr = g.ToArray();
                    int cnt = arr.Length;
                    var perm = Rand.Perm(cnt);
                    return new
                    {
                        key = g.Key,
                        arr = g.Select((t, i) => arr[perm[i]]).ToArray()
                    };
                }).ToDictionary(a => a.key, a => a.arr);

            int count = 0;
            foreach (var kvp in groupedRandomisedData)
            //for (int i = 0; i < TaskCount; i++)
            {
                corpus[TaskIdToIndex[kvp.Key]] = kvp.Value.First().BodyText;

                //corpus[i] = data.Where(d => d.TaskId == task_id).Select(d => d.BodyText).First();
                count++;
                //if (count % 10000 == 0)
                //    Console.WriteLine("Build word index per task matrix {0}", count);        
            }

            wordIndicesPerTaskIndex = TFIDFClass.GetWordIndexStemmedDocs(corpus, Vocabulary);
            wordCountsPerTaskIndex = wordIndicesPerTaskIndex.Select(t => t.Length).ToArray();
            //wordCountsPerTaskIndex = TFIDFClass.GetWordCountStemmedDocs(corpus, Vocabulary);
            
        }

        /// <summary>
        /// Returns the matrix of the labels (columns) of each worker (rows).
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>The matrix of the labels (columns) of each worker (rows).</returns>
        public int[] GetWordCountsPerTaskIndex(IEnumerable<Datum> data)
        {
            int[] result = new int[TaskCount];
            string[] corpus = new string[TaskCount];
            for (int i = 0; i < TaskCount; i++)
            {
                var wid = TaskIndexToId[i];
                corpus[i] = data.Where(d => d.TaskId == wid).Select(d => d.BodyText).First();
            }

            result = TFIDFClass.GetWordCountStemmedDocs(corpus, Vocabulary);
            return result;
        }



    }

    //end class DataMapping
}