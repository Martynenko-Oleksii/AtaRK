using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK.Models
{
    public class SystemAdmin : User
    {
        public string Name { get; set; }

        public bool IsMaster { get; set; }

        public List<TechMessageAnswer> TechMessageAnswers { get; set; }
    }
}
