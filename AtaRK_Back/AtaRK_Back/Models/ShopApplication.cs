using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK.Models
{
    public class ShopApplication
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string ContactPhone { get; set; }

        public string ContactEmail { get; set; }

        public string City { get; set; }

        public string Message { get; set; }

        public FastFoodFranchise FastFoodFranchise { get; set; }
    }
}
