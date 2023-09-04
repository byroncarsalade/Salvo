using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Salvo.Models
{
    public class GamePlayer
    {
        public long Id { get; set; }
        public DateTime JoinDate { get; set; }

        public long GameId { get; set; }
        public Game Game { get; set; }

        public long PlayerId { get; set; }
        public Player Player { get; set; }

        public ICollection<Ship> Ships { get; set; }
        public ICollection<Salvo> Salvos { get; set; }

        public Score GetScore()
        {
            return Player.GetScore(Game);
        }

        public GamePlayer GetOpponent()
        {
            return Game.GamePlayers.FirstOrDefault(gp => gp.Id != Id);
        }

        //metodo hits ,muestra los propios y de los oponentes
        public ICollection<SalvoHitDTO> GetHits() 
        {
            return Salvos.Select(salvo => new SalvoHitDTO
            {
                Turn = salvo.Turn,
                Hits = GetOpponent()?.Ships.Select(ship => new ShipHitDTO 
                { 
                    Type = ship.Type,
                    Hits = salvo.Locations
                        .Where(salvoLocation =>ship.Locations.Any(shipLocation => shipLocation.Location == salvoLocation.Location))
                        .Select(salvoLocation => salvoLocation.Location).ToList()
                }).ToList()
            }).ToList();
        }


        //get sunks
        public ICollection<string> GetSunks()
        {
            //identificar ultimo turno
            int lastTurn = Salvos.Count;
            //obtener listado de string de salvo location
            //debera entregar todos aquellos que sean ultimo turno <= al turno del salvo
            List<string> salvoLocations = GetOpponent()?.Salvos
                                           .Where(salvo => salvo.Turn <= lastTurn)
                                           .SelectMany(salvo => salvo.Locations.Select(location => location.Location)).ToList();

            return Ships?.Where(ship => ship.Locations.Select(shipLocation => shipLocation.Location)
                      .All(salvoLocation => salvoLocations != null ? salvoLocations.Any(shipLocation => shipLocation == salvoLocation) : false))
                      .Select(ship => ship.Type).ToList();

        }


        //retorna gameState
        public GameState GetGameState() 
        {
            //incilaizar en una variable el primer estado
            GameState gameState = GameState.ENTER_SALVO;

            //si los barcos son nulos o count 0
            //dejamos en place_ship ya que no ha posicionados los barcos
            if (Ships == null || Ships.Count() == 0)
            {
                gameState = GameState.PLACE_SHIPS;
            }

            //evalouar si el oponente es nulo
            //si lo es debe quedar en wait 
            else if (GetOpponent() == null)
            {
                //salvos count
                if (Salvos != null && Salvos?.Count() > 0)
                {
                    gameState = GameState.WAIT;
                }
            }
            //si no ocurre niguna d elas condicioens anteriores
            else
            {
                //obtener nuestro oponente
                GamePlayer opponent = GetOpponent();

                //determinar el turno
                int turn = Salvos != null ? Salvos.Count() : 0;

                //determinar turno oponente
                int opponentTurn = opponent.Salvos != null ? opponent.Salvos.Count() : 0;

                //si mi turno es mayor al del oponente debemos esperar
                if (turn > opponentTurn)
                {
                    gameState = GameState.WAIT;
                }
                else if (turn == opponentTurn && turn != 0)
                {
                    //hundidos sunks
                    //mis hundidos
                    int playerSunks = GetSunks().Count();
                    //hundidos del oponente
                    int opponentSunks = opponent.GetSunks().Count();

                    //evaluamos si existe un empate
                    if (playerSunks == Ships.Count() && opponentSunks == opponent.Ships.Count())
                    {
                        gameState = GameState.TIE;
                    }
                    //evaluar si erdio
                    else if (playerSunks == Ships.Count())
                    {
                        gameState = GameState.LOSS;
                    }
                    //evlasuar quien gano
                    else if (opponentSunks == opponent.Ships.Count())
                    { 
                        gameState = GameState.WIN;
                    }
                }
            }

            return gameState;
        }

    }
}
