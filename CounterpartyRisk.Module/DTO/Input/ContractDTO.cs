using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CounterpartyRisk.Module.DTO
{
    public class ContractDTO
    {
        public ContractType ContractType { get; set; }
        public CounterpartDTO Counterpart { get; set; }
        public double ReinsuranceShare { get; set; }
        public double MarketValueOfCollateral { get; set; }
        public bool UseSimplifiedCalculation { get; set; }
        public bool UseCollateralAgreement { get; set; }
        public double MarketValueNotRRC { get; set; }
        public double MarketValueRRC { get; set; }
        public double StandardRE { get; set; }      
    }
}
