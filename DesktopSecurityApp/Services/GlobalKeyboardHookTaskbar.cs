using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;

namespace DesktopSecurityApp.Services
{
    public class WindowManager
    {
        private const int SW_HIDE = 0;
        private const int SW_RESTORE = 9;

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public static void MinimizeTaskbar()
        {
            IntPtr taskbarHandle = FindWindow("Shell_TrayWnd", "");
            if (taskbarHandle != IntPtr.Zero)
            {
                ShowWindow(taskbarHandle, SW_HIDE);
            }
        }

        public static void RestoreTaskbar()
        {
            IntPtr taskbarHandle = FindWindow("Shell_TrayWnd", "");
            if (taskbarHandle != IntPtr.Zero)
            {
                ShowWindow(taskbarHandle, SW_RESTORE);
            }
        }
    }
}
