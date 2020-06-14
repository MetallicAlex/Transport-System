using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace TransportSystem
{
    public partial class Form3 : Form
    {
        private GraphicalSolution graphicalSolution;
        private TrasportSystem TrasportSystem { set; get; }
        private int indexMatrix;
        public Form3(TrasportSystem trasportSystem)
        {
            InitializeComponent();
            graphicalSolution = new GraphicalSolution();
            this.TrasportSystem = trasportSystem;
            this.indexMatrix = 0;
            this.textBox1.Text = indexMatrix.ToString();
            comboBox1.Text = comboBox1.Items[0].ToString();
            graphicalSolution.Visualize(TrasportSystem.Matrices[indexMatrix], dataGridView1);
            this.InitializeListMathStatistic();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (indexMatrix == 0)
                indexMatrix = TrasportSystem.Matrices.Count - 1;
            else indexMatrix--;
            this.textBox1.Text = indexMatrix.ToString();
            graphicalSolution.ClearMatrix(dataGridView1);
            graphicalSolution.Visualize(TrasportSystem.Matrices[indexMatrix], dataGridView1);
            graphicalSolution.ClearPlanRoute(chart1);
            graphicalSolution.Visualize(TrasportSystem, chart1, TrasportSystem.Matrices[indexMatrix].NumberTransportStop, comboBox1.Text, indexMatrix);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (indexMatrix == TrasportSystem.Matrices.Count - 1)
                indexMatrix = 0;
            else indexMatrix++;
            this.textBox1.Text = indexMatrix.ToString();
            graphicalSolution.ClearMatrix(dataGridView1);
            graphicalSolution.Visualize(TrasportSystem.Matrices[indexMatrix], dataGridView1);
            graphicalSolution.ClearPlanRoute(chart1);
            graphicalSolution.Visualize(TrasportSystem, chart1, TrasportSystem.Matrices[indexMatrix].NumberTransportStop, comboBox1.Text, indexMatrix);
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (int.TryParse(textBox1.Text, out indexMatrix))
                {
                    if (indexMatrix >= TrasportSystem.Matrices.Count)
                    {
                        indexMatrix = TrasportSystem.Matrices.Count - 1;
                    }
                    else if(indexMatrix < 0)
                    {
                        indexMatrix = 0;
                    }
                    this.textBox1.Text = indexMatrix.ToString();
                    graphicalSolution.ClearMatrix(dataGridView1);
                    graphicalSolution.Visualize(TrasportSystem.Matrices[indexMatrix], dataGridView1);
                    graphicalSolution.ClearPlanRoute(chart1);
                    graphicalSolution.Visualize(TrasportSystem, chart1, TrasportSystem.Matrices[indexMatrix].NumberTransportStop, comboBox1.Text, indexMatrix);
                }
                else MessageBox.Show("Вы должны вводить номер маршрута", "Маршрут", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void InitializeListMathStatistic()
        {
            foreach (var mathStatistic in TrasportSystem.algorithmRow.MathStatistics)
            {
                if (mathStatistic.numberRoute > 0)
                {
                    listBox1.Items.Add(string.Format("Статистика для маршрутов с {0} остановками", mathStatistic.numberTransportStop));
                    listBox1.Items.Add(string.Format("Кол-во маршрутов: {0}", mathStatistic.numberRoute));
                    listBox1.Items.Add(string.Format("Минимальное кол-во транспорта: {0}", mathStatistic.minNumberTransport));
                    listBox1.Items.Add(string.Format("Максимальное кол-во транспорта: {0}", mathStatistic.maxNumberTransport));
                    listBox1.Items.Add(string.Format("Математическое ожидание: {0}", Math.Round(mathStatistic.averageDistribution, 5)));
                    listBox1.Items.Add(string.Format("Дисперсия: {0}", Math.Round(mathStatistic.dispersion, 5)));
                    listBox1.Items.Add("");
                }
            }
            foreach (var mathStatistic in TrasportSystem.algorithmColumn.MathStatistics)
            {
                if (mathStatistic.numberRoute > 0)
                {
                    listBox2.Items.Add(string.Format("Статистика для маршрутов с {0} остановками", mathStatistic.numberTransportStop));
                    listBox2.Items.Add(string.Format("Кол-во маршрутов: {0}", mathStatistic.numberRoute));
                    listBox2.Items.Add(string.Format("Минимальное кол-во транспорта: {0}", mathStatistic.minNumberTransport));
                    listBox2.Items.Add(string.Format("Максимальное кол-во транспорта: {0}", mathStatistic.maxNumberTransport));
                    listBox2.Items.Add(string.Format("Математическое ожидание: {0}", Math.Round(mathStatistic.averageDistribution, 5)));
                    listBox2.Items.Add(string.Format("Дисперсия: {0}", Math.Round(mathStatistic.dispersion, 5)));
                    listBox2.Items.Add("");
                }
            }
            listBox1.Items.Add(string.Format("Время работы алгоритма в секундах: {0}", TrasportSystem.algorithmRow.AlgorithmRunningTime.Elapsed.TotalSeconds));
            listBox2.Items.Add(string.Format("Время работы алгоритма в секундах: {0}", TrasportSystem.algorithmColumn.AlgorithmRunningTime.Elapsed.TotalSeconds));
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Form6 form6 = new Form6(TrasportSystem.algorithmRow.PlanRoutes, TrasportSystem.algorithmColumn.PlanRoutes);
            form6.Show();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            graphicalSolution.ClearPlanRoute(chart1);
            graphicalSolution.Visualize(TrasportSystem, chart1, TrasportSystem.Matrices[indexMatrix].NumberTransportStop, comboBox1.Text, indexMatrix);
        }
    }
}
