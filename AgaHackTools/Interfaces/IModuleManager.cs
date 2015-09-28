using System;

namespace AgaHackTools.Main.Interfaces
{
    interface IModuleManager
    {
        Type GetInstance(string filename, string @interface);
        T ActivateInstance<T>(Type instance);
        T ActivateInstance<T>(Type instance,params object[] args);

    }
}
