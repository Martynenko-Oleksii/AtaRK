using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AtaRK.Models
{
    public class FranchiseImage
    {
        public int Id { get; set; }

        public string Path { get; set; }

        public bool IsBanner { get; set; }

        [JsonIgnore]
        public FastFoodFranchise FastFoodFranchise { get; set; }
    }
}
