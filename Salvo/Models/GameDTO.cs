using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Salvo.Models
{
    public class GameDTO
    {
        public long Id { get; set; }
        public DateTime CreationDate { get; set; }

        public ICollection<GamePlayerDTO> GamePlayers { get; set; }
    }
}
