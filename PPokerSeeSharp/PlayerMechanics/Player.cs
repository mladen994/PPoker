﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPoker.CardMechanics;

namespace PPoker.PlayerMechanics {
    public enum PlayerAction { CHECK, RAISE, CALL, FOLD, ALL_IN, BET };
    public class Player {
        public string nickname;
        public int ballance; //in čuna
        public Hand _hand;
        public int bag = 0;
        public int bagBackup = 0;
        public bool didFold = false;
        public bool didAllIn = false;

        public Player(string nickname, int ballance, Deck deck) {
            this.nickname = nickname;
            this.ballance = ballance;
            //this._hand = new Hand(deck);
        }

        public void dealCards(Deck deck) {
            _hand = new Hand(deck);
        }
        public int placeAnte(int ante) {
            bagBackup = 0;
            if (ballance < ante) {
                int tmpB = ballance;
                ballance = 0;
                return tmpB;
            } else {
                ballance -= ante;
                return ante;
            }
        }
        public void exchangeCards() {

        }
        public PlayerAction betAction(int currentRaise) {
            System.Console.WriteLine("choose an action: ");
            if (!didAllIn) {
                if (currentRaise == 0) {
                    System.Console.WriteLine("C for Check");
                } else if (currentRaise < ballance) {
                    System.Console.WriteLine("C for Call for " + currentRaise);
                }
                if (currentRaise < ballance) {
                    System.Console.WriteLine("R for Raise");
                }
                System.Console.WriteLine("A for All In");
                System.Console.WriteLine("F for Fold");
                string action = System.Console.ReadLine();
                action = action.ToUpper();
                if (action == "C")
                    return (currentRaise == 0) ? checkAction() : callAction(currentRaise);
                if (action == "R")
                    return raiseAction(currentRaise);
                if (action == "A")
                    return allInAction();
                if (action == "F")
                    return foldAction();
                System.Console.WriteLine("Invalid Key");
                return betAction(currentRaise);
            } else {
                System.Console.WriteLine("You have already went all in.");
                return PlayerAction.ALL_IN;
            }
        }
        private PlayerAction checkAction() {
            return PlayerAction.CHECK;
        }
        private PlayerAction callAction(int currentRaise) {
            ballance -= currentRaise;
            bagBackup += currentRaise;
            bag += currentRaise;
            return PlayerAction.CALL;
        }
        private PlayerAction raiseAction(int currentRaise) {
            int call = currentRaise;
            bagBackup += currentRaise;
            ballance -= call;
            bag += call;
            System.Console.WriteLine("How much do you want to raise? You have called for: " + call + " and you have " + ballance + " čuna left");
            int raise;
            while (!Int32.TryParse(System.Console.ReadLine(), out raise) && raise < ballance) {
                System.Console.WriteLine("Invalid number");
            }
            ballance -= raise;
            bag += raise;
            return PlayerAction.RAISE;
        }
        private PlayerAction allInAction() {
            bag += ballance;
            bagBackup += ballance;
            ballance = 0;
            didAllIn = true;
            return PlayerAction.ALL_IN;
        }
        private PlayerAction foldAction() {
            didFold = true;
            return PlayerAction.FOLD;
        }
    }
}