using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Additional creative feature.
/// Transient service providing battle functionality.
/// Each pokemon can attack from 1 up to 100 dmg per tick.
/// The winner gets anywhere from 1-100 XP.
/// Each instance refers to one battle.
/// </summary>
namespace PokemonServer {
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

            if (Pokemon1.Hp <= 0 || Pokemon2.Hp <= 0 || CurrentAttacker.Uuid == CurrentOpponent.Uuid)
                return false;

            while (Pokemon1.Hp > 0 && Pokemon2.Hp > 0) {
                NextTick();
                SwapAttackerOpponent();
            }
            if (Pokemon1.Hp > 0)
                Winner = Pokemon1;
            else
                Winner = Pokemon2;

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
            

            // Log this tick
            BattleTicks.Add(new BattleTick {
                // AttackerId = CurrentAttacker.PokemonId,
                // OpponentId = CurrentOpponent.PokemonId,
                AttackerUuid = CurrentAttacker.Uuid,
                AttackerName = CurrentAttacker.Name,
                AttackerSkill = CurrentAttacker.Skill,
                OpponentUuid = CurrentOpponent.Uuid,
                OpponentName = CurrentOpponent.Name,
                PokemonDamage = dmg,
                OpponentHealthAfterTick = CurrentOpponent.Hp
            });
        }

        private void SwapAttackerOpponent() {
            Pokemon temp = CurrentAttacker;
            CurrentAttacker = CurrentOpponent;
            CurrentOpponent = temp;
        }
    }
}