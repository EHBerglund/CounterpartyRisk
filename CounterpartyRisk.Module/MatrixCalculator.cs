using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CounterpartyRisk.Module
{
    public class MatrixCalculator
    {
        public double[] Multiply(double[][] matrix, double[] array)
        {
            // Dette er en generell utregning av "Matrise u" ganget med "TLGD". Bruker ingen input.
            int size = 0;
            size = array.Length;
            double[] resultArray = new double[size];
            
            for (int i = 0; i < size; i++)
            {
                double rowSum = 0;
                for (int k = 0; k < size; k++)
                {
                    double leftHand = matrix[k][i];
                    double rightHand = array[k];
                    rowSum += leftHand * rightHand;
                }
                resultArray[i] = rowSum;
            }
            return resultArray;
        }

        public double Multiply(double[] array1, double[] array2)
        {
            // Dette er en generell utregning av VInter basert på matrisemultiplikasjonen over. Bruker ingen input. Må "gi" arrays/"n*1"-matrisene verdier et annet sted. 
            int size = 0;
            size = array1.Length;

            double VInter = 0;
            for (int i = 0; i < size; i++)
            {
               VInter += array1[i] * array2[i];
            }
            return VInter;
        }
    }
}
