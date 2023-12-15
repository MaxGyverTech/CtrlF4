using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

class Program
{
    const int MOD_CONTROL = 0x0002;
    const int MOD_NOREPEAT = 0x4000;
    const int WM_HOTKEY = 0x0312;
    const uint VK_F4 = 0x73;

    static void Main()
    {
        if (RegisterHotKey(IntPtr.Zero, 1, MOD_CONTROL | MOD_NOREPEAT, VK_F4))
            Console.WriteLine("Hotkey 'ctrl+f4' registered");

        MSG msg = new MSG();
        while (GetMessage(ref msg, IntPtr.Zero, 0, 0) != 0)
            if (msg.message == WM_HOTKEY)
            {
                Console.WriteLine("WM_HOTKEY received");
                Process.GetProcessById((int)GetFocusedWindowPID()).Kill();
            }
        UnregisterHotKey(IntPtr.Zero, 1);
    }

    // Get focused window
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.U4)]
    static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();

    public static uint GetFocusedWindowPID()
    {
        uint pid = 0;
        GetWindowThreadProcessId(GetForegroundWindow(), out pid);
        return pid;
    }
    ////////////////////////


    // Hotkeys hook
    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    [StructLayout(LayoutKind.Sequential)]
    struct MSG
    {
        public IntPtr hwnd;
        public uint message;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public POINT pt;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct POINT
    {
        public int x;
        public int y;
    }

    [DllImport("user32.dll", SetLastError = true)]
    static extern int GetMessage(ref MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);
    //////////////////////

}
