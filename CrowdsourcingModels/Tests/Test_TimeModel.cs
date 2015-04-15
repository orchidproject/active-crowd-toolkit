using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrosoftResearch.Infer;
using MicrosoftResearch.Infer.Distributions;
using MicrosoftResearch.Infer.Maths;
using MicrosoftResearch.Infer.Models;
using MicrosoftResearch.Infer.Utils;

namespace CrowdsourcingModels
{
    public class Test_TimeModel
    {
        public static int[][] GenerateTimeData(
            int workerCount,
            int taskCount,
            out int[][] workerLabels,
            ref double[][] timeSpent,
            double percSpammer = 0.2,
            double initialWorkerBelief = 0.7,
            int labelCount = 3)
        {
            // Generate true labels
            Vector backgroundLabelProb = Dirichlet.Uniform(labelCount).Sample();
            Discrete trueLabelDistrib = new Discrete(backgroundLabelProb);
            int[] trueLabel = Util.ArrayInit<int>(taskCount, i => trueLabelDistrib.Sample());

            Console.WriteLine("***Ground truth task label***");
            for (int i = 0; i < taskCount; i++)
            {
                Console.WriteLine("Task {0}: {1}", i, trueLabel[i]);
            }

            // Generate worker confusion matrix
            Dirichlet[] confusionMatrixPrior = new Dirichlet[labelCount];
            for (int d = 0; d < labelCount; d++)
            {
                confusionMatrixPrior[d] = new Dirichlet(Util.ArrayInit(labelCount, i => i == d ? (initialWorkerBelief / (1.0 - initialWorkerBelief)) * (labelCount - 1) : 1.0));
            }

            Vector[][] workerConfusionMatrix = Util.ArrayInit(
                workerCount, k => Util.ArrayInit(
                    labelCount, d => confusionMatrixPrior[d].Sample()));

            using (StreamWriter writer = new StreamWriter(Console.OpenStandardOutput()))
            {
                writer.WriteLine("\n*** Ground truth worker confusion matrices ***");
                for (int k = 0; k < Math.Min(workerCount, 2); k++)
                {
                    Results.WriteWorkerConfusionMatrix(writer, k.ToString(), workerConfusionMatrix[k]);
                }
            }

            // Generate spammers
            Dirichlet dirUnif = Dirichlet.Uniform(labelCount);
            Vector[] spammerLabelProb = Util.ArrayInit(workerCount, k => dirUnif.Sample());
            bool[] isSpammer = Util.ArrayInit<bool>(workerCount, i => Bernoulli.Sample(percSpammer));

            Console.WriteLine("\n***Ground truth spammer flags***");
            for (int i = 0; i < workerCount; i++)
            {
                Console.WriteLine("Worker {0}: {1}", i, isSpammer[i]);
            }

            Console.WriteLine("\n*** Ground truth spammer label prob (worker 0) ***");
            Console.WriteLine("{0}", spammerLabelProb[0]);

            // Generate worker labels
            workerLabels = new int[workerCount][];
            int[][] taskIndex = new int[workerCount][];
            timeSpent = new double[workerCount][];
            double spammerTimeMean = 0.04;
            double spammerTimePrecision = 0.1;
            double nonSpammerTimeMean = 20;
            double nonSpammerTimePrecision = 0.1;
            int numJudgmentsPerWorker = 1000;
            for (int w = 0; w < workerCount; w++)
            {
                // Assume workers can judge a task only once
                // int[] perm = Rand.Perm(taskCount);
                // taskIndex[w] = perm.Take(NumJudgmentsPerWorker).ToArray();

                // Assume that workers can judge a task more than once
                taskIndex[w] = Util.ArrayInit<int>(numJudgmentsPerWorker, i => Rand.Int(0, taskCount));

                if (!isSpammer[w])
                {
                    workerLabels[w] = Util.ArrayInit<int>(taskIndex[w].Length, i => Discrete.Sample(workerConfusionMatrix[w][trueLabel[taskIndex[w][i]]]));
                    timeSpent[w] = Util.ArrayInit<double>(taskIndex[w].Length, i => Gaussian.Sample(nonSpammerTimeMean, nonSpammerTimePrecision));
                }
                else
                {
                    workerLabels[w] = Util.ArrayInit<int>(taskIndex[w].Length, i => Discrete.Sample(spammerLabelProb[w]));
                    timeSpent[w] = Util.ArrayInit<double>(taskIndex[w].Length, i => Gaussian.Sample(spammerTimeMean, spammerTimePrecision));
                }
            }

            Console.WriteLine("\n***Ground truth time distributions***");
            Console.WriteLine("Spammer: mean = {0}, precision = {1}", spammerTimeMean, spammerTimePrecision);
            Console.WriteLine("Non spammer: mean = {0}, precision = {1}\n", nonSpammerTimeMean, nonSpammerTimePrecision);

            return taskIndex;
        }

        public static List<Datum> GenerateSpammerData(IList<Datum> data, double perc, double spammerTimeMean, double spammerTimePrecision)
        {
            DataMapping mapping = new DataMapping(data);
            int numSpammers = (int) (mapping.WorkerCount * perc);
            List<Datum> spammerData = new List<Datum>();
            Dirichlet dirUnif = Dirichlet.Uniform(mapping.LabelCount);
            Dictionary<string, int?> goldLabels = mapping.GetGoldLabelsPerTaskId();
            Vector[] spammerLabelProb = Util.ArrayInit(numSpammers, k => dirUnif.Sample());
            int numJudgmentsPerSpammer = 2*300; // Mean value of per-worker judgment count for CF 
            for (int i=0; i<numSpammers; i++)
            {
                int[] taskIndex = Util.ArrayInit<int>(numJudgmentsPerSpammer, t => Rand.Int(0, mapping.TaskCount));
                for (int j = 0; j < numJudgmentsPerSpammer; j++)
                {
                    Datum d = new Datum
                        {
                            TaskId = mapping.TaskIndexToId[taskIndex[j]],
                            WorkerId = "Spammer " + i,
                            WorkerLabel = Discrete.Sample(spammerLabelProb[i]),
                            GoldLabel = goldLabels[mapping.TaskIndexToId[taskIndex[j]]],
                            TimeSpent = Gaussian.Sample(spammerTimeMean, spammerTimePrecision)
                        };
                    spammerData.Add(d);
                }
            }
            return spammerData;
        }

        public static void TestBCCTimeSpammer(int workerCount, int taskCount, int labelCount = 3, double percSpammer = 0.2, double initialWorkerBelief = 0.7)
        {

            int[][] taskIndex = null;
            int[][] workerLabels = null;
            double[][] timeSpent = null;
            taskIndex = GenerateTimeData(workerCount, taskCount, out workerLabels, ref timeSpent, percSpammer, initialWorkerBelief, labelCount);

            BCCTimeSpammer model = new BCCTimeSpammer();
            model.CreateModel(taskCount, labelCount);
            BCCTimeSpammerPosteriors result = model.Infer(taskIndex, workerLabels, timeSpent, null, taskCount);


            Console.WriteLine("\n***Inferred task label***");
            for (int i = 0; i < taskCount; i++)
            {
                Console.WriteLine("Task {0}: {1}, Mode: {2}", i, result.TrueLabel[i], result.TrueLabel[i].GetMode());
            }

            Console.WriteLine("\n***Inferred worker confusion matrix (worker 1)***");
            for (int i = 0; i < labelCount; i++)
            {
                Console.WriteLine(result.WorkerConfusionMatrix[1][i].GetMean());
            }

            Console.WriteLine("\n***Inferred spammer flags***");
            for (int i = 0; i < workerCount; i++)
            {
                Console.WriteLine("Worker {0}: {1:0.000}", i, result.IsSpammerPosterior[i].GetMode());
            }

            Console.WriteLine("\n***Inferred spammer label prob (worker 0)***");
            Console.WriteLine("{0}, {1}", result.SpammerLabelProbPosterior[0], result.SpammerLabelProbPosterior[0].GetMean());

            //Console.WriteLine("\n***Inferred time distributions***");
            //Console.WriteLine("Spammer: mean = {0}, precision = {1}", result.SpammerTimeMeanPosterior.GetMean(), result.SpammerTimePrecisionPosterior.GetMean());

            //for (int i = 0; i < taskCount; i++)
            //{
            //    Console.WriteLine("Task {2}: mean = {0}, precision = {1}", result.TaskNonSpammerTimeMeanPosterior[i].GetMean(), result.NonSpammerTimePrecisionPosterior.GetMean(), i);
            //}
        }


        public static void TestConjugateGammaFactor()
        {
            Rand.Restart(12347);
            Range S = new Range(10000); 

            var Engine = new InferenceEngine(new ExpectationPropagation());
            //var Engine = new InferenceEngine(new VariationalMessagePassing());
            Engine.ShowTimings = true;
            Engine.ShowWarnings = false;
            var shape = Variable.GammaFromShapeAndRate(2, 2);
            var rate = Variable.GammaFromShapeAndRate(2, 2);

            var sample = Variable.Array<double>(S);
            sample[S] = Variable.GammaFromShapeAndRate(shape, rate).ForEach(S);

            var distr = Gamma.FromShapeAndRate(2, 2);
            double sampledShape = distr.Sample();
            double sampledRate = distr.Sample();
            sample.ObservedValue = Util.ArrayInit<double>(S.SizeAsInt, i => Gamma.FromShapeAndRate(sampledShape, sampledRate).Sample());

            Console.WriteLine("Expected Shape: {0:0.000}, Inferred Shape: {1:0.000}", sampledShape, Engine.Infer<Gamma>(shape).GetMean());
            Console.WriteLine("Expected Rate: {0:0.000}, Inferred Rate: {1:0.000}", sampledRate, Engine.Infer<Gamma>(rate).GetMean());
        }

        //public static void TestBCCTimeSpammerGamma(int workerCount, int taskCount, int labelCount = 3, double percSpammer = 0.2, double initialWorkerBelief = 0.7)
        //{
        //    int[][] taskIndex = null;
        //    int[][] workerLabels = null;
        //    double[][] timeSpent = null;
        //    taskIndex = GenerateTimeData(workerCount, taskCount, out workerLabels, ref timeSpent, percSpammer, initialWorkerBelief, labelCount);

        //    BCCTimeSpammerGamma model = new BCCTimeSpammerGamma();
        //    model.CreateModel(taskCount, labelCount);
        //    BCCTimeSpammerGammaPosteriors result = model.Infer(taskIndex, workerLabels, timeSpent);


        //    Console.WriteLine("\n***Inferred task label***");
        //    for (int i = 0; i < taskCount; i++)
        //    {
        //        Console.WriteLine("Task {0}: {1}, Mode: {2}", i, result.TrueLabel[i], result.TrueLabel[i].GetMode());
        //    }

        //    Console.WriteLine("\n***Inferred worker confusion matrix (worker 1)***");
        //    for (int i = 0; i < labelCount; i++)
        //    {
        //        Console.WriteLine(result.WorkerConfusionMatrix[1][i].GetMean());
        //    }

        //    Console.WriteLine("\n***Inferred spammer flags***");
        //    for (int i = 0; i < workerCount; i++)
        //    {
        //        Console.WriteLine("Worker {0}: {1}", i, result.IsSpammerPosterior[i]);
        //    }

        //    Console.WriteLine("\n***Inferred spammer label prob (worker 0)***");
        //    Console.WriteLine("{0}, {1}", result.SpammerLabelProbPosterior[0], result.SpammerLabelProbPosterior[0].GetMean());

        //    Console.WriteLine("\n***Inferred time distributions***");
        //    Console.WriteLine("Spammer: shape = {0}, rate = {1}", result.SpammerTimeShapePosterior.GetMean(), result.SpammerTimeRatePosterior.GetMean());
        //    Console.WriteLine("Non spammer: shape = {0}, rate = {1}", result.NonSpammerTimeShapePosterior.GetMean(), result.NonSpammerTimeRatePosterior.GetMean());
        //}

    }
}
