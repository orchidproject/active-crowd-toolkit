// Authors: Julian Urbano and Panos Ipeirotis
//
// The original version of this code is available at: https://code.google.com/p/get-another-label-dotnet/

using System;

namespace GetAnotherLabel
{
    /// <summary>
    /// The data points in the format suitable for DawidSkene
    /// </summary>
    public class Labeling
    {
        /// <summary>
        /// The worker label.
        /// </summary>
        public string Label { get; protected set; }

        /// <summary>
        /// The worker id.
        /// </summary>
        public string AnnotatorId { get; protected set; }

        /// <summary>
        /// The task id.
        /// </summary>
        public string ObjectId { get; protected set; }

        /// <summary>
        /// The gold label.
        /// </summary>
        public string GoldLabel { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="o"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        public Labeling(string a, string o, string c, string d)
        {
            this.AnnotatorId = a;
            this.ObjectId = o;
            this.Label = c;
            this.GoldLabel = d;
        }

        //public override bool Equals(object obj)
        //{
        //    if (obj is Labeling) {
        //        Labeling p = (Labeling)obj;
        //        if (p.AnnotatorId == this.AnnotatorId && p.ObjectId == this.ObjectId) {
        //            return true;
        //        }
        //        return false;
        //    }
        //    throw new InvalidCastException();
        //}

        /// <summary>
        /// Return the hash code of a labelling object.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.AnnotatorId.GetHashCode() + this.ObjectId.GetHashCode() + this.Label.GetHashCode();
        }
    }
}
