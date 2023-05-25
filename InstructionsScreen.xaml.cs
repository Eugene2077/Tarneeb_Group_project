/**************************************************************************************************************
 * Author(s):   Ahmed Butt, Jugal Patel, Jermaine Henry, Nirmal Sureshbhai Patel, Fabian Mauricio Narvaez goyes
 * Date:        April 9, 2021
 * File:        InstructionsScreen.xaml.cs
 * Description: Initializes instructions screen which displays the instructions and provides button 
 *              navigation logic.
**************************************************************************************************************/

using System.Windows;
using System.Windows.Controls;

namespace Tarneeb
{
    /// <summary>
    /// Interaction logic for InstructionsScreen.xaml
    /// </summary>
    public partial class InstructionsScreen : UserControl
    {
        MainWindow mainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);

        /// <summary>
        /// Initializes instructions screen.
        /// </summary>
        public InstructionsScreen()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Occurs when back button is clicked. Unloads all children of the main windows grid and
        /// loads the home screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClick();

            mainWindow.gridContent.Children.Clear(); //unload previous user control to avoid overlap

            Control controlHomeScreen = new HomeScreen();
            mainWindow.gridContent.Children.Add(controlHomeScreen); //load user control
        }

        /// <summary>
        /// Occurs when start game button is clicked. Unloads all children of the main windows grid and
        /// loads the offline screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartGame_Click(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClick();

            mainWindow.gridContent.Children.Clear();

            Control controlOfflineScreen = new OfflineScreen();
            mainWindow.gridContent.Children.Add(controlOfflineScreen);
        }
    }
}
