using System;

namespace PokemonPocket.Models.Multiplayer {

    public class MPBattleTick {
        // necessary for opponent to see details
        public string AttackerUuid { get; set; }
        public string AttackerName { get; set; }
        public string AttackerSkill { get; set; }
        public string OpponentUuid { get; set; }
        public string OpponentName { get; set; }

        // can't deserialize the following
        // public Pokemon Attacker { get; set; }
        // public Pokemon Opponent { get; set; }
        public int PokemonDamage { get; set; }
        public int OpponentHealthAfterTick { get; set; }

        public override string ToString() {
            return String.Format("{0} has used {1} on {2} for {3} damage! {4} has {5} hp left.",
                                    AttackerName, AttackerSkill, OpponentName, PokemonDamage, OpponentName, OpponentHealthAfterTick);
        }
    }
}