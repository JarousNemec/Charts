using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Charts.Models;

namespace Charts
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private List<ChartPoint> _data;

        private void Form1_DoubleClick(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            var res = dialog.ShowDialog();
            if (res == DialogResult.OK)
            {
                _data = new List<ChartPoint>();
                using (StreamReader reader = new StreamReader(dialog.FileName))
                {
                    var line = string.Empty;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var row = line.Split(';');
                        _data.Add(new ChartPoint()
                        {
                            X = int.Parse(row[0]),
                            Y = int.Parse(row[1])
                        });
                    }
                }
                _chart.SetPoints(_data);
            }
        }
    }
}