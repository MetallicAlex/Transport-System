using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TransportSystem
{
    public partial class Form5 : Form
    {
        private int maxNumberRoute;
        private int minNumberTransportStop;
        private int maxNumberTransportStop;
        private int capacityTranport;
        private decimal factorIntensity;
        public List<Matrix> Matrices { private set; get; }
        public AlgorithmRow AlgorithmRow { private set; get; }
        public AlgorithmColumn AlgorithmColumn { private set; get; }
        public Form5(int maxNumberRoute, int minNumberTransportStop, int maxNumberTransportStop, int capacityTranport, decimal factorIntensity)
        {
            InitializeComponent();
            progressBar1.Maximum = maxNumberRoute;
            Matrices = new List<Matrix>();
            this.maxNumberRoute = maxNumberRoute;
            this.minNumberTransportStop = minNumberTransportStop;
            this.maxNumberTransportStop = maxNumberTransportStop;
            this.capacityTranport = capacityTranport;
            this.factorIntensity = factorIntensity;
        }
        private void Form5_Paint(object sender, PaintEventArgs e)
        {
            if (progressBar3.Value == progressBar3.Maximum)
            {
                DialogResult = DialogResult.OK;
                MessageBox.Show("Маршруты сгенерированы", "Генерация Маршрутов");
                this.Close();
            }
        }
        private void Form5_Shown(object sender, EventArgs e)
        {
            Random random = new Random();
            for (int i = 0; i < maxNumberRoute; i++)
            {
                Matrix matrix = new Matrix(random);
                matrix.Generate(minNumberTransportStop, maxNumberTransportStop, capacityTranport, factorIntensity);
                Matrices.Add(matrix);
                progressBar1.Value++;
                progressBar1.Update();
            }
            label1.Text = "Маршруты сгенерированы";
            label2.Text = "Идет выполнение алгоритма по строкам...";
            AlgorithmRow = new AlgorithmRow(minNumberTransportStop, maxNumberTransportStop);
            AlgorithmRow.OptimizeTransport(Matrices, capacityTranport);
            AlgorithmRow.CollectStatistic();
            progressBar2.Value++;
            progressBar2.Update();
            label2.Text = "Алгоритм по строкам выполнен";
            label3.Text = "Идет выполнение алгоритма по столбцам...";
            AlgorithmColumn = new AlgorithmColumn(minNumberTransportStop, maxNumberTransportStop);
            AlgorithmColumn.OptimizeTransport(Matrices, capacityTranport);
            AlgorithmColumn.CollectStatistic();
            progressBar3.Value++;
            progressBar3.Update();
            label3.Text = "Алгоритм по столбцам выполнен";
        }
    }
}
