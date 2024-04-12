 using System.Runtime.InteropServices;
using System.Windows.Forms;
using System;
using System.Reflection.Emit;
/// <summary>
///监控键盘钩子
/// </summary>
public class KeyboardHook
{
    private const int WM_KEYDOWN = 0x100;
    private const int WM_KEYUP = 0x101;
    private const int WM_SYSKEYDOWN = 0x104;
    private const int WM_SYSKEYUP = 0x105;

    //全局事件  
    public event KeyEventHandler OnKeyDownEvent;
    public event KeyEventHandler OnKeyUpEvent;
    public event KeyPressEventHandler OnKeyPressEvent;
    public event Action<string> OnCopyText;
    static int hKeyboardHook = 0;

    //鼠标常量  
    public const int WH_KEYBOARD_LL = 13;

    public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);

    //声明键盘钩子事件类型  
    HookProc KeyboardHookProcedure;

    /// <summary>  
    /// 声明键盘钩子的封送结构类型  
    /// </summary>  
    [StructLayout(LayoutKind.Sequential)]
    public class KeyboardHookStruct
    {
        public int vkCode;//表示一个1到254间的虚拟键盘码  
        public int scanCode;//表示硬件扫描码  
        public int flags;
        public int time;
        public int dwExtraInfo;
    }
    [DllImport("kernel32.dll")]
    public static extern IntPtr GetModuleHandle(string lpModuleName);

    //安装钩子  
    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);
    //下一个钩子  
    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    public static extern int CallNextHookEx(int idHook, int nCode, Int32 wParam, IntPtr lParam);
    //卸载钩子  
    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    public static extern bool UnhookWindowsHookEx(int idHook);



    private const int VK_CONTROL = 0x11;
    private const int VK_C = 0x43;




    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GlobalUnlock(IntPtr hMem);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern short GetKeyState(int nVirtKey);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool OpenClipboard(IntPtr hWndNewOwner);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CloseClipboard();

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetClipboardData(uint uFormat);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GlobalLock(IntPtr hMem);

    private static bool IsCtrlKeyPressed()
    {
        return (GetKeyState(VK_CONTROL) & 0x8000) != 0;
    }
    private static string GetClipboardText()
    {
        string clipboardText = string.Empty;

        if (OpenClipboard(IntPtr.Zero))
        {
            IntPtr clipboardData = GetClipboardData(13); // CF_UNICODETEXT

            if (clipboardData != IntPtr.Zero)
            {
                IntPtr globalLock = GlobalLock(clipboardData);

                if (globalLock != IntPtr.Zero)
                {
                    clipboardText = Marshal.PtrToStringUni(globalLock);

                    GlobalUnlock(clipboardData);
                }
            }

            CloseClipboard();
        }

        return clipboardText;
    }

    private int KeyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)
    {
        if ((nCode >= 0) && (OnKeyDownEvent != null || OnKeyUpEvent != null || OnKeyPressEvent != null))
        {
            int vkCode = Marshal.ReadInt32(lParam);
            if (vkCode == VK_C && IsCtrlKeyPressed())
            {
                Console.WriteLine("\nCtrl+C 被按下.");

                // 获取剪贴板中的文本
                string clipboardText = GetClipboardText();
                if (OnCopyText != null)
                {
                    OnCopyText.Invoke(clipboardText);
                }
                // 输出剪贴板中的文本
                Console.WriteLine("剪贴板中的文本: " + clipboardText);
            }


            KeyboardHookStruct MyKBHookStruct = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));

            //引发OnKeyDownEvent  
            if (OnKeyDownEvent != null && (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN))
            {
                Keys keyData = (Keys)MyKBHookStruct.vkCode;
                KeyEventArgs e = new KeyEventArgs(keyData);
                OnKeyDownEvent(this, e);
            }
        }
        return CallNextHookEx(hKeyboardHook, nCode, wParam, lParam);
    }

    public void Start()
    {
        if (hKeyboardHook == 0)
        {
            KeyboardHookProcedure = new HookProc(KeyboardHookProc);
            using (System.Diagnostics.Process curProcess = System.Diagnostics.Process.GetCurrentProcess())
            using (System.Diagnostics.ProcessModule curModule = curProcess.MainModule)
                hKeyboardHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardHookProcedure, GetModuleHandle(curModule.ModuleName), 0);

            if (hKeyboardHook == 0)
            {
                Stop();
                throw new Exception("Set GlobalKeyboardHook failed!");
            }
        }
    }

    public void Stop()
    {
        bool retKeyboard = true;
        if (hKeyboardHook != 0)
        {
            retKeyboard = UnhookWindowsHookEx(hKeyboardHook);
            hKeyboardHook = 0;
        }
        if (!retKeyboard)
            throw new Exception("Unload GlobalKeyboardHook failed!");
    }

    //构造函数中安装钩子  
    public KeyboardHook()
    {
    }
    //析构函数中卸载钩子  
    ~KeyboardHook()
    {
        Stop();
    }
}

/// <summary>
/// 监控鼠标钩子
/// </summary>
public class MouseHook
{
    private const int WM_MOUSEMOVE = 0x200;
    private const int WM_LBUTTONDOWN = 0x201;
    private const int WM_RBUTTONDOWN = 0x204;
    private const int WM_MBUTTONDOWN = 0x207;
    private const int WM_LBUTTONUP = 0x202;
    private const int WM_RBUTTONUP = 0x205;
    private const int WM_MBUTTONUP = 0x208;
    private const int WM_LBUTTONDBLCLK = 0x203;
    private const int WM_RBUTTONDBLCLK = 0x206;
    private const int WM_MBUTTONDBLCLK = 0x209;

    //全局的事件    
    public event MouseEventHandler OnMouseActivity;

    static int hMouseHook = 0; //鼠标钩子句柄    

    //鼠标常量    
    public const int WH_MOUSE_LL = 14; //mouse hook constant    

    HookProc MouseHookProcedure; //声明鼠标钩子事件类型.    

    //声明一个Point的封送类型    
    [StructLayout(LayoutKind.Sequential)]
    public class POINT
    {
        public int x;
        public int y;
    }

    //声明鼠标钩子的封送结构类型    
    [StructLayout(LayoutKind.Sequential)]
    public class MouseHookStruct
    {
        public POINT pt;
        public int hWnd;
        public int wHitTestCode;
        public int dwExtraInfo;
    }
    [DllImport("kernel32.dll")]
    public static extern IntPtr GetModuleHandle(string lpModuleName);
    //装置钩子的函数    
    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

    //卸下钩子的函数    
    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    public static extern bool UnhookWindowsHookEx(int idHook);

    //下一个钩挂的函数    
    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    public static extern int CallNextHookEx(int idHook, int nCode, Int32 wParam, IntPtr lParam);

    public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);

    /// <summary>    
    /// 墨认的构造函数构造当前类的实例.    
    /// </summary>    
    public MouseHook()
    {
    }

    //析构函数.    
    ~MouseHook()
    {
        Stop();
    }

    public void Start()
    {
        //安装鼠标钩子    
        if (hMouseHook == 0)
        {
            //生成一个HookProc的实例.    
            MouseHookProcedure = new HookProc(MouseHookProc);
            using (System.Diagnostics.Process curProcess = System.Diagnostics.Process.GetCurrentProcess())
            using (System.Diagnostics.ProcessModule curModule = curProcess.MainModule)
                hMouseHook = SetWindowsHookEx(WH_MOUSE_LL, MouseHookProcedure, GetModuleHandle(curModule.ModuleName), 0);

            //如果装置失败停止钩子    
            if (hMouseHook == 0)
            {
                Stop();
                throw new Exception("SetWindowsHookEx failed.");
            }
        }
    }

    public void Stop()
    {
        bool retMouse = true;
        if (hMouseHook != 0)
        {
            retMouse = UnhookWindowsHookEx(hMouseHook);
            hMouseHook = 0;
        }

        //如果卸下钩子失败    
        if (!(retMouse))
        {
            MessageBox.Show("UnhookWindowsHookEx failed.");
        }
    }

    private int MouseHookProc(int nCode, Int32 wParam, IntPtr lParam)
    {
        //如果正常运行并且用户要监听鼠标的消息    
        if ((nCode >= 0) && (OnMouseActivity != null))
        {
            MouseButtons button = MouseButtons.None;
            int clickCount = 0;
            int delta = 0;
            switch (wParam)
            {
                case WM_LBUTTONDOWN:
                    button = MouseButtons.Left;
                    clickCount = 1;
                    delta = 1;
                    break;
                case WM_LBUTTONUP:
                    button = MouseButtons.Left;
                    clickCount = 1;
                    break;
                case WM_LBUTTONDBLCLK:
                    button = MouseButtons.Left;
                    clickCount = 2;
                    break;
                case WM_RBUTTONDOWN:
                    button = MouseButtons.Right;
                    clickCount = 1;
                    break;
                case WM_RBUTTONUP:
                    button = MouseButtons.Right;
                    clickCount = 1;
                    break;
                case WM_RBUTTONDBLCLK:
                    button = MouseButtons.Right;
                    clickCount = 2;
                    break;
            }

            //从回调函数中得到鼠标的信息    
            MouseHookStruct MyMouseHookStruct = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));
            MouseEventArgs e = new MouseEventArgs(button, clickCount, MyMouseHookStruct.pt.x, MyMouseHookStruct.pt.y, delta);

            OnMouseActivity(this, e);
        }
        return CallNextHookEx(hMouseHook, nCode, wParam, lParam);
    }
}
