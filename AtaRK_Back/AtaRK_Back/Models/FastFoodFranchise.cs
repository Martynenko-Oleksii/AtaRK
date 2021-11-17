using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK.Models
{
    public class FastFoodFranchise : User
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public double MinTemperature { get; set; }

        public double MaxTemperature { get; set; }

        public double MinHuumidity { get; set; }

        public double MaxHuumidity { get; set; }

        public List<FranchiseImage> FranchiseImages { get; set; }

        public List<FranchiseContactInfo> FranchiseContactInfos { get; set; }

        public List<FranchiseShop> FranchiseShops { get; set; }

        public List<ShopApplication> ShopApplications { get; set; }
    }
}
