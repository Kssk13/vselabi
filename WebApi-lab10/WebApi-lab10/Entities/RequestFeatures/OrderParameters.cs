using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class OrderParameters : RequestParameters
    {
        public OrderParameters()
        {
            OrderBy = "name";
        }
        public uint MinAge { get; set; }
        public uint MaxAge { get; set; } = int.MaxValue;
        public bool ValidAgeRange => MaxAge > MinAge;
        public string SearchTerm { get; set; }

    }
}
