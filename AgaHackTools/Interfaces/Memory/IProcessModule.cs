using System.Diagnostics;

namespace AgaHackTools.Main.Interfaces
{
    public interface IProcessModule : ISmartPointer
    {
        ProcessModule ThisModule { get; }
        IProcessFunction FindFunction(string functionName);
        IProcessFunction this[string moduleName] { get; }
        int Size { get; }
        string Path { get; }
    }
}
