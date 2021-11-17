using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK.Models
{
    public class ClimateDevice
    {
        public int Id { get; set; }

        public int DeviceNumber { get; set; }

        public string PicturePath { get; set; }

        public bool IsOnline { get; set; }

        public FranchiseShop FranchiseShop { get; set; }

        public List<ClimateState> ClimateStates { get; set; }

        public List<TechMessage> TechMessages { get; set; }
    }
}
