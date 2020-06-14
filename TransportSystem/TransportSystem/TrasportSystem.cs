using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public bool IsMatricesGenerated()
        {
            if (Matrices != null)
                return true;
            return false;
        }
    }
}
