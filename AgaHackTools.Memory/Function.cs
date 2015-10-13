using AgaHackTools.Main.Interfaces;
using System;
using System.Runtime.InteropServices;

namespace AgaHackTools.Memory
{
    public class Function : IProcessFunction
    {
        public Function(IntPtr address, string name)
        {
            BaseAddress = address;
            Name = name;
        }
        public IntPtr BaseAddress { get; }
        public string Name { get; }
        public T GetDelegate<T>()
        {
           return Marshal.GetDelegateForFunctionPointer<T>(BaseAddress);
        }

    }
}
