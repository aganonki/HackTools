﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgaHackTools.Memory;

namespace AgaHackTools.Interfaces
{
    public interface IMemory : IPointer
    {
        Process MainProcess { get; }
        ISmartPointer this[string moduleName] { get; }
        IPattern Pattern { get; set; }
        bool IsRunning { get; }       
    }
    public interface IPointer : IDisposable
    {
        SafeMemoryHandle Handle { get; set; }

        T Read<T>(IntPtr address, bool isRelative = false) where T : struct;
        T Read<T>(IntPtr address, bool isRelative = false, params IntPtr[] offsets) where T : struct;
        T[] Read<T>(IntPtr address, int count, bool isRelative = false) where T : struct;
        string ReadString(IntPtr address, Encoding encoding, int maxLength = 256, bool isRelative = false);

        void Write(IntPtr address, byte[] data, bool isRelative = false);
        void WriteString(IntPtr address, string text, Encoding encoding, bool isRelative = false);
        void Write<T>(IntPtr address, T value, bool isRelative = false) where T : struct;
        void Write<T>(IntPtr address, T[] array, bool isRelative = false) where T : struct;
    }

    public interface ISmartPointer : IPointer
    {
        bool ForceRelative { get; set; }
        T Read<T>(object address, bool isRelative = false) where T : struct;
        T Read<T>(object address, bool isRelative = false, params object[] offsets) where T : struct;
        T[] Read<T>(object address, int count, bool isRelative = false) where T : struct;
        string ReadString(object address, Encoding encoding, int maxLength = 256, bool isRelative = false);
        void Write(object address, byte[] data, bool isRelative = false);
        void WriteString(object address, string text, Encoding encoding, bool isRelative = false);
        void Write<T>(object address, T value, bool isRelative = false) where T : struct;
        void Write<T>(object address, T[] array, bool isRelative = false) where T : struct;
    }
}
