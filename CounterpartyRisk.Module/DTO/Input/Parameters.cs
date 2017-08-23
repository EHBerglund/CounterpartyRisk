using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Denne må "kastes rundt" hvor enn den brukes. Dette vil si at vi legger den som parameter når vi kaller på metoder eller i klassens konstruktør.

namespace CounterpartyRisk.Module.DTO
{
    public class Parameters
    {
        public double EconomicFactorNonCollateral { get; set; }
        public double EconomicFactorCollateral { get; set; }
        public double EconomicFactorDerivative { get; set; }
        public double SumTypeTwoExposure { get; set; }
        public double SumReceivableOverdue { get; set; }
        public double MortgageValue { get; set; }
        public double SumReinsurersShareSimplified { get; set; }
        public double LossPotentialWithoutReinsurance { get; set; }
        public double LossPotentialWithReinsurance { get; set; }
    }
}
