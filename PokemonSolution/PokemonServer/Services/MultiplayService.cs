using System;
using System.Collections.Generic;

namespace PokemonServer {
    public class MultiplayService {
        #region Singleton
        private static MultiplayService _instance = null;
        public static MultiplayService GetInstance() {
            if (_instance == null)
                _instance = new MultiplayService();
            return _instance;
        }
        private MultiplayService() {}
        #endregion

        public Player player1 = null;
        public Player player2 = null;
        private List<BattleTick> battleResults = null;  // use GetResults()

        // <summary>
        // Returns a uuid, or null if not successful (battle already ongoing).
        // </sumamry>
        public string JoinPlayer(Pokemon pokemon) {
            if (player1 != null && player2 != null)
                return null;
            
            if (pokemon.Hp <= 0)
                return null;

            if (player1 != null) {
                if (player1.Pokemon.Uuid == pokemon.Uuid)
                    return null;
            }

            string uuid = Guid.NewGuid().ToString();
            if (player1 == null)
                player1 = new Player(pokemon, uuid);
            else {
                player2 = new Player(pokemon, uuid);
                StartBattle();
            }
            return uuid;
        }

        // <summary>
        // Gets battle results for the specific player and removes them from lobby.
        // Returns null if the player isn't found in lobby / already removed.
        // </summary>
        public List<BattleTick> GetResults(string uuid) {
            if (battleResults == null)
                return null;

            if (player1 != null) {
                if (player1.Uuid == uuid) {
                    player1 = null;
                    List<BattleTick> ret = new List<BattleTick>(battleResults);
                    
                    // hacky solution
                    if (player2 == null)
                        battleResults = null;

                    return ret;
                }
            }

            if (player2 != null) {
                if (player2.Uuid == uuid) {
                    player2 = null;
                    List<BattleTick> ret = new List<BattleTick>(battleResults);

                    // hacky solution
                    if (player1 == null)
                        battleResults = null;

                    return ret;
                }
            }

            return null;
        }

        private void StartBattle() {
            BattleService svc = new BattleService(player1.Pokemon, player2.Pokemon);
            svc.Fight();
            battleResults = svc.BattleTicks;
        }
    }
}