using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TransportSystem
{
    public class PlanRoute
    {
        public List<List<Point>> PlanRouteTrasnport { set; get; }
        public List<List<int>> RouteTransport { private set; get; }
        public int NumberTransport { private set; get; }
        public PlanRoute()
        {
            PlanRouteTrasnport = new List<List<Point>>();
        }
        public PlanRoute(List<List<Point>> planRouteTrasnport)
        {
            PlanRouteTrasnport = planRouteTrasnport;
            NumberTransport = planRouteTrasnport.Count;
            this.FindRoute();
        }
        private void FindRoute()
        {
            RouteTransport = new List<List<int>>();
            for (int i = 0; i < PlanRouteTrasnport.Count; i++)
            {
                RouteTransport.Add(new List<int>());
                SortedSet<int> route = new SortedSet<int>();
                for (int j = 0; j < PlanRouteTrasnport[i].Count; j++)
                {
                    route.Add(PlanRouteTrasnport[i][j].X);
                    route.Add(PlanRouteTrasnport[i][j].Y);
                }
                foreach (var item in route)
                    RouteTransport[i].Add(item);
            }
        }
    }
}
