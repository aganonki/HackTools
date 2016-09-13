using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AgaHackTools.Main.Native;
using AgaHackTools.Main.Native.Structs;

namespace AgaHackTools.Example.Shared
{
    public class KeyUtils
    {
        #region STATIC METHODS
        public static bool GetKeyDown(VirtualKeyShort key)
        {
            return GetKeyDown((Int32)key);
        }

        public static void LMouseClick(int sleeptime, int count)
        {
            for (var i = 0; i < count; i++)
            {
                LMouseClick(10);
                Thread.Sleep(sleeptime);
            }
        }

        public static void LMouseClick(int sleeptime)
        {
            NativeMethods.mouse_event(MOUSEEVENTF.LEFTDOWN, 0, 0, 0, 0);
            Thread.Sleep(sleeptime);
            NativeMethods.mouse_event(MOUSEEVENTF.LEFTUP, 0, 0, 0, 0);
        }
        public static bool GetKeyDown(int key)
        {
            return Convert.ToBoolean(NativeMethods.GetKeyState(key) & NativeMethods.KEY_PRESSED);
        }
        public static bool GetKeyDownAsync(int key)
        {
            return GetKeyDownAsync((VirtualKeyShort)key);
        }
        public static bool GetKeyDownAsync(VirtualKeyShort key)
        {
            return Convert.ToBoolean(NativeMethods.GetAsyncKeyState(key) & NativeMethods.KEY_PRESSED);
        }
        #endregion
    }
}
