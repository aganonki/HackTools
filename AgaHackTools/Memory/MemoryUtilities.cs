using System;

namespace AgaHackTools.Main.Memory
{
    public static class MemoryUtilities
    {
        public static IntPtr ToIntPtr(this object address) => new IntPtr(Convert.ToInt64(address));
    }
}
