/*
 * Slightly different from client - no abstract skill.
 */

 using System;

namespace PokemonServer {
    /// <summary>
    /// Base pokemon class
    /// </summary>
    public class Pokemon {
        // Primary key for database
        public int PokemonId { get; set; }
        public string Name { get; set; }
        public string Uuid { get; set; }
        public int Hp { get; set; }
        public int Exp { get; set; }
        public string Skill { get; set; }

        public Pokemon(string name, int hp, int exp) {
            Name = name;
            Hp = hp;
            Exp = exp;
        }

        public override string ToString() {
            return String.Format("{0} ({1})", Name, PokemonId);
        }
    }
}