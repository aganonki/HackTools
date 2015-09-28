using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgaHackTools.Interfaces
{
    interface IModuleManager
    {
        Type GetInstance(string filename, string @interface);
        T ActivateInstance<T>(Type instance) where T : new();
        T ActivateInstance<T>(Type instance,params object[] args) where T : new();

    }
}
