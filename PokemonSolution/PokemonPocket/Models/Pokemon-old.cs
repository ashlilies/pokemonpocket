/*
 * Name: Ashlee Tan
 * Admin number: 211362G
 */

using System;

namespace PokemonPocket {
    /// <summary>
    /// Base pokemon class
    /// </summary>
    public abstract class PokemonOld {
        public string Name { get; set; }
        public int Hp { get; set; }
        public int Exp { get; set; }
        public abstract string Skill { get; protected set; }
    }
}