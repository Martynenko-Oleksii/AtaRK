using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AtaRK.Models
{
    public class ShopAdmin : User
    {
        [JsonIgnore]
        public List<FranchiseShop> FranchiseShops { get; set; }

        [JsonIgnore]
        public List<TechMessage> TechMessages { get; set; }
    }
}
