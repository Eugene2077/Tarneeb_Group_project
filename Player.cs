/**************************************************************************************************************
 * Author(s):   Ahmed Butt, Jugal Patel, Jermaine Henry, Nirmal Sureshbhai Patel, Fabian Mauricio Narvaez goyes
 * Date:        April 9, 2021
 * File:        Player.cs
 * Description: A class for each player. Player identification and in-game data is stored here.
**************************************************************************************************************/

namespace Tarneeb
{
    public class Player
    {
        #region Class Attributes
        //Stores the players name
        private string playerName;

        //Stores the players number (1-4)
        private int playerNumber;

        //Stores the players selected bid
        private int theBid;

        //Stores the amount of tricks the player won
        private int tricksWon;

        //Stores the turn that they bid (first to bid, second to bid, etc.)
        private int bidTurn;

        //Stores if the player is first to play a card or not
        private bool isFirst;

        //Stores if the user is the bidder or not
        private bool isBidder;
        #endregion

        /// <summary>
        /// Parameterized constructor. Only sets player name, number, and isFirst value initially.
        /// </summary>
        /// <param name="playerName"></param>
        /// <param name="playerNumber"></param>
        public Player(string playerName, int playerNumber)
        {
            this.playerName = playerName;
            this.playerNumber = playerNumber;
            this.isFirst = false;
        }

        #region Getters and Setters
        /// <summary>
        /// Returns playerName.
        /// </summary>
        /// <returns>string</returns>
        public string GetPlayerName()
        {
            return this.playerName;
        }

        /// <summary>
        /// Sets playerName.
        /// </summary>
        /// <param name="playerName"></param>
        public void SetPlayerName(string playerName)
        {
            this.playerName = playerName;
        }

        /// <summary>
        /// Returns playerNumber.
        /// </summary>
        /// <returns>int</returns>
        public int GetPlayerNumber()
        {
            return this.playerNumber;
        }

        /// <summary>
        /// Sets playerNumber.
        /// </summary>
        /// <param name="playerNumber"></param>
        public void SetPlayerNumber(int playerNumber)
        {
            this.playerNumber = playerNumber;
        }

        /// <summary>
        /// Returns theBid.
        /// </summary>
        /// <returns>int</returns>
        public int GetTheBid()
        {
            return this.theBid;
        }

        /// <summary>
        /// Sets theBid.
        /// </summary>
        /// <param name="theBid"></param>
        public void SetTheBid(int theBid)
        {
            this.theBid = theBid;
        }

        /// <summary>
        /// Returns tricksWon.
        /// </summary>
        /// <returns>int</returns>
        public int GetTricksWon()
        {
            return this.tricksWon;
        }

        /// <summary>
        /// Sets tricksWon.
        /// </summary>
        /// <param name="tricksWon"></param>
        public void SetTricksWon(int tricksWon)
        {
            this.tricksWon = tricksWon;
        }

        /// <summary>
        /// Returns bidTurn.
        /// </summary>
        /// <returns>int</returns>
        public int GetBidTurn()
        {
            return this.bidTurn;
        }

        /// <summary>
        /// Sets bidTurn.
        /// </summary>
        /// <param name="bidTurn"></param>
        public void SetBidTurn(int bidTurn)
        {
            this.bidTurn = bidTurn;
        }

        /// <summary>
        /// Returns isFirst.
        /// </summary>
        /// <returns>bool</returns>
        public bool GetIsFirst()
        {
            return this.isFirst;
        }

        /// <summary>
        /// Sets isFirst.
        /// </summary>
        /// <param name="isFirst"></param>
        public void SetIsFirst(bool isFirst)
        {
            this.isFirst = isFirst;
        }

        /// <summary>
        /// Returns isBidder.
        /// </summary>
        /// <returns>bool</returns>
        public bool GetIsBidder()
        {
            return this.isBidder;
        }

        /// <summary>
        /// Sets isBidder.
        /// </summary>
        /// <param name="isBidder"></param>
        public void SetIsBidder(bool isBidder)
        {
            this.isBidder = isBidder;
        }
        #endregion
    }
}
