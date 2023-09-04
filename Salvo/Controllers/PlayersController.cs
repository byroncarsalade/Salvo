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
    [Route("api/players")]
    [ApiController]
    public class PlayersController : ControllerBase
    {

        private IPlayerRepository _repository;

        public PlayersController(IPlayerRepository repository)
        {
            _repository = repository;
        }

        // GET: api/<PlayersController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<PlayersController>/5
        [HttpGet("{id}", Name = "GetPlayer")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PlayersController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        [HttpPost]
        public IActionResult Post([FromBody] PlayerDTO player)
        {
            try
            {
                if (string.IsNullOrEmpty(player.Email) || string.IsNullOrEmpty(player.Password)) 
                {
                    return StatusCode(403, "datos invalidos");
                }

                Player dbPlayer = _repository.FindByEmail(player.Email);

                if (dbPlayer != null)
                {
                    return StatusCode(403, "email en uso");
                }

                Player newPlayer = new Player
                {
                    Email = player.Email,
                    Password = player.Password
                };

                _repository.Save(newPlayer);

                return StatusCode(201, newPlayer);
            }
            catch (Exception ex) 
            {
                return StatusCode(500, "Error de servidor");
            }
        }

        // PUT api/<PlayersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PlayersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
