using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;

namespace TransportSystem
{
	public class AlgorithmRow
	{
		public List<PlanRoute> PlanRoutes { private set; get; }
		public List<MathStatistic> MathStatistics { private set; get; }
		private List<Matrix> matrices;
		private int capacityTransport;
		private int minNumberTransportStop;
		private int maxNumberTransportStop;
		private List<int> numberPassengersDropOffTransportStop;
		public List<Stopwatch> RouteDeterminationRunningTime { private set; get; }
		public Stopwatch AlgorithmRunningTime { private set; get; }
		public AlgorithmRow(int minNumberTransportStop, int maxNumberTransportStop)
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
			AlgorithmRunningTime.Start();
			RouteDeterminationRunningTime = new List<Stopwatch>();
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
		private List<List<Point>> DistributeTransport(Matrix matrix)
		{
			List<List<Point>> pathTransports = new List<List<Point>>();
			pathTransports.Add(new List<Point>());
			int numberTransport = 1;
			int numberPassengersInTransport = 0;
			Point coordinateLine = new Point(0, 0);
			this.DropOffPassengers(matrix.NumberTransportStop);
			for (int i = 0; i < matrix.NumberTransportStop; i++)
			{
				for (int j = matrix.NumberTransportStop - 1; j >= 0; j--)
				{
					if (coordinateLine.X != matrix.NumberTransportStop - 1)
					{
						if (coordinateLine.X != j)
						{
							if (matrix[i, j] != 0)
							{
								numberPassengersInTransport += matrix[i, j];
								if (numberPassengersInTransport <= capacityTransport)
								{
									pathTransports[numberTransport - 1].Add(new Point(i, j));
									numberPassengersDropOffTransportStop[j] += matrix[i, j];
								}
								else
								{
									numberPassengersInTransport -= matrix[i, j];
									this.TakeMorePassengers(pathTransports[numberTransport - 1], ref numberPassengersInTransport, j + 1, matrix);
									numberPassengersInTransport = matrix[i, j];
									this.DropOffPassengers(matrix.NumberTransportStop);
									numberTransport++;
									pathTransports.Add(new List<Point>());
									pathTransports[numberTransport - 1].Add(new Point(i, j));
									numberPassengersDropOffTransportStop[j] += matrix[i, j];
								}
							}
						}
						else
						{
							this.TakeMorePassengers(pathTransports[numberTransport - 1], ref numberPassengersInTransport, j + 1, matrix);
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
				coordinateLine.X++;
			}
			return pathTransports;
		}
		private void TakeMorePassengers(List<Point> pathTransport, ref int numberPassengersInTransport, int row, Matrix matrix)
		{
			for (int i = row; i < matrix.NumberTransportStop - 1; i++)
			{
				numberPassengersInTransport -= numberPassengersDropOffTransportStop[i];
				for (int j = i + 1; j < matrix.NumberTransportStop; j++)
				{
					if (matrix[i, j] != 0)
					{
						numberPassengersInTransport += matrix[i, j];
						if (numberPassengersInTransport <= capacityTransport)
						{
							pathTransport.Add(new Point(i, j));
							numberPassengersDropOffTransportStop[j] += matrix[i, j];
						}
						else numberPassengersInTransport -= matrix[i, j];
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
