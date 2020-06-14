using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportSystem
{
    public class Matrix
    {
        private int[,] matrix;
        public int NumberTransportStop { private set; get; }
        private Random random;
        public int this[int i, int j]
        {
            set
            {
                matrix[i, j] = value;
            }
            get
            {
                return matrix[i, j];
            }
        }
        public Matrix(Random random)
        {
            this.random = random;
        }
        public void Generate(int minNumberTransportStop, int maxNumberTransportStop, int capacityTransport, decimal factorIntensity)
        {
            this.NumberTransportStop = GetRandomNumber(minNumberTransportStop, maxNumberTransportStop + 1);
            this.matrix = new int[this.NumberTransportStop, this.NumberTransportStop];
            for (int i = 0; i < this.NumberTransportStop; i++)
            {
                for (int j = i + 1; j < this.NumberTransportStop; j++)
                {
                    matrix[i, j] = GetRandomNumber(0, (int)(capacityTransport * factorIntensity));
                }
            }
        }
        private int GetRandomNumber(int minNumber = 0, int maxNumber = 0)
        {
            int number = 0;
            double R = random.NextDouble();
            number = GetNumberUniformDistribution(minNumber, maxNumber, R);
            return number;
        }
        private int GetNumberUniformDistribution(int minNumber, int maxNumber, double R)
        {
            return (int)(minNumber + (maxNumber - minNumber) * R);
        }
        public Matrix(Matrix matrix)
        {
            this.matrix = Copy(matrix.matrix);
            this.random = matrix.random;
            this.NumberTransportStop = matrix.NumberTransportStop;
        }
        private int[,] Copy(int[,] array)
        {
            int[,] newArray = new int[array.GetLength(0), array.GetLength(1)];
            for (int i = 0; i < array.GetLength(0); i++)
                for (int j = 0; j < array.GetLength(1); j++)
                    newArray[i, j] = array[i, j];
            return newArray;
        }
    }
}
