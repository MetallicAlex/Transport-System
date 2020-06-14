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
        private List<Matrix> matrices;
        public Form5(int maxNumberRoute, int minNumberTransportStop, int maxNumberTransportStop, int capacityTranport, decimal factorIntensity)
        {
            InitializeComponent();
            progressBar1.Maximum = maxNumberRoute;
            matrices = new List<Matrix>();
            this.maxNumberRoute = maxNumberRoute;
            this.minNumberTransportStop = minNumberTransportStop;
            this.maxNumberTransportStop = maxNumberTransportStop;
            this.capacityTranport = capacityTranport;
            this.factorIntensity = factorIntensity;
        }
        public List<Matrix> GetMatrices()
        {
            return matrices;
        }
        private void Form5_Load(object sender, EventArgs e)
        {
            Random random = new Random();
            for (int i = 0; i < maxNumberRoute; i++)
            {
                Matrix matrix = new Matrix(random);
                matrix.Generate(minNumberTransportStop, maxNumberTransportStop, capacityTranport, factorIntensity);
                matrices.Add(matrix);
                progressBar1.PerformStep();
            }
        }
        private void Form5_Paint(object sender, PaintEventArgs e)
        {
            if (progressBar1.Value == progressBar1.Maximum)
            {
                this.Close();
            }
        }
        private void Form5_FormClosed(object sender, FormClosedEventArgs e)
        {
            MessageBox.Show("Маршруты сгенерированы", "Генерация Маршрутов");
        }
    }
}
