using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSVToJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVToJson.Tests
{
    [TestClass()]
    public class CsvJsonConverterTests
    {

        const string csv = "one;two;three;four\r\n1;2;3;four,4";
        const string json = @"[
  {
    ""one"": ""1"",
    ""two"": ""2"",
    ""three"": ""3"",
    ""four"": [
      ""four"",
      ""4""
    ]
  }
]";

        private CsvJsonConverter _converter = new CsvJsonConverter(';', ',');

        [TestMethod()]
        public void ConvertCSVtoJSONTest()
        {
            string result = _converter.ConvertCSVtoJSON(csv);
            Assert.AreEqual(json, result);
        }

        [TestMethod()]
        public void ConvertJSONtoCSVTest()
        {
            string result = _converter.ConvertJSONtoCSV(json);
            Assert.AreEqual(csv, result);
        }
    }
}