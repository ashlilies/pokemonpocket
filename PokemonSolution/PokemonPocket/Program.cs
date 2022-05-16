/*
 * Name: Ashlee Tan
 * Admin number: 211362G
 */

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

using PokemonPocket.Models.Multiplayer;

// Todo remove testing
// Publish on GitHub and send link; cannot send by email due to file size
// Battle: comprehensive, multi-user battles: comprehensive
// 100 line jumbo methods: bad
// Additional creative feature: useful, can be stuff like updating
namespace PokemonPocket {
    public class Program {
        private static PokemonService _svc = PokemonService.GetInstance();

        // async main method for Multiplayer Battle
        public static async Task Main(string[] args) {
            //PokemonMaster list for checking pokemon evolution availability.    
            List<PokemonMaster> pokemonMasters = new List<PokemonMaster>(){
                new PokemonMaster("Pikachu", 2, "Raichu"),
                new PokemonMaster("Eevee", 3, "Flareon"),
                new PokemonMaster("Charmander", 1, "Charmeleon")
            };

            //Use "Environment.Exit(0);" if you want to implement an exit of the console program
            //Start your assignment 1 requirements below.
            _svc.Masters = pokemonMasters;
            await MenuView();
        }

        private static async Task MenuView() {
            bool running = true;
            while (running) {
                PrintMenu();

                Console.Write("Please only enter [1,2,3,4] or Q to quit: ");
                string userInput = Console.ReadLine();

                switch (userInput.ToLower()) {
                case "1":
                    AddPokemon();
                    break;
                case "2":
                    ListPokemon();
                    break;
                case "3":
                    CheckEvolve();
                    break;
                case "4":
                    Evolve();
                    break;
                case "b":
                    Battle();
                    break;
                case "i":
                    ListAlivePokemonWithIds();
                    break;
                case "m":
                    await MPBattle();
                    break;
                case "x":
                    EmptyPocket();
                    break;
                case "q":
                    running = false;
                    Console.WriteLine("Goodbye!");
                    break;
                default:
                    Console.WriteLine("Invalid option! Please try again.");
                    break;
                }
            }
        }

        private static void PrintMenu() {
            Console.WriteLine("*****************************");
            Console.WriteLine("Welcome to Pokemon Pocket App");
            Console.WriteLine("*****************************");
            Console.WriteLine("(1). Add pokemon to my pocket");
            Console.WriteLine("(2). List pokemon(s) in my Pocket");
            Console.WriteLine("(3). Check if I can evolve pokemon");
            Console.WriteLine("(4). Evolve pokemon");
            Console.WriteLine("(b). Battle Pokemon! (additional feature)");
            Console.WriteLine("(i). List alive pokemon(s) in my Pocket (with IDs)");
            Console.WriteLine("(m). Multiplayer Battles! (additional comprehensive feature)");
            Console.WriteLine("(X). Empty Pocket (!!)");
        }

        // Maximum of 1000 hp
        private static void AddPokemon() {
            string name;
            int hp;
            int exp;

            for (; ; ) {
                Console.Write("Enter Pokemon's Name: ");
                name = Console.ReadLine();
                if (name.ToLower() == "eevee"
                        || name.ToLower() == "pikachu"
                        || name.ToLower() == "charmander")
                    break;
                Console.WriteLine("You can only add Pikachus, Eevees and Charmanders. Try again?");
            }


            for (; ; ) {
                Console.Write("Enter Pokemon's HP: ");
                while (!Int32.TryParse(Console.ReadLine(), out hp))
                    Console.Write("HP must be an integer! Try again: ");
                if (hp >= 0 && hp <= 1000)
                    break;
                if (hp >= 1000) {
                    Console.WriteLine("Maximum HP is 1000!");
                    continue;
                }
                Console.WriteLine("HP must be 0 or positive!");
            }

            for (; ; ) {
                Console.Write("Enter Pokemon's Exp: ");
                while (!Int32.TryParse(Console.ReadLine(), out exp))
                    Console.Write("Exp must be an integer! Try again: ");
                if (exp >= 0)
                    break;
                Console.WriteLine("Exp must be 0 or positive!");
            }

            if (_svc.AddPokemon(name, hp, exp))
                Console.WriteLine("Successfully added pokemon!");
            else
                Console.WriteLine("Failed to add pokemon.");
        }

        private static void ListPokemon() {
            List<Pokemon> sortedPokemons = (from p in _svc.Pokemons
                                            orderby p.Hp ascending
                                            select p
                                           ).ToList();
            if (sortedPokemons.Count == 0) {
                Console.WriteLine("----------------------------");
                Console.WriteLine("No pokemon found.");
                Console.WriteLine("----------------------------");
                return;
            }

            foreach (Pokemon p in sortedPokemons) {
                Console.WriteLine("----------------------------");
                Console.WriteLine("Name: {0}", p.Name);
                Console.WriteLine("HP: {0}", p.Hp);
                Console.WriteLine("Exp: {0}", p.Exp);
                Console.WriteLine("Skill: {0}", p.Skill);
                Console.WriteLine("----------------------------");
            }
        }

        // Returns a List of pokemon that are alive, and also prints it out.
        private static List<Pokemon> ListAlivePokemonWithIds() {
            List<Pokemon> sortedPokemons = (from p in _svc.Pokemons
                                            where p.Hp > 0
                                            orderby p.Hp ascending
                                            select p
                                           ).ToList();
            if (sortedPokemons.Count == 0) {
                Console.WriteLine("----------------------------");
                Console.WriteLine("No pokemon found.");
                Console.WriteLine("----------------------------");
                return sortedPokemons;
            }

            foreach (Pokemon p in sortedPokemons) {
                Console.WriteLine("----------------------------");
                Console.WriteLine("ID: {0}", p.PokemonId);
                Console.WriteLine("Name: {0}", p.Name);
                Console.WriteLine("HP: {0}", p.Hp);
                Console.WriteLine("Exp: {0}", p.Exp);
                Console.WriteLine("Skill: {0}", p.Skill);
                Console.WriteLine("----------------------------");
            }

            return sortedPokemons;
        }

        private static void CheckEvolve() {
            List<Tuple<string, string>> evolutions = _svc.CheckEvolve();
            if (evolutions.Count == 0) {
                Console.WriteLine("No possible evolutions found. Catch more!");
                return;
            }

            foreach (var evolution in evolutions)
                Console.WriteLine("{0} --> {1}", evolution.Item1, evolution.Item2);
        }

        private static void Evolve() {
            if (_svc.Evolve())
                Console.WriteLine("Successfully evolved pokemon(s)!");
            else
                Console.WriteLine("Unable to evolve any pokemon yet! Catch more!");
        }

        // TODO: Clean up code
        private static void Battle() {
            int pokemon1Id, pokemon2Id;
            Pokemon pokemon1, pokemon2;

            Console.WriteLine("Alive pokemon: ");
            List<Pokemon> pokemonList = ListAlivePokemonWithIds();
            if (pokemonList.Count < 2) {
                Console.WriteLine("Not enough pokemon to battle! Catch more!");
                return;
            }

            for (; ; ) {
                Console.Write("Enter ID of first pokemon (-1 to cancel): ");
                while (!Int32.TryParse(Console.ReadLine(), out pokemon1Id)) {
                    Console.Write("Invalid ID, try again?: ");
                }

                if (pokemon1Id == -1)
                    return;

                using (PokemonDbContext dbctx = new PokemonDbContext()) {
                    pokemon1 = (from p in dbctx.Pokemons
                                where p.PokemonId == pokemon1Id
                                select p)
                               .FirstOrDefault();
                }
                if (pokemon1 != null) {
                    if (pokemon1.Hp > 0) {
                        break;
                    } else {
                        Console.WriteLine("Pokemon with id {0} isn't alive", pokemon1.PokemonId);
                        continue;
                    }
                }
                Console.WriteLine("Couldn't find pokemon with id {0}", pokemon1Id);
            }

            for (; ; ) {
                Console.Write("Enter ID of second pokemon (-1 to cancel): ");
                while (!Int32.TryParse(Console.ReadLine(), out pokemon2Id)) {
                    Console.Write("Invalid ID, try again?: ");
                }

                if (pokemon2Id == -1)
                    return;

                using (PokemonDbContext dbctx = new PokemonDbContext()) {
                    pokemon2 = (from p in dbctx.Pokemons
                                where p.PokemonId == pokemon2Id
                                select p)
                               .FirstOrDefault();
                }
                if (pokemon2 != null) {
                    if (pokemon2.Hp > 0) {
                        break;
                    } else {
                        Console.WriteLine("Pokemon with id {0} isn't alive", pokemon2.PokemonId);
                        continue;
                    }
                }
                Console.WriteLine("Couldn't find pokemon with id {0}", pokemon2Id);
            }

            // TODO: Figure out why I can't just compare two object references here
            if (pokemon1.PokemonId == pokemon2.PokemonId) {
                Console.WriteLine("Can't battle a pokemon against itself!");
                return;
            }

            Console.WriteLine("Battling {0} ({1} hp) with {2} ({3} hp)...",
                              pokemon1, pokemon1.Hp, pokemon2, pokemon2.Hp);
            BattleService battle = new BattleService(pokemon1, pokemon2);
            bool res = battle.Fight();
            if (!res) {
                Console.WriteLine("Unable to start battle.");
                return;
            }

            foreach (BattleTick tick in battle.BattleTicks) {
                Console.WriteLine(tick);
                Thread.Sleep(1000);
            }
            Console.WriteLine("The winner is {0}! Congratulations!", battle.Winner);
            Console.WriteLine("{0} has gained {1} exp from this battle.", battle.Winner, battle.WinnerExpGain);
        }

        // TODO: Exp support, db support
        private static async Task MPBattle() {
            Pokemon sendForBattle;
            MPBattleService svc;
            List<MPBattleTick> battleTicks;

            Console.Write("Enter battle server host/IP and port [localhost:5000]: ");
            string host = Console.ReadLine();

            if (host == "") {
                Console.WriteLine("Using default of localhost:5000...");
                host = "localhost:5000";
            }

            try {
                svc = new MPBattleService(host);
            } catch (Exception) {
                Console.WriteLine("Invalid host/port specified!");
                return;
            }


            Console.WriteLine("Alive pokemon: ");
            List<Pokemon> pokemonList = ListAlivePokemonWithIds();
            if (pokemonList.Count < 1) {
                Console.WriteLine("Not enough Pokemon to battle! Catch more!");
                return;
            }

            for (; ; ) {
                int pokemonId;

                Console.Write("Enter ID of pokemon you wish to send (-1 to cancel): ");
                while (!Int32.TryParse(Console.ReadLine(), out pokemonId)) {
                    Console.Write("Invalid ID, try again?: ");
                }

                if (pokemonId == -1)
                    return;

                using (PokemonDbContext dbctx = new PokemonDbContext()) {
                    sendForBattle = (from p in dbctx.Pokemons
                                     where p.PokemonId == pokemonId
                                     select p)
                                     .FirstOrDefault();
                }
                if (sendForBattle != null) {
                    if (sendForBattle.Hp > 0) {
                        break;
                    } else {
                        Console.WriteLine("Pokemon with id {0} isn't alive", sendForBattle.PokemonId);
                        continue;
                    }
                }
                Console.WriteLine("Couldn't find pokemon with id {0}", pokemonId);
            }

            Console.WriteLine("Sending {0} ({1} hp) for battle...", sendForBattle, sendForBattle.Hp);

            // Connect to server using created service...
            Console.WriteLine("Connecting to server...");

            try {
                bool res = await svc.JoinBattle(sendForBattle);
                Console.WriteLine("Successfully connected!");
                if (res == false) {
                    Console.WriteLine("Failed to join battle (is another battle ongoing!?), cancelling...");
                    return;
                }

                Console.WriteLine("Waiting for opponent to join...");
                battleTicks = await svc.GetBattleResults();
            // // } catch (Exception e) {  // debug
            } catch (Exception) {
                Console.WriteLine("Failed to connect to server! Cancelling match...");
                // Console.WriteLine($"Detailed error: {e.Message}");  // debug
                return;
            }

            foreach (MPBattleTick tick in battleTicks) {
                Console.WriteLine(tick);
                Thread.Sleep(1000);
            }

            string winner = battleTicks.Last().AttackerName;
            // int winnerId = battleTicks.Last().OpponentId;
            Console.WriteLine("The winner is {0}! Congratulations!", winner);
            if (svc.MetaResults.win) {
                Console.WriteLine("You have won! Congratulations!");
                Console.WriteLine("{0} has gained {1} exp from this battle.", sendForBattle.Name, svc.MetaResults.exp);
            } else {
                Console.WriteLine("You lost this time. Better luck next time!");
            }

            // Console.WriteLine("{0} has gained {1} exp from this battle.", battle.Winner, battle.WinnerExpGain);
        }

        private static void EmptyPocket() {
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.WriteLine("Are you sure you wish to release all your Pokemons?");
            Console.WriteLine("This CANNOT BE UNDONE!");
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

            Console.Write("Enter 'I am very sure' to continue: ");
            string userInput = Console.ReadLine().ToLower();
            if (userInput != "i am very sure") {
                Console.WriteLine("Hopefully never...");
                return;
            }

            using (PokemonDbContext dbctx = new PokemonDbContext()) {
                dbctx.Pokemons.RemoveRange((from p in dbctx.Pokemons
                                            select p).ToArray());
                dbctx.SaveChanges();
            }

            Console.WriteLine("Emptied pocket :c");
        }
    }
}
