using System;
using System.Runtime.InteropServices;

namespace AgaHackTools.Main.Memory
{
    public static class MemoryUtilities
    {
        public static IntPtr ToIntPtr(this int address) => new IntPtr(address);
        public static IntPtr ToIntPtr(this uint address) => new IntPtr(address);
        public static IntPtr ToIntPtr(this long address) => new IntPtr(address);

        public static IntPtr GetFunctionPointer(this Delegate @delegate)
        {
            return Marshal.GetFunctionPointerForDelegate(@delegate);
        }

        //public static IntPtr ToIntPtr(this object address)
        //{
        //    var obj = address;
        //    if (Equals(obj, null))
        //    {
        //        return IntPtr.Zero;
        //    }
        //    long number = 0;
        //    Type objType = obj.GetType();
        //    objType = Nullable.GetUnderlyingType(objType) ?? objType;

        //    if (objType.IsPrimitive)
        //    {
        //        if (objType != typeof (bool) &&
        //            objType != typeof (char) )
        //            //&&objType != typeof (IntPtr) &&
        //            //objType != typeof (UIntPtr))
        //        {
        //            long.TryParse(string.Format("{0}", obj),out number);
        //            //number = (Int64) address;
        //        }
        //    }



        //    //            if(address.IsNumber())

        //    return new IntPtr(number);
        //}

        //public static bool IsNumber(this object obj)
        //{
        //    if (Equals(obj, null))
        //    {
        //        return false;
        //    }

        //    Type objType = obj.GetType();
        //    objType = Nullable.GetUnderlyingType(objType) ?? objType;

        //    if (objType.IsPrimitive)
        //    {
        //        return objType != typeof(bool) &&
        //            objType != typeof(char) &&
        //            objType != typeof(IntPtr) &&
        //            objType != typeof(UIntPtr);
        //    }

        //    return objType == typeof(decimal);
        //}
    }
}
