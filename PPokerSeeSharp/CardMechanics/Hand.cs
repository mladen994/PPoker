using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPoker.CardMechanics {
    public enum ComparisonResult {
        WIN,
        LOSE,
        DRAW
    }
    public class Hand {
        /// <summary>
        /// Cards in Hand organized in linked list.
        /// </summary>
        private List<Card> _cards = new List<Card>();
        private Deck _deck;
        private int _Power;

        public Hand(Deck _deck) {
            _Power = 0;
            this._deck = _deck;
            drawCards(5);
            sortHand();
            calculatePower();
        }
        public void drawCards(int numOfCards) {
            _cards = _deck.dealCards(numOfCards);
        }
        public void exchangeCards(int numOfCards) {
            System.Console.WriteLine("At what positions? [0..4] separated by new line.");
            List<Card> toExchange = new List<Card>();
            for (int i = 0; i < numOfCards; ++i) {
                toExchange.Add(_cards[Int32.Parse(System.Console.ReadLine())]);
            }
            exchangeCards(toExchange);
        }
        public void exchangeCards(List<Card> toExchange) {
            _cards.RemoveAll(card => toExchange.Contains(card));
            _cards.AddRange(_deck.exchangeCards(toExchange));
            sortHand();
            calculatePower();
        }
        public void addCardAt(Card x, int position) {
            _cards[position] = x;
        }
        public void printHand() {
            sortHand();
            System.Console.WriteLine("\t------------------------------");
            foreach (Card x in _cards) {
                x.printCard();
            }
            System.Console.WriteLine("\t------------------------------");
        }
        private void swapCards(int x, int y) {
            if (x == y)
                return;
            Card temp = _cards[x];
            _cards[x] = _cards[y];
            _cards[y] = temp;
        }
        private string findPair() {
            sortHand();
            for (int i = 0; i < 4; ++i) {
                if (_cards[i].isSameValueAs(_cards[i + 1]))
                    return _cards[i].getValue();
            }
            throw new ArgumentException("HOW?!?!?", "This line should have never been executed! :S");
        }
        public void CheckRanks() {
            System.Console.WriteLine("------------------------");
            switch (_Power) {
                case 1:
                    System.Console.WriteLine("You have a high " + _cards[0].getValue() + ".");
                    break;
                case 2:
                    System.Console.WriteLine("You have a pair of " + findPair() + "s.");
                    break;
                case 3:
                    System.Console.WriteLine("You have two pairs of " + _cards[1].getValue() + "s and " + _cards[3].getValue() + "s.");
                    break;
                case 4:
                    System.Console.WriteLine("You have three " + _cards[2].getValue() + "s of a kind!");
                    break;
                case 5:
                    System.Console.WriteLine("You have a straght " + _cards[4].getValue() + " to " + _cards[0].getValue() + "!");
                    break;
                case 6:
                    System.Console.WriteLine("You have a flush of " + _cards[0].getSuite() + "!");
                    break;
                case 7:
                    System.Console.WriteLine("You have a full house of " + _cards[0].getValue() + "s and " + _cards[3].getValue() + "s!");
                    break;
                case 8:
                    System.Console.WriteLine("You have four " + _cards[3].getValue() + "s of a kind!!");
                    break;
                case 9:
                    System.Console.WriteLine("You have a straght flush of " + _cards[0].getSuite() + " ranked " + _cards[4].getValue() + " to " + _cards[0].getValue() + "!!");
                    break;
                case 10:
                    System.Console.WriteLine("You have a royal flush!!!");
                    break;
                default:
                    break;
            }
            System.Console.WriteLine("------------------------");
        }
        public void calculatePower() {
            _Power = 0;
            int temp = 0;
            for (int i = 0; i < _cards.Count() - 1; ++i) {
                for (int j = i + 1; j < _cards.Count(); ++j) {
                    if (_cards[i].isSameValueAs(_cards[j]))
                        ++temp;
                }
            }
            switch (temp) {
                case 1:
                    _Power = 2;
                    break;
                case 2:
                    _Power = 3;
                    break;
                case 3:
                    _Power = 4;
                    break;
                case 4:
                    _Power = 7;
                    break;
                case 6:
                    _Power = 8;
                    break;
                default:
                    break;
            }
            if (_Power == 0) {
                string substr = _cards[0].getValue() + _cards[1].getValue() + _cards[2].getValue() + _cards[3].getValue() + _cards[4].getValue();
                bool isStraight = "AceKQJ1098765432Ace5432".Contains(substr);
                bool isFlush = _cards[0].isSameSuiteAs(_cards[1]) && _cards[1].isSameSuiteAs(_cards[2]) && _cards[2].isSameSuiteAs(_cards[3]) && _cards[3].isSameSuiteAs(_cards[4]);
                if (isStraight && isFlush) {
                    _Power = 9;
                    if ("Ace".Equals(_cards[0].getValue()))
                        ++_Power;
                } else if (isStraight) {
                    _Power = 5;
                } else if (isFlush) {
                    _Power = 6;
                } else _Power = 1;
            }
        }
        private void sortHand() {
            for (int i = 0; i < 4; ++i) {
                for (int j = i + 1; j < 5; ++j) {
                    if (_cards[i] < _cards[j])
                        swapCards(i, j);
                }
            }
            sortByRelevance();
        }
        private void sortByRelevance() {
            switch (_Power) {
                case 2:
                    for (int i = 0; i < 4; ++i) {
                        if (_cards[i].isSameValueAs(_cards[i + 1])) {
                            swapCards(0, i);
                            swapCards(1, i + 1);
                            break;
                        }
                    }
                    for (int i = 2; i < 4; ++i) {
                        for (int j = i + 1; j < _cards.Count(); ++j) {
                            if (_cards[i] < _cards[j])
                                swapCards(i, j);
                        }
                    }
                    break;
                case 3:
                    for (int i = 0; i < 4; ++i) {
                        if (_cards[i].isSameValueAs(_cards[i + 1])) {
                            swapCards(0, i);
                            swapCards(1, i + 1);
                            break;
                        }
                    }
                    for (int i = 2; i < 4; ++i) {
                        if (_cards[i].isSameValueAs(_cards[i + 1])) {
                            swapCards(2, i);
                            swapCards(3, i + 1);
                            break;
                        }
                    }
                    if (_cards[0] < _cards[2]) {
                        swapCards(0, 2);
                        swapCards(1, 3);
                    }
                    break;
                case 4:
                case 7:
                    swapCards(0, 2);
                    break;
                case 8:
                    if (!_cards[0].isSameValueAs(_cards[1]))
                        swapCards(0, 4);
                    break;

                default:
                    break;
            }
        }
        public int getPower() {
            return _Power;
        }
        public ComparisonResult compareHands(Hand other) {
            if (this != other) {
                sortHand();
                other.sortHand();
                if (_Power > other.getPower()) {
                    return ComparisonResult.WIN;
                } else if (_Power < other.getPower()) {
                    return ComparisonResult.LOSE;
                } else {
                    for (int i = 0; i < 5; ++i) {
                        if (_cards[i].getValueAsInt() > other._cards[i].getValueAsInt())
                            return ComparisonResult.WIN;
                        else if (_cards[i].getValueAsInt() < other._cards[i].getValueAsInt())
                            return ComparisonResult.LOSE;
                    }
                }
            }
            return ComparisonResult.DRAW;
        }
        public void ReturnAllToDeck() {
            _deck.returnCardsToDeck(_cards);
            _cards.Clear();
        }
        public void ReturnCardToDeck(Card x) {
            _deck.returnCardToDeck(x);
            _cards.Remove(x);
        }
        public void ReturnCardsToDeck(List<Card> SelectedFew) {
            _deck.returnCardsToDeck(SelectedFew);
            foreach (Card card in SelectedFew) {
                _cards.Remove(card);
            }
        }
        public void ResetHand() {
            _cards.Clear();
            _Power = 0;
        }
        public static bool operator <(Hand x, Hand y) {
            return x.compareHands(y) == ComparisonResult.LOSE;
        }
        public static bool operator >(Hand x, Hand y) {
            return x.compareHands(y) == ComparisonResult.WIN;
        }
        public static bool operator >=(Hand x, Hand y) {
            return x.compareHands(y) != ComparisonResult.LOSE;
        }
        public static bool operator <=(Hand x, Hand y) {
            return x.compareHands(y) != ComparisonResult.WIN;
        }
        //public static bool operator ==(Hand x, Hand y) {
        //    return x.compareHands(y) == ComparisonResult.DRAW;
        //}
        //public static bool operator !=(Hand x, Hand y) {
        //    return x.compareHands(y) != ComparisonResult.DRAW;
        //}
    }
    /* Power levels:
     * 10 - Royal Flush
     * 9 - Straight Flush
     * 8 - Four of a Kind
     * 7 - Full House
     * 6 - Flush
     * 5 - Straight
     * 4 - Three of a kind
     * 3 - Two Pairs
     * 2 - One Pair
     * 1 - A highcard
     */
}
