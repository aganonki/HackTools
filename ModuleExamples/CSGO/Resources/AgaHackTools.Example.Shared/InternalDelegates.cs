using System;
using System.Runtime.InteropServices;

namespace AgaHackTools.Example.Shared
{
    public static class InternalDelegates
    {
        public const string IEngineTraceVTable = "EngineTraceClient004";
        public unsafe delegate IntPtr CreateInterfaceFn([MarshalAs(UnmanagedType.LPStr)] string pName,ref int pReturnCode);
    }
}
