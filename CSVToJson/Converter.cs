using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Data;

namespace CSVToJson
{
    public class CsvJsonConverter
    {

        public char ColumnSeparator { get; set; }
        public char ValueSeparator { get; set; }

        public CsvJsonConverter(char columnSeparator, char valueSeparator)
        {
            ColumnSeparator = columnSeparator;
            ValueSeparator = valueSeparator;
        }

        public string ConvertCSVtoJSON(string csv)
        {
            //get all lines
            var lines = csv.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            //if no header, show error
            if (lines.Count() < 2) return "Invalid CSV! Requires header row to construct key-value JSON.";

            //first line is the header - keys
            var keys = lines[0].Split(ColumnSeparator);

            //iterate lines
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            for (int i = 1; i < lines.Count(); i++)
            {
                if (i != 1) sb.Append(',');

                //if we have more columns then keys return error
                var values = lines[i].Split(ColumnSeparator);
                if (values.Count() != keys.Count())
                {
                    return "Error! Invalid column size.";
                }

                //start json object
                sb.Append("{");
                for (int j = 0; j < values.Count(); j++)
                {
                    if (j != 0) sb.Append(',');

                    //handle multi-valued column
                    if (values[j].Contains(ValueSeparator))
                    {
                        var subValues = values[j].Split(ValueSeparator).Select(v => v.Trim()).ToArray();
                        sb.Append(string.Format("\"{0}\":[\"", keys[j].Trim()));
                        sb.Append(string.Join("\"" + ValueSeparator + "\"", subValues));
                        sb.Append("\"]");
                        continue;
                    }

                    sb.Append(string.Format("\"{0}\":\"{1}\"", keys[j].Trim(), values[j].Trim()));
                }
                sb.Append("}");
            }
            sb.Append("]");

            return PrettifyJSON(sb.ToString());
        }

        public string ConvertJSONtoCSV(string json)
        {
            try
            {
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(json);
                StringBuilder sb = new StringBuilder();

                IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
                                      Select(column => column.ColumnName);
                sb.AppendLine(string.Join(ColumnSeparator.ToString(), columnNames));

                foreach (DataRow row in dt.Rows)
                {
                    IEnumerable<string> fields = row.ItemArray.Select(field =>
                    {
                        if (field.GetType().IsArray)
                        {
                            return string.Join(ValueSeparator.ToString(), field as string[]);
                        }
                        return field.ToString();
                    });
                    sb.AppendLine(string.Join(ColumnSeparator.ToString(), fields));
                }

                return sb.ToString().Trim();
            }
            catch (Exception e)
            {
                return "Error! Invalid Json.";
            }
        }


        public static string PrettifyJSON(string json)
        {
            dynamic parsedJson = JsonConvert.DeserializeObject(json);
            return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
        }

    }
}
