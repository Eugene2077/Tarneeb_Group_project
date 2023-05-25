/**************************************************************************************************************
 * Author(s):   Ahmed Butt, Jugal Patel, Jermaine Henry, Nirmal Sureshbhai Patel, Fabian Mauricio Narvaez goyes
 * Date:        April 9, 2021
 * File:        OfflineScreen.xaml.cs
 * Description: This is where Tarneeb is played and where all of the gameplay logic occurs (some AI logic is
 *              included here as well).
**************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Tarneeb
{
    /// <summary>
    /// Interaction logic for OfflineScreen.xaml
    /// </summary>
    public partial class OfflineScreen : UserControl
    {
        #region Attributes
        //Variables declared here are accessible by all methods

        //Accessing main window
        MainWindow mainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);

        //Bitmap of the back of the cards
        BitmapImage redBack = new BitmapImage(new Uri("/Tarneeb;component/Images/CardsPNG/red_back.png", UriKind.Relative));

        //Images of cards that will be handed out to each player (simply for animation purposes)
        Image handoutLeft = new Image();
        Image handoutTop = new Image();
        Image handoutRight = new Image();
        Image handoutBottom = new Image();

        //Labels for AI players (displays number of cards they currently have)
        Label lblLeftAmount = new Label();
        Label lblTopAmount = new Label();
        Label lblRightAmount = new Label();

        //Declaring four Players
        Player playerOne;
        Player playerTwo;
        Player playerThree;
        Player playerFour;

        //A List of Players
        List<Player> players;

        //Creating a new Deck
        Deck deck = new Deck();

        //Hands for each player 
        Hand playerOneHand;
        Hand playerTwoHand;
        Hand playerThreeHand;
        Hand playerFourHand;

        //List of Hands
        List<Hand> hands;

        //Timers: (used to delay actions for better user experience)
        //Timer for each turn (Players picking a card)
        DispatcherTimer turnTimer;

        //Timer for each bid turn (Players selecting a bid)
        DispatcherTimer bidTimer;

        //Stores the current Player that needs to select a card
        Player currentPlayer;

        //Stores the current bidder
        Player bidder;

        //Stores number of players that haven't played a card
        int playersLeft;

        //Stores number of players that haven't yet bid
        int biddersLeft;

        //Team scores beginning at 0
        int team1Score = 0;
        int team2Score = 0;

        //Imporant to prevent the user from not being able to select a card when it's not the users turn
        bool isRunning;

        //Enable or disable leave button (avoids bugs with timers)
        bool leaveDisabled;

        //Stores current trump
        string currentTrump;

        //Stores the current playable suit (when the first player in a turn plays a card, it's suit is stored here)
        string currentPlayableSuit;

        //List of current Cards played (Cards currently on the field)
        List<Card> cardsPlayed = new List<Card> { };

        //Constants
        const string maxCardsLabel = "13";

        const int winningScore = 31;

        const int kaboot = 13;

        const int cardDefaultWidth = 80;

        const int cardDefaultHeight = 130;

        const int labelDefaultFontSize = 20;
        #endregion

        #region Setting Up Round
        /// <summary>
        /// Initializes offline screen. On start, gives card images default values and instantiates players.
        /// Also begins the game.
        /// </summary>
        public OfflineScreen()
        {
            InitializeComponent();

            LoggingAndStats.Log("System", "New game started");

            //Delcaring initial values, placing images/labels to thier starting location with initial properties
            leaveDisabled = true;

            mainWindow.SetBid(0);
            mainWindow.SetTrump("");

            //Giving default values to each AI players card images and displaying them (they
            //start at the middle of the screen)
            handoutLeft.Source = redBack;
            handoutLeft.Width = cardDefaultWidth;
            handoutLeft.Height = cardDefaultHeight;
            gridTable.Children.Add(handoutLeft);

            handoutTop.Source = redBack;
            handoutTop.Width = cardDefaultWidth;
            handoutTop.Height = cardDefaultHeight;
            gridTable.Children.Add(handoutTop);

            handoutRight.Source = redBack;
            handoutRight.Width = cardDefaultWidth;
            handoutRight.Height = cardDefaultHeight;
            gridTable.Children.Add(handoutRight);

            handoutBottom.Source = redBack;
            handoutBottom.Width = cardDefaultWidth;
            handoutBottom.Height = cardDefaultHeight;
            gridTable.Children.Add(handoutBottom);

            //Giving default values to each Label of each AI player 
            lblLeftAmount.Content = maxCardsLabel;
            lblLeftAmount.FontSize = labelDefaultFontSize;
            lblLeftAmount.HorizontalAlignment = HorizontalAlignment.Center;
            lblLeftAmount.VerticalAlignment = VerticalAlignment.Center;
            lblLeftAmount.Margin = new System.Windows.Thickness(0, 0, 900, 0);
            lblLeftAmount.Visibility = Visibility.Hidden;
            gridTable.Children.Add(lblLeftAmount);

            lblTopAmount.Content = maxCardsLabel;
            lblTopAmount.FontSize = labelDefaultFontSize;
            lblTopAmount.HorizontalAlignment = HorizontalAlignment.Center;
            lblTopAmount.VerticalAlignment = VerticalAlignment.Center;
            lblTopAmount.Margin = new System.Windows.Thickness(0, 0, 0, 500);
            lblTopAmount.Visibility = Visibility.Hidden;
            gridTable.Children.Add(lblTopAmount);

            lblRightAmount.Content = maxCardsLabel;
            lblRightAmount.FontSize = labelDefaultFontSize;
            lblRightAmount.HorizontalAlignment = HorizontalAlignment.Center;
            lblRightAmount.VerticalAlignment = VerticalAlignment.Center;
            lblRightAmount.Margin = new System.Windows.Thickness(900, 0, 0, 0);
            lblRightAmount.Visibility = Visibility.Hidden;
            gridTable.Children.Add(lblRightAmount);

            //Creating Players and then adding them to a list of Players
            playerOne = new Player(mainWindow.GetUserName(), 1);
            playerTwo = new Player("Player Two", 2);
            playerThree = new Player("Player Three", 3);
            playerFour = new Player("Player Four", 4);

            //Updating player ones (user) name on the scoreboard
            txtScorePlayer1.Text = playerOne.GetPlayerName();

            //Adding all Players to a List of Players
            players = new List<Player> { playerOne, playerTwo, playerThree, playerFour };

            //Starting the round
            ResetRound();
        }

        /// <summary>
        /// Resets values for the new round, plays animations, displays users cards, and begins the
        /// bidding process.
        /// </summary>
        public void ResetRound()
        {
            //Changing some variables back to default
            leaveDisabled = true;
            mainWindow.SetBid(0);
            mainWindow.SetTrump("");
            mainWindow.SetPlay(false);
            mainWindow.SetUserBid(0);
            mainWindow.SetUserHasBid(false);

            //Resetting some Player attributes
            foreach (Player player in players)
            {
                player.SetIsBidder(false);
                player.SetTricksWon(0);
                player.SetIsFirst(false);
                player.SetTheBid(0);
            }

            //Resetting names (to remove the "(Bidder)" text)
            lblPlayerOne.Content = mainWindow.GetUserName();
            lblPlayerTwo.Content = "Player Two";
            lblPlayerTwo.Margin = new System.Windows.Thickness(0, 0, 90, 160);
            lblPlayerThree.Content = "Player Three";
            lblPlayerFour.Content = "Player Four";
            lblPlayerFour.Margin = new System.Windows.Thickness(90, 0, 0, 160);

            //Resetting tricks won display for each team
            lblT1.Content = "0";
            lblT2.Content = "0";

            //Resetting the deck
            deck.Reset();

            LoggingAndStats.Log("System", "New deck has been created");

            //Shuffling the deck
            deck.Shuffle();

            LoggingAndStats.Log("System", "Deck has been shuffled");

            //Making the Labels invisible (will become visible again after animation completes)
            lblRightAmount.Visibility = Visibility.Hidden;
            lblTopAmount.Visibility = Visibility.Hidden;
            lblLeftAmount.Visibility = Visibility.Hidden;

            //Before animations start, making this image visible so it doesn't animate an invisible image
            handoutBottom.Visibility = Visibility.Visible;

            //Resetting amounts to 13
            lblRightAmount.Content = maxCardsLabel;
            lblTopAmount.Content = maxCardsLabel;
            lblLeftAmount.Content = maxCardsLabel;

            //Timer to play sounds, counter to only play 4 times (once for each animation)
            int count = 0;
            var delaySoundTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
            delaySoundTimer.Start();
            delaySoundTimer.Tick += (sender, args) =>
            {
                count++;

                Sound.PlayCardSwipe();

                if (count == 4)
                {
                    delaySoundTimer.Stop();
                }
            };

            //Starting handout animations
            ObjectAnimation leftAnim = new ObjectAnimation(handoutLeft, lblLeftAmount, 0, 0, -450, 0, 0.5, 0.5, 0, 1000);
            leftAnim.StartAnimation();

            ObjectAnimation topAnim = new ObjectAnimation(handoutTop, lblTopAmount, 0, 0, 0, -250, 0.5, 0, 1, 1500);
            topAnim.StartAnimation();

            ObjectAnimation rightAnim = new ObjectAnimation(handoutRight, lblRightAmount, 0, 0, 450, 0, 0.5, 1.5, 0, 2000);
            rightAnim.StartAnimation();

            ObjectAnimation bottomAnim = new ObjectAnimation(handoutBottom, 0, 0, 0, 260, 0.5, 0, 2, 2500);
            bottomAnim.StartAnimation();

            //This will make the users handout Card image invisible
            bottomAnim.DelayImage(false, 2500);

            //Giving each player new hands and making a List of Hands
            playerOneHand = new Hand(deck);
            playerTwoHand = new Hand(deck);
            playerThreeHand = new Hand(deck);
            playerFourHand = new Hand(deck);

            hands = new List<Hand> { playerOneHand, playerTwoHand, playerThreeHand, playerFourHand };

            LoggingAndStats.Log("System", "13 cards given to each player");

            //Giving each card in each hand an owned by value
            int p = 1;
            foreach (Hand hand in hands)
            {
                foreach (Card card in hand.GetHand())
                {
                    card.SetOwnedBy(p);
                }
                p++;
            }

            //Initial x and y locations for the users cards
            int y = 520;
            int x = 700;

            //Initial delay for displaying a card
            int delay = 2500;

            //Ordering the players hand by suit before hand is displayed on screen
            playerOneHand.OrderBySuit();

            //This loop displays the users hand onto the screen
            foreach (Card card in playerOneHand.GetHand())
            {
                //Setting the current card to a location on screen (currently invisible)
                card.GetCard().Margin = new System.Windows.Thickness(0, y, x, 0);
                gridTable.Children.Add(card.GetCard());
                card.GetCard().Visibility = Visibility.Collapsed;

                //Making each card visible after a delay
                ObjectAnimation objectVisible = new ObjectAnimation(card.GetCard());
                objectVisible.DelayImage(true, delay);

                //Giving each card a click event
                card.GetCard().MouseUp += Card_IsPicked;

                //Incrementing the x value, so the next card will be a reasonable distance beside
                //the previous card
                x -= 120;

                //Incrementing delay so the next card appears slightly later
                delay += 100;
            }

            isRunning = true;

            //Starting bidding after a delay
            var delayBidTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
            delayBidTimer.Start();
            delayBidTimer.Tick += (sender, args) =>
            {
                delayBidTimer.Stop();
                StartBidding();
            };

            //A timer that keeps looping unless mainWindow's boolean play value is true
            var checkerTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
            checkerTimer.Start();
            checkerTimer.Tick += (sender, args) =>
            {
                //Looks for a bid from the main window
                txtTheBid.Text = mainWindow.GetBid().ToString();

                //Looks for a trump from the main window
                currentTrump = mainWindow.GetTrump();

                //If a trump has been selected
                if (currentTrump != null)
                {
                    txtTrump.Text = currentTrump.ToString();
                }

                //If GetUserHasBid returns false, it means it is not yet the users turn to pick (bidTimer is still running),
                //if GetUserHasBid returns true, it not only means that has the bidTimer stopped, but also means the user has
                //selected a bid and the bidTimer should start again
                if (mainWindow.GetUserHasBid())
                {
                    //Set back to false so this if statement doesn't run again
                    mainWindow.SetUserHasBid(false);

                    //Find and set the users chosen bid
                    players[0].SetTheBid(mainWindow.GetUserBid());

                    LoggingAndStats.Log(mainWindow.GetUserName() + " (User)", "Bid " + mainWindow.GetUserBid());

                    //This will remove the "Bidding..." text
                    lblPlayerOne.Content = mainWindow.GetUserName();

                    //Next player to select a bid must be player 2 (going counterclockwise)
                    bidder = players[1];

                    //Starting the bidTimer once again
                    bidTimer.Start();
                }

                //Stopping the timer when play boolean value from main window is true (which is only set to true when a trump is selected)
                if (mainWindow.GetPlay())
                {
                    checkerTimer.Stop();
                    TakeTurns();
                }
            };
        }
        #endregion

        #region Bidding Process
        /// <summary>
        /// Starts the bidding process. AI bidding behaviour changes based on difficulty.
        /// </summary>
        public void StartBidding()
        {
            //Selecting a random player to start bidding
            Random player = new Random();
            int rand = player.Next(players.Count);

            bidder = players[rand];

            biddersLeft = 4;

            //This will keep looping, allowing each player to select a bid
            bidTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            bidTimer.Start();
            bidTimer.Tick += (sender, args) =>
            {
                //Resetting text for player 3
                lblPlayerThree.Content = "Player Three";

                //Making "Bidding..." text disappear
                lblBidding2.Visibility = Visibility.Collapsed;
                lblBidding4.Visibility = Visibility.Collapsed;

                //If there are players that still need to bid
                if (biddersLeft != 0)
                {
                    //If current bidder is player 1
                    if (bidder == players[0])
                    {
                        //Open bidding control to allow user to select a bid
                        Control controlBidding = new Bidding();
                        mainWindow.gridContent.Children.Add(controlBidding);

                        lblPlayerOne.Content = mainWindow.GetUserName() + " Bidding...";

                        players[0].SetBidTurn(biddersLeft);

                        //Stopping the timer to wait for user input (timer will start when user selects a bid)
                        bidTimer.Stop();
                    }
                    //If current bidder is player 2
                    else if (bidder == players[1])
                    {
                        lblBidding2.Visibility = Visibility.Visible;

                        //If difficulty easy or medium, select a random bid
                        if (mainWindow.GetDifficulty() == 1 || mainWindow.GetDifficulty() == 2)
                        {
                            players[1].SetTheBid(RandomBid());
                        }
                        //Else if difficulty hard, select a logical bid
                        else if (mainWindow.GetDifficulty() == 3)
                        {
                            players[1].SetTheBid(LogicalBid(playerTwoHand));
                        }

                        LoggingAndStats.Log(playerTwo.GetPlayerName(), "Bid " + playerTwo.GetTheBid());

                        players[1].SetBidTurn(biddersLeft);

                        //Next player to select a bid must be player 2 (going counterclockwise)
                        bidder = players[2];
                    }
                    //If current bidder is player 3
                    else if (bidder == players[2])
                    {
                        lblPlayerThree.Content = "Player Three Bidding...";

                        //If difficulty easy or medium, select a random bid
                        if (mainWindow.GetDifficulty() == 1 || mainWindow.GetDifficulty() == 2)
                        {
                            players[2].SetTheBid(RandomBid());
                        }
                        //Else if difficulty hard, select a logical bid
                        else if (mainWindow.GetDifficulty() == 3)
                        {
                            players[2].SetTheBid(LogicalBid(playerThreeHand));
                        }

                        LoggingAndStats.Log(playerThree.GetPlayerName(), "Bid " + playerThree.GetTheBid());

                        players[2].SetBidTurn(biddersLeft);

                        //Next player to select a bid must be player 2 (going counterclockwise)
                        bidder = players[3];
                    }
                    //If current bidder is player 4
                    else if (bidder == players[3])
                    {
                        lblBidding4.Visibility = Visibility.Visible;

                        //If difficulty easy or medium, select a random bid
                        if (mainWindow.GetDifficulty() == 1 || mainWindow.GetDifficulty() == 2)
                        {
                            players[3].SetTheBid(RandomBid());
                        }
                        //Else if difficulty hard, select a logical bid
                        else if (mainWindow.GetDifficulty() == 3)
                        {
                            players[3].SetTheBid(LogicalBid(playerFourHand));
                        }

                        LoggingAndStats.Log(playerFour.GetPlayerName(), "Bid " + playerFour.GetTheBid());

                        players[3].SetBidTurn(biddersLeft);

                        //Next player to select a bid must be player 2 (going counterclockwise)
                        bidder = players[0];
                    }

                    //Decrement players left that still need to bid
                    biddersLeft--;
                }
                //Else if all players have selected a bid
                else
                {       
                    //This will change the bidder variable to contain the player that bid the highest
                    FindHighestBidder();

                    //Setting bidders isFirst value as true (bidder will be the first to play a card)  
                    bidder.SetIsFirst(true);     

                    //Stop looping
                    bidTimer.Stop();
                }
            };
        }

        /// <summary>
        /// Generates a random bid and returns it.
        /// </summary>
        /// <returns>int</returns>
        public int RandomBid()
        {
            //Selecting a random bid is based on a random confidence level
            Random confidence = new Random();
            int rand = confidence.Next(1, 21); //1-20

            //AI tend to bid too high, with this they have a 70% chance of bidding 7-8, 20% chance of bidding 7-9, and another
            //10% chance of bidding 7-13 (at least that's how I think this works)
            Random bid = new Random();
            if (rand < 15) //confidence 1-14
            {
                rand = bid.Next(7, 9); //7-8
            }
            else if (rand < 19) //if confidence 15-18
            {
                rand = bid.Next(7, 10); //7-9
            }
            else //if confidence 19-20
            {
                rand = bid.Next(8, 14); //8-13
            }

            //Update bid in main window if this bid is greater
            if (rand > mainWindow.GetBid())
            {
                mainWindow.SetBid(rand);
            }

            return rand;
        }

        /// <summary>
        /// Returns a bid based on number of cards from most common occurring suit in a hand.
        /// </summary>
        /// <param name="hand"></param>
        /// <returns>int</returns>
        public int LogicalBid(Hand hand)
        {
            //Getting a List of Card Lists
            List<List<Card>> cardLists = hand.GetCardsOfEachSuit();
            int bid;

            //If cards of most common occurring suit is less than or equal to 4, select 7
            if (cardLists[0].Count <= 4)
            {
                bid = 7;
            }
            //If 5, select 8
            else if (cardLists[0].Count == 5)
            {
                bid = 8;
            }
            //If 6, select 9
            else if (cardLists[0].Count == 6)
            {
                bid = 9;
            }
            //If 7, select 10
            else if (cardLists[0].Count == 7)
            {
                bid = 10;
            }
            //If 8, select 11
            else if (cardLists[0].Count == 8)
            {
                bid = 11;
            }
            //If 9, select 12
            else if (cardLists[0].Count == 9)
            {
                bid = 12;
            }
            //Else, select 13
            else
            {
                bid = 13;
            }

            //Update bid in main window if this bid is greater
            if (bid > mainWindow.GetBid())
            {
                mainWindow.SetBid(bid);
            };

            return bid;
        }
        #endregion

        #region Trump Selection
        /// <summary>
        /// Finds the highest bidder and allows that player to select a random or logical trump, based
        /// on difficulty.
        /// </summary>
        public void FindHighestBidder()
        {
            //Ordering the list of players by thier bid turn last to first
            List<Player> bidders = players.OrderByDescending(a => a.GetTheBid()).ToList();

            //If any bid matches the highest bid, remove all bidders with matching bids that did not go last
            if (bidders[0].GetTheBid() == bidders[1].GetTheBid() || bidders[0].GetTheBid() == bidders[2].GetTheBid() ||
                bidders[0].GetTheBid() == bidders[3].GetTheBid())
            {
                if (playerOne.GetTheBid() == bidders[0].GetTheBid() && playerOne.GetBidTurn() != 1)
                {
                    bidders.Remove(playerOne);
                }

                if (playerTwo.GetTheBid() == bidders[0].GetTheBid() && playerTwo.GetBidTurn() != 1)
                {
                    bidders.Remove(playerTwo);
                }

                if (playerThree.GetTheBid() == bidders[0].GetTheBid() && playerThree.GetBidTurn() != 1)
                {
                    bidders.Remove(playerThree);
                }

                if (playerFour.GetTheBid() == bidders[0].GetTheBid() && playerFour.GetBidTurn() != 1)
                {
                    bidders.Remove(playerFour);
                }
            }

            //Set bidder to first bidder on bidders List
            bidder = bidders[0];
            bidder.SetIsBidder(true);

            LoggingAndStats.Log(bidder.GetPlayerName(), "Is the bidder");

            //If player one (user) won bidding, load Trump user control to allow the user to select a trump
            if (bidder.GetPlayerNumber() == 1)
            {
                lblPlayerOne.Content = mainWindow.GetUserName() + " (Bidder)";

                Control controlTrump = new Trump();
                mainWindow.gridContent.Children.Add(controlTrump);
            }
            //If player two won bidding, select random trump (if easy) or select logical trump (if medium or hard)
            else if (bidder.GetPlayerNumber() == 2)
            {
                lblPlayerTwo.Content = "Player Two (Bidder)";
                lblPlayerTwo.Margin = new System.Windows.Thickness(0, 0, 50, 160);

                if (mainWindow.GetDifficulty() == 1)
                {
                    RandomTrump();
                }
                else if (mainWindow.GetDifficulty() == 2 || mainWindow.GetDifficulty() == 3)
                {
                    LogicalTrump(hands[1]);
                }

            }
            //If player three won bidding, select random trump (if easy) or select logical trump (if medium or hard)
            else if (bidder.GetPlayerNumber() == 3)
            {
                lblPlayerThree.Content = "Player Three (Bidder)";

                if (mainWindow.GetDifficulty() == 1)
                {
                    RandomTrump();
                }
                else if (mainWindow.GetDifficulty() == 2 || mainWindow.GetDifficulty() == 3)
                {
                    LogicalTrump(hands[2]);
                }
            }
            //If player four won bidding, select random trump (if easy) or select logical trump (if medium or hard)
            else if (bidder.GetPlayerNumber() == 4)
            {
                lblPlayerFour.Content = "Player Four (Bidder)";
                lblPlayerFour.Margin = new System.Windows.Thickness(50, 0, 0, 160);

                if (mainWindow.GetDifficulty() == 1)
                {
                    RandomTrump();
                }
                else if (mainWindow.GetDifficulty() == 2 || mainWindow.GetDifficulty() == 3)
                {
                    LogicalTrump(hands[3]);
                }
            }

            LoggingAndStats.Log(bidder.GetPlayerName(), "Selected " + mainWindow.GetTrump() + " as the trump");

            //Bidder becomes current player (player that will play the first card)
            currentPlayer = bidder;
        }

        /// <summary>
        /// Sets a random trump in main window.
        /// </summary>
        public void RandomTrump()
        {
            //Generating a random number
            Random trump = new Random();
            int rand = trump.Next(1, 5); //1-4

            //Setting a trump based on the random number
            if (rand == 1)
            {
                mainWindow.SetTrump("Spades");
            }
            else if (rand == 2)
            {
                mainWindow.SetTrump("Hearts");
            }
            else if (rand == 3)
            {
                mainWindow.SetTrump("Diamonds");
            }
            else if (rand == 4)
            {
                mainWindow.SetTrump("Clubs");
            }

            //How the game knows to move on to the next phase
            mainWindow.SetPlay(true);
        }

        /// <summary>
        /// Sets the trump to be the most owned suit in a hand.
        /// </summary>
        /// <param name="highestBidder"></param>
        public void LogicalTrump(Hand highestBidder)
        {           
            mainWindow.SetTrump(highestBidder.GetMostOwnedSuit());

            //How the game knows to move on to the next phase
            mainWindow.SetPlay(true);
        }
        #endregion

        #region Tricks
        /// <summary>
        /// Loops through each player with a timer (for input delay), allowing each to play a card
        /// </summary>
        public void TakeTurns()
        {
            playersLeft = 4;

            turnTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
            turnTimer.Start();
            turnTimer.Tick += (sender, args) =>
            {
                if (playersLeft != 0)
                {
                    //Changing the turn indicator text to the name of the currentPlayer
                    txtCurrent.Text = currentPlayer.GetPlayerName();

                    //If current player is player 1
                    if (currentPlayer.GetPlayerNumber() == 1)
                    {
                        //Allow user to select a card
                        isRunning = false;
                        ReadyPlayerOne();

                        //Changing the current player to player 2 (keeping it counterclockwise)
                        currentPlayer = players[1];

                        leaveDisabled = false;

                        //Stop the timer to allow user to select a card first before moving on to the next players
                        turnTimer.Stop();                     
                    }
                    //If current player is player 1
                    else if (currentPlayer.GetPlayerNumber() == 2)
                    {
                        //Allowing player 2 to select a card
                        ReadyAI(playerTwoHand, currentPlayer);

                        //Changing the current player to player 3
                        currentPlayer = players[2];
                    }
                    //If current player is player 1
                    else if (currentPlayer.GetPlayerNumber() == 3)
                    {
                        //Allowing player 3 to select a card
                        ReadyAI(playerThreeHand, currentPlayer);

                        //Changing the current player to player 4
                        currentPlayer = players[3];
                    }
                    //If current player is player 1
                    else if (currentPlayer.GetPlayerNumber() == 4)
                    {
                        //Allowing player 4 to select a card
                        ReadyAI(playerFourHand, currentPlayer);

                        //Changing the current player to player 1
                        currentPlayer = players[0];
                    }

                    //Decrement playersLeft
                    playersLeft--;
                }
                else //If all players have already played a card, stop the loop and remove cards
                {
                    txtCurrent.Text = "";
                    turnTimer.Stop();
                    RemoveCards();
                    mainWindow.SetPlay(false);
                }
            };
        }

        /// <summary>
        /// Removes all cards on the field and either starts the next turn or the next round, based on the amount of 
        /// cards in player one's hand because if one player has no cards by the time this is called, all players should
        /// have no cards, thus start new round.
        /// </summary>
        public void RemoveCards()
        {
            //Find who won the round
            WhoWonWhosNext();

            //Clear List of cards played
            cardsPlayed.Clear();

            //Update labels that shows tricks won by each team
            lblT1.Content = playerOne.GetTricksWon() + playerThree.GetTricksWon();
            lblT2.Content = playerTwo.GetTricksWon() + playerFour.GetTricksWon();

            //Remove cards after a short delay
            var removeTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
            removeTimer.Start();
            removeTimer.Tick += (sender, args) =>
            {
                //Timer only needs to run once
                removeTimer.Stop();

                //For each card in each hand, set played cards invisible, remove card from the hand, and set all unplayable
                foreach (Hand hand in hands)
                {
                    foreach (Card card in hand.GetHand())
                    {
                        if (card.GetIsPlayed())
                        {
                            card.GetCard().Visibility = Visibility.Collapsed;
                        }
                    }
                    hand.UpdateTheHand(); //remove played card
                    hand.SetAllPlayable(false); //playable value is changed depending on the suit
                }

                //If the round is not over (players don't have cards)
                if (playerOneHand.GetHand().Count != 0)
                {
                    //Set isFirst value for each player to false
                    foreach (Player player in players)
                    {
                        player.SetIsFirst(false);
                    }

                    //WhoWonWhosNext method set currentPlayer to equal the player that won the trick
                    //Thus, make this players isFirst value to true
                    currentPlayer.SetIsFirst(true);

                    //Begin the next trick
                    TakeTurns();
                }
                //If round is over (players don't have cards)
                else if (playerOneHand.GetHand().Count == 0)
                {
                    //Update score after a delay
                    var waitASecond = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
                    waitASecond.Start();
                    waitASecond.Tick += (sender2, args2) =>
                    {
                        UpdateScore();
                        waitASecond.Stop();
                    };
                }
            };
        }

        /// <summary>
        /// Allows player one to play a card. If player one is the first to go in the turn, all cards should be playable, 
        /// if not, only the cards matching the first card's suit should be playable
        /// </summary>
        public void ReadyPlayerOne()
        {
            leaveDisabled = false;

            if (playerOne.GetIsFirst() == true)
            {
                playerOneHand.SetAllPlayable(true);
            }
            else
            {
                playerOneHand.SetSomePlayable(currentPlayableSuit);
            }
        }

        /// <summary>
        /// Allows an AI player to play a card. Takes the Hand object and Player object of the player
        /// whos turn it is.
        /// </summary>
        /// <param name="hand"></param>
        /// <param name="player"></param>
        public void ReadyAI(Hand hand, Player player)
        {
            Sound.PlayCardClick();

            //If there are cards in this Hand
            if (hand.GetHand().Count != 0)
            {
                Thickness margin;

                //If current player is 2
                if (player.GetPlayerNumber() == 2)
                {
                    //Decrementing right amount label
                    string old = (string)lblRightAmount.Content;
                    int newAmount = Int32.Parse(old);
                    newAmount -= 1;
                    lblRightAmount.Content = newAmount.ToString();
                    margin = new System.Windows.Thickness(700, 0, 0, 0);
                }
                //If current player is 3
                else if (player.GetPlayerNumber() == 3)
                {
                    //Decrementing top  amount label
                    string old = (string)lblTopAmount.Content;
                    int newAmount = Int32.Parse(old);
                    newAmount -= 1;
                    lblTopAmount.Content = newAmount.ToString();
                    margin = new System.Windows.Thickness(0, 0, 0, 240);
                }
                //If current player is 4
                else
                {
                    //Decrementing left amount label
                    string old = (string)lblLeftAmount.Content;
                    int newAmount = Int32.Parse(old);
                    newAmount -= 1;
                    lblLeftAmount.Content = newAmount.ToString();
                    margin = new System.Windows.Thickness(0, 0, 700, 0);
                }

                Card card;

                //If this player is first to pick a card
                if (player.GetIsFirst() == true)
                {
                    //Set all cards playable
                    hand.SetAllPlayable(true);

                    //If difficulty is 1 (easy), play a random card
                    if (mainWindow.GetDifficulty() == 1)
                    {
                        card = hand.PlayRandomCard();
                    }
                    //Else (medium or hard), play a logical card
                    else
                    {
                        card = hand.PlayLogicalCard(null, null, cardsPlayed, 0);
                    }

                    //Increasing the cards rank after it has been played (so AI don't always select trump cards)
                    card.SetRank(card.GetRank() + 50);

                    if (card.GetSuit() == currentTrump)
                    {
                        card.SetRank(card.GetRank() + 100);
                    }

                    //Current playable suit becomes the suit of the card played
                    currentPlayableSuit = hand.FindPlayedSuit();
                }
                //If not first to pick a card
                else
                {
                    //Set cards of matching suit playable
                    hand.SetSomePlayable(currentPlayableSuit);

                    //If difficulty is 1 (easy), play the first playable card
                    if (mainWindow.GetDifficulty() == 1)
                    {
                        card = hand.PlayFirstPlayable(currentPlayableSuit, currentTrump);
                    }
                    //Else (medium or hard), play logical card (taking into account the playable suit, trump, cards played
                    //and the difficulty)
                    else
                    {
                        card = hand.PlayLogicalCard(currentPlayableSuit, currentTrump, cardsPlayed, mainWindow.GetDifficulty());
                    }
                }

                //Displaying a card to the specific play area of the player
                card.GetCard().Margin = margin;
                gridTable.Children.Add(card.GetCard());

                LoggingAndStats.Log(player.GetPlayerName(), "Played " + card.GetCode());

                //Add the card played to cardsPlayed List
                cardsPlayed.Add(card);
            }
        }
        #endregion

        #region Scoring
        /// <summary>
        /// Finds who won the current turn, sets that player as currentPlayer (winner will go first in the next turn).
        /// A trick is won by the highest tarneeb (trump) or the highest card of the suit led.
        /// </summary>
        public void WhoWonWhosNext()
        {
            //Getting the trump
            string trump = txtTrump.Text;

            //List of played cards
            List<Card> playedCards = new List<Card> { };

            //Finding the played Card in each hand, adding it to playedCards List
            foreach (Hand hand in hands)
            {
                Card card = hand.FindPlayedCard();
                playedCards.Add(card);
            }

            //Ordering the playedCards List by rank highest to lowest
            playedCards = playedCards.OrderByDescending(a => a.GetRank()).ToList();

            //If highest card is owned by player 1
            if (playedCards[0].GetOwnedBy() == 1)
            {
                //Player one is the winner, set currentPlayer to be player one
                currentPlayer = playerOne;

                //Increment tricks won
                playerOne.SetTricksWon(playerOne.GetTricksWon() + 1);
            }
            //If highest card is owned by player 2
            else if (playedCards[0].GetOwnedBy() == 2)
            {
                //Player one is the winner, set currentPlayer to be player one
                currentPlayer = playerTwo;

                //Increment tricks won
                playerTwo.SetTricksWon(playerTwo.GetTricksWon() + 1);
            }
            //If highest card is owned by player 3
            else if (playedCards[0].GetOwnedBy() == 3)
            {
                //Player one is the winner, set currentPlayer to be player one
                currentPlayer = playerThree;

                //Increment tricks won
                playerThree.SetTricksWon(playerThree.GetTricksWon() + 1);
            }
            //If highest card is owned by player 4
            else if (playedCards[0].GetOwnedBy() == 4)
            {
                //Player one is the winner, set currentPlayer to be player one
                currentPlayer = playerFour;

                //Increment tricks won
                playerFour.SetTricksWon(playerFour.GetTricksWon() + 1);
            }

            LoggingAndStats.Log(currentPlayer.GetPlayerName(), "Won the trick");

            //Reset the ranks of every card in each hand
            foreach (Hand hand in hands)
            {
                hand.ResetRanks();
            }     
        }

        /// <summary>
        /// Updates the score. If it finds that a teams score is greater than or equal to the winning score,
        /// it ends the game. Otherwise, starts the next round.
        /// </summary>
        public void UpdateScore()
        {
            //Calculating total tricks won by each team
            int totalTricks1 = players[0].GetTricksWon() + players[2].GetTricksWon();
            int totalTricks2 = players[1].GetTricksWon() + players[3].GetTricksWon();

            if (totalTricks1 > totalTricks2)
            {
                LoggingAndStats.Log("System", "Team One won the round");
            }
            else if (totalTricks2 > totalTricks1)
            {
                LoggingAndStats.Log("System", "Team Two won the round");
            }

            int biddingTeam = 0;

            //Finding the team that bid
            //If player 1 or player 2 is the bidder
            if (players[0].GetIsBidder() || players[2].GetIsBidder())
            {
                biddingTeam = 1;

            }
            //if player 2 or player 4 is the bidder
            else if (players[1].GetIsBidder() || players[3].GetIsBidder())
            {
                biddingTeam = 2;
            }

            //If bidding team was team 1 and bid was achieved
            if (biddingTeam == 1 && totalTricks1 >= mainWindow.GetBid())
            {
                team1Score += totalTricks1;
                LoggingAndStats.Log("System", "Team One gained " + totalTricks1 + " points");

                //If it was an attempted Kaboot
                if (mainWindow.GetBid() == kaboot)
                {
                    team1Score += 3;
                    LoggingAndStats.Log("System", "Team One won a Kaboot! (gained 3 bonus points)");
                }
            }
            //If bidding team was team 1 and bid was not achieved
            else if (biddingTeam == 1 && totalTricks1 < mainWindow.GetBid())
            {
                team1Score -= mainWindow.GetBid();
                LoggingAndStats.Log("System", "Team One lost " + mainWindow.GetBid() + " points");

                //If it was an attempted Kaboot
                if (mainWindow.GetBid() == kaboot)
                {
                    team1Score -= 3;
                    LoggingAndStats.Log("System", "Team One lost 3 more points for failed Kaboot");

                    team2Score += totalTricks2 * 2;
                    LoggingAndStats.Log("System", "Team Two gained " + totalTricks2 * 2 + " points");
                }
            }
            //If bidding team was team 2 and bid was achieved
            else if (biddingTeam == 2 && totalTricks2 >= mainWindow.GetBid())
            {
                team2Score += totalTricks2;
                LoggingAndStats.Log("System", "Team Two gained " + totalTricks2 + " points");

                //If it was an attempted Kaboot
                if (mainWindow.GetBid() == kaboot)
                {
                    team2Score += 3;
                    LoggingAndStats.Log("System", "Team Two won a Kaboot! (gained 3 bonus points)");
                }
            }
            //If bidding team was team 2 and bid was not achieved
            else if (biddingTeam == 2 && totalTricks2 < mainWindow.GetBid())
            {
                team2Score -= mainWindow.GetBid();
                LoggingAndStats.Log("System", "Team Two lost " + mainWindow.GetBid() + " points");

                //If it was an attempted Kaboot
                if (mainWindow.GetBid() == kaboot)
                {
                    team2Score -= 3;
                    LoggingAndStats.Log("System", "Team Two lost 3 more points for failed Kaboot");

                    team1Score += totalTricks1 * 2;
                    LoggingAndStats.Log("System", "Team One gained " + totalTricks1 * 2 + " points");
                }
            }

            LoggingAndStats.Log("System", "Team One score is " + team1Score);
            LoggingAndStats.Log("System", "Team Two score is " + team2Score);

            //Update score texts
            txtScore1.Text = team1Score.ToString();
            txtScore2.Text = team2Score.ToString();

            //If neither team has reached the winning score (31)
            if (team1Score < winningScore && team2Score < winningScore)
            {
                ResetRound();
            }
            //Else if a team has reached a score of 31 or higher
            else
            {
                Sound.PlayButtonClick();

                //Send values to main window
                if (team1Score >= winningScore)
                {
                    LoggingAndStats.Log("System", "Team One won the match");
                    mainWindow.SetWinningTeam("TEAM ONE");
                }
                else if (team2Score >= winningScore)
                {
                    LoggingAndStats.Log("System", "Team Two won the match");
                    mainWindow.SetWinningTeam("TEAM TWO");
                }

                mainWindow.SetTeamOneScore(team1Score);
                mainWindow.SetTeamTwoScore(team2Score);

                //Reset some values
                mainWindow.SetUserHasBid(false);
                playersLeft = 0;

                //End game (clear children, load game over screen)
                mainWindow.gridContent.Children.Clear();

                //Loading game over screen
                Control gameOverScreen = new GameOverScreen();
                mainWindow.gridContent.Children.Add(gameOverScreen);
            }
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Starts the turnTimer if it finds that player one has played a card. Does nothing if isRunning is true.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Card_IsPicked(object sender, MouseEventArgs e)
        {
            //If is running is false (user is allowed to pick a card)
            if (!isRunning)
            {
                Sound.PlayCardClick();

                //Loops through each card in the users hand
                foreach (Card card in playerOneHand.GetHand())
                {
                    //If current card is the card that was played
                    if (card.GetIsPlayed())
                    {
                        leaveDisabled = true;
                        isRunning = true;

                        //All cards in the users hand are now unplayable
                        playerOneHand.SetAllPlayable(false);

                        LoggingAndStats.Log(playerOne.GetPlayerName(), "Played " + card.GetCode());

                        //If user is first to pick a Card, current playable suit becomes the suit of the card the
                        //user selected
                        if (playerOne.GetIsFirst())
                        {
                            currentPlayableSuit = playerOneHand.FindPlayedSuit();
                        }

                        //Increasing the selected Cards rank (based on trump and suit led)
                        if (card.GetSuit() == currentTrump)
                        {
                            card.SetRank(card.GetRank() + 100);
                        }
                        if (card.GetSuit() == currentPlayableSuit)
                        {
                            card.SetRank(card.GetRank() + 50);
                        }

                        //Adding the Card to the main
                        cardsPlayed.Add(card);

                        //Starting the turnTimer
                        turnTimer.Start();

                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Simply leaves the match (clears all controls, loads home control). Sometimes the button is disabled 
        /// to avoid strange errors with the timers (timer code would run after user has left).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLeaveMatch_Click(object sender, RoutedEventArgs e)
        {
            //If user is allowed to leave
            if (!leaveDisabled)
            {
                Sound.PlayButtonClick();

                //Asks user to confirm and if user does confirm, reset some values, clear children, and load home screen
                if (MessageBox.Show("Are you sure you want to leave the match?", "Warning",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    LoggingAndStats.Log(playerOne.GetPlayerName(), "Left the match");

                    mainWindow.SetUserHasBid(false);

                    playersLeft = 0;

                    mainWindow.gridContent.Children.Clear();

                    Control controlHomeScreen = new HomeScreen();
                    mainWindow.gridContent.Children.Add(controlHomeScreen);
                }
            }
            //If not allowed to leave
            else
            {
                MessageBox.Show("You can only choose to leave during your turn.", "Notice",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        #endregion
    }
}
