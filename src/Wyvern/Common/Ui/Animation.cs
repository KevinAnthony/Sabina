#region Using

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Noside.Common.Animation;

#endregion

namespace Noside.Common.Ui
{
    internal static class Animation
    {
        #region Fields

        private static readonly EasingFunctionBase EasingFunction = new ExponentialEase {EasingMode = EasingMode.EaseInOut};
        private static readonly Duration SlideAnimationTime = new Duration(TimeSpan.FromMilliseconds(500.0d));

        #endregion

        #region Public Methods

        public static void AnimationSlide(ColumnDefinition column, Button button, double from, double to)
        {
            if (button == null)
            {
                column.AnimateWidth(from, to);
                return;
            }


            SolidColorBrush brush = (SolidColorBrush) button.Background;
            Color color = brush.Color;
            if (brush.IsFrozen || brush.IsSealed)
            {
                brush = new SolidColorBrush(color);
                button.Background = brush;
            }

            Color flashColor = Theme.HighlightColor;

            ColorAnimationUsingKeyFrames flash = new ColorAnimationUsingKeyFrames();

            flash.KeyFrames.Add(new LinearColorKeyFrame(color, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0))));
            flash.KeyFrames.Add(new LinearColorKeyFrame(flashColor, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(75))));
            flash.KeyFrames.Add(new LinearColorKeyFrame(color, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(150))));
            flash.KeyFrames.Add(new LinearColorKeyFrame(flashColor, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(225))));
            flash.KeyFrames.Add(new LinearColorKeyFrame(color, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(300))));
            flash.KeyFrames.Add(new LinearColorKeyFrame(flashColor, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(375))));
            flash.KeyFrames.Add(new LinearColorKeyFrame(color, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(450))));
            flash.KeyFrames.Add(new LinearColorKeyFrame(flashColor, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(525))));
            flash.KeyFrames.Add(new LinearColorKeyFrame(color, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(600))));
            flash.Completed += (s, a) => { column.AnimateWidth(from, to); };
            brush.BeginAnimation(SolidColorBrush.ColorProperty, flash);
        }

        #endregion

        #region Methods

        private static void AnimateWidth(this ColumnDefinition column, double from, double to)
        {
            GridLengthAnimation animation = new GridLengthAnimation {Duration = SlideAnimationTime, From = from, To = to, EasingFunction = EasingFunction};
            column.BeginAnimation(ColumnDefinition.WidthProperty, animation);
        }

        #endregion
    }
}