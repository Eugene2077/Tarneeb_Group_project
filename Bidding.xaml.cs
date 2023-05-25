/**************************************************************************************************************
 * Author(s):   Ahmed Butt, Jugal Patel, Jermaine Henry, Nirmal Sureshbhai Patel, Fabian Mauricio Narvaez goyes
 * Date:        April 9, 2021
 * File:        Bidding.xaml.cs
 * Description: This file is for the functionality of the Bidding.xaml in-game pop-up. It allows the user to
 *              select a bid.
**************************************************************************************************************/

using System;
using System.Windows;
using System.Windows.Controls;

namespace Tarneeb
{
    /// <summary>
    /// Interaction logic for Bidding.xaml
    /// </summary>
    public partial class Bidding : UserControl
    {
        //Accessing main window
        MainWindow mainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);

        /// <summary>
        /// Initializes bidding user control.
        /// </summary>
        public Bidding()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sends a bid (integer value ranging 7-13, which is selected by the user) to the main window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBid_Click(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClick();

            //Finding the selected number, converting it to an integer, and storing it into a variable
            ComboBoxItem selectedItem = (ComboBoxItem)cboBidSelect.SelectedItem;
            int selection = Int32.Parse(selectedItem.Content.ToString());

            //Change the bid value in the main window only if the users bid selection is greater
            if (selection > mainWindow.GetBid())
            {
                mainWindow.SetBid(selection);
            }

            //Setting other values (userBid and userHasBid) in main window that are used by offline screen
            mainWindow.SetUserBid(selection);

            mainWindow.SetUserHasBid(true);

            mainWindow.gridContent.Children.Remove(this); //remove itself
        }
    }
}
