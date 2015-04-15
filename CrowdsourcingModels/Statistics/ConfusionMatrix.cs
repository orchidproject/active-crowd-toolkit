using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrowdsourcingProject.Statistics
{
    /// <summary>
    ///   Confusion Matrix class
    /// </summary>
    public class ConfusionMatrix
    {

        //  2x2 confusion matrix
        private int truePositives;
        private int trueNegatives;
        private int falsePositives;
        private int falseNegatives;


        /// <summary>
        ///   Constructs a new Confusion Matrix.
        /// </summary>
        public ConfusionMatrix(int truePositives, int trueNegatives,
            int falsePositives, int falseNegatives)
        {
            this.truePositives = truePositives;
            this.trueNegatives = trueNegatives;
            this.falsePositives = falsePositives;
            this.falseNegatives = falseNegatives;
        }



        /// <summary>
        ///   Gets the number of observations for this matrix
        /// </summary>
        public int Observations
        {
            get
            {
                return trueNegatives + truePositives +
                    falseNegatives + falsePositives;
            }
        }

        /// <summary>
        ///   Gets the number of actual positives
        /// </summary>
        public int ActualPositives
        {
            get { return truePositives + falseNegatives; }
        }

        /// <summary>
        ///   Gets the number of actual negatives
        /// </summary>
        public int ActualNegatives
        {
            get { return trueNegatives + falsePositives; }
        }

        /// <summary>
        ///   Gets the number of predicted positives
        /// </summary>
        public int PredictedPositives
        {
            get { return truePositives + falsePositives; }
        }

        /// <summary>
        ///   Gets the number of predicted negatives
        /// </summary>
        public int PredictedNegatives
        {
            get { return trueNegatives + falseNegatives; }
        }



        /// <summary>
        ///   Cases correctly identified by the system as positives.
        /// </summary>
        public int TruePositives
        {
            get { return truePositives; }
        }

        /// <summary>
        ///   Cases correctly identified by the system as negatives.
        /// </summary>
        public int TrueNegatives
        {
            get { return trueNegatives; }
        }

        /// <summary>
        ///   Cases incorrectly identified by the system as positives.
        /// </summary>
        public int FalsePositives
        {
            get { return falsePositives; }
        }

        /// <summary>
        ///   Cases incorrectly identified by the system as negatives.
        /// </summary>
        public int FalseNegatives
        {
            get { return falseNegatives; }
        }

        /// <summary>
        ///   Sensitivity, also known as True Positive Rate
        /// </summary>
        /// <remarks>
        ///   Sensitivity = TPR = TP / (TP + FN)
        /// </remarks>
        public double Sensitivity
        {
            get { return (double)truePositives / (truePositives + falseNegatives); }
        }

        /// <summary>
        ///   Specificity, also known as True Negative Rate
        /// </summary>
        /// <remarks>
        ///   Specificity = TNR = TN / (FP + TN)
        ///    or also as:  TNR = (1-False Positive Rate)
        /// </remarks>
        public double Specificity
        {
            get { return (double)trueNegatives / (trueNegatives + falsePositives); }
        }

        /// <summary>
        ///  Efficiency, the arithmetic mean of sensitivity and specificity
        /// </summary>
        public double Efficiency
        {
            get { return (Sensitivity + Specificity) / 2.0; }
        }

        /// <summary>
        ///   Accuracy, or raw performance of the system
        /// </summary>
        /// <remarks>
        ///   ACC = (TP + TN) / (P + N)
        /// </remarks>
        public double Accuracy
        {
            get
            {
                return 1.0 * (truePositives + trueNegatives) / Observations;
            }
        }

        /// <summary>
        ///   Positive Predictive Value, also known as Positive Precision
        /// </summary>
        /// <remarks>
        ///   The Positive Predictive Value tells us how likely is 
        ///   that a patient has a disease, given that the test for
        ///   this disease is positive.
        ///      
        ///   It can be calculated as: PPV = TP / (TP + FP)
        /// </remarks>
        public double PositivePredictiveValue
        {
            get
            {
                double f = truePositives + FalsePositives;
                if (f != 0) return truePositives / f;
                return 1.0;
            }
        }

        /// <summary>
        ///   Negative Predictive Value, also known as Negative Precision
        /// </summary>
        /// <remarks>
        ///   The Negative Predictive Value tells us how likely it is
        ///   that the disease is NOT present for a patient, given that
        ///   the patient's test for the disease is negative.
        ///   
        ///   It can be calculated as: NPV = TN / (TN + FN)
        /// </remarks>
        public double NegativePredictiveValue
        {
            get
            {
                double f = (trueNegatives + falseNegatives);
                if (f != 0) return trueNegatives / f;
                else return 1.0;
            }
        }


        /// <summary>
        ///   False Positive Rate, also known as false alarm rate.
        /// </summary>
        /// <remarks>
        ///   It can be calculated as: FPR = FP / (FP + TN)
        ///                or also as: FPR = (1-specifity)
        /// </remarks>
        public double FalsePositiveRate
        {
            get
            {
                return (double)falsePositives / (falsePositives + trueNegatives);
            }
        }

        /// <summary>
        ///   False Discovery Rate, or the expected false positive rate.
        /// </summary>
        /// <remarks>
        ///   The False Discovery Rate is actually the expected false positive rate.
        ///   
        ///   For example, if 1000 observations were experimentally predicted to
        ///   be different, and a maximum FDR for these observations was 0.10, then
        ///   100 of these observations would be expected to be false positives.
        ///   
        ///   It is calculated as: FDR = FP / (FP + TP)
        /// </remarks>
        public double FalseDiscoveryRate
        {
            get
            {
                double d = falsePositives + truePositives;
                if (d != 0.0) return falsePositives / d;
                else return 1.0;
            }
        }

        /// <summary>
        ///   Matthews Correlation Coefficient, also known as Phi coefficient
        /// </summary>
        /// <remarks>
        ///   A coefficient of +1 represents a perfect prediction, 0 an
        ///   average random prediction and −1 an inverse prediction.
        /// </remarks>
        public double MatthewsCorrelationCoefficient
        {
            get
            {
                double s = System.Math.Sqrt(
                    (truePositives + falsePositives) *
                    (truePositives + falseNegatives) *
                    (trueNegatives + falsePositives) *
                    (trueNegatives + falseNegatives));

                if (s != 0.0)
                    return (truePositives * trueNegatives) / s;
                else return 0.0;
            }
        }
    }
}
