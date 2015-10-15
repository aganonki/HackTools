using AgaHackTools.Main.Interfaces;
using System;
using System.Runtime.InteropServices;

namespace AgaHackTools.Native
{
    public class VirtualClass
    {
        #region Constructors

        //private IMemory mem;

        public VirtualClass(IntPtr address)
        {
            ClassAddress = address;
        }

        #endregion

        #region Properties

        public IntPtr ClassAddress { get; private set; }

        #endregion

        #region Public methods

        public T GetVirtualFunction<T>(int functionIndex) where T : class
        {
            return Marshal.GetDelegateForFunctionPointer<T>(GetObjectVtableFunction(ClassAddress, functionIndex));
        }

        public IntPtr GetObjectVtableFunction(IntPtr objectBase, int functionIndex)
        {
            //if (mem.Internal)
               return objectBase.VTable(functionIndex);
            //return mem.ReadMultilevelPointer<IntPtr>(objectBase,false, new IntPtr(functionIndex * 4));
        }

        #endregion
    }
    public static class VTableHelper{
        #region Public methods

        public static unsafe IntPtr VTable(this IntPtr addr, int index)
        {
            var pAddr = (void***)addr.ToPointer();
            return new IntPtr((*pAddr)[index]);
        }

        #endregion
    }
}
