using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace TransportSystem
{
    class GraphicalSolution
    {
        private Graph graph = new Graph();
        private Color[] colors = new Color[20];
        public GraphicalSolution()
        {
            colors[0] = Color.Red; colors[1] = Color.Blue; colors[2] = Color.Green; colors[3] = Color.Brown; colors[4] = Color.Purple;
            colors[5] = Color.Cyan; colors[6] = Color.Coral; colors[7] = Color.DarkRed; colors[8] = Color.Gray; colors[9] = Color.DarkGreen;
            colors[10] = Color.YellowGreen; colors[11] = Color.BurlyWood; colors[12] = Color.Gold; colors[13] = Color.Firebrick; colors[14] = Color.DarkOrange;
            colors[15] = Color.DarkBlue; colors[16] = Color.Yellow; colors[17] = Color.DarkCyan; colors[18] = Color.BlueViolet; colors[19] = Color.DarkMagenta;
        }
        public void Visualize(Matrix matrix, DataGridView dataGridView)
        {
            Font font = new Font("Arial", 8);
            dataGridView.RowHeadersVisible = true;
            dataGridView.ColumnHeadersHeight = 22;
            dataGridView.RowHeadersWidth = 32;
            dataGridView.RowHeadersDefaultCellStyle.Font = font;
            dataGridView.ColumnHeadersDefaultCellStyle.Font = font;

            dataGridView.RowCount = matrix.NumberTransportStop;
            dataGridView.ColumnCount = matrix.NumberTransportStop;
            for (int i = 0; i < dataGridView.ColumnCount; i++)
            {
                dataGridView.Columns[i].HeaderCell.Style.Font = font;
                dataGridView.Columns[i].Name = i.ToString();
                dataGridView.Columns[i].Width = 22;
                dataGridView.Rows[i].Height = 15;
                dataGridView.Columns[i].DefaultCellStyle.Font = font;
            }

            for (int i = 0; i < dataGridView.RowCount; i++)
            {
                dataGridView.Rows[i].HeaderCell.Style.Font = font;
                dataGridView.Rows[i].HeaderCell.Value = i.ToString();
                for (int j = 0; j < dataGridView.ColumnCount; j++)
                {
                    dataGridView.Rows[i].Cells[j].Value = matrix[i, j].ToString();
                }
            }
            dataGridView.Refresh();
        }
        public void ClearMatrix(DataGridView dataGridView)
        {
            dataGridView.Rows.Clear();
            dataGridView.Columns.Clear();
            dataGridView.Refresh();
        }
        public void Visualize(TrasportSystem system, Chart chart, int numberTransportStop, string variableGraph,int index)
        {
            graph.CreateVertices(numberTransportStop);
            int vertex = 0;
            foreach (var point in graph.coordinatesVertices)
            {
                chart.Series[0].Points.AddXY(point.X, point.Y);
                chart.Series[0].Points[vertex].Label = vertex.ToString();
                vertex++;
            }
            switch (variableGraph)
            {
                case "Лучший результат":
                    this.Visualize(system.algorithmRow.PlanRoutes[index].PlanRouteTrasnport, chart);
                    break;
                case "Алгоритм по строкам":
                    this.Visualize(system.algorithmRow.PlanRoutes[index].PlanRouteTrasnport, chart);
                    break;
                case "Алгоритм по столбцам":
                    this.Visualize(system.algorithmRow.PlanRoutes[index].PlanRouteTrasnport, chart);
                    break;
                case "Маршрут лучший результат":
                    this.Visualize(system.algorithmRow.PlanRoutes[index].RouteTransport, chart);
                    break;
                case "Маршрут алгоритм по строкам":
                    this.Visualize(system.algorithmRow.PlanRoutes[index].RouteTransport, chart);
                    break;
                case "Маршрут алгоритм по столбцам":
                    this.Visualize(system.algorithmRow.PlanRoutes[index].RouteTransport, chart);
                    break;
            }
        }
        private void Visualize(List<List<Point>> planRouteTrasnport, Chart chart)
        {
            for (int i = 0; i < planRouteTrasnport.Count; i++)
            {
                Series series = new Series();
                series.Legend = "Legend2";
                series.Name = "Транспорт " + (i + 1).ToString();
                series.ChartType = SeriesChartType.Line;
                series.Color = colors[i % colors.Length];
                series.BorderWidth = 2;
                for (int j = 0; j < planRouteTrasnport[i].Count; j++)
                {
                    series.Points.AddXY(graph.coordinatesVertices[planRouteTrasnport[i][j].X].X, graph.coordinatesVertices[planRouteTrasnport[i][j].X].Y);
                    series.Points.AddXY(graph.coordinatesVertices[planRouteTrasnport[i][j].Y].X, graph.coordinatesVertices[planRouteTrasnport[i][j].Y].Y);
                }
                chart.Series.Add(series);
            }
            chart.Update();
        }
        private void Visualize(List<List<int>> planRouteTrasnport, Chart chart)
        {
            for (int i = 0; i < planRouteTrasnport.Count; i++)
            {
                Series series = new Series();
                series.Legend = "Legend2";
                series.Name = "Транспорт " + (i + 1).ToString();
                series.ChartType = SeriesChartType.Line;
                series.Color = colors[i % colors.Length];
                series.BorderWidth = 2;
                for (int j = 0; j < planRouteTrasnport[i].Count; j++)
                {
                    series.Points.AddXY(graph.coordinatesVertices[planRouteTrasnport[i][j]].X, graph.coordinatesVertices[planRouteTrasnport[i][j]].Y);
                }
                chart.Series.Add(series);
            }
            chart.Update();
        }
        public void ClearPlanRoute(Chart chart)
        {
            Series series = chart.Series[0];
            chart.Series.Clear();
            chart.Series.Add(series);
            chart.Series[0].Points.Clear();
        }
    }
}
