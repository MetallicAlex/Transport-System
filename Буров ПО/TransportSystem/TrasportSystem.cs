using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace TransportSystem
{
    public class TrasportSystem
    {
        public List<PlanRoute> PlanRoutes { set; get; }
        public List<Matrix> Matrices { set; get; }
        public int NumberRoute { set; get; }
        public int CapacityTransport { set; get; }
        public int MinNumberTransportStop { set; get; }
        public int MaxNumberTransportStop { set; get; }
        public decimal FactorIntensity { set; get; }
        public AlgorithmRow algorithmRow;
        public AlgorithmColumn algorithmColumn;
        public bool IsMatricesGenerated()
        {
            if (Matrices != null)
                return true;
            return false;
        }
        public void DetermineBestRoutes()
        {
            PlanRoutes = new List<PlanRoute>();
            for (int i = 0; i < Matrices.Count; i++)
            {
                if (algorithmRow.PlanRoutes[i].NumberTransport < algorithmColumn.PlanRoutes[i].NumberTransport)
                    PlanRoutes.Add(algorithmRow.PlanRoutes[i]);
                else if(algorithmRow.PlanRoutes[i].NumberTransport == algorithmColumn.PlanRoutes[i].NumberTransport)
                {
                    if(algorithmRow.RouteDeterminationRunningTime[i].Elapsed < algorithmColumn.RouteDeterminationRunningTime[i].Elapsed)
                        PlanRoutes.Add(algorithmRow.PlanRoutes[i]);
                    else PlanRoutes.Add(algorithmColumn.PlanRoutes[i]);
                }
                else PlanRoutes.Add(algorithmColumn.PlanRoutes[i]);
            }
        }
    }
}
