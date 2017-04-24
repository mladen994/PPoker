using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPoker.CardMechanics {
    public class Deck {
        private List<Card> _cards = new List<Card>();
        private static Random _rng = new Random();

        public Deck() {
            _cards.Clear();
            ResetDeck();
            ShuffleDeck();

        }
        public void ResetDeck() {
            _cards.Clear();
            for (int i = 0; i < 4; ++i) {
                for (int j = 0; j < 13; ++j) {
                    _cards.Add(new Card(j, i));
                }
            }
        }
        public void ShuffleDeck() {
            int n = _cards.Count();
            while (n > 1) {
                --n;
                int k = _rng.Next(n + 1);
                Card value = _cards[k];
                _cards[k] = _cards[n];
                _cards[n] = value;
            }
        }
        public void printDeck() {
            System.Console.WriteLine("------------------------------");
            System.Console.WriteLine("Cards currently in deck:");
            foreach (Card x in _cards) {
                System.Console.Write(" ");
                x.printCard();
            }
            System.Console.WriteLine("------------------------------");
        }
        public List<Card> dealCards(int numOfCards) {
            List<Card> returner = new List<Card>(numOfCards);
            while (numOfCards > 0) {
                returner.Add(_cards[0]);
                _cards.RemoveAt(0);
                --numOfCards;
            }
            return returner;
        }
        public void addCards(List<Card> toBeAdded) {
            _cards.AddRange(toBeAdded);
        }
        public void returnCardsToDeck(List<Card> newCards) {
            addCards(newCards);
        }
        public void returnCardToDeck(Card newCard) {
            _cards.Add(newCard);
        }
        public List<Card> exchangeCards(List<Card> toBeAdded) {
            List<Card> returner = dealCards(toBeAdded.Count());
            addCards(toBeAdded);
            return returner;
        }

    }
}
