/************************************************************************************************************
 * Author(s): Ahmed Butt, Jugal Patel, Jermaine Henry, Nirmal Sureshbhai Patel, Fabian Mauricio Narvaez goyes
 * Date: April 9, 2021
 * File: Deck.cs
 * Description: This class holds a list of card codes (treated as a deck of cards because the Card class uses
 * a code to create "create" itself), and generic deck functionality is included. 
*************************************************************************************************************/

using System;
using System.Collections.Generic;

namespace Tarneeb
{
    public class Deck
    {
        //List of Card codes
        List<string> cards = new List<string>{"AS", "2S", "3S", "4S", "5S", "6S", "7S", "8S", "9S", "0S", "JS", "QS", "KS",
            "AH", "2H", "3H", "4H", "5H", "6H", "7H", "8H", "9H", "0H", "JH", "QH", "KH",
            "AD", "2D", "3D", "4D", "5D", "6D","7D", "8D", "9D", "0D", "JD", "QD", "KD",
            "AC", "2C", "3C", "4C", "5C", "6C", "7C", "8C", "9C", "0C", "JC", "QC", "KC"};

        /// <summary>
        /// Removes the first card code from the list and returns it.
        /// </summary>
        /// <returns></returns>
        public string Draw()
        {
            string card = cards[0];
            cards.RemoveAt(0);
            return card;
        }

        /// <summary>
        /// Shuffles the deck.
        /// </summary>
        public void Shuffle()
        {
            Random rng = new Random();
            int cardCount = cards.Count;

            //Keep looping as long as card count is greater than 1
            while (cardCount > 1)
            {
                cardCount--;

                //This generates a random number with a given max possible number (number of cards)
                int rand = rng.Next(cardCount);

                //This is a generic swapping method, swapping 1 string (card) with another string (card)
                string save = cards[rand];
                cards[rand] = cards[cardCount];
                cards[cardCount] = save;
            }
        }

        /// <summary>
        /// Resets the deck to the default state.
        /// </summary>
        public void Reset()
        {
            cards = new List<string>{"AS", "2S", "3S", "4S", "5S", "6S", "7S", "8S", "9S", "0S", "JS", "QS", "KS",
            "AH", "2H", "3H", "4H", "5H", "6H", "7H", "8H", "9H", "0H", "JH", "QH", "KH",
            "AD", "2D", "3D", "4D", "5D", "6D","7D", "8D", "9D", "0D", "JD", "QD", "KD",
            "AC", "2C", "3C", "4C", "5C", "6C", "7C", "8C", "9C", "0C", "JC", "QC", "KC"};
        }
    }
}
