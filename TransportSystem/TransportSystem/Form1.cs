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
                Form5 form5 = new Form5(form2.NumberRoute, form2.MinNumberTransportStop, form2.MaxNumberTransportStop, form2.CapacityTransport, form2.FactorIntensity);
                form5.ShowDialog();
                trasportSystem.Matrices = form5.GetMatrices();
                trasportSystem.algorithmRow = new AlgorithmRow();
                trasportSystem.algorithmRow.OptimizeTransport(form5.GetMatrices(), form2.CapacityTransport);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (trasportSystem.IsMatricesGenerated())
            {
                List<MathStatistic> mathStatisticsRow = new List<MathStatistic>();
                mathStatisticsRow.Add(new MathStatistic(15.5, 14.0, 3, 6, 16, 2));
                mathStatisticsRow.Add(new MathStatistic(13.5, 12.0, 6, 7, 13, 3));
                List<MathStatistic> mathStatisticsCol = new List<MathStatistic>();
                mathStatisticsCol.Add(new MathStatistic(15.5, 14.0, 3, 6, 16, 2));
                Form3 form3 = new Form3(trasportSystem, mathStatisticsRow, mathStatisticsCol, 9.34, 53);
                form3.ShowDialog();
            }
            else MessageBox.Show("Маршруты не были созданы", "План Развозок", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(trasportSystem.IsMatricesGenerated())
            {
                Form4 form4 = new Form4();
                form4.ShowDialog();
            }
            else MessageBox.Show("Маршруты не были созданы", "Отчет", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
