/**************************************************************************************************************
 * Author(s):   Ahmed Butt, Jugal Patel, Jermaine Henry, Nirmal Sureshbhai Patel, Fabian Mauricio Narvaez goyes
 * Date:        April 9, 2021
 * File:        HomeScreen.xaml.cs
 * Description: Initializes home screen and provides button navigation logic.
**************************************************************************************************************/

using System;
using System.Windows;
using System.Windows.Controls;

namespace Tarneeb
{
    /// <summary>
    /// Interaction logic for HomeScreen.xaml
    /// </summary>
    public partial class HomeScreen : UserControl
    {
        //Accessing main window
        MainWindow mainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);

        /// <summary>
        /// Initializes home screen.
        /// </summary>
        public HomeScreen()
        {
            InitializeComponent();
        }

        #region Event Handlers
        /// <summary>
        /// Occurs when the play button is clicked. Unloads all children of the main windows grid and
        /// loads the offline screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClick();

            //Initiate Offline Mode
            mainWindow.gridContent.Children.Clear(); //unload previous user control to avoid overlap

            Control controlOfflineScreen = new OfflineScreen();
            mainWindow.gridContent.Children.Add(controlOfflineScreen); //load user control
        }

        /// <summary>
        /// Occurs when how to play button is clicked. Unloads all children of the main windows grid and
        /// loads the instructions screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHowTo_Click(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClick();

            mainWindow.gridContent.Children.Clear();

            Control controlInstructionsScreen = new InstructionsScreen();
            mainWindow.gridContent.Children.Add(controlInstructionsScreen);
        }

        /// <summary>
        /// Occurs when the settings button is clicked. Unloads all children of the main windows grid and
        /// loads the settings screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClick();

            mainWindow.gridContent.Children.Clear();

            Control controlSettingsScreen = new SettingsScreen();
            mainWindow.gridContent.Children.Add(controlSettingsScreen);
        }

        /// <summary>
        /// Closes application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCloseApp_Click(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClick();

            Environment.Exit(0);
        }
        #endregion
    }
}
