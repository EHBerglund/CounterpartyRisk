using System.Collections.Generic;

namespace CounterpartyRisk.Module
{
    public class AggregatedMeasures
    {
        private double[] LGDMeasures;
        private double[] LGDSquaredMeasures;
        private double[] ProbabilityOfDefault;
        private double[][] VInterMatrix;
        private double[] ToTheRightArray;

        public AggregatedMeasures()
        {
            LGDMeasures = new double[10];
            LGDMeasures[0] = 0;
            LGDMeasures[1] = 0;
            LGDMeasures[2] = 0;
            LGDMeasures[3] = 0;
            LGDMeasures[4] = 0;
            LGDMeasures[5] = 0;
            LGDMeasures[6] = 0;
            LGDMeasures[7] = 0;
            LGDMeasures[8] = 0;
            LGDMeasures[9] = 0;

            LGDSquaredMeasures = new double[10];
            LGDSquaredMeasures[0] = 0;
            LGDSquaredMeasures[1] = 0;
            LGDSquaredMeasures[2] = 0;
            LGDSquaredMeasures[3] = 0;
            LGDSquaredMeasures[4] = 0;
            LGDSquaredMeasures[5] = 0;
            LGDSquaredMeasures[6] = 0;
            LGDSquaredMeasures[7] = 0;
            LGDSquaredMeasures[8] = 0;
            LGDSquaredMeasures[9] = 0;

            ProbabilityOfDefault = new double[10];
            ProbabilityOfDefault[0] = 0.00002;
            ProbabilityOfDefault[1] = 0.0001;
            ProbabilityOfDefault[2] = 0.0005;
            ProbabilityOfDefault[3] = 0.0024;
            ProbabilityOfDefault[4] = 0.012;
            ProbabilityOfDefault[5] = 0.042;
            ProbabilityOfDefault[6] = 0.042;
            ProbabilityOfDefault[7] = 0.0001;
            ProbabilityOfDefault[8] = 0.005;
            ProbabilityOfDefault[9] = 0.042;

            CreateToTheRightArray();

            CreateVInterMatrix();
        }

        private void CreateToTheRightArray()
        {
            ToTheRightArray = new double[10];
            for (int i=0;i<10;i++)
            {
                double x = ProbabilityOfDefault[i];
                ToTheRightArray[i] = 1.5 * x * (1 - x) / (2.5 - x);
            }
        }

        private void CreateVInterMatrix()
        {
            VInterMatrix = new double[10][];
            for (int i = 0; i < 10; i++)
            {
                VInterMatrix[i] = new double[10];
                for (int k = 0;k<10;k++)
                {
                    double x = ProbabilityOfDefault[i];
                    double y = ProbabilityOfDefault[k];
                    VInterMatrix[i][k] = x * (1 - x) * y * (1 - y) / (1.25 * (x + y) - x * y);
                }
            }
        }



        public double GetLGD(int rating)
        {
            return LGDMeasures[rating];
        }

        public double GetLGDSquared(int rating)
        {
            return LGDSquaredMeasures[rating];
        }

        public void AddLGD(int rating, double LGD)
        {
            LGDMeasures[rating] += LGD;
        }

        public void AddLGDSquared(int rating, double LGDSquared)
        {
            LGDSquaredMeasures[rating] += LGDSquared;
        }

        public double GetProbabilityOfDefault(int rating)
        {
            return ProbabilityOfDefault[rating];
        }

        public double[] GetLGDArray()
        {
            return LGDMeasures;
        }

        public double[] GetLGDSquaredArray()
        {
            return LGDSquaredMeasures;
        }

        public double[] GetToTheRightArray()
        {
            return ToTheRightArray;
        }

        public double[][] GetVInterMatrix()
        {
            return VInterMatrix;
        }

        public double[] GetProbabilityOfDefaultArray()
        {
            return ProbabilityOfDefault;
        }
    }
}
