/*
 * Name: Ashlee Tan
 * Admin number: 211362G
 */

using System;

namespace PokemonPocket {
    // todo: call base construv
    public class Pikachu : Pokemon {
        public override string Skill { get; protected set; }

        public Pikachu(int hp, int exp) : base("Pikachu", hp, exp) {
            Skill = "Lightning Bolt";
        }
    }
}