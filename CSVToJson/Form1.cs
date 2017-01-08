using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSVToJson
{
    public partial class Form1 : Form
    {
        private CsvJsonConverter _converter;

        public Form1()
        {
            InitializeComponent();
            _converter = new CsvJsonConverter(';', ',');
        }

        private void UpdateDelimiters()
        {
            _converter.ColumnSeparator = textBox1.Text[0];
            _converter.ValueSeparator = textBox2.Text[0];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox2.Text = _converter.ConvertCSVtoJSON(richTextBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = _converter.ConvertJSONtoCSV(richTextBox2.Text);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            UpdateDelimiters();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            UpdateDelimiters();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
