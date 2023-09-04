using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Salvo.Models
{
    public class Player
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public ICollection<Score> Scores { get; set; }
        public ICollection<GamePlayer> GamePlayers { get; set; }

        public Score GetScore(Game game) 
        {
            return Scores.FirstOrDefault(p => p.GameId == game.Id);
        }


    }

}

