using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AcriveCrowdGUI
{

    /// <summary>
    /// Comparer class for sorting datasetItems in the data grid view 
    /// </summary>
    class DatasetItemComparer: IComparer<DatasetItem>
    {
        string memberName = string.Empty; // specifies the member name to be sorted
        SortOrder sortOrder = SortOrder.None; // Specifies the SortOrder.

        /// <summary>
        /// constructor to set the sort column and sort order.
        /// </summary>
        /// <param name="strMemberName"></param>
        /// <param name="sortingOrder"></param>
        public DatasetItemComparer(string strMemberName, SortOrder sortingOrder)
        {
            memberName = strMemberName;
            sortOrder = sortingOrder;
        }

        /// <summary>
        /// Compares two Students based on member name and sort order
        /// and return the result.
        /// </summary>
        /// <param name="datasetItem1"></param>
        /// <param name="datasetItem2"></param>
        /// <returns></returns>
        public int Compare(DatasetItem datasetItem1, DatasetItem datasetItem2)
        {
            int returnValue = 1;
            switch (memberName)
            {
                case "WorkerId" :

                    if (sortOrder == SortOrder.Ascending)
                    {
                        returnValue = datasetItem1.WorkerId.CompareTo(datasetItem2.WorkerId);
                    }
                    else
                    {
                        returnValue = datasetItem2.WorkerId.CompareTo(datasetItem1.WorkerId);
                    }

                    break;


                case "TaskId":
                    if (sortOrder == SortOrder.Ascending)
                    {
                        returnValue = datasetItem1.TaskId.CompareTo(datasetItem2.TaskId);
                    }
                    else
                    {
                        returnValue = datasetItem2.TaskId.CompareTo(datasetItem1.TaskId);
                    }
                    break;

                case "WorkerLabel":
                    if (sortOrder == SortOrder.Ascending)
                    {
                        returnValue = datasetItem1.WorkerLabel.CompareTo(datasetItem2.WorkerLabel);
                    }
                    else
                    {
                        returnValue = datasetItem2.WorkerLabel.CompareTo(datasetItem1.WorkerLabel);
                    }
                    break;

               case "GoldLabel":
                    if (sortOrder == SortOrder.Ascending)
                    {
                        returnValue = datasetItem1.GoldLabel.CompareTo(datasetItem2.GoldLabel);
                    }
                    else
                    {
                        returnValue = datasetItem2.GoldLabel.CompareTo(datasetItem1.GoldLabel);
                    }
                    break;

                default:
                    if (sortOrder == SortOrder.Ascending)
                    {
                        returnValue = datasetItem1.WorkerId.CompareTo(datasetItem2.WorkerId);
                    }
                    else
                    {
                        returnValue = datasetItem2.WorkerId.CompareTo(datasetItem1.WorkerId);
                    }
                    break;
            }
            return returnValue;
        }//end function
    }//end class


}//end namespace
