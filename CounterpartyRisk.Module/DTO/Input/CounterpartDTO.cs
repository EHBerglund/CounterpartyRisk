using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CounterpartyRisk.Module.DTO
{
    public class CounterpartDTO
    {
        public string Name { get; set; }
        public int Rating { get; set; }
        public double TotalLGD { get; set; }

        public override string ToString()
        {
            return $"{Name} - {TotalLGD}";
        }
    }
}
