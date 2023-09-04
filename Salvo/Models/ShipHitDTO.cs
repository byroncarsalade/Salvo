using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Salvo.Models
{
    public class ShipHitDTO
    {
        public string Type { get; set; }
        public List<string> Hits { get; set; }
    }
}
