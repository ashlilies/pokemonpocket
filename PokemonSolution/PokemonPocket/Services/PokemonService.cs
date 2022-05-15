/*
 * Name: Ashlee Tan
 * Admin number: 211362G
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace PokemonPocket {
    /// <summary>
    /// Pokemon service layer singleton
    /// </summary>
    public class PokemonService {
        #region Singleton
        private static PokemonService instance = null;
        public static PokemonService GetInstance() {
            if (instance == null)
                instance = new PokemonService();
            return instance;
        }
        private PokemonService() { }
        #endregion

        /// <summary>
        /// A list to check evolution ability.
        /// </summary>
        public List<PokemonMaster> Masters { get; set; } = new List<PokemonMaster>();
        /// <summary>
        /// A property to retrieve a list of all Pokemons.
        /// </summary>
        // public List<Pokemon> Pokemons { get; private set; } = new List<Pokemon>();
        public List<Pokemon> Pokemons {
            get {
                using (PokemonDbContext dbctx = new PokemonDbContext()) {
                    List<Pokemon> ret = (from p in dbctx.Pokemons
                                         select p).ToList();
                    return ret;
                }
            }
        }

        /// <summary>
        /// HP and EXP only applicable for Pikachu, Eevee, Charmander
        /// </summary>
        public Pokemon PokemonFactory(string pokemonName, int hp = 0, int exp = 0) {
            Pokemon pokemon;

            switch (pokemonName.ToLower()) {
            case "pikachu":
                pokemon = new Pikachu(hp, exp);
                break;
            case "eevee":
                pokemon = new Eevee(hp, exp);
                break;
            case "charmander":
                pokemon = new Charmander(hp, exp);
                break;
            case "raichu":
                pokemon = new Raichu();
                break;
            case "flareon":
                pokemon = new Flareon();
                break;
            case "charmeleon":
                pokemon = new Charmeleon();
                break;
            default:
                return null;
            }

            return pokemon;
        }

        /// <summary>
        /// Return success of adding a pokemon by specifying name.
        /// </summary>
        public bool AddPokemon(string name, int hp, int exp) {
            name = name.ToLower();
            if (name == "pikachu" || name == "eevee" || name == "charmander") {
                // Pokemons.Add(PokemonFactory(name, hp, exp));
                Pokemon pokemon = PokemonFactory(name, hp, exp);
                using (PokemonDbContext dbctx = new PokemonDbContext()) {
                    dbctx.Pokemons.Add(pokemon);
                    dbctx.SaveChanges();
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns a list of tuples of 2 types of pokemons available to evolve.
        /// First item: the pokemon that exists in the pocket.
        /// Second item: the pokemon type that can be evolved to.
        /// Using a list instead of dictionary for multiple evolutions of same
        /// type of pokemon.
        /// </summary>
        public List<Tuple<string, string>> CheckEvolve() {
            List<Tuple<string, string>> ret = new List<Tuple<string, string>>();
            Dictionary<string, int> seen = new Dictionary<string, int>();

            // Add pokemon and count to seen dictionary
            foreach (Pokemon p in Pokemons) {
                if (seen.ContainsKey(p.Name))
                    ++seen[p.Name];
                else
                    seen.Add(p.Name, 1);
            }

            // Check evolution ability against pokemon master
            foreach (var item in seen) {
                foreach (PokemonMaster master in Masters) {
                    if (item.Key == master.Name && item.Value >= master.NoToEvolve) {
                        for (int n = 1; n <= item.Value / master.NoToEvolve; n++)
                            ret.Add(new Tuple<string, string>(master.Name, master.EvolveTo));
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Evolve all possible pokemon combinations.
        /// Always evolves the highest exp pokemons first.
        /// Return success result.
        /// </summary>
        public bool Evolve() {
            var evolutions = CheckEvolve();
            if (evolutions.Count == 0)
                return false;

            // For each possible duplicate evolution
            foreach (var evolution in evolutions) {
                // Get number of original pokemon of this evolution to remove.
                int num = 0;

                foreach (PokemonMaster master in Masters) {
                    if (master.Name == evolution.Item1)
                        num = master.NoToEvolve;
                }

                // Remove number of pokemons from pocket.
                List<Pokemon> toRemove = (from p in Pokemons
                                          where p.Name == evolution.Item1
                                          orderby p.Exp descending
                                          select p
                                         )
                                         .Take(num)
                                         .ToList();
                
                // Pokemons = Pokemons.Except(toRemove).ToList();
                using (PokemonDbContext dbctx = new PokemonDbContext()) {
                    dbctx.Pokemons.RemoveRange(toRemove);
                    dbctx.SaveChanges();
                }

                // Create new pokemons
                Pokemon newPokemon = PokemonFactory(evolution.Item2);
                // Pokemons.Add(newPokemon);
                using (PokemonDbContext dbctx = new PokemonDbContext()) {
                    dbctx.Pokemons.Add(newPokemon);
                    dbctx.SaveChanges();
                }
            }

            return true;
        }
    }
}