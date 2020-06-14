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
    public partial class Form6 : Form
    {
        public Form6(List<PlanRoute> planRoutes)
        {
            InitializeComponent();
            chart1.ChartAreas[0].AxisX.Title = "Номер маршруты";
            chart1.ChartAreas[0].AxisY.Title = "Кол-во транспорта";
            for(int i=0;i<planRoutes.Count;i++)
            {
                chart1.Series[0].Points.AddXY(i, planRoutes[i].NumberTransport);
            }
        }
    }
}
