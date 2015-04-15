using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrosoftResearch.Infer.Utils;
using MicrosoftResearch.Infer.Distributions;

namespace CrowdsourcingProject.Statistics
{
    class Statistics_test
    {
        public static void Test()
        {
            var realData = Util.ArrayInit(20, d => Bernoulli.Sample(0.5) ? 1.0 : 0.0);
            var testData = Util.ArrayInit(20, d => Beta.Sample(1, 1));

            // Creates the Receiver Operating Curve of the given source
            var rocCurve = new ReceiverOperatingCharacteristic(realData, realData);

            // Compute the ROC curve with 20 points
            rocCurve.Compute(20);

            for(int i=0; i < rocCurve.Points.Count; i++)
            {
                Console.WriteLine("ROC curve at point {0}: false positive rate {1:0.000}, true positive rate {2:0.000}, accuracy {3:0.000}", i, 1 - rocCurve.Points[i].Specificity, rocCurve.Points[i].Specificity, rocCurve.Points[i].Accuracy);
            }

            Console.WriteLine("Area under the ROC curve: {0:0.000}", rocCurve.Area);
        }

    }
}
