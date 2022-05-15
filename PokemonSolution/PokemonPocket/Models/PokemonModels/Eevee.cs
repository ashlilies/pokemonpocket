/*
 * Name: Ashlee Tan
 * Admin number: 211362G
 */

using System;

namespace PokemonPocket {
    public class Eevee : Pokemon {
        public override string Skill { get; protected set; }

        public Eevee(int hp, int exp) : base("Eevee", hp, exp) {
            Skill = "Run Away";
        }
    }
}