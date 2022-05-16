using System;

namespace PokemonServer {
    public class Player {
        public string Uuid { get; set; }
        public Pokemon Pokemon { get; set; }
        public DateTime LastSeen { get; set; }

        public Player(Pokemon pokemon, string uuid) {
            Pokemon = pokemon;
            Uuid = uuid;
            LastSeen = DateTime.Now;
        }
    }
}