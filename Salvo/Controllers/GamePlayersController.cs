using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Salvo.Models;
using Salvo.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Salvo.Controllers
{
    [Route("api/gamePlayers")]
    [ApiController]
    [Authorize("PlayerOnly")]
    public class GamePlayersController : ControllerBase
    {

        private IGamePlayerRepository _repository;
        private IPlayerRepository _playerRepository;
        private IScoreRepository _scoreRepository;

        public GamePlayersController(IGamePlayerRepository repository, IPlayerRepository playerRepository, IScoreRepository scoreRepository) 
        {
            _repository = repository;
            _playerRepository = playerRepository;
            _scoreRepository = scoreRepository;
        }

        // GET: api/<GamePlayersController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<GamePlayersController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        [HttpGet("{id}", Name = "GetGameView")]
        public IActionResult GetGameView(int id)
        {
            try
            {
                string email = User.FindFirst("Player") != null ? User.FindFirst("Player").Value : "Guest";
                var gp = _repository.GetGamePlayerView(id);
                if(gp.Player.Email != email)
                {
                    return Forbid();
                }

                var gameView = new GameViewDTO
                {
                    Id = gp.Id,
                    CreationDate = gp.Game.CreationDate,
                    Ships = gp.Ships.Select(ship => new ShipDTO
                    { 
                        Id = ship.Id,
                        Type = ship.Type,
                        Locations = ship.Locations.Select(shipLocation => new ShipLocationDTO { 
                            Id = shipLocation.Id,
                            Location = shipLocation.Location
                        }).ToList()
                    }).ToList(),
                    GamePlayers = gp.Game.GamePlayers.Select(gps => new GamePlayerDTO { 
                        Id = gps.Id,
                        JoinDate = gps.JoinDate,
                        Player = new PlayerDTO { 
                            Id = gps.Player.Id,
                            Email = gps.Player.Email
                        },
                    }).ToList(),
                    Salvos = gp.Game.GamePlayers.SelectMany(gps => gps.Salvos.Select(salvo => new SalvoDTO
                    {
                        Id = salvo.Id,
                        Turn = salvo.Turn,
                        Player = new PlayerDTO
                        {
                            Id = gp.Player.Id,
                            Email = gp.Player.Email
                        },
                        Locations = salvo.Locations.Select(salvoLocation => new SalvoLocationDTO 
                        { 
                            Id = salvoLocation.Id,
                            Location = salvoLocation.Location
                        }).ToList()
                    })).ToList(),
                    //nueos metodos
                    //hi8ts
                    Hits = gp.GetHits(),
                    HitsOpponent = gp.GetOpponent()?.GetHits(),
                    //sunks
                    Sunks = gp.GetSunks(),
                    SunksOpponent = gp.GetOpponent()?.GetSunks(),
                    GameState = Enum.GetName(typeof(GameState), gp.GetGameState())
                };

                return Ok(gameView);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // POST api/<GamePlayersController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        // POST api/<GamePlayers/id/ships>
        [HttpPost("{id}/ships")]
        public IActionResult Post(int id, [FromBody] List<ShipDTO> ships)
        {
            try
            {
                string email = User.FindFirst("Player") != null ? User.FindFirst("Player").Value : "Guest";

                Player player = _playerRepository.FindByEmail(email);

                GamePlayer gamePlayer = _repository.FindById(id);

                if (gamePlayer == null) 
                {
                    return StatusCode(403, "No existe el juego");
                }

                if (gamePlayer.Player.Id != player.Id)
                {
                    return StatusCode(403, "El usuario no se encuentra en el juego");
                }

                if (gamePlayer.Ships.Count == 5)
                {
                    return StatusCode(403, "Ya se han posicionado los barcos");
                }

                gamePlayer.Ships = ships.Select(sh => new Ship 
                {
                    GamePlayerId = gamePlayer.Id,
                    Type = sh.Type,
                    Locations = sh.Locations.Select(loc => new ShipLocation 
                    { 
                        ShipId = sh.Id,
                        Location = loc.Location
                    }).ToList()
                }).ToList();

                _repository.Save(gamePlayer);

                return StatusCode(201);

            }
            catch (Exception ex) 
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("{id}/salvos")]
        public IActionResult Post(int id, [FromBody] SalvoDTO salvo)
        {
            try
            {
                string email = User.FindFirst("Player") != null ? User.FindFirst("Player").Value : "Guest";

                //obtener player autenticado
                Player player = _playerRepository.FindByEmail(email);

                //obtener gameplayer
                GamePlayer gamePlayer = _repository.FindById(id);

                //validar si juego existe
                if (gamePlayer == null)
                {
                    return StatusCode(403, "No existe el juego");
                }

                //usuario no se encuentra en el juego
                if (gamePlayer.Player.Id != player.Id)
                {
                    return StatusCode(403, "El usuario no se encuentra en el juego");
                }

                //obtener al oponente
                GamePlayer oponenteGamePlayer = gamePlayer.GetOpponent();

                //gvarvariables de turno
                int playerTurn = 0;
                int opponentTurn = 0;

                //lo que dfaltaba
                //aca debemos agregar 
                GameState gameState = gamePlayer.GetGameState();
                if (gameState == GameState.LOSS || gameState == GameState.WIN || gameState == GameState.TIE)
                {
                    return StatusCode(403, "Eljuego ya termino");
                }

                //determinadmos el turno del jugador actual
                playerTurn = gamePlayer.Salvos != null
                    ? gamePlayer.Salvos.Count() + 1
                    : 1;

                //validamos si el oponente existe
                if (oponenteGamePlayer != null)
                {
                    opponentTurn = oponenteGamePlayer.Salvos != null
                        ? oponenteGamePlayer.Salvos.Count()
                        :0;
                }

                //evaluando si esta adelantado al turno
                if ((playerTurn - opponentTurn) < -1 || (playerTurn - opponentTurn) > 1)
                {
                    return StatusCode(403, "No se puede adelantar tyurno");
                }

                //guardar
                gamePlayer.Salvos.Add(new Salvo.Models.Salvo 
                {
                    GamePlayerId = gamePlayer.Id,
                    Turn = playerTurn,
                    Locations = salvo.Locations.Select(location => new SalvoLocation 
                    {
                        SalvoId = salvo.Id,
                        Location = location.Location
                    }).ToList()
                });

                _repository.Save(gamePlayer);

                //guardar el score
                gameState = gamePlayer.GetGameState();
                if (gameState == GameState.WIN)
                {
                    Score score = new Score
                    {
                        FinishDate = DateTime.Now,
                        GameId = gamePlayer.GameId,
                        PlayerId = gamePlayer.PlayerId,
                        Point = 1
                    };

                    _scoreRepository.Save(score);

                    Score scoreOpponent = new Score
                    {
                        FinishDate = DateTime.Now,
                        GameId = gamePlayer.GameId,
                        PlayerId = oponenteGamePlayer.PlayerId,
                        Point = 0
                    };

                    _scoreRepository.Save(scoreOpponent);
                }
                else if (gameState == GameState.LOSS)
                {
                    Score score = new Score
                    {
                        FinishDate = DateTime.Now,
                        GameId = gamePlayer.GameId,
                        PlayerId = gamePlayer.PlayerId,
                        Point = 0
                    };

                    _scoreRepository.Save(score);

                    Score scoreOpponent = new Score
                    {
                        FinishDate = DateTime.Now,
                        GameId = gamePlayer.GameId,
                        PlayerId = oponenteGamePlayer.PlayerId,
                        Point = 1
                    };

                    _scoreRepository.Save(scoreOpponent);

                }
                else if (gameState == GameState.TIE)
                {
                    Score score = new Score
                    {
                        FinishDate = DateTime.Now,
                        GameId = gamePlayer.GameId,
                        PlayerId = gamePlayer.PlayerId,
                        Point = 0.5
                    };

                    _scoreRepository.Save(score);

                    Score scoreOpponent = new Score
                    {
                        FinishDate = DateTime.Now,
                        GameId = gamePlayer.GameId,
                        PlayerId = oponenteGamePlayer.PlayerId,
                        Point = 0.5
                    };

                    _scoreRepository.Save(scoreOpponent);

                }

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/<GamePlayersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<GamePlayersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
