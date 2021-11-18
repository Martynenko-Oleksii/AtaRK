using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK.Models
{
    public class TechMessageAnswer
    {
        public int Id { get; set; }

        public string Answer { get; set; }

        public TechMessage TechMessage { get; set; }

        public SystemAdmin SystemAdmin { get; set; }
    }
}
