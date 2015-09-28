using System;

namespace AgaHackTools.Main.Interfaces
{
    interface IModuleManager
    {
        Type GetInstance(string filename, string @interface);
        T ActivateInstance<T>(Type instance) where T : new();
        T ActivateInstance<T>(Type instance,params object[] args) where T : new();

    }
}
