using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokemonServer;

namespace PokemonServer.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class BattleController : ControllerBase {
        private MultiplayService _svc = MultiplayService.GetInstance();
        // POST: api/<BattleController>
        // Returns a UUID for the player to join the lobby, or 404, if battle
        // is ongoing and can't join lobby.
        [HttpPost]
        public IActionResult HttpPost([FromBody] PostPokemonBody newPokemonPost) {
            // if (newPokemonPost.PokemonId == null || newPokemonPost.Name == null
            //     || newPokemonPost.Hp == null || newPokemonPost.Exp == null)
            if (newPokemonPost.Name == null)  // rest mandatory to load this endpoint
                return BadRequest();

            Pokemon pokemon = new Pokemon(newPokemonPost.Name, newPokemonPost.Hp, newPokemonPost.Exp) {
                PokemonId = newPokemonPost.PokemonId,
                Uuid = newPokemonPost.Uuid,
                Skill = newPokemonPost.Skill
            };
            string uuid = _svc.JoinPlayer(pokemon);
            if (uuid != null)
                return Ok(uuid);
            return NotFound("Pokemon is already in lobby!");
        }

        // GET: api/<BattleController>/uuid
        // Get the battle results for a specific player in the lobby.
        // Keep polling this until you get a 200 OK with data.
        // If results not out yet, or battle hasn't started, return 404.
        [HttpGet("{uuid}")]
        public IActionResult HttpGet(string uuid) {
            List<BattleTick> res = _svc.GetResults(uuid);
            if (res == null)
                return NotFound("Battle hasn't started yet");
            return Ok(res);
        }
    }

    public class PostPokemonBody {
        public int PokemonId { get; set; }
        public string Uuid { get; set; }
        public string Name { get; set; }
        public int Hp { get; set; }
        public int Exp { get; set; }
        public string Skill { get; set; }
    }
}