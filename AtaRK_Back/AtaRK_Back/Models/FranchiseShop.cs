using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK.Models
{
    public class FranchiseShop : User
    {
        public string City { get; set; }

        public string Street { get; set; }

        public string BuildingNumber { get; set; }

        public string ContactPhone { get; set; }

        public string ContactEmail { get; set; }

        public FastFoodFranchise FastFoodFranchise { get; set; }

        public List<ClimateDevice> ClimateDevices { get; set; }

        public List<ShopAdmin> ShopAdmins { get; set; }
    }
}
