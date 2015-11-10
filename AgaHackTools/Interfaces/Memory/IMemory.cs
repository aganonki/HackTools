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
        T Read<T>(object address, bool isRelative = false) where T : struct;
        T ReadMultilevelPointer<T>(object address, bool isRelative = false, params object[] offsets) where T : struct;
        T[] ReadArray<T>(object address, int count, bool isRelative = false) where T : struct;
        string ReadString(object address, Encoding encoding, int maxLength = 256, bool isRelative = false);
        void Write(object address, byte[] data, bool isRelative = false);
        void WriteString(object address, string text, Encoding encoding, bool isRelative = false);
        void Write<T>(object address, T value, bool isRelative = false) where T : struct;
        void WriteArray<T>(object address, T[] array, bool isRelative = false) where T : struct;
    }
}
