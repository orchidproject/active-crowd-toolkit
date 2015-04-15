using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using MicrosoftResearch.Infer;
using MicrosoftResearch.Infer.Factors;
using MicrosoftResearch.Infer.Distributions;
using MicrosoftResearch.Infer.Maths;
using MicrosoftResearch.Infer.Models;
using MicrosoftResearch.Infer.Utils;

namespace CrowdsourcingModels
{
    class WWW15_BCCWordsExperiment
    {

         /*
         /// <summary>
         /// The initial number labels per task.
         /// </summary>
         public static int projectInitialNumLabelsPerTask = 1;
         */

        /// <summary>
        /// The results directory.
        /// </summary>
        public static string ResultsDir = @"ResultsBCCWords/";

        /// <summary>
        /// Main method to run the crowdsourcing experiments presented in Venanzi et.al (WWW14).
        /// </summary>
        /// <param name="args[0]">agrs for cluster development</param>
        /// <param name="args[1]">dataset selected</param>
        /// <param name="args[2]">model selected</param>
        /// <param name="args[3]">value of constant c</param>
        public static void Run_Main(string[] args)
        {
            Rand.Restart(12347);
            Directory.CreateDirectory(ResultsDir);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            Console.Write("Loading data...");
            var data = Datum.LoadDataSP();
            Console.WriteLine("done. Elapsed time: {0}", stopWatch.Elapsed);
            var fullData = new List<Datum>(data);

            int totalLabels = data.Count();
            //var perm = Rand.Perm(totalLabels);
            //Datum[] randomisedData = new Datum[(int)totalLabels / 5];
            //for (int i = 0; i < randomisedData.Length; i++)
            //    randomisedData[i] = data[perm[i]];


            //MeasureMemory();
            //RunModelsOnData(randomisedData.ToList(), fullData, RunType.DawidSkene, new BCC(), 3, true);
            TestBCCWordsGoldData();
            //RunProgressiveDataSampling(data);



            int wordCount = CountWordsInCorpus((List<Datum>)data);
            Console.WriteLine("Words in the corpus: {0}", wordCount);


            Console.WriteLine("done. Elapsed time: {0}", stopWatch.Elapsed);

            // Build vocabulary on sub data
            var VocabularyOnSubData = ResultsWords.BuildVocabularyOnSubdata((List<Datum>)data);

            // Measure execution time and memory
            GC.Collect();
            PerformanceCounter memCounter = new PerformanceCounter("Memory", "Available MBytes");
            float preMem = memCounter.NextValue();
            stopWatch.Reset();
            stopWatch.Start();

            // Run model and get results
            // Build data mapping and result object
            ResultsWords resultsWords = new ResultsWords(fullData, VocabularyOnSubData);
            resultsWords.SetVocabulary(VocabularyOnSubData);

            //data = randomisedData.ToList();

            BCCWords model = new BCCWords();
            DataMappingWords mapping = resultsWords.Mapping as DataMappingWords;

            resultsWords.RunBCCWords("BCCwords", data, fullData, model, Results.RunMode.ClearResults, true);
            stopWatch.Stop();
            GC.KeepAlive(model); // Keep the model alive to this point (for the	memory counter)
            float postMem = memCounter.NextValue();
            resultsWords.Memory = preMem - postMem;
            resultsWords.ExecutionTime = stopWatch.ElapsedMilliseconds / 1000;


            using (StreamWriter writer = new StreamWriter(Console.OpenStandardOutput()))
            {
                resultsWords.WriteAccuracy(writer);
                //resultsWords.WriteResults(writer, false, false, false, true);
            }
            using (StreamWriter writer = new StreamWriter(ResultsDir + "BCCWords_endpoints.csv", true))
            {
                resultsWords.WriteResults(writer, false, true, true, true, 300);
            }

            Console.WriteLine(String.Format("Approximate memory usage: {0:F2} MB", resultsWords.Memory));
            Console.WriteLine(String.Format("Approximate execution time (including model compilation): {0} seconds", stopWatch.ElapsedMilliseconds / 1000));


            // For evaluation with CF matlab script
            using (StreamWriter writer = new StreamWriter(ResultsDir + "BCCWords_CF_submission_file.csv", true))
            {
                foreach (var kvp in resultsWords.PredictedLabel)
                    writer.WriteLine("{0},{1}", kvp.Key, kvp.Value);
            }
        }



        public static void RunProgressiveDataSampling(IList<Datum> data)
        {
            int totalLabels = data.Count();
            int numRounds = 50; // percentage of data increments to be added to the training set, e.g. 5%, 10%, etc.
            int dataIncrement = (int)totalLabels / numRounds;
            //int dataIncrement = 1000;
            int numRuns = 1;

            for (int i = 1; i <= numRuns; i++)
            {
                Rand.Restart(i);
                Console.WriteLine("Run " + i);
                string ResultsDir = @"ResultsDataSampling/Run" + i + "/";
                Directory.CreateDirectory(ResultsDir);
                ActiveLearning.RunActiveLearning(data,
                    "MV",
                    RunType.MajorityVote,
                    new BCCWords(),
                    TaskSelectionMethod.RandomTask,
                    WorkerSelectionMethod.RandomWorker,
                    ResultsDir,
                    3,
                    -1,
                    1,
                    dataIncrement,
                    true);
            }
        }

        public static void MeasureMemory(IList<Datum> data)
        {
            int totalLabels = data.Count();
            List<double> memory = new List<double>();
            List<double> time = new List<double>();
            List<int> percData = new List<int>();

            // Build vocabulary on sub data
            var VocabularyOnSubData = ResultsWords.BuildVocabularyOnSubdata((List<Datum>)data);

            BCCWords model = new BCCWords();
            GC.Collect();
            PerformanceCounter memCounter = new PerformanceCounter("Memory", "Available MBytes");
            float preMem = memCounter.NextValue();

            Stopwatch stopWatch = new Stopwatch();
            for (int dataSize = 500; dataSize <= 50000; dataSize += 500)
            {
                //int limit = (int)totalLabels * dataSize / 100;
                Console.WriteLine("Data size: {0}", dataSize);
                var subData = data.Where((k, i) => i < dataSize).ToList();

                // Measure execution time and memory
                GC.Collect();
                stopWatch.Reset();
                stopWatch.Start();

                // Build data mapping and result object
                ResultsWords resultsWords = new ResultsWords(subData, VocabularyOnSubData);

                resultsWords.RunBCCWords("BCCwords", subData, data, model, Results.RunMode.ClearResults, true);
                GC.KeepAlive(model); // Keep the model alive to this point (for the	memory counter)
                stopWatch.Stop();
                float postMem = memCounter.NextValue();
                resultsWords.Memory = preMem - postMem;
                resultsWords.ExecutionTime = stopWatch.ElapsedMilliseconds / 1000;

                percData.Add(dataSize);
                memory.Add(resultsWords.Memory);
                time.Add(resultsWords.ExecutionTime);

                Console.WriteLine(String.Format("Approximate memory usage: {0:F2} MB", resultsWords.Memory));
                Console.WriteLine(String.Format("Approximate execution time (including model compilation): {0} seconds", stopWatch.ElapsedMilliseconds / 1000));
            }

            using (StreamWriter writer = new StreamWriter("MemoryBCCWords.csv"))
            {
                for (int i = 0; i < memory.Count; i++)
                    writer.WriteLine("{0},{1},{2}", percData[i], memory[i], time[i]);
            }

        }

        public static int CountWordsInCorpus(List<Datum> data)
        {
            int count = 0;
            foreach (var datum in data)
            {
                string[] splittedText = datum.BodyText.Split(' ');
                count += splittedText.Length;
            }
            return count;
        }

        public static Results RunModelsOnData(IList<Datum> data, IList<Datum> fullData, RunType runType, BCC model, int numCommunities = 3, bool isExportedToCSV = false)
        {

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();


            //var data = Datum.LoadData(@".\Data\SA.csv"); 

            int totalLabels = data.Count();
            Console.WriteLine("done. Elapsed time: {0}", stopWatch.Elapsed);

            // Measure execution time and memory
            GC.Collect();
            PerformanceCounter memCounter = new PerformanceCounter("Memory", "Available MBytes");
            float preMem = memCounter.NextValue();
            stopWatch.Reset();
            stopWatch.Start();

            string modelName = runType.ToString();
            Results results = new Results();

            switch (runType)
            {
                case RunType.VoteDistribution:
                    results.RunMajorityVote(data, fullData, true, true);
                    break;
                case RunType.MajorityVote:
                    results.RunMajorityVote(data, fullData, true, false);
                    break;
                case RunType.DawidSkene:
                    results.RunDawidSkene(data, fullData, true);
                    break;
                default:
                    results.RunBCC(modelName, data, fullData, model, Results.RunMode.ClearResults, true, numCommunities, false, false);
                    break;
            }

            stopWatch.Stop();
            GC.KeepAlive(model); // Keep the model alive to this point (for the	memory counter)
            float postMem = memCounter.NextValue();
            results.Memory = preMem - postMem;
            results.ExecutionTime = stopWatch.ElapsedMilliseconds / 1000;

            if (isExportedToCSV)
            {
                using (StreamWriter writer = new StreamWriter(ResultsDir + modelName + "_endpoints.csv", true))
                {
                    writer.WriteLine("{0}:,{1:0.000},{2:0.0000}", modelName, results.Accuracy, results.NegativeLogProb);
                    results.WriteResults(writer, false, true, false);
                }

                using (StreamWriter writer = new StreamWriter(ResultsDir + "statistics.csv"))
                {
                    results.WriteBasicStatistics(writer);
                }

                // For evaluation with CF matlab script
                //using (StreamWriter writer = new StreamWriter(ResultsDir + modelName+"_CF_submission_file.csv", true))
                //{
                //    foreach (var kvp in results.PredictedLabel)
                //        writer.WriteLine("{0},{1}", kvp.Key, kvp.Value);
                //}
            }

            using (StreamWriter writer = new StreamWriter(Console.OpenStandardOutput()))
            {
                results.WriteAccuracy(writer);
            }

            Console.WriteLine(String.Format("Approximate memory usage: {0:F2} MB", results.Memory));
            Console.WriteLine(String.Format("Approximate execution time (including model compilation): {0} seconds", stopWatch.ElapsedMilliseconds / 1000));

            return results;
        }

        public static void TestBCCWordsGoldData()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            Console.Write("Loading data...");
            List<Datum> data = (List<Datum>)Datum.LoadDataCF();
            int totalLabels = data.Count();
            Console.WriteLine("done. Elapsed time: {0}", stopWatch.Elapsed);

            // Measure execution time and memory
            GC.Collect();
            PerformanceCounter memCounter = new PerformanceCounter("Memory", "Available MBytes");
            float preMem = memCounter.NextValue();
            stopWatch.Reset();
            stopWatch.Start();

            // Run model and get results
            var VocabularyOnSubData = ResultsWords.BuildVocabularyOnSubdata((List<Datum>)data);

            BCCWords model = new BCCWords();
            ResultsWords resultsWords = new ResultsWords(data, VocabularyOnSubData);
            DataMappingWords mapping = resultsWords.Mapping as DataMappingWords;
            var goldLabels = mapping.GetGoldLabelsPerTaskId();
            var goldData = mapping.BuildDataFromAssignedLabels(goldLabels, data);
            resultsWords = new ResultsWords(goldData, VocabularyOnSubData);

            // Print word counts
            using (StreamWriter writer = new StreamWriter(ResultsDir + "CFWordCounts.csv", true))
            {
                for (int i = 0; i < mapping.WordCountsPerTaskIndex.Length; i++ )
                    writer.WriteLine(string.Format("\t{0}: \t{1}", i, mapping.WordCountsPerTaskIndex[i]));
            }
            return;

            resultsWords.RunBCCWords("BCCwords", goldData, data, model, Results.RunMode.ClearResults, true);
            stopWatch.Stop();
            GC.KeepAlive(model); // Keep the model alive to this point (for the	memory counter)
            float postMem = memCounter.NextValue();
            resultsWords.Memory = preMem - postMem;
            resultsWords.ExecutionTime = stopWatch.ElapsedMilliseconds / 1000;

            using (StreamWriter writer = new StreamWriter(ResultsDir + "BCCWordsOnGoldLabels_endpoints.csv", true))
            {
                resultsWords.WriteResults(writer, false, true, true, true, 300);
            }

            using (StreamWriter writer = new StreamWriter(Console.OpenStandardOutput()))
            {
                resultsWords.WriteResults(writer, false, false, false, true);
            }

            Console.WriteLine(String.Format("Approximate memory usage: {0:F2} MB", resultsWords.Memory));
            Console.WriteLine(String.Format("Approximate execution time (including model compilation): {0} seconds", stopWatch.ElapsedMilliseconds / 1000));
        }

    }
}
