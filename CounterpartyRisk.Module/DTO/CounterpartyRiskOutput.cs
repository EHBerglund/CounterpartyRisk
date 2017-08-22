using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CounterpartyRisk.Module.DTO
{
    public class CounterpartyRiskOutput
    {
        public double VInter { get; set; }
        public double VIntra { get; set; }
        public double V { get; set; }
        public double StD3 { get; set; }
        public double StD5 { get; set; }
        public double SumLGD { get; set; }
        public double KType1 { get; set; }
        public double KType2 { get; set; }
        public double TotalLossPotentialCounterPartyRisk { get; set; }
        public List<CounterpartDTO> LargestCounterparts;
    }
}
