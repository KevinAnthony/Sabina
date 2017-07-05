using System;
using System.Runtime.InteropServices;

namespace Noside.Common
{
    public static class NativeMethods
    {

        public static readonly IntPtr HwndBroadcast = new IntPtr(0xffff);

        public static class User32
        {
            [DllImport("user32.dll", SetLastError =true, CharSet = CharSet.Auto)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool PostMessage(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);

            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            public static extern uint RegisterWindowMessage(string lpstring);
        }
    }
}
