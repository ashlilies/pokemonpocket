/*
 * Name: Ashlee Tan
 * Admin number: 211362G
 */

 using System;

namespace PokemonPocket {
    /// <summary>
    /// Base pokemon class
    /// </summary>
    public abstract class Pokemon {
        // Primary key for database
        public int PokemonId { get; set; }
        public string Uuid { get; set; }  // used for multiplayer battles
        public string Name { get; set; }
        public int Hp { get; set; }
        public int Exp { get; set; }
        public abstract string Skill { get; protected set; }

        protected Pokemon(string name, int hp, int exp) {
            Name = name;
            Hp = hp;
            Exp = exp;
            Uuid = Guid.NewGuid().ToString();
        }

        public override string ToString() {
            return String.Format("{0} ({1})", Name, PokemonId);
        }
    }
}