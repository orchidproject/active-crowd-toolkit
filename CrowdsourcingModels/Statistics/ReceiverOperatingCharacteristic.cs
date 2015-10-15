using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrowdsourcingProject.Statistics
{
    /// <summary>
    ///   Receiver Operating Characteristic (ROC) Curve
    /// </summary>
    /// <remarks>
    ///   In signal detection theory, a receiver operating characteristic (ROC), or simply
    ///   ROC curve, is a graphical plot of the sensitivity vs. (1 − specificity) for a 
    ///   binary classifier system as its discrimination threshold is varied. 
    ///  
    /// References: 
    ///   http://en.wikipedia.org/wiki/Receiver_operating_characteristic
    ///   http://www.anaesthetist.com/mnm/stats/roc/Findex.htm
    ///   http://radiology.rsna.org/content/148/3/839.full.pdf
    /// </remarks>
    public class ReceiverOperatingCharacteristic
    {

        private double area = 0.0;
        private double error = 0.0;

        // The actual, measured data
        private double[] measurement;

        // The data, as predicted by a test
        private double[] prediction;


        // The real number of positives and negatives in the measured (actual) data
        private int positiveCount;
        private int negativeCount;

        /// <summary>
        /// The values which represent positive values in our
        /// measurement data (such as presence or absence of some disease)
        /// </summary>
        double dtrue;

        /// <summary>
        /// The values which represent positive and negative values in our
        /// measurement data (such as presence or absence of some disease)
        /// </summary>
        double dfalse;

        /// <summary>
        /// The collection to hold our curve point information.
        /// </summary>
        public PointCollection collection;

        /// <summary>
        ///   Constructs a new Receiver Operating Characteristic model
        /// </summary>
        /// <param name="measurement">An array of binary values. Tipically 0 and 1, or -1 and 1, indicating negative and positive cases, respectively.</param>
        /// <param name="prediction">An array of continuous values trying to approximate the measurement array.</param>
        public ReceiverOperatingCharacteristic(double[] measurement, double[] prediction)
        {
            this.measurement = measurement;
            this.prediction = prediction;

            // Determine which numbers correspont to each binary category
            dtrue = dfalse = measurement[0];
            for (int i = 1; i < measurement.Length; i++)
            {
                if (dtrue < measurement[i])
                    dtrue = measurement[i];
                if (dfalse > measurement[i])
                    dfalse = measurement[i];
            }

            // Count the real number of positive and negative cases
            for (int i = 0; i < measurement.Length; i++)
            {
                if (measurement[i] == dtrue)
                    this.positiveCount++;
            }

            // Negative cases is just the number of cases minus the number of positives
            this.negativeCount = this.measurement.Length - this.positiveCount;
        }



        #region Properties
        /// <summary>
        ///   Gets the points of the curve.
        /// </summary>
        public PointCollection Points
        {
            get { return collection; }
        }

        /// <summary>
        ///   Gets the number of actual positive cases.
        /// </summary>
        internal int Positives
        {
            get { return positiveCount; }
        }

        /// <summary>
        ///   Gets the number of actual negative cases.
        /// </summary>
        internal int Negatives
        {
            get { return negativeCount; }
        }

        /// <summary>
        ///   Gets the number of cases (observations) being analyzed.
        /// </summary>
        internal int Observations
        {
            get { return this.measurement.Length; }
        }

        /// <summary>
        ///  The area under the ROC curve. Also known as AUC-ROC.
        /// </summary>
        public double Area
        {
            get { return area; }
        }

        /// <summary>
        ///   Calculates the Standard Error associated with this ROC curve.
        /// </summary>
        public double Error
        {
            get { return error; }
        }
        #endregion


        #region Public Methods
        /// <summary>
        ///   Computes a n-points ROC curve.
        /// </summary>
        /// <remarks>
        ///   Each point in the ROC curve will have a threshold increase of
        ///   1/npoints over the previous point, starting at zero.
        /// </remarks>
        /// <param name="points">The number of points for the curve.</param>
        public void Compute(int points)
        {
            Compute((dtrue - dfalse) / points);
        }

        /// <summary>
        ///   Computes a ROC curve with 1/increment points
        /// </summary>
        /// <param name="increment">The increment over the previous point for each point in the curve.</param>
        public void Compute(double increment)
        {
            List<Point> points = new List<Point>();
            double cutoff;

            // Create the curve, computing a point for each cutoff value
            for (cutoff = dfalse; cutoff <= dtrue; cutoff += increment)
            {
                points.Add(ComputePoint(cutoff));
            }
            if (cutoff < dtrue) points.Add(ComputePoint(dtrue));

            // Sort the curve by descending specificity
            points.Sort(new Comparison<Point>(delegate(Point a, Point b)
            {
                return a.Specificity.CompareTo(b.Specificity);
            }
              ));

            // Create the point collection
            this.collection = new PointCollection(points.ToArray());

            // Calculate area and error associated with this curve
            this.area = calculateAreaUnderCurve();
            this.error = calculateStandardError();
        }


        Point ComputePoint(double threshold)
        {
            int truePositives = 0;
            int trueNegatives = 0;

            for (int i = 0; i < this.measurement.Length; i++)
            {
                bool measured = (this.measurement[i] == dtrue);
                bool predicted = (this.prediction[i] >= threshold);


                // If the prediction equals the true measured value
                if (predicted == measured)
                {
                    // We have a hit. Now we have to see
                    //  if it was a positive or negative hit
                    if (predicted == true)
                        truePositives++; // Positive hit
                    else trueNegatives++;// Negative hit
                }
            }



            // The other values can be computed from available variables
            int falsePositives = negativeCount - trueNegatives;
            int falseNegatives = positiveCount - truePositives;

            return new Point(this, threshold,
                truePositives, trueNegatives,
                falsePositives, falseNegatives);
        }


        /// <summary>
        ///   Compares two ROC curves.
        /// </summary>
        /// <param name="curve">The ROC curve to compare this to.</param>
        /// <param name="r">The amount of correlation between the two curves.</param>
        /// <returns></returns>
        public double Compare(ReceiverOperatingCharacteristic curve, double r)
        {
            // Areas
            double AUC1 = this.Area;
            double AUC2 = curve.Area;

            // Errors
            double se1 = this.Error;
            double se2 = curve.Error;

            // Standard error
            return (AUC1 - AUC2) / System.Math.Sqrt(se1 * se1 + se2 * se2 - 2 * r * se1 * se2);
        }
        #endregion


        #region Private Methods
        /// <summary>
        ///   Calculates the area under the ROC curve using the trapezium method
        /// </summary>
        private double calculateAreaUnderCurve()
        {
            double sum = 0.0;
            double tpz = 0.0;

            for (int i = 0; i < collection.Count - 1; i++)
            {
                // Obs: False Positive Rate = (1-specificity)
                tpz = collection[i].Sensitivity + collection[i + 1].Sensitivity;
                tpz = tpz * (collection[i].FalsePositiveRate - collection[i + 1].FalsePositiveRate) / 2.0;
                sum += tpz;
            }
            return sum;
        }

        /// <summary>
        ///   Calculates the standard error associated with this curve
        /// </summary>
        private double calculateStandardError()
        {
            double A = area;

            // real positive cases
            int Na = positiveCount;

            // real negative cases
            int Nn = negativeCount;

            double Q1 = A / (2.0 - A);
            double Q2 = 2 * A * A / (1.0 + A);

            return System.Math.Sqrt((A * (1.0 - A) +
                (Na - 1.0) * (Q1 - A * A) +
                (Nn - 1.0) * (Q2 - A * A)) / (Na * Nn));
        }
        #endregion



        #region Nested Classes

        /// <summary>
        ///   Object to hold information about a Receiver Operating Characteristic Curve Point
        /// </summary>
        public class Point : ConfusionMatrix
        {

            // Discrimination threshold (cutoff value)
            private double cutoff;

            // Parent curve
            ReceiverOperatingCharacteristic curve;

            /// <summary>
            ///   Constructs a new Receiver Operating Characteristic point.
            /// </summary>
            internal Point(ReceiverOperatingCharacteristic curve, double cutoff,
                int truePositives, int trueNegatives, int falsePositives, int falseNegatives)
                : base(truePositives, trueNegatives, falsePositives, falseNegatives)
            {
                this.curve = curve;
                this.cutoff = cutoff;
            }


            /// <summary>
            ///   Gets the cutoff value (discrimination threshold) for this point.
            /// </summary>
            public double Cutoff
            {
                get { return cutoff; }
            }
        }


        /// <summary>
        ///   Represents a Collection of Receiver Operating Characteristic (ROC) Curve points.
        ///   This class cannot be instantiated.
        /// </summary>
        public class PointCollection : ReadOnlyCollection<Point>
        {
            internal PointCollection(Point[] points)
                : base(points)
            {
            }

        }
        #endregion

    }
}
