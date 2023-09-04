using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Salvo.Models
{
    public class Ship
    {
        public long Id { get; set; }
        public string Type { get; set; }

        public long GamePlayerId { get; set; }
        public GamePlayer GamePlayer { get; set; }

        public ICollection<ShipLocation> Locations { get; set; }
    }
}
