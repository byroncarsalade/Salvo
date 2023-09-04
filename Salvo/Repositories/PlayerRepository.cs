using Microsoft.EntityFrameworkCore;
using Salvo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Salvo.Repositories
{
    public class PlayerRepository : RepositoryBase<Player>, IPlayerRepository
    {
        public PlayerRepository(SalvoContext repositoryContext)
           : base(repositoryContext)
        {
        }


        public Player FindByEmail(string email)
        {
            return FindByCondition(player => player.Email == email).FirstOrDefault();
        }

        public void Save(Player player)
        {
            Create(player);
            SaveChanges();
        }
    }
}
