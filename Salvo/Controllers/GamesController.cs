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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GamesController : ControllerBase
    {

        private IGameRepository _repository;
        private IPlayerRepository _playerRepository;
        private IGamePlayerRepository _gamePlayerRepository;

        //constructor
        public GamesController(IGameRepository repository, 
            IPlayerRepository playerRepository, 
            IGamePlayerRepository gamePlayerRepository)
        {
            _repository = repository;
            _playerRepository = playerRepository;
            _gamePlayerRepository = gamePlayerRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            try
            {
                GameListDTO gameList = new GameListDTO
                {
                    Email = User.FindFirst("Player") != null ? User.FindFirst("Player").Value : "Guest",
                    Games = _repository.GetAllGamesWithPlayers()
                    .Select(g => new GameDTO
                    {
                        Id = g.Id,
                        CreationDate = g.CreationDate,
                        GamePlayers = g.GamePlayers.Select(gp => new GamePlayerDTO
                        {
                            Id = gp.Id,
                            JoinDate = gp.JoinDate,
                            Player = new PlayerDTO
                            {
                                Id = gp.Player.Id,
                                Email = gp.Player.Email
                            },

                            Point = gp.GetScore() != null ? (double?)gp.GetScore().Point : null
                        }).ToList().OrderByDescending(p=>p.Point).ToList()
                    }).ToList()
                };

                return Ok(gameList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        // GET: api/<GamesController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<GamesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<GamesController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        [HttpPost]
        public IActionResult Post()
        {
            try
            {
                string email = User.FindFirst("Player") != null ? User.FindFirst("Player").Value : "Guest";
                
                Player player = _playerRepository.FindByEmail(email);

                GamePlayer gamePlayer = new GamePlayer
                {
                    PlayerId = player.Id,
                    JoinDate = DateTime.Now,
                    Game = new Game
                    {
                        CreationDate = DateTime.Now
                    }
                };

                _gamePlayerRepository.Save(gamePlayer);

                return StatusCode(201, gamePlayer.Id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("{id}/players")]
        public IActionResult Join(int id)
        {
            try
            {
                string email = User.FindFirst("Player") != null ? User.FindFirst("Player").Value : "Guest";

                Player player = _playerRepository.FindByEmail(email);

                Game game = _repository.FindById(id);

                if (game == null)
                {
                    return StatusCode(403, "No existe juego");
                }

                if (game.GamePlayers.Where(gp => gp.Player.Id == player.Id).FirstOrDefault() != null) 
                {
                    return StatusCode(403, "Ya se encuentra el jugador en el juego");
                }

                if (game.GamePlayers.Count > 1)
                {
                    return StatusCode(403, "Juego lleno");
                }

                GamePlayer gamePlayer = new GamePlayer
                {
                    GameId = game.Id,
                    PlayerId = player.Id,
                    JoinDate = DateTime.Now,
                };

                _gamePlayerRepository.Save(gamePlayer);

                return StatusCode(201, gamePlayer.Id);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/<GamesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<GamesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
