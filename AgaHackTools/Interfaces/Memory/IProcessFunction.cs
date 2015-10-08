using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgaHackTools.Main.Interfaces
{
    public interface IProcessFunction
    {
        IntPtr BaseAddress { get; }
        string Name { get; }
        T GetDelegate<T>(T value);
    }
}
