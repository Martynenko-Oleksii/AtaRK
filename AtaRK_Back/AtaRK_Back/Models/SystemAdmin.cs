using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AtaRK.Models
{
    public class SystemAdmin : User
    {
        public string Name { get; set; }

        public bool IsMaster { get; set; }

        [JsonIgnore]
        public List<TechMessageAnswer> TechMessageAnswers { get; set; }
    }
}
