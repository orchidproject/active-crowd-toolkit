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
    public class Test_TimeModelMultimode
    {
        public static int[][] GenerateMultimodeTimeData(
            int workerCount,
            int taskCount,
            out int[][] workerLabels,
            ref double[][] timeSpent,
            double initialWorkerBelief = 0.7,
            int labelCount = 3)
        {
            // Generate true labels
            Vector backgroundLabelProb = Dirichlet.Symmetric(labelCount, 10.0).Sample();
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
            Dirichlet dirUnifLabels = Dirichlet.Uniform(labelCount);
            Vector[] spammerLabelProb = Util.ArrayInit(workerCount, k => dirUnifLabels.Sample());
            Dirichlet dirUnifSpammerTypes = Dirichlet.Uniform(3);
            Vector p = Vector.FromArray(new double[] {0.8, 0.1, 0.1});
            Discrete spammerTypeProb = new Discrete(p);
            int[] spammerType = Util.ArrayInit<int>(workerCount, i => spammerTypeProb.Sample());

            Console.WriteLine("\n***Ground truth spammer flags***");
            for (int i = 0; i < workerCount; i++)
            {
                Console.WriteLine("Worker {0}: {1}", i, spammerType[i]);
            }

            Console.WriteLine("\n*** Ground truth spammer label prob (worker 0) ***");
            Console.WriteLine("{0}", spammerLabelProb[0]);

            // Generate task time
            double taskTimeMean = 30;
            double taskTimePrecision = 0.1;
            double[] taskTime = Util.ArrayInit<double>(taskCount, i => Gaussian.Sample(taskTimeMean, taskTimePrecision));

            // Generate worker labels
            workerLabels = new int[workerCount][];
            int[][] taskIndex = new int[workerCount][];
            timeSpent = new double[workerCount][];
            double spammerLongTimeMean = 50;
            double spammerLongTimePrecision = 1;
            double spammerShortTimeMean = 10;
            double spammerShortTimePrecision = 1;
            int numJudgmentsPerWorker = 1000;
            double taskTimeWorkerPrecision = 1.0;
            for (int w = 0; w < workerCount; w++)
            {
                // Assume workers can judge a task only once
                // int[] perm = Rand.Perm(taskCount);
                // taskIndex[w] = perm.Take(NumJudgmentsPerWorker).ToArray();

                // Assume that workers can judge a task more than once
                taskIndex[w] = Util.ArrayInit<int>(numJudgmentsPerWorker, i => Rand.Int(0, taskCount));

                switch (spammerType[w])
                {

                    case 0:
                        workerLabels[w] = Util.ArrayInit<int>(taskIndex[w].Length, i => Discrete.Sample(workerConfusionMatrix[w][trueLabel[taskIndex[w][i]]]));
                        timeSpent[w] = Util.ArrayInit<double>(taskIndex[w].Length, i => Gaussian.Sample(taskTime[taskIndex[w][i]], taskTimeWorkerPrecision));
                        break;
                    case 1: // short spammer
                        workerLabels[w] = Util.ArrayInit<int>(taskIndex[w].Length, i => Discrete.Sample(spammerLabelProb[w]));
                        timeSpent[w] = Util.ArrayInit<double>(taskIndex[w].Length, i => Gaussian.Sample(spammerShortTimeMean, spammerShortTimePrecision));
                        break;
                    case 2: // long spammer
                        workerLabels[w] = Util.ArrayInit<int>(taskIndex[w].Length, i => Discrete.Sample(spammerLabelProb[w]));
                        timeSpent[w] = Util.ArrayInit<double>(taskIndex[w].Length, i => Gaussian.Sample(spammerLongTimeMean, spammerLongTimePrecision));
                        break;
                }
            }

            Console.WriteLine("\n***Ground truth time distributions***");
            Console.WriteLine("Short spammer: mean = {0}, precision = {1}", spammerShortTimeMean, spammerShortTimePrecision);
            Console.WriteLine("Long spammer: mean = {0}, precision = {1}", spammerLongTimeMean, spammerLongTimePrecision);
            Console.WriteLine("Task time: mean = {0}, precision = {1}\n", taskTimeMean, taskTimePrecision);
            for (int i = 0; i < taskCount; i++)
            {
                Console.WriteLine("Task {0}: Time {1}", i, taskTime[i]);
            }

            return taskIndex;
        }

        public static void TestBCCTimeSpammerMultimode(int workerCount, int taskCount, int labelCount = 3, double initialWorkerBelief = 0.7)
        {

            int[][] taskIndex = null;
            int[][] workerLabels = null;
            double[][] timeSpent = null;
            taskIndex = GenerateMultimodeTimeData(workerCount, taskCount, out workerLabels, ref timeSpent, initialWorkerBelief, labelCount);

            BCCTimeSpammerMultimode model = new BCCTimeSpammerMultimode();
            model.CreateModel(taskCount, labelCount);
            model.NumberOfIterations = 5;
            BCCTimeSpammerMultimodePosteriors result = model.Infer(taskIndex, workerLabels, timeSpent, taskCount, true);

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
                Console.WriteLine("Worker {0}: {1}", i, result.SpammerTypePosterior[i]);
            }

            Console.WriteLine("\n***Inferred spammer label prob (worker 0)***");
            Console.WriteLine("{0}, {1}", result.SpammerLabelProbPosterior[5], result.SpammerLabelProbPosterior[5].GetMean());


            Console.WriteLine("\n***Inferred time distributions***");
            Console.WriteLine("Short spammer: mean = {0}, precision = {1}", result.SpammerShortTimeMeanPosterior.GetMean(), result.SpammerShortTimePrecisionPosterior.GetMean());
            Console.WriteLine("Long spammer: mean = {0}, precision = {1}", result.SpammerLongTimeMeanPosterior.GetMean(), result.SpammerLongTimePrecisionPosterior.GetMean());


            for (int i = 0; i < taskCount; i++)
            {
                Console.WriteLine("Task {2}: mean = {0}, precision = {1}", result.TaskTimeMeanPosterior[i].GetMean(), result.TaskTimePrecisionPosterior.GetMean(), i);
            }
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
