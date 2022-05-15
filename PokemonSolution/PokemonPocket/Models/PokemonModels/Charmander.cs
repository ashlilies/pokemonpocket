/*
 * Name: Ashlee Tan
 * Admin number: 211362G
 */

using System;

namespace PokemonPocket {
    public class Charmander : Pokemon {
        public override string Skill { get; protected set; }

        public Charmander(int hp, int exp) : base("Charmander", hp, exp) {
            Skill = "Solar Power";
        }
    }
}