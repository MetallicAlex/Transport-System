using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TransportSystem
{
    public partial class Form2 : Form
    {
        public int NumberRoute { private set; get; }
        public int CapacityTransport { private set; get; }
        public int MinNumberTransportStop { private set; get; }
        public int MaxNumberTransportStop { private set; get; }
        public decimal FactorIntensity { private set; get; }
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.NumberRoute = (int)numericUpDown1.Value;
            this.CapacityTransport = (int)numericUpDown2.Value;
            this.MinNumberTransportStop = (int)numericUpDown3.Value;
            this.MaxNumberTransportStop = (int)numericUpDown4.Value;
            this.FactorIntensity = numericUpDown5.Value;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown3.Maximum = numericUpDown4.Value;
            if (numericUpDown3.Value > numericUpDown4.Value)
                numericUpDown3.Value = numericUpDown4.Value;
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown3_ValueChanged(sender, e);
        }
    }
}
