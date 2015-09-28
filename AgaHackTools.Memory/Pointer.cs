using System;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using AgaHackTools.Main.Interfaces;
using AgaHackTools.Main.Memory;
using AgaHackTools.Main.Native;

namespace AgaHackTools.Memory
{
    public unsafe class Pointer : ISmartPointer
    {
        public Pointer(SafeMemoryHandle handle,IntPtr baseAddress)
        {
            Handle = handle;
            ImageBase = baseAddress;
            ForceRelative = baseAddress != null;
        }
        public Pointer(Process proc)
        {
            Handle = new SafeMemoryHandle(proc.MainWindowHandle);
            ForceRelative = ImageBase != null;
        }
     
        public SafeMemoryHandle Handle { get; set; }

        public bool ForceRelative{ get; set; }

        #region IMemory methods
        /// <summary> Reads a value from the specified address in memory. </summary>
        /// <remarks> Created 3/24/2012. </remarks>
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="address"> The address. </param>
        /// <returns> . </returns>
        public T Read<T>(IntPtr address, bool isRelative = false) where T : struct
        {
            if (isRelative||ForceRelative)
                address = GetAbsolute(address);
            return InternalRead<T>(address);
        }

        /// <summary> Reads a value from the specified address in memory. This method is used for multi-pointer dereferencing.</summary>
        /// <remarks> Created 3/24/2012. </remarks>
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="addresses"> A variable-length parameters list containing addresses. </param>
        /// <returns> . </returns>
        public T ReadMultilevelPointer<T>(IntPtr address, bool isRelative = false, params IntPtr[] addresses) where T : struct
        {
            if (isRelative || ForceRelative)
                address = GetAbsolute(address);
            if (addresses.Length == 0)
            {
                throw new InvalidOperationException("Cannot read a value from unspecified addresses.");
            }

            var temp = Read<IntPtr>(address);

            for (int i = 0; i < addresses.Length - 1; i++)
            {
                temp = Read<IntPtr>(temp + (int)addresses[i]);
            }
            return Read<T>(temp + (int)addresses[addresses.Length - 1]);
        }

        /// <summary> Reads an array of values from the specified address in memory. </summary>
        /// <remarks> Created 3/24/2012. </remarks>
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="address"> The address. </param>
        /// <param name="count"> Number of. </param>
        public T[] ReadArray<T>(IntPtr address, int count, bool isRelative = false) where T : struct
        {
            if (isRelative||ForceRelative)
                address = GetAbsolute(address);
            int size = SizeCache<T>.Size;
            var ret = new T[count];
            for (int i = 0; i < count; i++)
            {
                ret[i] = Read<T>(address + (i * size));
            }
            return ret;
        }

        public string ReadString(IntPtr address, Encoding encoding, int maxLength = 256, bool isRelative = false)
        {
            if (isRelative||ForceRelative)
                address = GetAbsolute(address);
            var data = ReadByte(address, maxLength);
            var text = encoding.GetChars(data).ToString();
            if (text.Contains("\0"))
                text = text.Substring(0, text.IndexOf('\0'));
            return text;

        }
        /// <summary> Writes bytes to the address in memory. </summary>
        /// <remarks> Created 3/24/2012. </remarks>
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="address"> The address. </param>
        /// <param name="value"> The value. </param>
        /// <returns> true if it succeeds, false if it fails. </returns>
        public void Write(IntPtr address, byte[] value, bool isRelative = false)
        {
            if (isRelative||ForceRelative)
                address = GetAbsolute(address);
            WriteBytes(address, value);
        }
        /// <summary> Writes a value specified to the address in memory. </summary>
        /// <remarks> Created 3/24/2012. </remarks>
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="address"> The address. </param>
        /// <param name="value"> The value. </param>
        /// <returns> true if it succeeds, false if it fails. </returns>
        public void Write<T>(IntPtr address, T value, bool isRelative = false) where T : struct
        {
            if (isRelative||ForceRelative)
                address = GetAbsolute(address);
            Marshal.StructureToPtr(value, address, false);
        }

        public void WriteString(IntPtr address, string text, Encoding encoding, bool isRelative = false)
        {
            if (isRelative||ForceRelative)
                address = GetAbsolute(address);
            WriteBytes(address, encoding.GetBytes(text));
        }

        /// <summary> Writes an array of values to the address in memory. </summary>
        /// <remarks> Created 3/24/2012. </remarks>
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="address"> The address. </param>
        /// <param name="value"> The value. </param>
        /// <returns> true if it succeeds, false if it fails. </returns>
        public void WriteArray<T>(IntPtr address, T[] array, bool isRelative = false) where T : struct
        {
            if (isRelative||ForceRelative)
                address = GetAbsolute(address);
            int size = SizeCache<T>.Size;
            for (int i = 0; i < array.Length; i++)
            {
                T val = array[i];
                Write(address + (i * size), val);
            }
        }
        #endregion

        #region ISmartMemory methods

        public IntPtr ImageBase;
        public T Read<T>(object address, bool isRelative = false) where T : struct
            => Read<T>(address.ToIntPtr(), isRelative);


        public T ReadMultilevelPointer<T>(object address, bool isRelative = false, params object[] offsets) where T : struct
        {
            var temp = Read<IntPtr>(address.ToIntPtr(), isRelative);

            for (int i = 0; i < offsets.Length - 1; i++)
            {
                temp = Read<IntPtr>(temp + (int)offsets[i]);
            }
            return Read<T>(temp + (int)offsets[offsets.Length - 1]);
        }

        public T[] ReadArray<T>(object address, int count, bool isRelative = false) where T : struct
        => ReadArray<T>(address.ToIntPtr(), count, isRelative);


        public string ReadString(object address, Encoding encoding, int maxLength = 256, bool isRelative = false)
        => ReadString(address.ToIntPtr(), encoding, maxLength, isRelative);


        public void Write(object address, byte[] data, bool isRelative = false)
        => Write(address.ToIntPtr(), data, isRelative);


        public void WriteString(object address, string text, Encoding encoding, bool isRelative = false)
        => WriteString(address.ToIntPtr(), text, encoding, isRelative);


        public void Write<T>(object address, T value, bool isRelative = false) where T : struct
        => Write(address.ToIntPtr(), value, isRelative);


        public void WriteArray<T>(object address, T[] array, bool isRelative = false) where T : struct
        => WriteArray(address.ToIntPtr(), array, isRelative);

        #endregion

        #region Protected methods
        protected byte[] ReadByte(IntPtr address, int count)
        {
            var ret = new byte[count];
            var ptr = (byte*)address;
            for (int i = 0; i < count; i++)
            {
                ret[i] = ptr[i];
            }
            return ret;
        }

        [HandleProcessCorruptedStateExceptions]
        private T InternalRead<T>(IntPtr address)
        {
            try
            {
                if (address == IntPtr.Zero)
                {
                    throw new InvalidOperationException("Cannot retrieve a value at address 0");
                }

                object ret;
                switch (SizeCache<T>.TypeCode)
                {
                    case TypeCode.Object:

                        if (SizeCache<T>.IsIntPtr)
                        {
                            return (T)(object)*(IntPtr*)address;
                        }

                        // If the type doesn't require an explicit Marshal call, then ignore it and memcpy the fuckin thing.
                        if (!SizeCache<T>.TypeRequiresMarshal)
                        {
                            T o = default(T);
                            void* ptr = SizeCache<T>.GetUnsafePtr(ref o);

                            NativeMethods.MoveMemory(ptr, (void*)address, SizeCache<T>.Size);

                            return o;
                        }

                        // All System.Object's require marshaling!
                        ret = Marshal.PtrToStructure(address, typeof(T));
                        break;
                    case TypeCode.Boolean:
                        ret = *(byte*)address != 0;
                        break;
                    case TypeCode.Char:
                        ret = *(char*)address;
                        break;
                    case TypeCode.SByte:
                        ret = *(sbyte*)address;
                        break;
                    case TypeCode.Byte:
                        ret = *(byte*)address;
                        break;
                    case TypeCode.Int16:
                        ret = *(short*)address;
                        break;
                    case TypeCode.UInt16:
                        ret = *(ushort*)address;
                        break;
                    case TypeCode.Int32:
                        ret = *(int*)address;
                        break;
                    case TypeCode.UInt32:
                        ret = *(uint*)address;
                        break;
                    case TypeCode.Int64:
                        ret = *(long*)address;
                        break;
                    case TypeCode.UInt64:
                        ret = *(ulong*)address;
                        break;
                    case TypeCode.Single:
                        ret = *(float*)address;
                        break;
                    case TypeCode.Double:
                        ret = *(double*)address;
                        break;
                    case TypeCode.Decimal:
                        // Probably safe to remove this. I'm unaware of anything that actually uses "decimal" that would require memory reading...
                        ret = *(decimal*)address;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                return (T)ret;
            }
            catch (AccessViolationException ex)
            {
                Trace.WriteLine("Access Violation on " + address + " with type " + typeof(T).Name);
                return default(T);
            }
        }

        protected void WriteBytes(IntPtr address, byte[] bytes)
        {
            using (new MemoryProtection(address, bytes.Length))
            {
                var ptr = (byte*)address;
                for (int i = 0; i < bytes.Length; i++)
                {
                    ptr[i] = bytes[i];
                }
            }
        }

        /// <summary>
        /// Gets the absolute.
        /// </summary>
        /// <param name="relative">The relative.</param>
        /// <returns></returns>
        /// <remarks>Created 2012-01-16 19:41</remarks>
        public IntPtr GetAbsolute(IntPtr relative)
        {
            return ImageBase + (int)relative;
        }
        #endregion

        #region Public Events

        /// <summary>
        ///     Raises when the object is disposed.
        /// </summary>
        public virtual event EventHandler OnDispose;

        #endregion

        /// <summary>
        ///     Releases all resources used by the object.
        /// </summary>
        public virtual void Dispose()
        {
            // Raise the event OnDispose
            OnDispose?.Invoke(this, new EventArgs());

            //// Close the process handle
            //Handle.Close();

            // Avoid the finalizer
            GC.SuppressFinalize(this);
        }
    }
}
