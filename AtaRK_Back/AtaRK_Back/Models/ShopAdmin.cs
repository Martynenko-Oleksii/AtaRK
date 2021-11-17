using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK.Models
{
    public class ShopAdmin : User
    {
        public List<FranchiseShop> FranchiseShops { get; set; }

        public List<TechMessage> TechMessages { get; set; }
    }
}
