using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TransportSystem
{
    public class Graph
    {
        public List<Point> coordinatesVertices;
        private Point centerCircle;
        private double angle;
        public Graph()
        {
            centerCircle.X = 330;
            centerCircle.Y = 200;
        }
        public void CreateVertices(int numberVertices)
        {
            angle = (2 * Math.PI / numberVertices);
            coordinatesVertices = new List<Point>();
            Point point = new Point(540, 410);
            coordinatesVertices.Add(point);
            for (int i = 0; i < numberVertices - 1; i++)
            {
                int rx = coordinatesVertices[i].X - centerCircle.X;
                int ry = coordinatesVertices[i].Y - centerCircle.Y;
                int x = (int)(centerCircle.X + rx * Math.Cos(angle) - ry * Math.Sin(angle));
                int y = (int)(centerCircle.Y + rx * Math.Sin(angle) + ry * Math.Cos(angle));
                coordinatesVertices.Add(new Point(x, y));
            }
        }
    }
}
