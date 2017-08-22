using CounterpartyRisk.Module.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Denne gjør endelig utregning av de kalkulerte tallene fra Contract.cs. Bruker de aggregerte verdiene vi har fått (skal få) derfra. 
//Naturlig at vi har en public metode per utregning. 
//Kan velge om vi skal regne ut når noen spør om å få de. Eller vi kan bruke en "initialize"-metode. Kommer an på hvor mye de bygger på hverandre. Hvis de ikke bygger på hverandre, regner vi ut for hver gang ???

namespace CounterpartyRisk.Module
{
    public class CounterpartyRiskCalculator
    {
        private AggregatedMeasures measures;
        private CounterpartyRiskOutput output;
        private Parameters Parameters;

        public CounterpartyRiskCalculator()
        {

        }
        public CounterpartyRiskOutput CalculateCounterpartyRisk(AggregatedMeasures measures, Parameters Parameters)
        {
            this.measures = measures;
            this.Parameters = Parameters;
            output = new CounterpartyRiskOutput();
            output.VInter = CalculateVInter();
            output.VIntra = CalculateVIntra();
            output.V = CalculateV();
            output.StD3 = CalculateStD3();
            output.StD5 = CalculateStD5();
            output.SumLGD = CalculateSumLGD();
            output.KType1 = CalculateKType1();
            output.KType2 = CalculateKType2();
            output.TotalLossPotentialCounterPartyRisk = CalculateTotalLossPotentialCounterpartyRisk();
            return output;

        }

        private double CalculateVInter()
        {
            MatrixCalculator calculator = new MatrixCalculator();

            double[] array = calculator.Multiply(measures.GetVInterMatrix(), measures.GetLGDArray());
            double VInter = calculator.Multiply(measures.GetLGDArray(), array);
            return VInter;
        }

        private double CalculateVIntra()
        {
            MatrixCalculator calculator = new MatrixCalculator();
            
            double VIntra = calculator.Multiply(measures.GetLGDSquaredArray(), measures.GetToTheRightArray());
            return VIntra;
        }

        private double CalculateV()
        {
            double V = CalculateVInter() + CalculateVIntra();
            return V;
        }

        private double CalculateStD3()
        {
            double VSquared = Math.Sqrt(CalculateV());
            double StD3 = 3 * VSquared;
            return StD3;
        }

        private double CalculateStD5()
        {
            double VSquared = Math.Sqrt(CalculateV());
            double StD5 = 5 * VSquared;
            return StD5;   
        }

        private double CalculateSumLGD() 
        {
            double SumLGD = measures.GetLGDArray().Sum();
            return SumLGD;
        }

        private double CalculateKType1()
        {
            double VSquared = Math.Sqrt(CalculateV());

            double SumLGD7Percent = 0.07 * CalculateSumLGD();

            double SumLGD20Percent = 0.20 * CalculateSumLGD();

            double KType1 = 0;

            if (VSquared <= SumLGD7Percent)
            {
                KType1 = output.StD3;
            }
            else if (VSquared <= SumLGD20Percent)
            {
                KType1 = output.StD5;
            }
            else
            {
                KType1 = output.SumLGD;
            }
            return KType1;
        }

        private double CalculateKType2()
        {
            double E = Parameters.SumTypeTwoExposure;
            double EOverdue = Parameters.SumReceivableOverdue;
            double MortgageAbove60Percent = Parameters.MortgageValue;

            double EPlusMortgage = E + MortgageAbove60Percent;
            double EPlusMortgage15Perc = 0.15 * EPlusMortgage;

            double EOverdue90Perc = 0.9 * EOverdue;

            double KType2 = EPlusMortgage15Perc + EOverdue90Perc;
            return KType2;
        }

        private double CalculateTotalLossPotentialCounterpartyRisk()
        {
            double KT1 = CalculateKType1();
            double KT2 = CalculateKType2();

            double TK2 = KT1 * KT1 + 1.5 * KT1 * KT2 + KT2 * KT2;
            double TotalLoss = Math.Sqrt(TK2);
            return TotalLoss;
        }

    }

}
