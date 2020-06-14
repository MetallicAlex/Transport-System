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
    public partial class Form6 : Form
    {
        public Form6(List<PlanRoute> planRoutesRow, List<PlanRoute> planRoutesColumn)
        {
            InitializeComponent();
            chart1.ChartAreas[0].AxisX.Title = "Номер маршруты";
            chart1.ChartAreas[0].AxisY.Title = "Кол-во транспорта";
            for (int i = 0; i < planRoutesRow.Count; i++)
            {
                chart1.Series[0].Points.AddXY(i, planRoutesRow[i].NumberTransport);
                chart1.Series[1].Points.AddXY(i, planRoutesColumn[i].NumberTransport);
            }
            chart1.SaveImage("Image.jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
        }
    }
}
