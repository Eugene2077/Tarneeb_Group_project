/**************************************************************************************************************
 * Author(s):   Ahmed Butt, Jugal Patel, Jermaine Henry, Nirmal Sureshbhai Patel, Fabian Mauricio Narvaez goyes
 * Date:        April 9, 2021
 * File:        Hand.cs
 * Description: This class is used as a Hand (13 Cards) that is given to each player during play. Most of the
 *              AIs Card selecting logic occurs here. 
**************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;

namespace Tarneeb
{
    public class Hand
    {
        //Maximum number of Cards
        private const int totalCards = 13;

        //List of Card codes
        private List<string> codes = new List<string>();      

        //List of Cards
        private List<Card> theHand = new List<Card>();

        /// <summary>
        /// Returns theHand.
        /// </summary>
        /// <returns>List<Card></returns>
        public List<Card> GetHand()
        {
            return theHand;
        }

        /// <summary>
        /// Parameterized Constructor
        /// Takes a Deck object and uses the Draw method to take card codes to fill it's own card code list.
        /// </summary>
        /// <param name="deck"></param>
        public Hand(Deck deck)
        {
            //For loop to fill card codes list
            for (int i = 1; i <= totalCards; i++)
            {
                string code = deck.Draw();
                codes.Add(code);
            }
            
            //Calls CreateHand that fills theHand
            CreateHand();
        }

        #region Methods
        /// <summary>
        /// Designed to remove the first card in the card list (theHand) that has been played (isPlayed value is true).
        /// </summary>
        public void UpdateTheHand()
        {
            for (int i = 0; i < theHand.Count; i++)
            {
                if (theHand[i].GetIsPlayed())
                {
                    theHand.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Orders theHand by suit.
        /// </summary>
        public void OrderBySuit()
        {
            theHand = theHand.OrderBy(a => a.GetSuit()).ToList();
        }

        /// <summary>
        /// Used to find the full name of the suit of the card that has been played (this is done by checking
        /// each card's isPlayed value and code).
        /// </summary>
        /// <returns>string</returns>
        public string FindPlayedSuit()
        {
            string playedSuit = "";

            for (int i = 0; i < theHand.Count; i++)
            {
                //If the current card's isPlayed value is true (card was played)
                if (theHand[i].GetIsPlayed())
                {
                    //If current card's code contains 'S', the suit must be Spades, etc.
                    if (theHand[i].GetCode().Contains("S"))
                    {
                        playedSuit = "Spades";
                    }
                    else if (theHand[i].GetCode().Contains("H"))
                    {
                        playedSuit = "Hearts";
                    }
                    else if (theHand[i].GetCode().Contains("D"))
                    {
                        playedSuit = "Diamonds";
                    }
                    else if (theHand[i].GetCode().Contains("C"))
                    {
                        playedSuit = "Clubs";
                    }
                }
            }

            return playedSuit;
        }

        /// <summary>
        /// Used to find the index of the first playable card in the hand (for AI).
        /// </summary>
        /// <param name="suitLed"></param>
        /// <param name="trump"></param>
        /// <returns>Card</returns>
        public Card PlayFirstPlayable(string suitLed, string trump)
        {
            Card firstPlayable = null;

            //Finding the first playable card in the hand
            foreach (Card card in theHand)
            {
                if (card.GetIsPlayable() == true)
                {
                    card.SetIsPlayed(true);
                    card.SetIsPlayable(false);
                    firstPlayable = card;
                    break;
                }
            }

            //Increase rank of the Card if it matches suit
            if (firstPlayable.GetSuit() == suitLed)
            {
                firstPlayable.SetRank(firstPlayable.GetRank() + 50);
            }

            //Increase rank of the Card if it matches trump
            if (firstPlayable.GetSuit() == trump)
            {
                firstPlayable.SetRank(firstPlayable.GetRank() + 100);
            }

            //Reminder: winner of a trick is decided by comparing ranks

            return firstPlayable;
        }

        /// <summary>
        /// Randomly selects a Card from theHand, sets it's isPlayed to true and isPlayable value to false,
        /// and returns the Card. Used by AI.
        /// </summary>
        /// <returns>Card</returns>
        public Card PlayRandomCard()
        {
            Random card = new Random();
            int rand = card.Next(theHand.Count);

            theHand[rand].SetIsPlayed(true);
            theHand[rand].SetIsPlayable(false);

            return theHand[rand];
        }

        /// <summary>
        /// Takes the suit led, trump, a list of current played cards, and selected difficulty as parameters
        /// and selects a logical card to play based on those values. Used by AI.
        /// </summary>
        /// <param name="suitLed"></param>
        /// <param name="trump"></param>
        /// <param name="playedCards"></param>
        /// <param name="difficulty"></param>
        /// <returns>Card</returns>
        public Card PlayLogicalCard(string suitLed, string trump, List<Card> playedCards, int difficulty)
        {
            Card logicalCard = null;
            Card teammateCard = null;
            List<Card> ranked = new List<Card> { } ;
            int teammate = 0;

            foreach (Card card in theHand)
            {
                if (card.GetIsPlayable() == true)
                {
                    if (suitLed == card.GetSuit())
                    {
                        card.SetRank(card.GetRank() + 50);
                    }

                    if (trump == card.GetSuit())
                    {
                        card.SetRank(card.GetRank() + 100);
                    }

                    ranked.Add(card);
                }
            }

            //Initially order the AIs Hand by rank from highest to lowest
            ranked = ranked.OrderByDescending(a => a.GetRank()).ToList();

            if (playedCards.Count != 0)
            {
                //Ordering the played Cards by rank from highest to lowest
                playedCards = playedCards.OrderByDescending(a => a.GetRank()).ToList();

                //If difficulty is 3 (hard), AI players will understand what Card thier teammate played
                if (difficulty == 3)
                {
                    //Finding the AI players number
                    int owner = theHand[0].GetOwnedBy();

                    //If AI player is 2, teammate must be 4 (AI)
                    if (owner == 2)
                    {
                        teammate = 4;
                    }
                    //If AI player is 3, teammate must be 1 (user)
                    else if (owner == 3)
                    {
                        teammate = 1;
                    }
                    //If AI player is 4, teammate must be 2 (AI)
                    else if (owner == 4)
                    {
                        teammate = 2;
                    }

                    //Finding the Card played by the AIs teammate
                    foreach (Card card in playedCards)
                    {
                        if (card.GetOwnedBy() == teammate)
                        {
                            teammateCard = card;
                        }
                    }
                }

                //If AI player does not own a Card that can defeat the highest played Card
                if (playedCards[0].GetRank() > ranked[0].GetRank())
                {
                    //Order by rank from lowest to highest (preventing the AI from wasting highest card)
                    ranked = ranked.OrderBy(a => a.GetRank()).ToList();
                }
                //If AI player does own a Card that can defeat the highest played Card AND the highest
                //played Card belongs the the AIs teammate
                else if (playedCards[0].GetRank() < ranked[0].GetRank() && playedCards[0].GetOwnedBy() == teammate)
                {
                    //Order by rank from lowest to highest (preventing the AI from wasting highest card/trying
                    //to "defeat" its own teammate)
                    ranked = ranked.OrderBy(a => a.GetRank()).ToList();
                }
            }

            //Play first card in the AI players Hand (will either be the highest or lowest ranked card)
            logicalCard = ranked[0];

            logicalCard.SetIsPlayed(true);
            logicalCard.SetIsPlayable(false);

            return logicalCard;
        }

        /// <summary>
        /// Finds the the played Card in theHand using its isPlayed value.
        /// </summary>
        /// <returns>Card</returns>
        public Card FindPlayedCard()
        {
            Card played = null;

            foreach (Card card in theHand)
            {
                //If current Cards isPlayed value is true
                if (card.GetIsPlayed() == true)
                {
                    played = card;
                    break;
                }
            }

            return played;
        }

        /// <summary>
        /// Resets the ranks of all Cards in theHand.
        /// </summary>
        public void ResetRanks()
        {
            foreach (Card card in theHand)
            {
                card.FindRank();
            }
        }

        /// <summary>
        /// Returns the name of the most owned suit based on the amount of Cards of that suit
        /// that exist in theHand.
        /// </summary>
        /// <returns>string</returns>
        public string GetMostOwnedSuit()
        {
            List<Card> countSpades = new List<Card> { };
            List<Card> countHearts = new List<Card> { };
            List<Card> countDiamonds = new List<Card> { };
            List<Card> countClubs = new List<Card> { };

            foreach (Card card in theHand)
            {
                //If Card code contains 'S', add to Spades list, etc.
                if (card.GetCode().Contains("S"))
                {
                    countSpades.Add(card);
                }
                else if (card.GetCode().Contains("H"))
                {
                    countHearts.Add(card);
                }
                else if (card.GetCode().Contains("D"))
                {
                    countDiamonds.Add(card);
                }
                else if (card.GetCode().Contains("C"))
                {
                    countClubs.Add(card);
                }
            }

            //List of Card Lists
            List<List<Card>> totals = new List<List<Card>> { countSpades, countHearts, countDiamonds, countClubs };

            //Ordering the list of Card lists by amount in each list (highest to lowest)
            totals = totals.OrderByDescending(a => a.Count).ToList();

            //If highest is spades, return spades, etc.
            if (totals[0] == countSpades)
            {
                return "Spades";
            }
            else if (totals[0] == countHearts)
            {
                return "Hearts";
            }
            else if (totals[0] == countDiamonds)
            {
                return "Diamonds";
            }
            else
            {
                return "Clubs";
            }
        }

        /// <summary>
        /// Returns a List of Card Lists of each suit.
        /// </summary>
        /// <returns>List<List<Card>></returns>
        public List<List<Card>> GetCardsOfEachSuit()
        {
            List<Card> countSpades = new List<Card> { };
            List<Card> countHearts = new List<Card> { };
            List<Card> countDiamonds = new List<Card> { };
            List<Card> countClubs = new List<Card> { };

            foreach (Card card in theHand)
            {
                //If Card code contains "S", add to Spades list, etc.
                if (card.GetCode().Contains("S"))
                {
                    countSpades.Add(card);
                }
                else if (card.GetCode().Contains("H"))
                {
                    countHearts.Add(card);
                }
                else if (card.GetCode().Contains("D"))
                {
                    countDiamonds.Add(card);
                }
                else if (card.GetCode().Contains("C"))
                {
                    countClubs.Add(card);
                }
            }

            //List of Card Lists
            List<List<Card>> totals = new List<List<Card>> { countSpades, countHearts, countDiamonds, countClubs };

            //Ordering the list of Card lists by amount in each list (highest to lowest)
            totals = totals.OrderByDescending(a => a.Count).ToList();

            return totals;
        }

        /// <summary>
        /// Populates theHand (List of Cards) with the help of the codes List
        /// </summary>
        public void CreateHand()
        {
            for (int i = 0; i < totalCards; i++)
            {
                theHand.Add(new Card(codes[i]));
            }
        }

        /// <summary>
        /// Return a code at a specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns>string</returns>
        public string SeeCode(int index)
        {
            string code = codes[index];
            return code;
        }

        /// <summary>
        /// Sets the isPlayable value of each card in theHand as either true or false
        /// </summary>
        /// <param name="playable"></param>
        public void SetAllPlayable(bool playable)
        {
            foreach (Card card in theHand)
            {
                card.SetIsPlayable(playable);
            }
        }

        /// <summary>
        /// Sets the isPlayable value of each card that matches the specified suit in theHand as true,
        /// if all cards in the hand are found to be "unplayable", all cards are set to playable instead.
        /// </summary>
        /// <param name="trump"></param>
        public void SetSomePlayable(string trump)
        {
            int unplayable = 0;

            foreach (Card card in theHand)
            {
                //If Card code contains 'S' AND current trump is Spades, set current Card to be playable, etc.
                if (card.GetCode().Contains("S") && trump == "Spades")
                {
                    card.SetIsPlayable(true);
                }
                else if (card.GetCode().Contains("H") && trump == "Hearts")
                {
                    card.SetIsPlayable(true);
                }
                else if (card.GetCode().Contains("D") && trump == "Diamonds")
                {
                    card.SetIsPlayable(true);
                }
                else if (card.GetCode().Contains("C") && trump == "Clubs")
                {
                    card.SetIsPlayable(true);
                }
                else
                {
                    //Increment unplayable
                    unplayable++;
                }
            }

            //If all Cards are found to be unplayable
            if (unplayable == theHand.Count)
            {
                //Make all Cards playable
                SetAllPlayable(true);
            }
        }

        /// <summary>
        /// Removes all Cards in theHand.
        /// </summary>
        public void Delete()
        {
            theHand.Clear();
        }
        #endregion
    }
}
