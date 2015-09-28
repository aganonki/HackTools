using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AgaHackTools.Memory
{
    public static class MemoryUtilities
    {
        public static IntPtr ToIntPtr(this object address) => new IntPtr(Convert.ToInt64(address));
    }
}
