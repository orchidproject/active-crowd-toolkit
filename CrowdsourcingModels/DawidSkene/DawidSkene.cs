using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetAnotherLabel
{
    public class DawidSkene
    {
        public static char splitChar = ',';

        List<Labeling> labels;

        // Maps annotators, object, and classes to integers
        Dictionary<string, int> annotators;
        Dictionary<string, int> contributions;
        Dictionary<string, int> objects;
        public Dictionary<string, int> classes;

        // Given the ID's retrieve the names
        Dictionary<int, string> annotators_names;
        Dictionary<int, string> objects_names;
        Dictionary<int, string> classes_names;

        // Expected cost per annotator
        Dictionary<int, double> annotators_cost_naive;
        Dictionary<int, double> annotators_cost_adjusted;
        Dictionary<int, double> annotators_cost_minimized;

        /// <summary>
        /// We have I objects
        /// </summary>
        int I;
        /// <summary>
        /// We have J possible labels per object
        /// </summary>
        int J;
        /// <summary>
        /// We have K annotators
        /// </summary>
        int K;
        /// <summary>
        /// The labels given to each object from each annotator.
        /// label[k][i] is the label assigned by annotator k to object i.
        /// We assign -1 if it has not been labeled!!!!
        /// </summary>
        int[,] label;
        /// <summary>
        /// The correct labels for each object.
        /// correct[i] is the correct label for object i.
        /// We assign -1 if we do not know the correct label.
        /// </summary>
        int[] correct;
        /// <summary>
        /// Error rate (confusion matrix) for each annotator.
        /// pi[k][j][l] is the probability that annotator k, classifies an item from category j to category l
        /// </summary>
        double[, ,] pi;
        /// <summary>
        /// Cost matrix for various errors.
        /// cost[i][j] is the cost of classifying an item from the correct category i to the category j
        /// </summary>
        double[,] cost;
        /// <summary>
        /// Priors for the different classes.
        /// </summary>
        double[] prior;
        /// <summary>
        /// The probabilities of different labels for each object.
        /// T[oid][l] is the probability that the object oid belong to class l.
        /// </summary>
        double[,] T;

        public DawidSkene(List<Labeling> labels, List<Labeling> correct_labels, List<Labeling> classification_cost)
        {

            this.labels = labels;

            // Assign IDs to the labelers, objects, and classes
            this.annotators = new Dictionary<string, int>();
            this.classes = new Dictionary<string, int>();
            this.objects = new Dictionary<string, int>();
            this.contributions = new Dictionary<string, int>();

            // Assign IDs to the labelers, objects, and classes
            this.annotators_names = new Dictionary<int, string>();
            this.classes_names = new Dictionary<int, string>();
            this.objects_names = new Dictionary<int, string>();

            int aid = 0; // Annotator IDs
            int cid = 0; // Class IDs
            int oid = 0; // Object IDs
            foreach (Labeling l in labels) {
                string annotatorid = l.AnnotatorId;
                if (!annotators.ContainsKey(annotatorid)) {
                    //System.out.println("Annotator:" + annotatorid + "=" + aid);
                    annotators[annotatorid] = aid;
                    annotators_names[aid] = annotatorid;
                    contributions[annotatorid] = 0;
                    aid++;
                }

                string objectid = l.ObjectId;
                if (!objects.ContainsKey(objectid)) {
                    //System.out.println("Object:" + objectid + "=" + oid);
                    objects[objectid] = oid;
                    objects_names[oid] = objectid;
                    oid++;
                }

                string labelid = l.Label;
                if (!classes.ContainsKey(labelid)) {
                    //System.out.println("Label:" + labelid + "=" + cid);
                    classes[labelid] = cid;
                    classes_names[cid] = labelid;
                    cid++;
                }
            }

            if (correct_labels != null) {
                foreach (Labeling l in correct_labels) {

                    string objectid = l.ObjectId;
                    if (!objects.ContainsKey(objectid)) {
                        //System.out.println("Object:" + objectid + "=" + oid);
                        objects[objectid] = oid;
                        objects_names[oid] = objectid;
                        oid++;
                    }

                    string labelid = l.Label;
                    if (!classes.ContainsKey(labelid)) {
                        //System.out.println("Label:" + labelid + "=" + cid);
                        classes[labelid] = cid;
                        classes_names[cid] = labelid;
                        cid++;
                    }
                }
            }


            this.K = annotators.Keys.Count;
            this.I = objects.Keys.Count;
            this.J = classes.Keys.Count;

            this.pi = new double[K, J, J];
            InitializeErrorRates();

            this.prior = new double[J];
            InitializePriors();

            // Initialize labels to -1 (i.e., not labeled by annotator)
            this.label = new int[K, I];
            for (int k = 0; k < K; k++) {
                for (int i = 0; i < I; i++) {
                    label[k, i] = -1;
                }
            }

            // Initialize correct labels to -1 (i.e., we do not know the correct answers)
            this.correct = new int[I];
            for (int i = 0; i < I; i++) {
                this.correct[i] = -1;
            }

            // Load the labelings into the label array
            foreach (Labeling l in labels) {
                int labelerid = annotators[l.AnnotatorId];
                int objectid = objects[l.ObjectId];
                int classid = classes[l.Label];

                int curr = contributions[l.AnnotatorId];
                contributions[l.AnnotatorId] = curr + 1;

                this.label[labelerid, objectid] = classid;
            }

            // Load the correct labels the correct array
            if (correct_labels != null) {
                foreach (Labeling l in correct_labels) {
                    int objectid = objects[l.ObjectId];
                    int classid = classes[l.Label];

                    this.correct[objectid] = classid;
                }
            }


            // Initialize the class probability assignments
            this.T = new double[I, J];
            InitializeObjectClassProbabilities();

            // Initialize the cost array 
            this.cost = new double[J, J];
            InitializeCosts();
            if (classification_cost != null) {
                // TODO: Using the Labeling class to load cost 
                // is ugly. Just create a proper class, ensuring
                // that all costs are loaded properly etc.
                foreach (Labeling c in classification_cost) {

                    // The first column is the "from/correct" class
                    int from = classes[c.AnnotatorId];

                    // The second column is the "to/classified" class
                    int to = classes[c.ObjectId];

                    // The second column is the cost of classifying
                    // an object from the "correct" to the "classified" class
                    double decision_cost = double.Parse(c.Label);

                    this.cost[from, to] = decision_cost;
                }
            }

        }

        private void InitializeCosts()
        {
            for (int i = 0; i < J; i++) {
                for (int j = 0; j < J; j++) {
                    if (i == j) {
                        cost[i, j] = 0;
                    } else {
                        cost[i, j] = 1;
                    }

                }
            }


        }
        private void InitializeErrorRates()
        {
            // Set error rates to be not-so perfect, to allow for errors
            for (int lid = 0; lid < K; lid++) {
                for (int j = 0; j < J; j++) {
                    for (int k = 0; k < J; k++) {
                        if (j == k) {
                            pi[lid, j, k] = 0.9;
                        } else {
                            pi[lid, j, k] = 0.1 / (J - 1);
                        }
                    }
                }
            }
        }
        private void InitializeObjectClassProbabilities()
        {

            for (int oid = 0; oid < I; oid++) {

                // First check if we already have the correct label
                // for this object. If yes, then give the appropriate
                // value to the T[oid][j] cells, and continue 
                if (correct[oid] != -1) {
                    for (int j = 0; j < J; j++) {
                        if (correct[oid] == j) {
                            T[oid, j] = 1.0;
                        } else {
                            T[oid, j] = 0.0;
                        }
                    }
                    continue;
                }

                // Count how many times oid has been labeled
                // as belonging to each class, and total.
                int[] cnt = new int[J];
                int total = 0;
                for (int k = 0; k < K; k++) {
                    if (label[k, oid] != -1) {
                        int given_label = label[k, oid];
                        cnt[given_label]++;
                        total++;
                    }
                }

                // Count how many times oid has been labeled as class j
                for (int j = 0; j < J; j++) {
                    T[oid, j] = 1.0 * cnt[j] / total;
                }
            }
        }
        private void InitializePriors()
        {

            // Set priors to be uniform
            for (int j = 0; j < J; j++) {
                prior[j] = 1.0 / J;
            }
        }

        public void UpdateAnnotatorCosts()
        {
            annotators_cost_naive = new Dictionary<int, double>();
            annotators_cost_adjusted = new Dictionary<int, double>();
            annotators_cost_minimized = new Dictionary<int, double>();

            for (int k = 0; k < K; k++) {
                annotators_cost_naive[k] = GetAnnotatorCostNaive(k);
                annotators_cost_adjusted[k] = GetAnnotatorCostAdjusted(k);
                annotators_cost_minimized[k] = GetAnnotatorCostMinimized(k);
            }
        }
        public void UpdateAnnotatorErrorRates()
        {
            for (int lid = 0; lid < K; lid++) {
                double[,] confusion_matrix = new double[J, J];
                for (int j = 0; j < J; j++) {
                    for (int l = 0; l < J; l++) {
                        confusion_matrix[j, l] = 0;
                    }
                }
                // Scan all objects and change the confusion matrix for each annotator
                // using
                // the class probability for each object
                for (int oid = 0; oid < I; oid++) {
                    int l = label[lid, oid];
                    // If the worker has not annotated the object,
                    // we do not use it in estimating the error rate
                    if (l == -1)
                        continue;

                    // We now get the classification of the object
                    // based on the votes of all the other workers
                    double[] T_lid = GetObjectClassProbabilities(oid, lid);

                    // If nobody else has labeled the object oid,
                    // we have no information
                    if (T_lid == null)
                        continue;

                    for (int j = 0; j < J; j++) {
                        double d = T_lid[j];
                        //System.out.printf("d %d %d: %s\n",oid,j,double.toString(d));

                        confusion_matrix[j, l] += d;
                    }
                }

                // Compute the total number of J cases
                double[] marginal = new double[J];
                for (int j = 0; j < J; j++) {
                    for (int n = 0; n < J; n++) {
                        marginal[j] += confusion_matrix[j, n];
                    }
                }

                // Evaluate the values of the probabilistic error rates using the
                // confusion matrix
                for (int j = 0; j < J; j++) {
                    for (int l = 0; l < J; l++) {
                        pi[lid, j, l] = (marginal[j] > 0) ? Math.Round(1.0 * confusion_matrix[j, l] / marginal[j], 5) : -1.0;

                        // Version with Laplace smoothing
                        // pi[lid][j][l] =  Utils.round( (confusion_matrix[j][l]+1) / (marginal[j]+J), 5);
                    }
                }
            }
            //System.out.print(printAnnotatorCosts2(true));
        }
        public void UpdateObjectClassProbabilities()
        {
            for (int oid = 0; oid < I; oid++) {

                // First check if we already have the correct label
                // for this object. If yes, then give the appropriate
                // value to the T[oid][j] cells, and continue 
                if (correct[oid] != -1) {
                    // So if we **have** gold data, we get in this.
                    // Note: In principle, the following for-loop
                    // is unnecessary. The gold T values should have been
                    // correctly set during the initialization phase
                    // But we keep this here, just in case.
                    for (int j = 0; j < J; j++) {
                        if (correct[oid] == j) {
                            T[oid, j] = 1.0;
                        } else {
                            T[oid, j] = 0.0;
                        }
                    }

                    continue;

                }

                // If we do not have the correct label, then we proceed as 
                // usual with the M-phase of the EM-algorithm of Dawid&Skene
                for (int j = 0; j < J; j++) {
                    // Estimate nominator for Eq 2.5 of Dawid&Skene
                    double nom = prior[j];
                    // Go through all annotators
                    for (int k = 0; k < K; k++) {
                        // Find the label given by annotator K to the object oid
                        int given_label = label[k, oid];
                        if (given_label == -1)
                            continue;
                        // Having the annotator gave the given_label, and given how often
                        // given_label is mixed with j
                        double evidence_for_j = pi[k, j, given_label];

                        // Use the evidence only if we have a real estimate for pi[k,j,l] (i.e., it is not -1)
                        if (pi[k, j, given_label] > -0.1) {
                            nom *= evidence_for_j;
                        }
                    }

                    // Estimate denominator for Eq 2.5 of Dawid&Skene
                    double denom = 0;
                    for (int q = 0; q < J; q++) {
                        double pdenom = prior[q];
                        for (int k = 0; k < K; k++) {
                            int given_label = label[k, oid];
                            if (given_label == -1)
                                continue;
                            double evidence_for_q = pi[k, q, given_label];

                            // Use the evidence only if we have a real estimate for pi[k,j,l] (i.e., it is not -1)
                            if (pi[k, q, given_label] > -0.1) {
                                pdenom *= evidence_for_q;
                            }
                        }
                        denom += pdenom;
                    }
                    T[oid, j] = (denom > 0) ? Math.Round(nom / denom, 5) : 0.0;
                    //Tdenom[oid][j] = (denom > 0) ? Utils.round( nom / denom, 5) : 0.0;
                }
            }
        }

        public double[] GetObjectClassProbabilities(int oid, int lid)
        {
            // First check if we already have the correct label
            // for this object. If yes, then give the appropriate
            // value to the T[oid][j] cells, and continue 
            if (correct[oid] != -1) {
                // So if we **have** gold data, we get in this.
                // Note: In principle, the following for-loop
                // is unnecessary. The gold T values should have been
                // correctly set during the initialization phase
                // But we keep this here, just in case.
                for (int j = 0; j < J; j++) {
                    if (correct[oid] == j) {
                        T[oid, j] = 1.0;
                    } else {
                        T[oid, j] = 0.0;
                    }
                }
                double[] res = new double[J];
                for (int i = 0; i < J; i++)
                    res[i] = T[oid, i];
                return res;
            }

            // If we do not have the correct label, then we proceed as 
            // usual with the M-phase of the EM-algorithm of Dawid&Skene

            // Estimate denominator for Eq 2.5 of Dawid&Skene
            double denom = 0;
            for (int q = 0; q < J; q++) {
                double pdenom = prior[q];
                for (int k = 0; k < K; k++) {
                    int given_label = label[k, oid];
                    if (given_label == -1)
                        continue;
                    if (k == lid)
                        continue;

                    double evidence_for_q = pi[k, q, given_label];

                    // Use the evidence only if we have a real estimate for pi[k][j][l] (i.e., it is not -1)
                    if (pi[k, q, given_label] > -0.1) {
                        pdenom *= evidence_for_q;
                    }
                }
                denom += pdenom;
            }

            // If the denominator is 0, we have no basis for creating probability distribution
            if (denom == 0) return null;

            // This is the object probabilities without the influence of the annotator
            double[] Tnew = new double[J];

            for (int j = 0; j < J; j++) {
                // Estimate nominator for Eq 2.5 of Dawid&Skene
                double nom = prior[j];
                // Go through all annotators
                for (int k = 0; k < K; k++) {
                    // Find the label given by annotator K to the object oid
                    int given_label = label[k, oid];
                    if (given_label == -1)
                        continue;
                    if (k == lid)
                        continue;
                    // Having the annotator gave the given_label, and given how often
                    // given_label is mixed with j
                    double evidence_for_j = pi[k, j, given_label];

                    // Use the evidence only if we have a real estimate for pi[k][j][l] (i.e., it is not -1)
                    if (pi[k, j, given_label] > -0.1) {
                        nom *= evidence_for_j;
                    }
                }
                Tnew[j] = Math.Round(nom / denom, 5);
            }
            return Tnew;
        }

        public void UpdatePriors()
        {
            for (int j = 0; j < J; j++) {
                double pr = 0;
                for (int oid = 0; oid < I; oid++) {
                    pr += T[oid, j];
                }
                prior[j] = Math.Round(pr / I, 5);
            }
        }

        /// <summary>
        /// Estimates the cost for annotator k without attempting corrections of labels.
        /// </summary>
        /// <param name="k">k</param>
        /// <returns>The expected cost of misclassifications of annotator k.</returns>
        public double GetAnnotatorCostNaive(int k)
        {
            double c = 0;
            double s = 0;
            for (int i = 0; i < J; i++) {
                for (int j = 0; j < J; j++) {
                    if (pi[k, i, j] > 0) {
                        c += pi[k, i, j] * cost[i, j] * prior[i];
                        s += prior[i] * cost[i, j];
                    }
                }
            }
            if (s > 0) return c / s;
            return 0;
        }
        /// <summary>
        /// Estimates the cost for annotator k after adjusting class estimates using the error rates of the annotator.
        /// </summary>
        /// <param name="k">k</param>
        /// <returns>The expected cost of misclassifications of annotator k.</returns>
        public double GetAnnotatorCostMinimized(int k)
        {
            double c = 0;

            // The annotator will give one of the J possible labels for each example
            //
            // We estimate first how often the annotator will give label l
            // based on the "true priors" for each class and the error rates of annotator k

            double[] annotator_prior = new double[J];

            // Let's go over each of the classes and see how often this "true" class j 
            // gets misclassified as l
            // TODO: Create this at the initialization time. Just count how many times this
            // annotator classifies an object as l
            for (int l = 0; l < J; l++) {
                for (int j = 0; j < J; j++) {
                    if (pi[k, j, l] > -0.1) {
                        annotator_prior[l] += prior[j] * pi[k, j, l];
                    }
                }
            }

            // We now know the frequency with which we will see a label "assigned_label" from annotator k
            // Each of this "hard" labels from the annotator k will corresponds to a corrected
            // "soft" label
            for (int assigned_label = 0; assigned_label < J; assigned_label++) {
                // Let's find the soft label that corresponds to assigned_label
                double[] soft = new double[J];
                for (int i = 0; i < J; i++) {
                    if (pi[k, i, assigned_label] > -0.1 && annotator_prior[assigned_label] > 0) {
                        soft[i] = pi[k, i, assigned_label] * prior[i] / annotator_prior[assigned_label];
                    }
                }

                // And add the cost of this label, weighted with the prior of seeing this label.
                c += GetMinSoftLabelCost(soft) * annotator_prior[assigned_label];
            }
            return c / GetMinSpammerCost();
        }
        /// <summary>
        /// Estimates the cost for annotator k after adjusting class estimates using the error rates of the annotator
        /// </summary>
        /// <param name="k">k</param>
        /// <returns>The expected cost of misclassifications of annotator k</returns>
        public double GetAnnotatorCostAdjusted(int k)
        {
            double c = 0;

            // The annotator will give one of the J possible labels for each example
            //
            // We estimate first how often the annotator will give label l
            // based on the "true priors" for each class and the error rates of annotator k

            double[] annotator_prior = new double[J];

            // Let's go over each of the classes and see how often this "true" class j 
            // gets misclassified as l
            // TODO: Create this at the initialization time. Just count how many times this
            // annotator classifies an object as l
            for (int l = 0; l < J; l++) {
                for (int j = 0; j < J; j++) {
                    if (pi[k, j, l] > -0.1) {
                        annotator_prior[l] += prior[j] * pi[k, j, l];
                    }
                }
            }

            // We now know the frequency with which we will see a label "assigned_label" from annotator k
            // Each of this "hard" labels from the annotator k will corresponds to a corrected
            // "soft" label
            for (int assigned_label = 0; assigned_label < J; assigned_label++) {
                // Let's find the soft label that corresponds to assigned_label
                double[] soft = new double[J];
                for (int i = 0; i < J; i++) {
                    if (pi[k, i, assigned_label] > -0.1 && annotator_prior[assigned_label] > 0) {
                        soft[i] = pi[k, i, assigned_label] * prior[i] / annotator_prior[assigned_label];
                    }
                }

                // And add the cost of this label, weighted with the prior of seeing this label.
                c += GetSoftLabelCost(soft) * annotator_prior[assigned_label];
            }
            return c / GetSpammerCost();
        }
        /// <summary>
        /// Gets as input a "soft label" (i.e., a distribution of probabilities over classes) and returns the expected cost of this soft label.
        /// </summary>
        /// <param name="p">p</param>
        /// <returns>The expected cost of this soft label.</returns>
        private double GetSoftLabelCost(double[] p)
        {
            double c = 0;

            for (int i = 0; i < J; i++) {
                for (int j = 0; j < J; j++) {
                    c += p[i] * p[j] * cost[i, j];
                }
            }

            return c;
        }
        /// <summary>
        /// Gets as input a "soft label" (i.e., a distribution of probabilities over classes) and returns the expected cost of this soft label.
        /// </summary>
        /// <param name="p">p</param>
        /// <returns>The expected cost of this soft label.</returns>
        private double GetMinSoftLabelCost(double[] p)
        {
            double min_cost = double.MaxValue;
            for (int i = 0; i < J; i++) {
                // So, with probability p[i] it belongs to class i

                // What is the expected cost in this case?
                double costfor_i = 0;
                for (int j = 0; j < J; j++) {
                    costfor_i += p[j] * cost[i, j];
                }

                if (costfor_i < min_cost) {
                    min_cost = costfor_i;
                }
            }
            return min_cost;
        }
        /// <summary>
        /// Returns the cost of a "spammer" worker, who assigns completely random labels.
        /// </summary>
        /// <returns>The expected cost of a spammer worker.</returns>
        private double GetSpammerCost()
        {
            return GetSoftLabelCost(prior);
        }
        /// <summary>
        /// Returns the cost of a "spammer" worker, who assigns completely random labels.
        /// </summary>
        /// <returns>The expected cost of a spammer worker</returns>
        private double GetMinSpammerCost()
        {
            return GetMinSoftLabelCost(prior);
        }
        private int GetMajorityClass(int oid)
        {
            double max = -1;
            int majorityclass = -1;

            for (int j = 0; j < J; j++) {
                if (T[oid, j] > max) {
                    max = T[oid, j];
                    majorityclass = j;
                } else if (T[oid, j] == max) {
                    // In case of a tie, break ties based on the priors
                    if (prior[j] > prior[majorityclass]) {
                        max = T[oid, j];
                        majorityclass = j;
                    }
                }
            }
            return majorityclass;
        }
        public Dictionary<string, string> GetMajorityVote()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            for (int oid = 0; oid < I; oid++) {
                string object_name = objects_names[oid];
                int majority = GetMajorityClass(oid);
                string class_name = classes_names[majority];

                result[object_name] = class_name;
            }
            return result;
        }

        public void Estimate(int iterations)
        {
            for (int i = 0; i < iterations; i++) {
                //Console.WriteLine("DawidSkene: Iteration " + i);
                UpdateObjectClassProbabilities();
                UpdatePriors();
                UpdateAnnotatorErrorRates();
            }
        }

        public string PrintAnnotatorCostsSummary()
        {
            StringBuilder sb = new StringBuilder();

            for (int k = 0; k < K; k++) {
                string annotator_name = annotators_names[k];
                double cost_naive = annotators_cost_naive[k];
                double cost_adj = annotators_cost_adjusted[k];
                double cost_min = annotators_cost_minimized[k];

                int contribution = contributions[annotator_name];
                sb.AppendLine(annotator_name + splitChar + Math.Round(100 * cost_naive, 2) + "%" + splitChar + Math.Round(100 * (1 - cost_adj)) + "%" + splitChar +
                    Math.Round(100 * (1 - cost_min)) + "%" + splitChar + contribution);
            }
            return sb.ToString();
        }
        public string PrintAllWorkerScores()
        {
            StringBuilder sb = new StringBuilder();

            for (int k = 0; k < K; k++) {
                sb.Append(PrintWorkerScore(k));
            }
            return sb.ToString();
        }
        /// <summary>
        /// Like printAnnotatorCosts, but doesn't print cost_naive and cost_adj
        /// </summary>
        /// <param name="workerid"></param>
        /// <returns></returns>
        public string PrintWorkerScore(int workerid)
        {
            StringBuilder sb = new StringBuilder();

            string annotator_name = annotators_names[workerid];
            double cost_naive = annotators_cost_naive[workerid];
            double cost_adj = annotators_cost_adjusted[workerid];
            double cost_min = annotators_cost_minimized[workerid];

            int contribution = contributions[annotator_name];

            sb.AppendLine("Worker: " + annotator_name);
            sb.AppendLine("Error Rate: " + Math.Round(100 * cost_naive, 2) + "%");
            sb.AppendLine("Quality_Expected: " + Math.Round(100 * (1 - cost_adj)) + "%");
            sb.AppendLine("Quality_Optimized: " + Math.Round(100 * (1 - cost_min)) + "%");
            sb.AppendLine("Number of Annotations: " + contribution);
            sb.AppendLine("Confusion Matrix:");
            for (int i = 0; i < J; i++) {
                for (int j = 0; j < J; j++) {
                    string correct_name = classes_names[i];
                    string assigned_name = classes_names[j];
                    sb.Append("P[" + correct_name + "->" + assigned_name + "]=" + ((pi[workerid, i, j] > -0.1) ?
                        Math.Round(100 * pi[workerid, i, j], 3).ToString() : "----") + "%\t");
                }
                sb.AppendLine();
            }
            sb.AppendLine();

            return sb.ToString();
        }
        /// <summary>
        /// Prints the objects that have probability distributions with entropy higher than the given threshold.
        /// </summary>
        /// <param name="entropy_threshold">entropy_threshold</param>
        /// <returns></returns>
        public string PrintObjectClassProbabilities(double entropy_threshold)
        {

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < I; i++) {
                double entropy = DawidSkene.Entropy(T, i);
                if (entropy < entropy_threshold) continue;

                string object_name = objects_names[i];
                sb.Append(object_name + "\t");
                for (int j = 0; j < J; j++) {
                    string class_name = classes_names[j];
                    sb.Append("Pr[" + class_name + "]=" + T[i, j] + "\t");
                }
                sb.AppendLine();
            }

            return sb.ToString();

        }

        public void PrintObjectClassProbabilities()
        {

            for (int i = 0; i < I; i++)
            {

                string object_name = objects_names[i];
                Console.Write("Hit "+object_name + ":\t");
                for (int j = 0; j < J; j++)
                {
                    string class_name = classes_names[j];
                    Console.Write("Pr[" + class_name + "]=" + T[i, j] + "\t");
                }
                Console.Write("\n");
            }

        }

        public double[][] GetObjectClassProbabilities()
        {
            double[][] predVector = new double[I][];
            for (int i = 0; i < I; i++)
            {
                predVector[i] = new double[J];
                for (int j = 0; j < J; j++)
                {
                    predVector[i][j] = T[i, j];

                }

            }

            return predVector;

        }

        public string PrintPriors()
        {
            StringBuilder sb = new StringBuilder();
            for (int j = 0; j < J; j++) {
                string class_name = classes_names[j];
                sb.AppendLine("Prior[" + class_name + "]=" + prior[j]);
            }
            return sb.ToString();
        }
        public string PrintDiffVote(Dictionary<string, string> prior_voting, Dictionary<string, string> posterior_voting)
        {
            StringBuilder sb = new StringBuilder();

            foreach (string obj in prior_voting.Keys) { // TODO sorted keys?
                string prior_vote = prior_voting[obj];
                string posterior_vote = posterior_voting[obj];

                if (prior_vote == posterior_vote) {
                    sb.AppendLine("SAME\t" + obj + "\t" + prior_vote);
                } else {
                    sb.AppendLine("DIFF\t" + obj + "\t" + prior_vote + "->" + posterior_vote);
                }
            }
            return sb.ToString();
        }
        public string PrintVote()
        {
            StringBuilder sb = new StringBuilder();
            Dictionary<string, string> vote = GetMajorityVote();

            foreach (string obj in vote.Keys) { // TODO sorted keys?
                string majority_vote = vote[obj];
                sb.AppendLine(obj + "\t" + majority_vote);
            }
            return sb.ToString();
        }

        //public static void LoadLabels(List<string> lines, out List<Labeling> labelings, out List<Labeling> GoldLabels)
        //{
        //    Random r = new Random(10);
        //    GoldLabels = new List<Labeling>();
        //    labelings = new List<Labeling>();
        //    for (int i = 0; i < lines.Count && i < Program.NUM_JUDGMENTS; i++)
        //    {
                

        //        //Console.WriteLine(line);
        //        string line = null;
        //        if (Program.NUM_JUDGMENTS < 3000)
        //        {
        //            int range = lines.Count;
        //            int ind = r.Next(0, range);
        //            line = lines[ind];
        //            lines.RemoveAt(ind);
        //        }
        //        else
        //        {
        //            line = lines[i];
        //        }
        //        string[] entries = line.Split(Program.splitChar);


        //        // We need 3 entries per line. If the annotator has not provided an entry, ignore.
        //        // In the future, we may need to treat the empty submission as a separate class.
        //        Labeling lb = null;
        //        if (entries.Length == 3)
        //        {
        //            lb = new Labeling(entries[0], entries[1], entries[2], null);

        //        }
        //        else
        //        {
        //            lb = new Labeling(entries[0], entries[1], entries[2], entries[3]);
        //        }

        //        labelings.Add(lb);


        //        int HitId = Convert.ToInt32(entries[1]);
        //        int idxHit = ContainsHitId(GoldLabels, HitId);
        //        if (idxHit == -1)
        //        {
        //            // Hit not yet in the list, add it.
        //            GoldLabels.Add( new Labeling(entries[0], entries[1], entries[3], entries[3]));

        //        }


        //    }
        //}

        //public static int ContainsHitId(List<Labeling> HitList, int HitId)
        //{
        //    int indexHit = -1;
        //    for (int i = 0; i < HitList.Count; i++)
        //    {
        //        if (Convert.ToInt32(HitList[i].ObjectId) == HitId)
        //            return i;
        //    }
        //    return indexHit;
        //}

        public static double Entropy(double[,] p, int i)
        {
            double h = 0;
            for (int j = 0; j < p.GetLength(1); j++)
            {
                h += (p[i, j] > 0) ? p[i, j] * Math.Log(p[i, j]) : 0.0;
            }
            return -h;
        }
    }
}