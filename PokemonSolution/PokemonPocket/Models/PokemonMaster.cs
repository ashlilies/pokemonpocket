/*
 * Name: Ashlee Tan
 * Admin number: 211362G
 */

using System;
using System.Collections.Generic;

namespace PokemonPocket {
    public class PokemonMaster {
        public string Name { get; set; }
        public int NoToEvolve { get; set; }
        public string EvolveTo { get; set; }

        public PokemonMaster(string name, int noToEvolve, string evolveTo) {
            this.Name = name;
            this.NoToEvolve = noToEvolve;
            this.EvolveTo = evolveTo;
        }
    }
}