/**************************************************************************************************************
 * Author(s):   Ahmed Butt, Jugal Patel, Jermaine Henry, Nirmal Sureshbhai Patel, Fabian Mauricio Narvaez goyes
 * Date:        April 9, 2021
 * File:        GameOverScreen.xaml.cs
 * Description: Initializes game over screen, takes values from main window to display the winner, and 
 *              provides button navigation logic.
**************************************************************************************************************/

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Tarneeb
{
    /// <summary>
    /// Interaction logic for GameOverScreen.xaml
    /// </summary>
    public partial class GameOverScreen : UserControl
    {
        //Accessing main window
        MainWindow mainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);

        /// <summary>
        /// Initializes game over screen. Sets up the screen by getting values from main window.
        /// </summary>
        public GameOverScreen()
        {
            InitializeComponent();

            //Gets the scores and the winning team from the main window and displays them
            lblScore1.Content = mainWindow.GetTeamOneScore();
            lblScore2.Content = mainWindow.GetTeamTwoScore();
            txtWinner.Text = mainWindow.GetWinningTeam();

            if (mainWindow.GetWinningTeam() == "TEAM TWO")
            {
                //Rectangle colour changes to red if user lost
                recColour.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));

                LoggingAndStats.UpdateStats(false);
            }
            else if (mainWindow.GetWinningTeam() == "TEAM ONE")
            {
                LoggingAndStats.UpdateStats(true);
            }

            //Resetting the values
            mainWindow.SetTeamOneScore(0);
            mainWindow.SetTeamTwoScore(0);
            mainWindow.SetWinningTeam("");
        }

        #region Event Handlers
        /// <summary>
        /// Occurs when main menu button is clicked. Unloads all children of the main windows grid and
        /// then loads home screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMainMenu_Click(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClick();

            mainWindow.gridContent.Children.Clear();

            Control homeScreen = new HomeScreen();
            mainWindow.gridContent.Children.Add(homeScreen);
        }

        /// <summary>
        /// Occurs when replay button is clicked. Unloads all children of the main windows grid and
        /// then loads offline screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReplay_Click(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClick();

            mainWindow.gridContent.Children.Clear();

            Control offlineScreen = new OfflineScreen();
            mainWindow.gridContent.Children.Add(offlineScreen);
        }
        #endregion
    }
}
