/**************************************************************************************************************
 * Author(s):   Ahmed Butt, Jugal Patel, Jermaine Henry, Nirmal Sureshbhai Patel, Fabian Mauricio Narvaez goyes
 * Date:        April 9, 2021
 * File:        LoggingAndStats.cs
 * Description: All of the database functionality happens here. This class alters the TarneebLog database 
 *              (Logging table and UserStats table).
**************************************************************************************************************/

using System;
using System.Data.SqlClient;
using System.Windows;

namespace Tarneeb
{
    public class LoggingAndStats
    {
        //Connection string
        static string connect = Properties.Settings.Default.TarneebLogConnectionStringRelative;

        /// <summary>
        /// Inserts into the Logging table, including who or what triggered the event, what the event is
        /// and at what time the event occurred.
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="ev"></param>
        public static void Log(string trigger, string ev)
        {
            DateTime eventDateTime = DateTime.Now;

            try
            {
                SqlConnection conn = new SqlConnection(connect);
                conn.Open();

                string insertQuery = "INSERT INTO [Logging]([LogID], [Trigger], [Event], [TimeOccurred]) " +
                    "VALUES((SELECT ISNULL(MAX(LogID) + 1, 1) FROM [Logging]), '" + trigger + "', '" + ev + "', '" + eventDateTime + "')";

                SqlCommand command = new SqlCommand(insertQuery, conn);
                command.ExecuteNonQuery();

                conn.Close();
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// Updates UserStats tables, increasing either wins, or losses by 1 (depending on whether user
        /// won or not), and always increasing games played by 1.
        /// </summary>
        /// <param name="win"></param>
        public static void UpdateStats(bool win)
        {
            string updateQuery = null;

            try
            {
                SqlConnection conn = new SqlConnection(connect);
                conn.Open();

                if (win)
                {
                    updateQuery = "UPDATE UserStats SET Wins = Wins + 1, GamesPlayed = GamesPlayed + 1 WHERE PlayerID = 1";
                }
                else if (!win)
                {
                    updateQuery = "UPDATE UserStats SET Losses = Losses + 1, GamesPlayed = GamesPlayed + 1 WHERE PlayerID = 1";
                }

                SqlCommand command = new SqlCommand(updateQuery, conn);
                command.ExecuteNonQuery();

                conn.Close();
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// Selects the users name that is stored in UserStats table and returns that value. This makes
        /// it so the application "remembers" the users preferred name.
        /// </summary>
        /// <returns>string</returns>
        public static string GetUserNameFromDatabase()
        {
            string name = null;

            try
            {
                SqlConnection conn = new SqlConnection(connect);
                conn.Open();

                string selectQuery = "SELECT PlayerName FROM UserStats WHERE PlayerID = 1";

                SqlCommand command = new SqlCommand(selectQuery, conn);
                name = (string)command.ExecuteScalar();
                name = name.Trim();

                conn.Close();
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
            }

            return name;
        }

        /// <summary>
        /// Updates the users name in UserStats table.
        /// </summary>
        /// <param name="name"></param>
        public static void SetUserNameInDatabase(string name)
        {
            try
            {
                SqlConnection conn = new SqlConnection(connect);
                conn.Open();

                string updateQuery = "UPDATE UserStats SET PlayerName = '" + name + "' WHERE PlayerID = 1";

                SqlCommand command = new SqlCommand(updateQuery, conn);
                command.ExecuteNonQuery();

                conn.Close();
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// Selects the number of wins in UserStats table and returns that value as a string (displaying on
        /// stats screen purposes).
        /// </summary>
        /// <returns>string</returns>
        public static string GetWins()
        {
            int wins = 0;

            try
            {
                SqlConnection conn = new SqlConnection(connect);
                conn.Open();

                string selectQuery = "SELECT Wins FROM UserStats WHERE PlayerID = 1";

                SqlCommand command = new SqlCommand(selectQuery, conn);
                wins = (int)command.ExecuteScalar();

                conn.Close();
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
            }

            return wins.ToString();
        }

        /// <summary>
        /// Selects the number of losses in UserStats table and returns that value as a string (displaying on
        /// stats screen purposes).
        /// </summary>
        /// <returns>string</returns>
        public static string GetLosses()
        {
            int losses = 0;

            try
            {
                SqlConnection conn = new SqlConnection(connect);
                conn.Open();

                string selectQuery = "SELECT Losses FROM UserStats WHERE PlayerID = 1";

                SqlCommand command = new SqlCommand(selectQuery, conn);
                losses = (int)command.ExecuteScalar();

                conn.Close();
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
            }

            return losses.ToString();
        }

        /// <summary>
        /// Selects the number of games played in UserStats table and returns that value as a string (displaying on
        /// stats screen purposes).
        /// </summary>
        /// <returns>string</returns>
        public static string GetGamesPlayed()
        {
            int gamesPlayed = 0;

            try
            {
                SqlConnection conn = new SqlConnection(connect);
                conn.Open();

                string selectQuery = "SELECT GamesPlayed FROM UserStats WHERE PlayerID = 1";

                SqlCommand command = new SqlCommand(selectQuery, conn);
                gamesPlayed = (int)command.ExecuteScalar();

                conn.Close();
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
            }

            return gamesPlayed.ToString();
        }

        /// <summary>
        /// Resets the database (removes all rows in Logging and sets UserStats to the default values).
        /// </summary>
        public static void ResetDatabase()
        {
            try
            {
                SqlConnection conn = new SqlConnection(connect);
                conn.Open();

                string deleteQuery = "DELETE FROM Logging";

                SqlCommand command = new SqlCommand(deleteQuery, conn);
                command.ExecuteNonQuery();

                 string updateQuery = "UPDATE UserStats SET PlayerName = 'Player One', Wins = 0, " +
                    "Losses = 0, GamesPlayed = 0 WHERE PlayerID = 1";

                command = new SqlCommand(updateQuery, conn);
                command.ExecuteNonQuery();

                conn.Close();
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
