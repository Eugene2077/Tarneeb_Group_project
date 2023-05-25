/**************************************************************************************************************
 * Author(s):   Ahmed Butt, Jugal Patel, Jermaine Henry, Nirmal Sureshbhai Patel, Fabian Mauricio Narvaez goyes
 * Date:        April 9, 2021
 * File:        Card.cs
 * Description: This class is used to create a card object that the user and AI can work with to indentify 
 *              what card thay have, which cards are playable, and which aren't.
**************************************************************************************************************/

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Tarneeb
{
    public class Card
    {
        #region Class Attributes
        //Stores the Card Image
        private Image theCard;

        //Stores the Cards rank
        private int theRank;

        //Stores the Cards suit
        private string theSuit;

        //Stores the Cards code
        private string code;

        //Stores if Card is playable or not
        private bool isPlayable;

        //Stores if Card has been played or not
        private bool isPlayed;

        //Stores who the card is owned by
        private int ownedBy;
        #endregion

        #region Getters & Setters
        /// <summary>
        /// Returns theCard
        /// </summary>
        /// <returns>Image</returns>
        public Image GetCard()
        {
            return this.theCard;
        }

        /// <summary>
        /// Sets theCard
        /// </summary>
        /// <param name="theCard"></param>
        public void SetCard(Image theCard)
        {
            this.theCard = theCard;
        }

        /// <summary>
        /// Returns isPlayable
        /// </summary>
        /// <returns>bool</returns>
        public bool GetIsPlayable()
        {
            return this.isPlayable;
        }

        /// <summary>
        /// Sets isPlayable
        /// </summary>
        /// <param name="isPlayable"></param>
        public void SetIsPlayable(bool isPlayable)
        {
            this.isPlayable = isPlayable;
        }

        /// <summary>
        /// Returns isPlayed
        /// </summary>
        /// <returns>bool</returns>
        public bool GetIsPlayed()
        {
            return this.isPlayed;
        }

        /// <summary>
        /// Sets isPlayed
        /// </summary>
        /// <param name="isPlayed"></param>
        public void SetIsPlayed(bool isPlayed)
        {
            this.isPlayed = isPlayed;
        }

        /// <summary>
        /// Returns theRank
        /// </summary>
        /// <returns>int</returns>
        public int GetRank()
        {
            return this.theRank;
        }

        /// <summary>
        /// Sets theRank
        /// </summary>
        /// <param name="theRank"></param>
        public void SetRank(int theRank)
        {
            this.theRank = theRank;
        }

        /// <summary>
        /// Returns theSuit
        /// </summary>
        /// <returns>string</returns>
        public string GetSuit()
        {
            return this.theSuit;
        }

        /// <summary>
        /// Sets theSuit
        /// </summary>
        /// <param name="theSuit"></param>
        public void SetSuit(string theSuit)
        {
            this.theSuit = theSuit;
        }

        /// <summary>
        /// Returns code
        /// </summary>
        /// <returns>string</returns>
        public string GetCode()
        {
            return this.code;
        }

        /// <summary>
        /// Sets code
        /// </summary>
        /// <param name="code"></param>
        public void SetCode(string code)
        {
            this.code = code;
        }

        /// <summary>
        /// Returns ownedBy
        /// </summary>
        /// <returns>int</returns>
        public int GetOwnedBy()
        {
            return this.ownedBy;
        }

        /// <summary>
        /// Sets ownedBy
        /// </summary>
        /// <param name="ownedBy"></param>
        public void SetOwnedBy(int ownedBy)
        {
            this.ownedBy = ownedBy;
        }
        #endregion

        /// <summary>
        /// Parameterized Constructor
        /// Finds the card image, rank, and suit based on card code provided. Initially sets isPlayable as false, sets width and height
        /// of the image, gives the image a hover effect, and a click effect.
        /// </summary>
        /// <param name="code"></param>
        public Card(string code)
        {
            isPlayable = false;

            this.code = code;

            theCard = new Image();

            //The card codes are equivalent to the image names with is why the following works:
            BitmapImage image = new BitmapImage(new Uri("/Tarneeb;component/Images/CardsPNG/" + code + ".png", UriKind.Relative));
            theCard.Source = image;

            FindRank();
            FindSuit();

            theCard.Width = 80;
            theCard.Height = 130;
            theCard.HorizontalAlignment = HorizontalAlignment.Center;
            theCard.VerticalAlignment = VerticalAlignment.Center;

            //Following events are only meant for player one (user)
            //These two lines together create a hover effect
            theCard.MouseEnter += Card_MouseEnter; 
            theCard.MouseLeave += Card_MouseLeave;

            //Giving the card a click event (there is another click event in offline screen that fires at the same time as this but
            //executes different code)
            theCard.MouseUp += Card_Click;
        }

        #region Methods
        /// <summary>
        /// This method sets the Card's rank based on the card code.
        /// </summary>
        public void FindRank()
        {
            if (code.Contains("2"))
            {
                theRank = 1;
            }
            else if (code.Contains("3"))
            {
                theRank = 2;
            }
            else if (code.Contains("4"))
            {
                theRank = 3;
            }
            else if (code.Contains("5"))
            {
                theRank = 4;
            }
            else if (code.Contains("6"))
            {
                theRank = 5;
            }
            else if (code.Contains("7"))
            {
                theRank = 6;
            }
            else if (code.Contains("8"))
            {
                theRank = 7;
            }
            else if (code.Contains("9"))
            {
                theRank = 8;
            }
            else if (code.Contains("0"))
            {
                theRank = 9;
            }
            else if (code.Contains("J"))
            {
                theRank = 10;
            }
            else if (code.Contains("Q"))
            {
                theRank = 11;
            }
            else if (code.Contains("K"))
            {
                theRank = 12;
            }
            else if (code.Contains("A"))
            {
                theRank = 13;
            }
        }

        /// <summary>
        /// Finds the Card's stui based on the card code
        /// </summary>
        public void FindSuit()
        {
            if (code.Contains("S"))
            {
                theSuit = "Spades";
            }
            else if (code.Contains("H"))
            {
                theSuit = "Hearts";
            }
            else if (code.Contains("D"))
            {
                theSuit = "Diamonds";
            }
            else if (code.Contains("C"))
            {
                theSuit = "Clubs";
            }
        }

        /// <summary>
        /// Changes the position of an image based on the specified location.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="y1"></param>
        /// <param name="y2"></param>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        public void PlayHere(Image image, int y1, int y2, int x1, int x2)
        {
            image.Width = 80;
            image.Height = 130;
            image.Visibility = Visibility.Visible;
            image.Margin = new System.Windows.Thickness(y1, y2, x1, x2);
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Occurs when cursor enters the image space. Image becomes enlargened only if the card is playable.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Card_MouseEnter(object sender, MouseEventArgs e)
        {
            if (isPlayable)
            {
                Image image = (Image)sender;
                image.Width = 100;
                image.Height = 150;
            }
        }

        /// <summary>
        /// Occurs when cursor leaves the image space. Image becomes smaller only if the card is playable.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Card_MouseLeave(object sender, MouseEventArgs e)
        {
            if (isPlayable)
            {
                Image image = (Image)sender;
                image.Width = 80;
                image.Height = 130;
            }
        }

        /// <summary>
        /// Occurs when the card image is clicked. Image moves from the users list of cards (on screen) area 
        /// to the users play card area only if the card is playable.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Card_Click(object sender, MouseEventArgs e)
        {
            if (isPlayable)
            {
                Image image = (Image)sender;
                image.Visibility = Visibility.Collapsed;
                this.isPlayed = true;
                PlayHere(image, 0, 250, 0, 0);
            }
        }
        #endregion
    }
}
