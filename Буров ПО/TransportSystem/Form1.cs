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
    public partial class Form1 : Form
    {
        private TrasportSystem trasportSystem;
        public Form1()
        {
            InitializeComponent();
            trasportSystem = new TrasportSystem();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
            if (DialogResult.Cancel == form2.DialogResult)
                MessageBox.Show("Вы не задали параметры маршрутов!", "Генерация Маршрутов", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                trasportSystem.NumberRoute = form2.NumberRoute;
                trasportSystem.MinNumberTransportStop = form2.MinNumberTransportStop;
                trasportSystem.MaxNumberTransportStop = form2.MaxNumberTransportStop;
                trasportSystem.FactorIntensity = form2.FactorIntensity;
                trasportSystem.CapacityTransport = form2.CapacityTransport;
                Form5 form5 = new Form5(form2.NumberRoute, form2.MinNumberTransportStop, form2.MaxNumberTransportStop, form2.CapacityTransport, form2.FactorIntensity);
                form5.ShowDialog();
                if (form5.DialogResult == DialogResult.OK)
                {
                    trasportSystem.Matrices = form5.Matrices;
                    trasportSystem.algorithmRow = form5.AlgorithmRow;
                    trasportSystem.algorithmColumn = form5.AlgorithmColumn;
                    trasportSystem.DetermineBestRoutes();
                }
                form5.Dispose();
            }
            form2.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (trasportSystem.IsMatricesGenerated())
            {
                Form3 form3 = new Form3(trasportSystem);
                form3.ShowDialog();
                form3.Dispose();
            }
            else MessageBox.Show("Маршруты не были созданы", "План Развозок", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(trasportSystem.IsMatricesGenerated())
            {
                Form4 form4 = new Form4(trasportSystem);
                form4.ShowDialog();
            }
            else MessageBox.Show("Маршруты не были созданы", "Отчет", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.IO.File.Delete("Image.jpeg");
        }
    }
}
