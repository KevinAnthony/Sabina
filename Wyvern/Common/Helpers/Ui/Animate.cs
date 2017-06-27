#region Using

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Noside.Common.Helpers.Animation;

#endregion

namespace Noside.Common.Helpers.Ui
{
    internal static class Animate
    {
        #region Fields

        private static readonly EasingFunctionBase EasingFunction = new ExponentialEase {EasingMode = EasingMode.EaseInOut};
        private static readonly Duration SlideAnimationTime = new Duration(TimeSpan.FromMilliseconds(500.0d));

        #endregion

        #region Public Methods

        public static void AnimateWidth(this ColumnDefinition column, double from, double to)
        {
            GridLengthAnimation animation = new GridLengthAnimation {Duration = SlideAnimationTime, From = from, To = to, EasingFunction = EasingFunction};
            column.BeginAnimation(ColumnDefinition.WidthProperty, animation);
        }

        #endregion
    }
}