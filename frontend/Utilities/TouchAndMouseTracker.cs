using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using Point = System.Windows.Point;

namespace Lastik.Utilities;

public class TouchAndMouseTracker : IDisposable
{
    private readonly IntPtr _mouseHookId = IntPtr.Zero;
    private LowLevelMouseProc _mouseHookCallback;
    private Point? _lastInputPosition;
    private readonly IntPtr _hwnd;
    private readonly HwndSource? _hwndSource;
    private readonly bool _isWindows8OrNewer;

    public TouchAndMouseTracker(Window window)
    {
        // Проверка версии Windows
        _isWindows8OrNewer = Environment.OSVersion.Version.Major >= 6 && Environment.OSVersion.Version.Minor >= 2;

        // Настройка мыши
        _mouseHookCallback = MouseHookCallback;
        _mouseHookId = SetMouseHook(_mouseHookCallback);

        // Настройка сенсора
        _hwnd = new WindowInteropHelper(window).Handle;
        _hwndSource = HwndSource.FromHwnd(_hwnd);
        _hwndSource?.AddHook(WndProc);
        RegisterTouchWindow(_hwnd, 0);
    }

    public Point? GetLastInputPosition()
    {
        return _lastInputPosition;
    }

    private IntPtr SetMouseHook(LowLevelMouseProc proc)
    {
        using var curProcess = Process.GetCurrentProcess();
        using var curModule = curProcess.MainModule;
        return SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
    }

    private IntPtr MouseHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && wParam == (IntPtr)WM_LBUTTONDOWN)
        {
            MSLLHOOKSTRUCT hookStruct = Marshal.PtrToStructure<MSLLHOOKSTRUCT>(lParam);
            _lastInputPosition = new Point(hookStruct.pt.x, hookStruct.pt.y);
        }
        return CallNextHookEx(_mouseHookId, nCode, wParam, lParam);
    }

    private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        if (msg == WM_TOUCH)
        {
            HandleTouch(lParam);
        }
        return IntPtr.Zero;
    }

    private void HandleTouch(IntPtr lParam)
    {
        uint touchInputCount = (uint)(lParam.ToInt32() & 0xFFFF);
        var inputs = new TOUCHINPUT[touchInputCount];
        int inputSize = Marshal.SizeOf<TOUCHINPUT>();

        if (GetTouchInputInfo(lParam, touchInputCount, inputs, inputSize))
        {
            foreach (var input in inputs)
            {
                if ((input.dwFlags & TOUCHEVENTF_DOWN) != 0)
                {
                    // Конвертация координат в пиксели
                    var point = new Point(input.x / 100.0, input.y / 100.0);
                    _lastInputPosition = point;
                }
            }
            CloseTouchInputHandle(lParam);
        }
    }

    // Определения для мыши
    private const int WH_MOUSE_LL = 14;
    private const int WM_LBUTTONDOWN = 0x0201;

    [StructLayout(LayoutKind.Sequential)]
    private struct POINT
    {
        public int x;
        public int y;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MSLLHOOKSTRUCT
    {
        public POINT pt;
        public uint mouseData;
        public uint flags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    // Определения для сенсора
    private const int WM_TOUCH = 0x0240;
    private const int TOUCHEVENTF_DOWN = 0x0001;

    [StructLayout(LayoutKind.Sequential)]
    private struct TOUCHINPUT
    {
        public int x;
        public int y;
        public IntPtr hSource;
        public uint dwID;
        public uint dwFlags;
        public uint dwMask;
        public uint dwTime;
        public IntPtr dwExtraInfo;
        public uint cxContact;
        public uint cyContact;
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool RegisterTouchWindow(IntPtr hWnd, uint ulFlags);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool GetTouchInputInfo(IntPtr hTouchInput, uint cInputs, [In, Out] TOUCHINPUT[] pInputs, int cbSize);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern void CloseTouchInputHandle(IntPtr lParam);

    // UnregisterTouchWindow доступен только в Windows 8 и новее
    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool UnregisterTouchWindow(IntPtr hWnd);

    private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

    public void Dispose()
    {
        if (_mouseHookId != IntPtr.Zero)
        {
            UnhookWindowsHookEx(_mouseHookId);
        }
        if (_hwndSource != null)
        {
            _hwndSource.RemoveHook(WndProc);
            if (_isWindows8OrNewer)
            {
                UnregisterTouchWindow(_hwnd);
            }
        }
    }
}