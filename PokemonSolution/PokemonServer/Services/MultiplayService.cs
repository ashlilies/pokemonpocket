using System;
using System.Collections.Generic;

namespace PokemonServer {
    // This service removes players who are inactive longer than 
    // INACTIVE_TIMEOUT_SECONDS; if the user performs a CTRL-C, for example.
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
        // Remove players who are inactive longer than this time
        public readonly double INACTIVE_TIMEOUT_SECONDS = 7;
        private List<BattleTick> battleResults = null;  // use GetResults()

        // <summary>
        // Returns a uuid, or null if not successful (battle already ongoing).
        // </sumamry>
        public string JoinPlayer(Pokemon pokemon) {
            KickInactive();

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
            UpdateLastSeen(uuid);

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

        /// <summary>
        /// If uuid is a valid one, update the last seen to the current time.
        /// </summary>
        private void UpdateLastSeen(string uuid) {
            if (player1 != null) {
                if (player1.Uuid == uuid) {
                    player1.LastSeen = DateTime.Now;
                    return;
                }
            }

            if (player2 != null) {
                if (player2.Uuid == uuid) {
                    player2.LastSeen = DateTime.Now;
                    return;
                }
            }
        }

        /// <summary>
        /// When this method is called, inactive players
        /// (more than INACTIVE_TIMEOUT_SECONDS) are kicked out of the lobby.
        /// </summary>
        private void KickInactive() {
            if (player1 != null) {
                DateTime expiry = player1.LastSeen.AddSeconds(INACTIVE_TIMEOUT_SECONDS);
                if (expiry < DateTime.Now)
                    player1 = null;
            }

            if (player2 != null) {
                DateTime expiry = player2.LastSeen.AddSeconds(INACTIVE_TIMEOUT_SECONDS);
                if (expiry < DateTime.Now)
                    player2 = null;
            }
        }

        private void StartBattle() {
            BattleService svc = new BattleService(player1.Pokemon, player2.Pokemon);
            svc.Fight();
            battleResults = svc.BattleTicks;
        }
    }
}