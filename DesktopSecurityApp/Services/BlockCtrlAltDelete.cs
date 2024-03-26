using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Timers; // Dodana using direktiva za System.Timers

namespace DesktopSecurityApp.Services
{
    public class BlockCtrlAltDelete
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int VK_DELETE = 0x2E;
        private const int VK_CONTROL = 0x11;
        private const int VK_MENU = 0x12;

        private static IntPtr hookId = IntPtr.Zero;
        private static System.Timers.Timer timer; // Eksplicitno navođenje System.Timers.Timer

        public static void StartBlocking()
        {
            hookId = SetHook(HookCallback);
            SetTopMost(true);

            // Create and start the timer
            timer = new System.Timers.Timer(); // Eksplicitno navođenje System.Timers.Timer
            timer.Interval = 500; // 500 milliseconds
            timer.Elapsed += Timer_Tick;
            timer.Start();
        }

        public static void StopBlocking()
        {
            UnhookWindowsHookEx(hookId);
            timer.Stop();
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (var curProcess = Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);

                // Check if the Delete key is pressed
                if (vkCode == VK_DELETE)
                {
                    // Check if Ctrl and Alt are pressed simultaneously
                    int ctrlState = GetKeyState(VK_CONTROL);
                    int altState = GetKeyState(VK_MENU);

                    // Block Ctrl + Alt + Delete if all three keys are pressed simultaneously
                    if ((ctrlState & 0x8000) != 0 && (altState & 0x8000) != 0)
                    {
                        return (IntPtr)1;
                    }
                }
            }
            return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        private static extern short GetKeyState(int nVirtKey);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;

        private static void Timer_Tick(object sender, ElapsedEventArgs e)
        {
            // Kill taskmgr.exe and procexp.exe processes
            KillProcess("taskmgr");
            KillProcess("procexp");
        }

        private static void KillProcess(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            foreach (Process process in processes)
            {
                process.Kill();
                process.WaitForExit(); // Wait for the process to exit
            }
        }

        private static void SetTopMost(bool topMost)
        {
            IntPtr handle = Process.GetCurrentProcess().MainWindowHandle;
            if (handle != IntPtr.Zero)
            {
                if (topMost)
                {
                    SetForegroundWindow(handle);
                    ShowWindow(handle, SW_SHOW);
                }
                else
                {
                    ShowWindow(handle, SW_HIDE);
                }
            }
        }
    }
}
