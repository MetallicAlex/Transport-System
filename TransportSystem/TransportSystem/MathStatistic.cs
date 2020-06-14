using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportSystem
{
    public struct MathStatistic
    {
        public double averageDistribution;
        public double dispersion;
        public int minNumberTransport;
        public int maxNumberTransport;
        public int numberTransportStop;
        public int numberRoute;
        public MathStatistic(double averageDistribution, double dispersion, int minNumberTransport, int maxNumberTransport, int numberTransportStop,int numberRoute)
        {
            this.averageDistribution = averageDistribution;
            this.dispersion = dispersion;
            this.minNumberTransport = minNumberTransport;
            this.maxNumberTransport = maxNumberTransport;
            this.numberTransportStop = numberTransportStop;
            this.numberRoute = numberRoute;
        }
    }
}
