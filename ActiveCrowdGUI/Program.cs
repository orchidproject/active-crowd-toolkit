using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using LINQtoCSV;
using CrowdsourcingModels;
namespace AcriveCrowdGUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
      
            //Main Page of the application
            Application.Run(new MainPage());
            
            //Loading Page of the application 
            //Application.Run(new LoadingPage());

        }//end Main method
        
        /// <summary>
        /// Loading the csv dataset file into DatasetItems
        /// </summary>
        /// <param name="csvFile"></param>
        /// <returns></returns>
        public static IEnumerable<DatasetItem> ReadFromCsv(string csvFile)
        {

            IEnumerable<DatasetItem> datasetItemList = null;
            try
            {
                //Read From Csv
                var csvFileDescription = new CsvFileDescription
                {
                    SeparatorChar = ',', //Specify the separator character.
                    FirstLineHasColumnNames = false,
                    FileCultureName = "en-US", // default is the current culture
                    EnforceCsvColumnAttribute = true
                };

                var csvContext = new CsvContext();

                datasetItemList = csvContext.Read<DatasetItem>(csvFile, csvFileDescription);

            }
            catch (AggregatedException ae) 
            {
                List<Exception> innerExceptionsList = (List<Exception>)ae.Data["InnerExceptionList"];

                foreach (Exception e in innerExceptionsList) 
                {
                    Console.WriteLine(e.Message);
                }

            }

            return datasetItemList;
        }
    } //End Class
} //End Namespace
