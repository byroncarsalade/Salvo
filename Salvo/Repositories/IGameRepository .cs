using Salvo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Salvo.Repositories
{
    public interface IGameRepository
    {
        IEnumerable<Game> GetAllGames();

        IEnumerable<Game> GetAllGamesWithPlayers();

        Game FindById(int id);
    }

}
