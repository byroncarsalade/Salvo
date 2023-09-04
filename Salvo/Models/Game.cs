using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Salvo.Models
{
    public class Game
    {
        public long Id { get; set; }
        public DateTime CreationDate { get; set; }

        public ICollection<GamePlayer> GamePlayers { get; set; }

        //score
        //public ICollection<Score> Scores { get; set; }
    }
}
