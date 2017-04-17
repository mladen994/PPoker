using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPoker.CardMechanics {
    public class Card {
        private string value;
        private string suite;

        public Card(int value, int suite) {
            this.value = values[value];
            this.suite = suites[suite];
        }
        public Card(string value, string suite) {
            if (values.Contains(value) && suites.Contains(suite)) {
                this.value = value;
                this.suite = suite;
            } else {
                throw new ArgumentException("Invalid parameter(s)!");
            }
        }
        public bool isSameValueAs(Card x) {
            return x.value.Equals(value);
        }
        public bool isSameSuiteAs(Card x) {
            return x.suite.Equals(suite);
        }
        public string getValue() {
            return value;
        }
        public string getSuite() {
            return suite;
        }
        public int getValueAsInt() {
            return values.IndexOf(value);
        }
        public void printCard() {
            System.Console.WriteLine("\t" + value + " of " + suite + ".");
        }
        /// <summary>
        /// Returns all card properties in a string of three words: 'value' + 'of' + 'suite'.
        /// </summary>
        /// <returns></returns>
        public string getCard() {
            return value + " of " + suite;
        }
        public static bool operator <(Card x, Card y) {            
            return values.IndexOf(x.value) < values.IndexOf(y.value);
        }
        public static bool operator >(Card x, Card y) {
            return values.IndexOf(x.value) > values.IndexOf(y.value);
        }
        public static bool operator >=(Card x, Card y) {
            return values.IndexOf(x.value) >= values.IndexOf(y.value);
        }
        public static bool operator <=(Card x, Card y) {
            return values.IndexOf(x.value) <= values.IndexOf(y.value);
        }
        private static List<string> values = new List<string> { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "Ace" };
        private static List<string> suites = new List<string> { "Diamonds", "Hearts", "Clubs", "Spades" };
    }
}
