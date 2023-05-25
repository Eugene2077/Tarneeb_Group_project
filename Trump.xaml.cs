/**************************************************************************************************************
 * Author(s):   Ahmed Butt, Jugal Patel, Jermaine Henry, Nirmal Sureshbhai Patel, Fabian Mauricio Narvaez goyes
 * Date:        April 9, 2021
 * File:        Trump.xaml.cs
 * Description: This file is for the functionality of the Trump.xaml in-game pop-up. It allows the user to
 *              select a trump.
**************************************************************************************************************/

using System.Windows;
using System.Windows.Controls;

namespace Tarneeb
{
    /// <summary>
    /// Interaction logic for Trump.xaml
    /// </summary>
    public partial class Trump : UserControl
    {
        //Accessing main window
        MainWindow mainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);

        /// <summary>
        /// Initializes the trump control.
        /// </summary>
        public Trump()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Occurs when the confrim button is clicked. Sends values (trump and a boolean value) to the main window. 
        /// The trump sent is based on the selected radio button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClick();

            if (rbSpades.IsChecked == true || rbHearts.IsChecked == true || rbDiamonds.IsChecked == true || rbClubs.IsChecked == true)
            {
                mainWindow.gridContent.Children.Remove(this);

                string selection = "";

                if (rbSpades.IsChecked == true)
                {
                    selection = "Spades";
                }
                else if (rbHearts.IsChecked == true)
                {
                    selection = "Hearts";
                }
                else if (rbDiamonds.IsChecked == true)
                {
                    selection = "Diamonds";
                }
                else if (rbClubs.IsChecked == true)
                {
                    selection = "Clubs";
                }

                mainWindow.SetTrump(selection);
                mainWindow.SetPlay(true);
            }
        }
    }
}
