using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AtaRK.Models
{
    public class TechMessage
    {
        public int Id { get; set; }

        public int State { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public string ContactEmail { get; set; }

        public ClimateDevice ClimateDevice { get; set; }

        public ShopAdmin ShopAdmin { get; set; }

        public int? TechMessageAnswerId { get; set; }
        [JsonIgnore]
        public TechMessageAnswer TechMessageAnswer { get; set; }
    }
}
