using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK.Models
{
    public class FranchiseImage
    {
        public int Id { get; set; }

        public string Path { get; set; }

        public bool IsBanner { get; set; }

        public FastFoodFranchise FastFoodFranchise { get; set; }
    }
}
