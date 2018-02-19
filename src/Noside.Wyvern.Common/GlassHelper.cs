using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace Noside.Wyvern.Common
{
    public class GlassHelper
    {
	    public static bool ExtendGlassFrame(Window window) {
		    return ExtendGlassFrame(window, new Thickness(-1));
	    }

        public static bool ExtendGlassFrame(Window window, Thickness margin)
        {
            IntPtr hwnd = new WindowInteropHelper(window).Handle;
            if (hwnd == IntPtr.Zero)
                throw new InvalidOperationException("The Window must be shown before extending glass.");


            if (NativeMethods.DwmApi.DwmIsCompositionEnabled())
            {
                var windowHelper = new WindowInteropHelper(window);

                var accent = new NativeMethods.Structs.AccentPolicy();
                var accentStructSize = Marshal.SizeOf(accent);
                accent.AccentState = NativeMethods.AccentState.ACCENT_ENABLE_BLURBEHIND;

                var accentPtr = Marshal.AllocHGlobal(accentStructSize);
                Marshal.StructureToPtr(accent, accentPtr, false);

                var data = new NativeMethods.Structs.WindowCompositionAttributeData();
                data.Attribute = NativeMethods.WindowCompositionAttribute.WCA_ACCENT_POLICY;
                data.SizeOfData = accentStructSize;
                data.Data = accentPtr;

                NativeMethods.User32.SetWindowCompositionAttribute(windowHelper.Handle, ref data);

                Marshal.FreeHGlobal(accentPtr);
                return true;
            }
                        
            // Set the background to transparent from both the WPF and Win32 perspectives
            window.Background = Brushes.Transparent;
            HwndSource.FromHwnd(hwnd).CompositionTarget.BackgroundColor = Colors.Transparent;


            NativeMethods.Structs.MARGINS margins = new NativeMethods.Structs.MARGINS(margin);
            NativeMethods.DwmApi.DwmExtendFrameIntoClientArea(hwnd, ref margins);
            return true;
        }
    }
}
