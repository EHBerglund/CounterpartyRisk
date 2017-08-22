using CounterpartyRisk.Module.Contracts;
using CounterpartyRisk.Module.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Dette er hovedklassen som utfører logikken. Dette vinduet er controlleren. Den delegerer arbeidsoppgavene ut. "Main"-metoden. "Tror kanskje vi er ferdig med metoden her" - Eirik

namespace CounterpartyRisk.Module
{
    public class CounterpartyRisk
    {
        public CounterpartyRiskOutput GetCounterpartyRisk(CounterpartyRiskInput input)
        {
            List<Contract> Contracts = RefineDTOContracts(input);

            input.Parameters.SumReinsurersShareSimplified = CalcSumReinsurersShareSimplified(input);

            AggregatedMeasures measures = AggregateMeasuresOnRating(Contracts);

            CounterpartyRiskCalculator calculator = new CounterpartyRiskCalculator();
            CounterpartyRiskOutput output = calculator.CalculateCounterpartyRisk(measures, input.Parameters);

            ContractsFilter filter = new ContractsFilter();
            List<CounterpartDTO> largestCounterparts = filter.FilterCounterpartsOnLGD(Contracts);
            output.LargestCounterparts = largestCounterparts;

            return output;
        }

        public List<Contract> RefineDTOContracts(CounterpartyRiskInput input)
        {
            List<Contract> contracts = new List<Contract>();
            foreach (ContractDTO contract in input.Contracts)
            {
                if (contract.ContractType == ContractType.CashDeposits)
                {
                    contracts.Add(new CashDepositContract(contract, input.Parameters));
                }
                if (contract.ContractType == ContractType.Derivative)
                {
                    contracts.Add(new DerivativeContract(contract, input.Parameters));
                }
                if (contract.ContractType == ContractType.Reinsurance)
                {
                    contracts.Add(new ReinsuranceContract(contract, input.Parameters));
                }
            }

            return contracts;
        }

        public AggregatedMeasures AggregateMeasuresOnRating(List<Contract> contracts)
        {
            List<int> ratings = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            AggregatedMeasures measures = new AggregatedMeasures();
            foreach (var rating in ratings)
            {
                foreach (Contract contract in contracts.Where(c => c.Rating == rating))
                {
                    double LGD = contract.CalculateLGD();
                    measures.AddLGD(rating, LGD);
                    measures.AddLGDSquared(rating, LGD * LGD);
                }
            }

            return measures;
        }
        private double CalcSumReinsurersShareSimplified(CounterpartyRiskInput input)
        {
            return input.Contracts.Where(c => c.ContractType == ContractType.Reinsurance && c.UseSimplifiedCalculation).Sum(c => c.ReinsuranceShare);
        }
    }
}
