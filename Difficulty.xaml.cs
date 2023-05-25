/**************************************************************************************************************
 * Author(s):   Ahmed Butt, Jugal Patel, Jermaine Henry, Nirmal Sureshbhai Patel, Fabian Mauricio Narvaez goyes
 * Date:        April 9, 2021
 * File:        Difficulty.xaml.cs
 * Description: Allows the user to select a difficulty. Changing the difficulty alters the AIs behaviour.
**************************************************************************************************************/

using System.Windows;
using System.Windows.Controls;

namespace Tarneeb
{
    /// <summary>
    /// Interaction logic for Difficulty.xaml
    /// </summary>
    public partial class Difficulty : UserControl
    {
        //Accessing main window
        MainWindow mainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);

        /// <summary>
        /// Initializes difficulty user control. Remembers the user set difficulty.
        /// </summary>
        public Difficulty()
        {
            InitializeComponent();

            //Gets the difficulty that is set in main window; a way to "remember" the user set difficulty
            if (mainWindow.GetDifficulty() == 1)
            {
                rbEasy.IsChecked = true;
            }
            else if (mainWindow.GetDifficulty() == 2)
            {
                rbMedium.IsChecked = true;
            }
            else if (mainWindow.GetDifficulty() == 3)
            {
                rbHard.IsChecked = true;
            }
        }

        /// <summary>
        /// Occurs when the confirm button is clicked. Changes the difficulty value in main window based on the
        /// selected radio button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClick();

            if (rbEasy.IsChecked == true)
            {
                mainWindow.SetDifficulty(1);
            }
            else if (rbMedium.IsChecked == true)
            {
                mainWindow.SetDifficulty(2);
            }
            else if (rbHard.IsChecked == true)
            {
                mainWindow.SetDifficulty(3);
            }

            mainWindow.gridContent.Children.Remove(this);
        }
    }
}
