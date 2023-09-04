using Salvo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Salvo.Repositories
{
    public interface IScoreRepository
    {
        void Save(Score score);
    }
}
