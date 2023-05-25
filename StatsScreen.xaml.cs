/**************************************************************************************************************
 * Author(s):   Ahmed Butt, Jugal Patel, Jermaine Henry, Nirmal Sureshbhai Patel, Fabian Mauricio Narvaez goyes
 * Date:        April 9, 2021
 * File:        Sound.cs
 * Description: Provides sound to the application.
**************************************************************************************************************/

using System.Windows;
using System.Windows.Controls;

namespace Tarneeb
{
    /// <summary>
    /// Interaction logic for StatsScreen.xaml
    /// </summary>
    public partial class StatsScreen : UserControl
    {
        //Accessing main window
        MainWindow mainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);

        /// <summary>
        /// Initializes stats screen. Gets wins, losses, games played from the database, and displays
        /// them.
        /// </summary>
        public StatsScreen()
        {
            InitializeComponent();

            lblWins.Content = LoggingAndStats.GetWins();
            lblLosses.Content = LoggingAndStats.GetLosses();
            lblGamesPlayed.Content = LoggingAndStats.GetGamesPlayed();
        }

        /// <summary>
        /// Occurs when back button is clicked. Removes itself.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClick();

            mainWindow.gridContent.Children.Remove(this);
        }
    }
}
