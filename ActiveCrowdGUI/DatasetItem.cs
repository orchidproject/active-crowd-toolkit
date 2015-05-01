using LINQtoCSV;

namespace AcriveCrowdGUI
{
    /// <summary>
    /// Class of each row of the dataset
    /// </summary>
    class DatasetItem
    {
        [CsvColumn(FieldIndex = 0, CanBeNull = false, Name = "WorkerId")]
        public string WorkerId { get; set; }

        [CsvColumn(FieldIndex = 1, CanBeNull = true, Name = "TaskId")]
        public string TaskId { get; set; }

        [CsvColumn(FieldIndex = 2, CanBeNull = true, Name = "WorkerLabel")]
        public int WorkerLabel { get; set; }

        [CsvColumn(FieldIndex = 3, CanBeNull = true, Name = "GoldLabel")]
        public int GoldLabel { get; set; }

        [CsvColumn(FieldIndex = 4, CanBeNull = true, Name = "TimeSpent(sec)")]
        public int TimeSpent { get; set; }
    }
}
