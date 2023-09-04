using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Salvo.Models
{
    public class Salvo
    {
        public long Id { get; set; }
        public int Turn { get; set; }

        public long GamePlayerId { get; set; }
        public GamePlayer GamePlayer { get; set; }

        public ICollection<SalvoLocation> Locations { get; set; }

    }
}
