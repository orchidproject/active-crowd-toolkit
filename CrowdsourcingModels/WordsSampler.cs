using MicrosoftResearch.Infer.Distributions;
using MicrosoftResearch.Infer.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelShared;
using CommunityBCC;
using MicrosoftResearch.Infer.Utils;

namespace BCCWords
{
    class WordsSampler
    {

        public static HitListManager SampleDataset(int numUsers, int numObjects, int numClasses, int numJudgments, int VocabSize)
        {

            int[] gtTruth = null;
            HitListManager hlm = CommunitySampler.SampleDataset(numUsers, numObjects, numClasses, numJudgments, out gtTruth);

            // Sample words probabilities
            Vector[] probWords = new Vector[numClasses];
            for (int c = 0; c < numClasses; c++)
            {
                var probVector = Vector.FromArray(Util.ArrayInit(VocabSize, w => 1.0 + 100.0* ((w+c) % numClasses)));
                probVector.Scale(1.0 / probVector.Sum());
                probWords[c] = probVector;
            }

            // Sample word counts
            int[] wordCounts = new int[numObjects];
            for (int i = 0; i < numObjects; i++)
            {
                wordCounts[i] = Poisson.Sample(3.0);
                //wordCounts[i]++;
            }

            // Sample word indeces
            int[][] words = new int[numObjects][];
            //Discrete samplingProb = Discrete.Uniform(VocabSize);
            for (int i = 0; i < numObjects; i++)
            {
                List<int> wordListPerObject = new List<int>();
                for (int j = 0; j < wordCounts[i]; j++)
                    wordListPerObject.Add(Discrete.Sample(probWords[gtTruth[i]]));

                words[i] = wordListPerObject.ToArray();

            }

            Console.WriteLine("\nWord Class probabilities");
            for (int c = 0; c < numClasses; c++)
                Console.WriteLine("Class" + c + ": " +Utils.arrayToString(probWords[c].ToArray()));


            HitListManager hlmwords = new HitListManager(hlm.HitList, hlm.NumJudges, hlm.NumJudgments, hlm.VotesPerJudge, hlm.UserId, hlm.VoteList, hlm.NumClasses, hlm.RESULTS_DIR, true);
            hlmwords.VocabSize = VocabSize;
            hlmwords.Words = words;
            hlmwords.WordCounts = wordCounts;
            return hlmwords;
        }


    }
}
