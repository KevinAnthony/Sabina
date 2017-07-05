#region Using

#endregion

#region Using

using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

#endregion

namespace Noside.Common.Ui
{
    internal static class Theme
    {
        #region Properties

        public static SolidColorBrush HighlightBrush { get; set; } = Application.Current.Resources[nameof(HighlightBrush)] as SolidColorBrush;

        public static Color HighlightColor { get; set; } = (Color) Application.Current.Resources[nameof(HighlightColor)];

        public static Path Icon { get; set; } = Application.Current.Resources[nameof(Icon)] as Path;

        public static SolidColorBrush PrimaryBrush { get; set; } = Application.Current.Resources[nameof(PrimaryBrush)] as SolidColorBrush;

        public static Color PrimaryColor { get; set; } = (Color) Application.Current.Resources[nameof(PrimaryColor)];

        public static SolidColorBrush TextAndLineBrush { get; set; } = Application.Current.Resources[nameof(TextAndLineBrush)] as SolidColorBrush;

        public static Color TextAndLineColor { get; set; } = (Color) Application.Current.Resources[nameof(TextAndLineColor)];

        #endregion
    }
}