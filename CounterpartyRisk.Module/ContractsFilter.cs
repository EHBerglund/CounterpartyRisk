using CounterpartyRisk.Module.Contracts;
using CounterpartyRisk.Module.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CounterpartyRisk.Module
{
    public class ContractsFilter
    {
        private const int FilterCount = 10;
        public List<CounterpartDTO> FilterCounterpartsOnLGD(List<Contract> contracts)
        {
            List<CounterpartDTO> counterparts = new List<CounterpartDTO>();
            counterparts = CalculateLGDOnCounterparts(counterparts, contracts);
            counterparts = counterparts.OrderByDescending(c => c.TotalLGD).Take(FilterCount).ToList();
            return counterparts;
        }

        private List<CounterpartDTO> CalculateLGDOnCounterparts(List<CounterpartDTO> counterparts, List<Contract> contracts)
        {
            foreach (Contract contract in contracts)
            {
                contract.AddLGDToCounterpart();
                if (counterparts.FirstOrDefault(c => c.Name.Equals(contract.GetCounterpartName())) == null)
                {
                    counterparts.Add(contract.GetCounterpart());
                }
                else
                {
                    counterparts.FirstOrDefault(c => c.Name.Equals(contract.GetCounterpartName())).TotalLGD += contract.GetLGD();
                }
            }
            return counterparts;
        }

    }
}

