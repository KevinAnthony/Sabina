using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace Noside.Common
{
    public static class NativeMethods
    {
        internal enum AccentState
        {
            ACCENT_DISABLED = 0,
            ACCENT_ENABLE_GRADIENT = 1,
            ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
            ACCENT_ENABLE_BLURBEHIND = 3,
            ACCENT_INVALID_STATE = 4
        }

        internal enum WindowCompositionAttribute
        {
            // ...
            WCA_ACCENT_POLICY = 19
            // ...
        }


        public static readonly IntPtr HwndBroadcast = new IntPtr(0xffff);

        public static class User32
        {
            [DllImport("user32.dll", SetLastError =true, CharSet = CharSet.Auto)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool PostMessage(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);

            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            public static extern uint RegisterWindowMessage(string lpstring);

            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref Structs.WindowCompositionAttributeData data);

        }

        public static class Structs
        {
            [StructLayout(LayoutKind.Sequential)]
            internal struct AccentPolicy
            {
                public AccentState AccentState;
                public int AccentFlags;
                public int GradientColor;
                public int AnimationId;
            }

            [StructLayout(LayoutKind.Sequential)]
            internal struct WindowCompositionAttributeData
            {
                public WindowCompositionAttribute Attribute;
                public IntPtr Data;
                public int SizeOfData;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct MARGINS
            {
                public int Left;      // width of left border that retains its size  
                public int Right;     // width of right border that retains its size  
                public int Top;      // height of top border that retains its size  
                public int Bottom;   // height of bottom border that retains its size  
                public MARGINS(Thickness t)
                {
                    Left = (int)t.Left;
                    Right = (int)t.Right;
                    Top = (int)t.Top;
                    Bottom = (int)t.Bottom;
                }
            };
        }


        public static class DwmApi
        {
            [DllImport("DwmApi.dll", PreserveSig = false)]
            public static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref Structs.MARGINS pMarInset);

            [DllImport("DwmApi.dll", PreserveSig = false)]
            public static extern bool DwmIsCompositionEnabled();
        }
    }
}
