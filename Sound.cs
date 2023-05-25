/**************************************************************************************************************
 * Author(s):   Ahmed Butt, Jugal Patel, Jermaine Henry, Nirmal Sureshbhai Patel, Fabian Mauricio Narvaez goyes
 * Date:        April 9, 2021
 * File:        Sound.cs
 * Description: Provides sound to the application.
**************************************************************************************************************/

using System.IO;
using System.Reflection;

namespace Tarneeb
{
    public class Sound
    {
        /// <summary>
        /// Locates and plays a button clicking sound. Used for all buttons.
        /// </summary>
        public static void PlayButtonClick()
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\..\..\Sounds\button_click.wav");
            player.Play();
        }

        /// <summary>
        /// Locates and plays a card swiping sound. Used for handing out cards.
        /// </summary>
        public static void PlayCardSwipe()
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\..\..\Sounds\card_swipe.wav");
            player.Play();
        }

        /// <summary>
        /// Locates and plays a card clicking sound. Used for playing a card.
        /// </summary>
        public static void PlayCardClick()
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\..\..\Sounds\card_click.wav");
            player.Play();
        }
    }
}
