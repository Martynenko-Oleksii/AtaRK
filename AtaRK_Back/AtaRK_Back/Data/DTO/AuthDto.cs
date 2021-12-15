using AtaRK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK.DTO
{
    public class AuthDto
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public FastFoodFranchise FastFoodFranchise { get; set; }
    }
}
