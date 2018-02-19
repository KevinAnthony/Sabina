#region Using

using System;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Interop;

#endregion

namespace Noside.Wyvern
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        #region Fields

        private uint _message;
        private Mutex _mutex;
        private bool _hooked = false;

	    #endregion

        #region Methods

        protected override void OnStartup(StartupEventArgs e)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            bool mutexCreated;
            string mutexName = $"Local\\{assembly.GetType().GUID}{assembly.GetName().Name}";
            this._mutex = new Mutex(true, mutexName, out mutexCreated);
            this._message = Common.NativeMethods.User32.RegisterWindowMessage(mutexName);

            if (!mutexCreated)
            {
                Common.NativeMethods.User32.PostMessage(Common.NativeMethods.HwndBroadcast, this._message, IntPtr.Zero, IntPtr.Zero);
                Current.Shutdown();
                return;
            }
	        base.OnStartup(e);

	        var bootstrapper = new Bootstrapper();
	        bootstrapper.Run();
		}

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            if (this._hooked) return;
//	        HwndSource hwndSource = HwndSource.FromHwnd(new WindowInteropHelper(this.MainWindow).Handle);
//            hwndSource?.AddHook(this.HandleMessages);
            this._hooked = true;
        }

        private IntPtr HandleMessages(IntPtr handle, Int32 message, IntPtr wParameter, IntPtr lParameter, ref Boolean handled)
        {
            if (message == this._message)
            {
                if (this.MainWindow.WindowState == WindowState.Minimized)
                    this.MainWindow.WindowState = WindowState.Normal;

                bool topmost = this.MainWindow.Topmost;

                this.MainWindow.Topmost = true;
                this.MainWindow.Topmost = topmost;
            }

            return IntPtr.Zero;
        }

        #endregion

    }
}
