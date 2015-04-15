//#define useSharedVariables

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MicrosoftResearch.Infer.Models;
using MicrosoftResearch.Infer;
using MicrosoftResearch.Infer.Distributions;
using MicrosoftResearch.Infer.Maths;
using System.IO;
using MicrosoftResearch.Infer.Utils;

namespace CrowdsourcingModels
{
    using DiscreteArray = DistributionRefArray<Discrete, int>;
    public class BCCWords : BCC
    {
        // Add extra ranges
        private Range w;
        private Range nw;

        // Model evidence
        private Variable<bool> evidence;

        // Set up model variables
#if useSharedVariables
        Model DefiningModel;
        Model UserModel;
        private SharedVariableArray<int> TruthSV;
        private VariableArray<int> TruthCopyForUserModels;
        int NumBatches;
#endif

        // Additional variables for BCCWords
        private VariableArray<Vector> ProbWord;
        private VariableArray<int> WordCount;
        private VariableArray<VariableArray<int>, int[][]> Words;
        private Variable<Dirichlet> ProbWordPrior;

#if useSharedVariables
        private InferenceEngine EngineForDefModel = new InferenceEngine(new VariationalMessagePassing())
        {
            ShowProgress = false
        };
#endif

        public void CreateModel(int NumTasks, int NumClasses, int VocabSize, int numBatches = 3)
        {

            WorkerCount = Variable.New<int>().Named("WorkerCount");

            // Set up inference engine
            Engine = new InferenceEngine(new VariationalMessagePassing());

            // Set engine flags
            Engine.ShowFactorGraph = false;
            Engine.ShowWarnings = true;
            Engine.ShowProgress = false;
            Engine.Compiler.WriteSourceFiles = true;
            Engine.Compiler.UseParallelForLoops = true;

            evidence = Variable.Bernoulli(0.5).Named("evidence");
            IfBlock block = Variable.If(evidence);

            // Set up ranges
            n = new Range(NumTasks).Named("N");
            c = new Range(NumClasses).Named("C");
            k = new Range(WorkerCount).Named("K");
            WorkerTaskCount = Variable.Array<int>(k).Named("WorkerTaskCount");
            kn = new Range(WorkerTaskCount[k]).Named("KN");
            WorkerTaskIndex = Variable.Array(Variable.Array<int>(kn), k).Named("Task");
            WorkerTaskIndex.SetValueRange(n);

            //v = new Range(WorkerTaskCount[k][kn]).Named("V");

            // Initialise truth
            BackgroundLabelProbPrior = Variable.New<Dirichlet>().Named("TruthProbPrior");
            BackgroundLabelProb = Variable<Vector>.Random(BackgroundLabelProbPrior).Named("TruthProb");
            BackgroundLabelProb.SetValueRange(c);

            // Truth distributions
            TrueLabel = Variable.Array<int>(n).Named("Truth");
            TrueLabel[n] = Variable.Discrete(BackgroundLabelProb).ForEach(n);

            //VocabSize = Variable.New<int>();
            w = new Range(VocabSize).Named("W");
            ProbWord = Variable.Array<Vector>(c).Named("ProbWord");
            ProbWord.SetValueRange(w);
            WordCount = Variable.Array<int>(n).Named("WordCount");
            nw = new Range(WordCount[n]).Named("WN");
            Words = Variable.Array(Variable.Array<int>(nw), n).Named("Word");
            ProbWordPrior = Variable.New<Dirichlet>().Named("ProbWordPrior");
            ProbWord[c] = Variable<Vector>.Random(ProbWordPrior).ForEach(c);

#if useSharedVariables

            TruthSV = SharedVariable<int>.Random(n, (DiscreteArray)(Distribution<int>.Array(Util.ArrayInit(NumTasks, t => Discrete.Uniform(NumClasses))))).Named("TruthSV");
            DefiningModel = new Model(1).Named("Defining Model");
            TruthSV.SetDefinitionTo(DefiningModel, TrueLabel);
            UserModel = new Model(numBatches).Named("UserModel");
            TruthCopyForUserModels = TruthSV.GetCopyFor(UserModel).Named("TruthCopyForUserModels");
            TruthCopyForUserModels.SetValueRange(c);
            //NumBatches = numBatches;
#endif

            // Initialise user profiles
            ConfusionMatrixPrior = Variable.Array(Variable.Array<Dirichlet>(c), k).Named("WorkerConfusionMatrixPrior");
            WorkerConfusionMatrix = Variable.Array(Variable.Array<Vector>(c), k).Named("WorkerConfusionMatrix");
            WorkerConfusionMatrix[k][c] = Variable<Vector>.Random(ConfusionMatrixPrior[k][c]);
            WorkerConfusionMatrix.SetValueRange(c);

            // Vote distributions
            WorkerLabel = Variable.Array(Variable.Array<int>(kn), k).Named("WorkerLabel");

            using (Variable.ForEach(k))
            {
                var trueLabel = Variable.Subarray(TrueLabel, WorkerTaskIndex[k]).Named("TrueLabelSubarray");
                trueLabel.SetValueRange(c);
                using (Variable.ForEach(kn))
                {
#if useSharedVariables
                    using (Variable.Switch(TruthCopyForUserModels[WorkerTaskIndex[k][kn]]))
                    {
                        WorkerLabel[k][kn] = Variable.Discrete(WorkerConfusionMatrix[k][TruthCopyForUserModels[WorkerTaskIndex[k][kn]]]);
                    }
#else
                    using (Variable.Switch(trueLabel[kn]))
                    {
                        WorkerLabel[k][kn] = Variable.Discrete(WorkerConfusionMatrix[k][trueLabel[kn]]);
                    }
#endif
                }
            }

            // Words inference
            using (Variable.ForEach(n))
            {
                using (Variable.Switch(TrueLabel[n]))
                {
                    Words[n][nw] = Variable.Discrete(ProbWord[TrueLabel[n]]).ForEach(nw);
                }
            }
            block.CloseBlock();
        }

        public void ClearVoteObservations()
        {
            WorkerLabel.ClearObservedValue();
            WorkerTaskCount.ClearObservedValue();
            WorkerTaskIndex.ClearObservedValue();
            WorkerTaskCount.ClearObservedValue();
        }


        private void ObserveCrowdLabels(int[][] workerLabel, int[][] workerTaskIndex)
        { 
            BackgroundLabelProbPrior.ObservedValue = Dirichlet.Uniform(c.SizeAsInt);
            WorkerCount.ObservedValue = workerLabel.Length;
            WorkerLabel.ObservedValue = workerLabel;
            WorkerTaskCount.ObservedValue = workerTaskIndex.Select(tasks => tasks.Length).ToArray();
            WorkerTaskIndex.ObservedValue = workerTaskIndex;
            SetBiasedPriors(WorkerCount.ObservedValue);
        }


        private void ObserveWords(int[][] words, int[] wordCounts)
        {

            //VocabSize.ObservedValue = words.Length;
            Words.ObservedValue = words;
            WordCount.ObservedValue = wordCounts;

        }

        private void ObserveTrueLabels(int[] trueLabels)
        {
            TrueLabel.ObservedValue = trueLabels;
        }

        public void SetUninformedPriors(int workerCount)
        {
            BackgroundLabelProbPrior.ObservedValue = Dirichlet.Uniform(c.SizeAsInt);
            ConfusionMatrixPrior.ObservedValue = Util.ArrayInit(workerCount, k => Util.ArrayInit(c.SizeAsInt, l => Dirichlet.Uniform(c.SizeAsInt)));
        }

        public void SetBiasedPriors(int workerCount)
        {
            // uniform over true values
            BackgroundLabelProbPrior.ObservedValue = Dirichlet.Uniform(c.SizeAsInt);
            ConfusionMatrixPrior.ObservedValue = Util.ArrayInit(workerCount, k => Util.ArrayInit(c.SizeAsInt, l => new Dirichlet(Util.ArrayInit(c.SizeAsInt, l1 => l1 == l ? 5.5 : 1))));
            ProbWordPrior.ObservedValue = Dirichlet.Symmetric(w.SizeAsInt, 1);
        }

        public void UpdatePriorsWithOldPosteriors(BCCWordsPosteriors posteriors)
        {
            BackgroundLabelProbPrior.ObservedValue = posteriors.BackgroundLabelProb;
            ConfusionMatrixPrior.ObservedValue = posteriors.WorkerConfusionMatrix;
        }


        /* Inference */
        public BCCWordsPosteriors InferPosteriors(
            int[][] workerLabel, int[][] workerTaskIndex, int[][] words, int[] wordCounts, int[] trueLabels = null,
#if useSharedVariables
int numIterations = 35, int numPasses = 5, int numIterationsPerPass = 10, int NumBatches = 3)
#else
            int numIterations = 35)
#endif
        {
            //NumBatches = (int)Math.Sqrt(workerLabel.Length);
            
            ObserveCrowdLabels(workerLabel, workerTaskIndex);

            ObserveWords(words, wordCounts);

            if (trueLabels != null)
            {
                ObserveTrueLabels(trueLabels);
            }

            BCCWordsPosteriors posteriors = new BCCWordsPosteriors();
#if useSharedVariables
            Console.WriteLine("\n***** Scalable BCC Words *****\n");
            Engine.OptimiseForVariables = new IVariable[] { WorkerConfusionMatrix, TruthCopyForUserModels };
            EngineForDefModel.OptimiseForVariables = new IVariable[]
            { TrueLabel,
              ProbWord,  
              BackgroundLabelProb };
            

            int numUsers = workerLabel.Length;
            posteriors.WorkerConfusionMatrix = new Dirichlet[numUsers][];
            int[] boundary = GetBatchBoundaries(numUsers, NumBatches);
            for (int pass = 0; pass < numPasses; pass++)
            {
                

                int numIterationsDefModel = 4;
                for (int it = 1; it <= numIterationsDefModel; it++)
                {
                    EngineForDefModel.NumberOfIterations = it;
                    DefiningModel.InferShared(EngineForDefModel, 0);
                    posteriors.BackgroundLabelProb = EngineForDefModel.Infer<Dirichlet>(BackgroundLabelProb);
                    posteriors.ProbWordPosterior = EngineForDefModel.Infer<Dirichlet[]>(ProbWord);

                    //Console.WriteLine("DefModel: Iteration {0}:\t{1:0.0000}", it, posteriors.BackgroundLabelProb);
                }

                Console.WriteLine("Pass {0} DefModel, :\t{1}", pass, posteriors.BackgroundLabelProb);

                Engine.NumberOfIterations = numIterationsPerPass;
                for (int batch = 0; batch < NumBatches; batch++)
                {
                    int startUser = boundary[batch];
                    int endUser = boundary[batch + 1];
                    if (startUser >= numUsers)
                        break;
                    int numUsersInThisBatch = endUser - startUser;

                    var batchLabel = workerLabel.Where((v, i) => i >= startUser && i < endUser).ToArray();
                    var batchTasks = workerTaskIndex.Where((v, i) => i >= startUser && i < endUser).ToArray();
                    ObserveCrowdLabels(batchLabel, batchTasks);

                    UserModel.InferShared(Engine, batch);
                    posteriors.TrueLabel = Distribution.ToArray<Discrete[]>(TruthSV.Marginal<IDistribution<int[]>>());
                    var batchCPTUserProfilesPosterior = Engine.Infer<Dirichlet[][]>(WorkerConfusionMatrix);
                    for (int u = startUser, i = 0; u < endUser; u++, i++)
                        posteriors.WorkerConfusionMatrix[u] = batchCPTUserProfilesPosterior[i];
                    Console.WriteLine("Pass {0} Batch {1}:\t{2:0.0000}", pass, batch, posteriors.TrueLabel[0]);

                }
            }

#else
            //Console.WriteLine("\n***** BCC Words *****\n");
            for (int it = 1; it <= numIterations; it++)
            {
                Engine.NumberOfIterations = it;
                posteriors.TrueLabel = Engine.Infer<Discrete[]>(TrueLabel);
                posteriors.WorkerConfusionMatrix = Engine.Infer<Dirichlet[][]>(WorkerConfusionMatrix);
                posteriors.BackgroundLabelProb = Engine.Infer<Dirichlet>(BackgroundLabelProb);
                posteriors.ProbWordPosterior = Engine.Infer<Dirichlet[]>(ProbWord);
                Console.WriteLine("Iteration {0}:\t{1:0.0000}", it, posteriors.TrueLabel[0]);
            }

            //Console.WriteLine();
            posteriors.Evidence = Engine.Infer<Bernoulli>(evidence);
#endif
            return posteriors;
        }


#if useSharedVariables
        public static int[] GetBatchBoundaries(int numUsers, int numBatches)
        {
            double numUsersPerBatch = ((double)numUsers) / numBatches;
            if (numUsersPerBatch == 0)
                numUsersPerBatch = 1;
            int[] boundary = new int[numBatches + 1];
            boundary[0] = 0;
            double currBoundary = 0.0;
            for (int batch = 1; batch <= numBatches; batch++)
            {
                currBoundary += numUsersPerBatch;
                int bnd = (int)currBoundary;
                if (bnd > numUsers)
                    bnd = numUsers;
                boundary[batch] = bnd;
            }
            boundary[numBatches] = numUsers;

            return boundary;
        }
#endif
    }
}
