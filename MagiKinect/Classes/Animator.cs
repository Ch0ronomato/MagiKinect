using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;


namespace MagiKinect
{
    class Animator
    {
        public Canvas canvasForAnimation;
        public AssetHandler.DeckType? backgroundForAnimation;
        public delegate void AnimationFadeInComplete(Canvas canvas, AssetHandler.DeckType color);
        public event AnimationFadeInComplete FadeInComplete;
        public static Animator Instance;
        public Animator()
        {
            // Store static instance of this object.
            Animator.Instance = this;
        }

        public void AnimateBackground(Canvas canvas, AssetHandler.DeckType background)
        {
            // Store the canvas and background argument for later use.
            Animator.Instance.canvasForAnimation = canvas;
            Animator.Instance.backgroundForAnimation = background;

            // Setup the background animation.
            DoubleAnimation BackgroundAnimation = new DoubleAnimation(canvas.Opacity, 0.0, new Duration(new TimeSpan(1000 * 10000)));

            // Bind the complete event. This will allow for a fade out and
            // fade in effect.
            BackgroundAnimation.Completed += ContinueAnimation;

            // Start the fade out animation!
            canvas.Background.BeginAnimation(Brush.OpacityProperty, BackgroundAnimation);
        }

        public void ContinueAnimation(object sender, EventArgs e)
        {
            // Get the canvas and background from the Animator instance.
            Canvas canvas = Animator.Instance.canvasForAnimation;
            AssetHandler.DeckType background = Animator.Instance.backgroundForAnimation.Value;

            // Get the canvas background image source.
            canvas.Background = AssetHandler.SetupAssets().GetImageFromColor(background);

            // Setup the fade in animation.
            DoubleAnimation BackgroundAnimation = new DoubleAnimation(0.0, 0.7, new Duration(new TimeSpan(1000 * 10000)));

            // Bind the complete event for the fade in effect.
            BackgroundAnimation.Completed += AnimationFinished;

            // Start the fade in!
            canvas.Background.BeginAnimation(Brush.OpacityProperty, BackgroundAnimation);
        }

        public void AnimationFinished(object sender, EventArgs e)
        {
            // Get the canvas and background from the Animator instance.
            Canvas canvas = Animator.Instance.canvasForAnimation;
            AssetHandler.DeckType background = Animator.Instance.backgroundForAnimation.Value;

            // Clear event values
            Animator.Instance.canvasForAnimation = null;
            Animator.Instance.backgroundForAnimation = null;

            // Invoke the end animation event.
            if (this.FadeInComplete != null)
            {
                Animator.Instance.FadeInComplete.Invoke(canvas, background);
            }
        }
    }
}
