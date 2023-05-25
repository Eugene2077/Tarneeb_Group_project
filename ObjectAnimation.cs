/**************************************************************************************************************
 * Author(s):   Ahmed Butt, Jugal Patel, Jermaine Henry, Nirmal Sureshbhai Patel, Fabian Mauricio Narvaez goyes
 * Date:        April 9, 2021
 * File:        ObjectAnimation.cs
 * Description: Designed to make images on the screen easier to move around. Label objects are only set
 *              visible or invisible (this class can be altered to move around labels as well). Many more
 *              objects can be added to make them moveable.
**************************************************************************************************************/

using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Tarneeb
{
    public class ObjectAnimation
    {
        #region Class Attributes
        //Stores an image
        private Image image;

        //Stores a Label
        private Label label;

        //Stores old X value (starting X axis position)
        private double oldX;

        //Stores old Y value (starting Y axis position)
        private double oldY;

        //Stores new X value (X axis position to get to from old X)
        private double newX;

        //Stores new Y value (Y axis position to get to from old Y)
        private double newY;

        //Stores the time it should take to travel
        private double length;

        //Stores the time to delay X travel
        private double delayX;

        //Stores the time to delay Y travel
        private double delayY;

        //Stores a general delay
        private int delay;
        #endregion

        #region Constructors
        /// <summary>
        /// Parameterized constructor (for user).
        /// </summary>
        /// <param name="image"></param>
        /// <param name="oldX"></param>
        /// <param name="oldY"></param>
        /// <param name="newX"></param>
        /// <param name="newY"></param>
        /// <param name="length"></param>
        /// <param name="delayX"></param>
        /// <param name="delayY"></param>
        /// <param name="delay"></param>
        public ObjectAnimation(Image image, double oldX, double oldY, double newX, double newY, double length, 
            double delayX, double delayY, int delay)
        {
            this.image = image;
            this.oldX = oldX;
            this.oldY = oldY;
            this.newX = newX;
            this.newY = newY;
            this.length = length;
            this.delayX = delayX;
            this.delayY = delayY;
            this.delay = delay;
        }
        /// <summary>
        /// Parameterized constructor (for AI players, user doesn't have a label). Takes an extra Label as a parameter.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="label"></param>
        /// <param name="oldX"></param>
        /// <param name="oldY"></param>
        /// <param name="newX"></param>
        /// <param name="newY"></param>
        /// <param name="length"></param>
        /// <param name="delayX"></param>
        /// <param name="delayY"></param>
        /// <param name="delay"></param>
        public ObjectAnimation(Image image, Label label, double oldX, double oldY, double newX, double newY, double length, 
            double delayX, double delayY, int delay)
        {
            this.image = image;
            this.label = label;
            this.oldX = oldX;
            this.oldY = oldY;
            this.newX = newX;
            this.newY = newY;
            this.length = length;
            this.delayX = delayX;
            this.delayY = delayY;
            this.delay = delay;
        }

        /// <summary>
        /// Parameterized constructor that only takes an image as a paramter (for images that don't need to move). 
        /// </summary>
        /// <param name="image"></param>
        public ObjectAnimation(Image image)
        {
            this.image = image;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Begins the movement from one specified location to another with specified length and delay.
        /// </summary>
        public void StartAnimation()
        {
            TranslateTransform translate = new TranslateTransform();
            image.RenderTransform = translate;

            //Move from oldX to newX for specified length (increasing length will slow down animation)
            DoubleAnimation animateX = new DoubleAnimation(oldX, newX, TimeSpan.FromSeconds(length));

            //Start moving after delay
            animateX.BeginTime = System.TimeSpan.FromSeconds(delayX);

            //Begin animation
            translate.BeginAnimation(TranslateTransform.XProperty, animateX);

            //Move from oldY to newY for specified length
            DoubleAnimation animateY = new DoubleAnimation(oldY, newY, TimeSpan.FromSeconds(length));

            //Start moving after delay
            animateY.BeginTime = System.TimeSpan.FromSeconds(delayY);

            //Begin animation
            translate.BeginAnimation(TranslateTransform.YProperty, animateY);

            AfterDelay();
        }

        /// <summary>
        /// Sets an image invisible or visible after a delay.
        /// </summary>
        /// <param name="setVis"></param>
        /// <param name="delay"></param>
        public async Task DelayImage(bool setVis, int delay)
        {
            if (setVis)
            {
                await Task.Delay(delay);
                this.image.Visibility = Visibility.Visible;
            }
            else if (!setVis)
            {
                await Task.Delay(delay);
                this.image.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Makes a label visible after a delay.
        /// </summary>
        async Task AfterDelay()
        {
            if (this.label != null)
            {
                await Task.Delay(delay);
                this.label.Visibility = Visibility.Visible;
            }
        }
        #endregion
    }
}
