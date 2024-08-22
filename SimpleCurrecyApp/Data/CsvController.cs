using System.Data;
using System.IO;

namespace SimpleCurrecyApp.Data
{
    class CsvController
    {
        public static DataTable LoadCsvToDataTable(string filePath)
        {

            var dataTable = new DataTable();
            using (var reader = new StreamReader(filePath))
            {
                bool isFirstLine = true;

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    
                    if (isFirstLine)
                    {
                        dataTable.Columns.Add(values[0], typeof(string));
                        dataTable.Columns.Add(values[1], typeof(double));
                        dataTable.Columns.Add(values[2], typeof(string));
                        isFirstLine = false;
                    }
                    else
                    {
                        var dataRow = dataTable.NewRow();
                        dataRow.ItemArray = [values[0], double.Parse(values[1]), values[2]];
                        dataTable.Rows.Add(dataRow);
                    }
                }
            }

            return dataTable;
        }
        public static void SaveDataTableToCsv(DataTable dataTable, string filePath)
        {

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            File.Create(filePath).Close();


            var lines = new List<string>();
            string[] columnNames = dataTable.Columns.Cast<DataColumn>().Select(column => column.ColumnName).ToArray();
            var header = string.Join(",", columnNames);
            lines.Add(header);
            var valueLines = dataTable.AsEnumerable().Select(row => string.Join(",", row.ItemArray));
            lines.AddRange(valueLines);
            File.WriteAllLines(filePath, lines);
        }
    }
}
