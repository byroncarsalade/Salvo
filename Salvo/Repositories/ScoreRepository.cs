using Salvo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Salvo.Repositories
{
    public class ScoreRepository : RepositoryBase<Score>, IScoreRepository
    {
        public ScoreRepository(SalvoContext repositoryContext) 
            :base(repositoryContext)
        {
        }

        public void Save(Score score)
        {
            Create(score);
            SaveChanges();
        }
    }
}
