using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AtaRK.Models
{
    public class ClimateDevice
    {
        public int Id { get; set; }

        public int DeviceNumber { get; set; }

        public string PicturePath { get; set; }

        public bool IsOnline { get; set; }

        public FranchiseShop FranchiseShop { get; set; }

        [JsonIgnore]
        public List<ClimateState> ClimateStates { get; set; }

        [JsonIgnore]
        public List<TechMessage> TechMessages { get; set; }
    }
}
