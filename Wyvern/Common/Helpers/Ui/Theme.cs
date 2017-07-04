#region Using

#endregion

using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Noside.Common.Helpers.Ui
{
    internal static class Theme
    {
        public static Color PrimaryColor { get; set; } = (Color) Application.Current.Resources[nameof(PrimaryColor)];

        public static Color HighlightColor { get; set; } = (Color) Application.Current.Resources[nameof(HighlightColor)];

        public static Color TextAndLineColor { get; set; } = (Color) Application.Current.Resources[nameof(TextAndLineColor)];

        public static SolidColorBrush PrimaryBrush { get; set; } = Application.Current.Resources[nameof(PrimaryBrush)] as SolidColorBrush;

        public static SolidColorBrush HighlightBrush { get; set; } = Application.Current.Resources[nameof(HighlightBrush)] as SolidColorBrush;

        public static SolidColorBrush TextAndLineBrush { get; set; } = Application.Current.Resources[nameof(TextAndLineBrush)] as SolidColorBrush;
        public static Path Icon { get; set; } = Application.Current.Resources[nameof(Icon)] as Path;
    }
}