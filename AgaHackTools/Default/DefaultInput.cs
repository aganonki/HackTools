using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AgaHackTools.Main.Native;
using AgaHackTools.Main.Native.Structs;

namespace AgaHackTools.Main.Default
{
    public static class DefaultInput
    {
        public static void LeftMouseClick(int sleeptime = 25)
        {
            NativeMethods.mouse_event(MOUSEEVENTF.LEFTDOWN, 0, 0, 0, 0);
            Thread.Sleep(sleeptime);
            NativeMethods.mouse_event(MOUSEEVENTF.LEFTUP, 0, 0, 0, 0);
        }
        public static bool GetKeyDown(Keys key)
        {
            return Convert.ToBoolean(NativeMethods.GetKeyState((int)key) & NativeMethods.KEY_PRESSED);
        }
    }
}
