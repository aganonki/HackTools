using AgaHackTools.Main.Interfaces;
using System;
using System.Runtime.InteropServices;

namespace AgaHackTools.Memory
{
    public class Function : IProcessFunction
    {
        #region Constructors

        public Function(IntPtr address, string name)
        {
            BaseAddress = address;
            Name = name;
        }

        #endregion

        #region Interface members

        public T GetDelegate<T>()
        {
           return Marshal.GetDelegateForFunctionPointer<T>(BaseAddress);
        }

        
        #endregion

        #region Properties

        public IntPtr BaseAddress { get; }
        public string Name { get; }

        #endregion
    }
}
