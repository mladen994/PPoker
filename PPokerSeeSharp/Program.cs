using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPoker.CardMechanics;
using PPoker.PlayerMechanics;

namespace PPoker {
    class Program {
        static void Main(string[] args) {
            System.Console.WriteLine("Create new Game by entering a number of players");
            Deck spil = new Deck();
            Table igraonica = new Table(spil, "Super Luda Igraonica", 4, 4, 1, 2);
            igraonica.addPlayer("Lazar", 500);
            igraonica.addPlayer("Mladen", 666);
            igraonica.addPlayer("Boban", 420);
            igraonica.addPlayer("Miksa", 69000);
            igraonica.tableState();

            //for (int i = 0; i < 4; i++) {
            //    System.Console.WriteLine("Enter nickname for " + (i + 1) + ". player");
            //    string nick = Console.ReadLine();
            //    System.Console.WriteLine("Insert ballance for " + (i + 1) + ". player");
            //    int ballance = 0;
            //    while (!Int32.TryParse(System.Console.ReadLine(), out ballance)) {
            //        System.Console.WriteLine("Invalid number");
            //    }
            //    igraonica.addPlayer(nick, ballance);
            //}
            if (igraonica.isReady())
                igraonica.startGame();


            //Deck spil = new Deck();
            //Hand hand = new Hand(spil);
            //hand.printHand();
            //hand.CheckRanks();

            //System.Console.WriteLine("Do you wish to exchange a card? Y/N");
            //var checker = System.Console.ReadLine();
            //while (checker == "y" || checker == "Y") {
            //    System.Console.WriteLine("How many?");
            //    int numOfCards = Int32.Parse(System.Console.ReadLine());
            //    hand.exchangeCards(numOfCards);
            //    hand.printHand();
            //    hand.CheckRanks();
            //    System.Console.WriteLine("Do you wish to exchange any more cards? Y/N");
            //    checker = System.Console.ReadLine();
            //}
            //System.Console.Read();

            System.Console.WriteLine("Press enter to close...");
            System.Console.ReadLine();
        }
    }
}
