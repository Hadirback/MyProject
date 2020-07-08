using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BindingTest
{
    public partial class Form1 : Form
    {
        private Meters meters { get; set; }
        public Form1()
        {
            InitializeComponent();
            meters = new Meters()
            {
                TotalIn = 1213,
                TotalInValid = 1
            };
            BindMeters();


            String test;

            if (Decimal.TryParse(textBox1.Text, out Decimal resultOverwiteDenom))
            {
                test = String.Format("overwrite_denomination = {0}", resultOverwiteDenom);
            }
            else
            {
                test = String.Format("overwrite_denomination = {0}", "NULL");
            }
        }

        private void BindMeters()
        {
            testLabel.DataBindings.Clear();
            testNumericUpDown.DataBindings.Clear();
            testLabel.DataBindings.Add(new Binding("Text", meters, "TotalInShow", false, DataSourceUpdateMode.OnPropertyChanged));
            testNumericUpDown.DataBindings.Add(new Binding("Value", meters, "TotalInValid", false, DataSourceUpdateMode.OnPropertyChanged));
        }

        private void checkButton_Click(object sender, EventArgs e)
        {
            totalInLabel.Text = meters.TotalIn.ToString();
            totalInValidLabel.Text = meters.TotalInValid.ToString();
        }

        private void innnerNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            meters.TotalInValid = Convert.ToInt32(innnerNumericUpDown.Value);
        }

        private void newObjButton_Click(object sender, EventArgs e)
        {
            Meters meters2 = new Meters()
            {
                TotalIn = 358,
                TotalInValid = 6
            };
            Console.WriteLine(testLabel.DataBindings.Count);
            testLabel.DataBindings.Clear();
            testNumericUpDown.DataBindings.Clear();
            testLabel.DataBindings.Add(new Binding("Text", meters2, "TotalInShow"));
            testNumericUpDown.DataBindings.Add(new Binding("Value", meters2, "TotalInValid"));
        }
    }

    public class Meters : INotifyPropertyChanged
    {
        public Int32 TotalIn { get; set; }
        private Int32 totalInShow;
        public Int32 TotalInShow { get { return totalInShow; } set { totalInShow = value; RaisePropertyChanged("TotalInShow"); } }

        private Int32 totalInValid;
        public Int32 TotalInValid
        { 
            get { return totalInValid; } 
            set { totalInValid = value; TotalInShow = totalInValid * TotalIn; } 
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


