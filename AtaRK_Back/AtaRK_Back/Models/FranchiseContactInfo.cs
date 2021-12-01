using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AtaRK.Models
{
    public class FranchiseContactInfo
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public bool IsPhone { get; set; }

        public bool IsEmail { get; set; }

        public bool IsUrl { get; set; }

        public string UrlType { get; set; }

        [JsonIgnore]
        public FastFoodFranchise FastFoodFranchise { get; set; }
    }
}
