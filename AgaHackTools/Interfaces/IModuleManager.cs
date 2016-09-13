using System;

namespace AgaHackTools.Main.Interfaces
{
    public interface IModuleManager
    {
        Type GetInstance(string path,string filename, string @interface);
        T ActivateInstance<T>(Type instance);
        T ActivateInstance<T>(Type instance,params object[] args);

    }
}
