using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Noside.Common.Helpers
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
