using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace SimpleCurrecyApp.Data
{
    class CsvReader
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
                        foreach (var header in values) {
                            dataTable.Columns.Add(new DataColumn(header));
                        }
                        var dataRow = dataTable.NewRow();
                        dataRow.ItemArray = ["Select", "0"];
                        dataTable.Rows.Add(dataRow);
                        isFirstLine = false;
                    }
                    else
                    {
                        var dataRow = dataTable.NewRow();
                        dataRow.ItemArray = values;
                        dataTable.Rows.Add(dataRow);
                    }
                }
            }

            return dataTable;
        }
        
    }
}
