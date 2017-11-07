#region Using

using System;
using System.Windows;
using System.Windows.Media.Animation;

#endregion

namespace Noside.Common.Animation
{
    internal class GridLengthAnimation : AnimationTimeline
    {
        #region Fields

        public static readonly DependencyProperty EasingFunctionProperty;
        public static readonly DependencyProperty FromProperty;
        public static readonly DependencyProperty ToProperty;

        #endregion

        #region Constructors and Destructors

        static GridLengthAnimation()
        {
            FromProperty = DependencyProperty.Register("From", typeof(double), typeof(GridLengthAnimation));
            ToProperty = DependencyProperty.Register("To", typeof(double), typeof(GridLengthAnimation));
            EasingFunctionProperty = DependencyProperty.Register("EasingFunction", typeof(EasingFunctionBase), typeof(GridLengthAnimation));
        }

        #endregion

        #region Properties

        public EasingFunctionBase EasingFunction {
            get { return (EasingFunctionBase) this.GetValue(FromProperty); }
            set { this.SetValue(EasingFunctionProperty, value); }
        }

        public double From {
            get { return (double) this.GetValue(FromProperty); }
            set { this.SetValue(FromProperty, value); }
        }

        public override Type TargetPropertyType => typeof(GridLength);

        public double To {
            get { return (double) this.GetValue(ToProperty); }
            set { this.SetValue(ToProperty, value); }
        }

        #endregion

        #region Public Methods

        public override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
        {
            double fromVal = (double) this.GetValue(FromProperty);
            double toVal = (double) this.GetValue(ToProperty);
            EasingFunctionBase ease = this.GetValue(EasingFunctionProperty) as EasingFunctionBase;
            double time = ease?.Ease(animationClock.CurrentProgress.GetValueOrDefault()) ?? animationClock.CurrentProgress.GetValueOrDefault();
            if (fromVal > toVal)
            {
                return new GridLength((1 - time) * (fromVal - toVal) + toVal, GridUnitType.Pixel);
            }
            return new GridLength(time * (toVal - fromVal) + fromVal, GridUnitType.Pixel);
        }

        #endregion

        #region Methods

        protected override Freezable CreateInstanceCore()
        {
            return new GridLengthAnimation();
        }

        #endregion
    }
}