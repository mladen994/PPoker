using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPoker.CardMechanics;

namespace PPoker.PlayerMechanics {
    public class Table {
        public List<Player> players = new List<Player>();
        public int maxPlayers;
        public int minPlayers;
        public int ante;
        private int _round;
        private int _cycle;
        private int _dealerCounter;
        private bool _gameStarted;
        private int _pot;
        private int _anteIncrement;
        public string roomName;
        public Deck deck;
        private Dictionary<List<Player>, int> _sidePots;

        public Table(Deck deck,
                    string roomName,
                    int maxPlayers = 4,
                    int minPlayers = 2,
                    int initAnte = 1,
                    int anteIncrement = 2) {
            this.roomName = roomName;
            this.deck = deck;
            this.maxPlayers = maxPlayers;
            this.minPlayers = minPlayers;
            this.ante = initAnte;
            this._anteIncrement = anteIncrement;
            _cycle = _round = _pot = 0;
            _gameStarted = false;
        }
        /// <summary>
        /// Is the table ready to start a game?
        /// </summary>
        /// <returns></returns>
        public bool isReady() {
            return players.Count <= maxPlayers && players.Count >= minPlayers;
        }
        public void startGame() {
            if (isReady()) {
                _gameStarted = true;
                game();
            } else {
                throw new ArgumentException("Player number not satisfied!");
            }
        }
        public void addPlayer(string nickname, int ballance) {
            if (!_gameStarted) {
                if (players.Count < maxPlayers) {
                    Player player = new Player(nickname, ballance);
                    players.Add(player);
                } else {
                    throw new ArgumentException("Room full!");
                }
            } else {
                throw new ArgumentException("Game already started!");
            }
        }
        public void removePlayer(string nickname) {
            Player player = players.First(plr => plr.nickname == nickname);
            _pot += player.ballance;
            players.Remove(player);
        }
        private void game() {
            while (players.Count > 1) {
                resetRoundPhase();
                drawPhase();
                if (firstBetPhase()) {
                    exchangePhase();
                    if (secondBetPhase()) {
                        preShowdownPhase();
                        showdownPhase();
                    }
                }
                kickPhase();
            }
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.WriteLine("Congratulations \"" + players[0].nickname + "\" on winning this match!\nYou won " + players[0].ballance + " čuna!");
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        }
        private void resetRoundPhase() {
            ++_round;
            _pot = 0;
            _dealerCounter = ++_dealerCounter % players.Count;
            if (_dealerCounter == 0) {
                ++_cycle;
                ante += _anteIncrement;
            }
            deck.ResetDeck();
            deck.ShuffleDeck();
            foreach (var player in players) {
                player.didFold = false;
                player.didAllIn = false;
                player.bagBackup = 0;
            }
        }
        private void drawPhase() {
            for (int i = 0; i < players.Count; ++i) {
                int curIndex = (_dealerCounter + i) % players.Count;
                players[curIndex].dealCards(deck);
                _pot += players[curIndex].placeAnte(ante);
            }
        }
        private bool firstBetPhase() {
            int currentRaise = 0;
            int playerIndex = _dealerCounter;
            int playersFolded = 0;
            int counter = players.Count() - playersFolded;
            while (counter > 0) {
                if (!players[playerIndex].didFold) {
                    Console.WriteLine("---------------------------------");
                    Console.WriteLine("Pot ballance: " + _pot + " čuna.");
                    foreach (Player x in players) {
                        Console.WriteLine(x.nickname + ": bag:" + x.bag + " čuna." + ((x.didFold) ? "[FOLD]" : ""));
                    }
                    Console.WriteLine("---------------------------------");
                    Console.Write(players[playerIndex].nickname + " ");
                    players[playerIndex]._hand.printHand();
                    PlayerAction action = players[playerIndex].betAction(currentRaise - players[playerIndex].bag);
                    if (action == PlayerAction.RAISE || (action == PlayerAction.ALL_IN && players[playerIndex].bag > currentRaise)) {
                        currentRaise = players[playerIndex].bag;
                        counter = players.Count - playersFolded;
                    }
                    if (action == PlayerAction.FOLD) {
                        if (++playersFolded == players.Count() - 1) {
                            dumpBagsToPot();
                            return false;
                        }
                    }
                    --counter;
                }
                playerIndex = ++playerIndex % players.Count;
            }
            dumpBagsToPot();
            return true;
        }
        private void exchangePhase() {
            for (int i = 0; i < players.Count; ++i) {
                int j = (i + _dealerCounter) % players.Count;

                players[j]._hand.printHand();
                Console.WriteLine(players[j].nickname + ", do you wish to exchange a card? Y/N");
                var checker = System.Console.ReadLine().ToUpper();
                while (!checker.Equals("Y") && !checker.Equals("N")) {
                    Console.WriteLine("Invalid Input, please try again.");
                    checker = System.Console.ReadLine().ToUpper();
                }
                if (checker.Equals("Y")) {
                    System.Console.WriteLine("How many?");
                    int numOfCards = Int32.Parse(System.Console.ReadLine());
                    players[j]._hand.exchangeCards(numOfCards);
                    players[j]._hand.printHand();
                }
            }
        }
        private bool secondBetPhase() {
            int currentRaise = 0;
            int playerIndex = _dealerCounter;
            int playersFolded = 0;
            foreach (Player player in players) {
                if (player.didFold) {
                    ++playersFolded;
                }
            }
            int counter = players.Count() - playersFolded;
            while (counter > 0) {
                if (!players[playerIndex].didFold) {
                    Console.WriteLine("---------------------------------");
                    Console.WriteLine("Pot ballance: " + _pot + " čuna.");
                    foreach (Player x in players) {
                        Console.WriteLine(x.nickname + ": bag : " + x.bag);
                    }
                    Console.WriteLine("---------------------------------");
                    Console.Write(players[playerIndex].nickname + " ");
                    PlayerAction action = players[playerIndex].betAction(currentRaise - players[playerIndex].bag);
                    if (action == PlayerAction.RAISE || (action == PlayerAction.ALL_IN && players[playerIndex].bag > currentRaise)) {
                        currentRaise = players[playerIndex].bag;
                        counter = players.Count - playersFolded;
                    }
                    if (action == PlayerAction.FOLD) {
                        if (++playersFolded == players.Count() - 1) {
                            dumpBagsToPot();
                            return false;
                        }
                    }
                    --counter;
                }
                playerIndex = ++playerIndex % players.Count;
            }
            dumpBagsToPot();
            return true;
        }
        private void preShowdownPhase() {
            _sidePots = new Dictionary<List<Player>, int>();
            var playersByBet = new List<Player>();
            playersByBet = players.Where(plr => !plr.didFold).ToList<Player>();
            playersByBet = playersByBet.OrderBy(plr => plr.bagBackup).ToList<Player>();
            for (int i = 0; i < playersByBet.Count - 1; ++i) {
                if (playersByBet[i].bagBackup != 0) {
                    List<Player> sidePotOwners = new List<Player>();
                    int potVal = playersByBet[i].bagBackup;
                    int sidePotSum = 0;
                    for (int j = i; j < playersByBet.Count; ++j) {
                        sidePotOwners.Add(playersByBet[j]);
                        sidePotSum += potVal;
                        playersByBet[j].bagBackup -= potVal;
                    }
                    _sidePots.Add(sidePotOwners, sidePotSum);
                }
            }
            playersByBet.Last().ballance += playersByBet.Last().bagBackup;
            playersByBet.Last().bagBackup = 0;
            foreach (Player player in playersByBet) {
                if (player.bagBackup > 0) {
                    Console.WriteLine("Ooops :S");
                }
            }

            foreach (KeyValuePair<List<Player>, int> sidePot in _sidePots) {
                Console.WriteLine("Players participating of sidepot of value '{0}':", sidePot.Value);
                foreach (Player player in sidePot.Key) {
                    Console.WriteLine(player.nickname);
                }
            }
        }
        private void showdownPhase() {
            foreach (KeyValuePair<List<Player>, int> sidePot in _sidePots) {
                foreach (Player x in sidePot.Key) {
                    foreach (Player y in sidePot.Key) {
                        if (y != x && !y.didFold && !x.didFold) {
                            ComparisonResult obracun = x._hand.compareHands(y._hand);
                            y.didFold = obracun == ComparisonResult.WIN;
                            x.didFold = obracun == ComparisonResult.LOSE;
                        }
                    }

                }
                int winnerCount = sidePot.Key.Where(plr => !plr.didFold).Count();
                Console.WriteLine("Winner" + (winnerCount == 1 ? "s" : "") + " of the sidepot of value '{0}' in which participated: ", sidePot.Value);
                foreach (Player x in sidePot.Key) {
                    Console.WriteLine(x.nickname);
                }
                Console.WriteLine((winnerCount == 1 ? "is:" : "are:"));
                foreach (Player x in sidePot.Key) {
                    if (!x.didFold) {
                        Console.WriteLine(x.nickname);
                        x.ballance += sidePot.Value / winnerCount;
                    }
                    x.didFold = false;
                }
            }
            Console.WriteLine("Balance of each player at the end of round '{0}' is:", _round);
            foreach (Player x in players) {
                Console.WriteLine(x.nickname + "\t" + x.ballance);
            }
        }
        private void kickPhase() {
            players.RemoveAll(x => x.ballance == 0);
        }
        public void tableState() {
            System.Console.WriteLine("**********************************************");
            System.Console.WriteLine("Table State:\n");
            System.Console.WriteLine("Current round: " + _round + ", current cycle: " + _cycle + ".");
            System.Console.WriteLine("Number of players: " + players.Count + ".");
            System.Console.WriteLine("Pot ballance: " + _pot + " čuna.");
            System.Console.WriteLine();
            for (int i = 0; i < players.Count; i++) {
                System.Console.WriteLine("Player " + i + " aka \"" + players[i].nickname + "\"" + ((i == _dealerCounter) ? " is dealer:" : ":"));
                System.Console.WriteLine("\tCurrent ballance: " + players[i].ballance);
                System.Console.WriteLine("\tCurrent bag ballance: " + players[i].bag);
                if (players[i]._hand != null) {
                    players[i]._hand.printHand();
                }
                System.Console.WriteLine("=================================================");
            }
            System.Console.WriteLine("**********************************************");
        }
        private void dumpBagsToPot() {
            foreach (Player player in players) {
                _pot += player.bag;
                player.bag = 0;
            }
        }
    }
}
