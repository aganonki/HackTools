using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using AgaHackTools.Main.Memory;

namespace AgaHackTools.Main.Interfaces
{
    public interface IMemory : IPointer
    {
        Process MainProcess { get; }
        IPattern Pattern { get; set; }
        bool IsRunning { get; }
        void Load();
    }
    public interface ISmartMemory : IMemory,ISmartPointer
    {
        IProcessModule this[string moduleName] { get; }
        IDictionary<string, IProcessModule> Modules { get; }
    }
    public interface IPointer : IDisposable
    {
        SafeMemoryHandle Handle { get; set; }
        bool Internal { get; set; }
        byte[] ReadBytes(IntPtr address, int count);
        T Read<T>(IntPtr address, bool isRelative = false) where T : struct;
        T ReadMultilevelPointer<T>(IntPtr address, bool isRelative = false, params IntPtr[] offsets) where T : struct;
        T[] ReadArray<T>(IntPtr address, int count, bool isRelative = false) where T : struct;
        string ReadString(IntPtr address, Encoding encoding, int maxLength = 256, bool isRelative = false);

        void Write(IntPtr address, byte[] data, bool isRelative = false);
        void WriteString(IntPtr address, string text, Encoding encoding, bool isRelative = false);
        void Write<T>(IntPtr address, T value, bool isRelative = false) where T : struct;
        void WriteArray<T>(IntPtr address, T[] array, bool isRelative = false) where T : struct;
    }

    public interface ISmartPointer : IPointer
    {
        bool ForceRelative { get; }
        IntPtr ImageBase { get; set; }
        T Read<T>(int address, bool isRelative = false) where T : struct;
        T ReadMultilevelPointer<T>(int address, bool isRelative = false, params int[] offsets) where T : struct;
        T[] ReadArray<T>(int address, int count, bool isRelative = false) where T : struct;
        string ReadString(int address, Encoding encoding, int maxLength = 256, bool isRelative = false);
        void Write(int address, byte[] data, bool isRelative = false);
        void WriteString(int address, string text, Encoding encoding, bool isRelative = false);
        void Write<T>(int address, T value, bool isRelative = false) where T : struct;
        void WriteArray<T>(int address, T[] array, bool isRelative = false) where T : struct;

        T Read<T>(long address, bool isRelative = false) where T : struct;
        T ReadMultilevelPointer<T>(long address, bool isRelative = false, params int[] offsets) where T : struct;
        T[] ReadArray<T>(long address, int count, bool isRelative = false) where T : struct;
        string ReadString(long address, Encoding encoding, int maxLength = 256, bool isRelative = false);
        void Write(long address, byte[] data, bool isRelative = false);
        void WriteString(long address, string text, Encoding encoding, bool isRelative = false);
        void Write<T>(long address, T value, bool isRelative = false) where T : struct;
        void WriteArray<T>(long address, T[] array, bool isRelative = false) where T : struct;
    }
}
