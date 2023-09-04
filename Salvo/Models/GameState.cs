using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Salvo.Models
{
    public enum GameState
    {
        ENTER_SALVO = 0,
        PLACE_SHIPS,
        WAIT,
        WIN,
        LOSS,
        TIE
    }
}
