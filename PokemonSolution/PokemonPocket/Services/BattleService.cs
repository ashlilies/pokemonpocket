using System;
using System.Collections.Generic;
using System.Linq;

namespace PokemonPocket {
    /// <summary>
    /// Additional creative feature.
    /// Transient service providing battle functionality.
    /// Each pokemon can attack from 1 up to 100 dmg per tick.
    /// The winner gets anywhere from 1-100 XP.
    /// Each instance refers to one battle.
    /// </summary>
    public class BattleService {

        public Pokemon Pokemon1 { get; }
        public Pokemon Pokemon2 { get; }
        public List<BattleTick> BattleTicks { get; } = new List<BattleTick>();
        public Pokemon Winner { get; private set; } = null;
        public int WinnerExpGain { get; private set; }
        private Pokemon CurrentAttacker { get; set; }
        private Pokemon CurrentOpponent { get; set; }

        /// <summary>
        /// Creates a battle between two pokemons. Use Fight() method to start.
        /// </summary>
        public BattleService(Pokemon pokemonOne, Pokemon pokemonTwo) {
            Pokemon1 = pokemonOne;
            Pokemon2 = pokemonTwo;
        }

        /// <summary>
        /// Starts the fight loop.
        /// Returns true if fight started and ended successfully.
        /// </summary>
        public bool Fight() {
            CurrentAttacker = Pokemon1;
            CurrentOpponent = Pokemon2;

            if (Pokemon1.Hp <= 0 || Pokemon2.Hp <= 0 || CurrentAttacker.PokemonId == CurrentOpponent.PokemonId)
                return false;

            while (Pokemon1.Hp > 0 && Pokemon2.Hp > 0) {
                NextTick();
                SwapAttackerOpponent();
            }
            if (Pokemon1.Hp > 0)
                Winner = Pokemon1;
            else
                Winner = Pokemon2;

            // Update winner exp
            Random rand = new Random();
            WinnerExpGain = rand.Next(1, 100 + 1);
            Winner.Exp += WinnerExpGain;
            using (PokemonDbContext dbctx = new PokemonDbContext()) {
                Pokemon pokemonToRemove = (from p in dbctx.Pokemons
                                           where p.PokemonId == Winner.PokemonId
                                           select p
                                          ).First();
                dbctx.Pokemons.Remove(pokemonToRemove);
                dbctx.Pokemons.Add(Winner);
                dbctx.SaveChanges();
            }
            return true;
        }

        // For internal use only
        private void NextTick() {
            Random rand = new Random();
            int dmg = rand.Next(1, 100 + 1);

            // Deal damage on the opponent
            CurrentOpponent.Hp = ((CurrentOpponent.Hp - dmg) >= 0)
                                 ? CurrentOpponent.Hp - dmg
                                 : 0;

            // Update db
            using (PokemonDbContext dbctx = new PokemonDbContext()) {
                Pokemon pokemonToRemove = (from p in dbctx.Pokemons
                                           where p.PokemonId == CurrentOpponent.PokemonId
                                           select p
                                          ).First();
                dbctx.Pokemons.Remove(pokemonToRemove);
                dbctx.Pokemons.Add(CurrentOpponent);
                dbctx.SaveChanges();
            }

            // Log this tick
            BattleTicks.Add(new BattleTick(CurrentAttacker, CurrentOpponent, dmg, CurrentOpponent.Hp));
        }

        private void SwapAttackerOpponent() {
            Pokemon temp = CurrentAttacker;
            CurrentAttacker = CurrentOpponent;
            CurrentOpponent = temp;
        }
    }

    /// <summary>
    /// Used for results logging to output.
    /// </summary>
    public struct BattleTick {
        public Pokemon Attacker { get; }
        public Pokemon Opponent { get; }
        public int PokemonDamage { get; }
        public int OpponentHealthAfterTick { get; }

        public BattleTick(Pokemon attacker, Pokemon opponent, int pokemonDamage, int opponentHealthAfterTick) {
            Attacker = attacker;
            Opponent = opponent;
            PokemonDamage = pokemonDamage;
            OpponentHealthAfterTick = opponentHealthAfterTick;
        }

        public override string ToString() {
            return String.Format("{0} has used {1} on {2} for {3} damage! {4} has {5} hp left.",
                                 Attacker, Attacker.Skill, Opponent, PokemonDamage, Opponent, OpponentHealthAfterTick);
        }
    }
}