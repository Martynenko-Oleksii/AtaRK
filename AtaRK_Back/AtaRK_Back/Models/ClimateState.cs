using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK.Models
{
    public class ClimateState
    {
        public int Id { get; set; }

        public double Temperature { get; set; }

        public double Huumidity { get; set; }

        public ClimateDevice ClimateDevice { get; set; }

        public FranchiseShop FranchiseShop { get; set; }
    }
}
