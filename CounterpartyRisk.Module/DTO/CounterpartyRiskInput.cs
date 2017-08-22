using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CounterpartyRisk.Module.DTO
{
    public class CounterpartyRiskInput
    {
        public List<ContractDTO> Contracts { get; set; }
        public Parameters Parameters { get; set; }
    }
}
