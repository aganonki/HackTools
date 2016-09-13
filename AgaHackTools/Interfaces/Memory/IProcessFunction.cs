using System;

namespace AgaHackTools.Main.Interfaces
{
    public interface IProcessFunction
    {
        IntPtr BaseAddress { get; }
        string Name { get; }
        T GetDelegate<T>();
    }
}
