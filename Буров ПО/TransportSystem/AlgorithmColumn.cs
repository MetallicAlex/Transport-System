using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;

namespace TransportSystem
{
    public class AlgorithmColumn
    {
		public List<PlanRoute> PlanRoutes { private set; get; }
		public List<MathStatistic> MathStatistics { private set; get; }
		private List<Matrix> matrices;
		private int capacityTransport;
		private int minNumberTransportStop;
		private int maxNumberTransportStop;
		private List<int> numberPassengersDropOffTransportStop;
		public Stopwatch AlgorithmRunningTime { private set; get; }
		public List<Stopwatch> RouteDeterminationRunningTime { private set; get; }
		public AlgorithmColumn(int minNumberTransportStop, int maxNumberTransportStop)
		{
			this.minNumberTransportStop = minNumberTransportStop;
			this.maxNumberTransportStop = maxNumberTransportStop;
		}
		public void OptimizeTransport(List<Matrix> matrices, int capacityTransport)
		{
			this.CopyMatrices(matrices);
			this.capacityTransport = capacityTransport;
			PlanRoutes = new List<PlanRoute>();
			AlgorithmRunningTime = new Stopwatch();
			RouteDeterminationRunningTime = new List<Stopwatch>();
			AlgorithmRunningTime.Start();
			foreach (var matrix in this.matrices)
			{
				RouteDeterminationRunningTime.Add(new Stopwatch());
				RouteDeterminationRunningTime[RouteDeterminationRunningTime.Count - 1].Start();
				PlanRoutes.Add(new PlanRoute(this.DistributeTransport(matrix)));
				RouteDeterminationRunningTime[RouteDeterminationRunningTime.Count - 1].Stop();
			}
			AlgorithmRunningTime.Stop();
		}
		public void CollectStatistic()
		{
			int numberDifferentTransportStop = maxNumberTransportStop - minNumberTransportStop + 1;
			List<List<PlanRoute>> routes = new List<List<PlanRoute>>();
			MathStatistics = new List<MathStatistic>();
			for (int i = 0; i < numberDifferentTransportStop; i++)
			{
				routes.Add(new List<PlanRoute>());
				MathStatistics.Add(new MathStatistic());
			}
			for (int i = 0; i < PlanRoutes.Count; i++)
			{
				routes[matrices[i].NumberTransportStop - minNumberTransportStop].Add(PlanRoutes[i]);
			}
			for (int i = 0; i < routes.Count; i++)
			{
				MathStatistic mathStatistic = MathStatistics[i];
				mathStatistic.averageDistribution = ComputeAverageDistribution(routes[i]);
				mathStatistic.dispersion = ComputeDispersion(routes[i]);
				mathStatistic.minNumberTransport = FindMinNumberTransport(routes[i]);
				mathStatistic.maxNumberTransport = FindMaxNumberTransport(routes[i]);
				mathStatistic.numberTransportStop = minNumberTransportStop + i;
				mathStatistic.numberRoute = routes[i].Count;
				MathStatistics[i] = mathStatistic;
			}
		}
		private double ComputeAverageDistribution(List<PlanRoute> routes)
		{
			double averageDistribution = 0;
			foreach (var route in routes)
				averageDistribution += route.NumberTransport;
			return averageDistribution / routes.Count;
		}
		private double ComputeDispersion(List<PlanRoute> routes)
		{
			double dispersion = 0;
			double averageDistribution = 0;
			double averageDistributionPow2 = 0;
			foreach (var route in routes)
			{
				averageDistribution += route.NumberTransport * route.NumberTransport;
				averageDistributionPow2 += route.NumberTransport;
			}
			averageDistribution /= routes.Count;
			averageDistributionPow2 /= routes.Count;
			dispersion = averageDistribution - averageDistributionPow2 * averageDistributionPow2;
			return dispersion;
		}
		private int FindMinNumberTransport(List<PlanRoute> routes)
		{
			PlanRoute minNumberTransport = routes.Min();
			if (minNumberTransport == null)
				return 0;
			return minNumberTransport.NumberTransport;
		}
		private int FindMaxNumberTransport(List<PlanRoute> routes)
		{
			PlanRoute maxNumberTransport = routes.Max();
			if (maxNumberTransport == null)
				return 0;
			return maxNumberTransport.NumberTransport;
		}
		public List<List<Point>> DistributeTransport(Matrix matrix)
		{
			List<List<Point>> pathTransports = new List<List<Point>>();
			pathTransports.Add(new List<Point>());
			numberPassengersDropOffTransportStop = new List<int>();
			for (int i = 0; i < matrix.NumberTransportStop; i++)
				numberPassengersDropOffTransportStop.Add(0);
			int numberTransport = 1;
			int numberPassengersInTransport = 0;
			for (int i = matrix.NumberTransportStop - 1; i >= 0; i--)
			{
				for (int j = 0; j < matrix.NumberTransportStop; j++)
				{
					if (i != j)
					{
						if (matrix[j, i] > 0)
						{
							numberPassengersInTransport += matrix[j, i];
							if (numberPassengersInTransport <= 40)
							{
								pathTransports[numberTransport - 1].Add(new Point(j, i));
								numberPassengersDropOffTransportStop[j] += matrix[j, i];
							}
							else
							{
								numberPassengersInTransport -= matrix[j, i];
								this.TakeMorePassengers(pathTransports[numberTransport - 1], i, matrix);
								this.DropOffPassengers(matrix.NumberTransportStop);
								numberTransport++;
								numberPassengersInTransport = matrix[j, i];
								numberPassengersDropOffTransportStop[j] += matrix[j, i];
								pathTransports.Add(new List<Point>());
								pathTransports[numberTransport - 1].Add(new Point(j, i));
							}
						}
					}
					else
					{
						this.TakeMorePassengers(pathTransports[numberTransport - 1], i, matrix);
						this.DropOffPassengers(matrix.NumberTransportStop);
						if (this.AreThereAnyMorePassengers(matrix))
						{
							numberTransport++;
							pathTransports.Add(new List<Point>());
						}
						numberPassengersInTransport = 0;
					}
				}
			}
			return pathTransports;
		}
		private void TakeMorePassengers(List<Point> pathTransport, int lastColumn, Matrix matrix)
		{
			int nowNumberPassengersInTransport = 0;
			int subNumberPassengersInTransport;
			List<int> newPassengers = new List<int>();
			for (int i = 0; i < matrix.NumberTransportStop; i++)
			{
				nowNumberPassengersInTransport += numberPassengersDropOffTransportStop[i];
				subNumberPassengersInTransport = nowNumberPassengersInTransport;
				for (int j = i + 1; j < lastColumn; j++)
				{
					if (matrix[i, j] > 0)
					{
						if (j != i + 1)
						{
							subNumberPassengersInTransport += numberPassengersDropOffTransportStop[j - 1];
						}
						subNumberPassengersInTransport += matrix[i, j];
						if (subNumberPassengersInTransport <= 40)
						{
							pathTransport.Add(new Point(i, j));
							nowNumberPassengersInTransport += matrix[i, j];
						}
						else
						{
							for (int k = 0; k < pathTransport.Count; k++)
							{
								if (pathTransport[k].Y == i + 1)
								{
									nowNumberPassengersInTransport -= matrix[pathTransport[k].X, pathTransport[k].Y];
								}
							}
							break;
						}
					}
				}
			}
			this.DropOffPassengers(matrix, pathTransport);
		}
		private void DropOffPassengers(int numberTransportStop)
		{
			numberPassengersDropOffTransportStop = new List<int>();
			for (int i = 0; i < numberTransportStop; i++)
				numberPassengersDropOffTransportStop.Add(0);
		}
		private void DropOffPassengers(Matrix matrix, List<Point> pathTransport)
		{
			for (int i = 0; i < pathTransport.Count; i++)
			{
				matrix[pathTransport[i].X, pathTransport[i].Y] = 0;
			}
		}
		private bool AreThereAnyMorePassengers(Matrix matrix)
		{
			for (int i = 0; i < matrix.NumberTransportStop; i++)
			{
				for (int j = 0; j < matrix.NumberTransportStop; j++)
				{
					if (matrix[i, j] > 0)
						return true;
				}
			}
			return false;
		}
		private void CopyMatrices(List<Matrix> matrices)
		{
			this.matrices = new List<Matrix>();
			for (int i = 0; i < matrices.Count; i++)
			{
				this.matrices.Add(new Matrix(matrices[i]));
			}
		}
	}
}
