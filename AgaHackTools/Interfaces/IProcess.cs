using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgaHackTools.Main.Interfaces
{
    public interface IProcess
    {
        ILog Logger { get; set; }
        bool IsRunning { get; }
        void Load();
        void Start();
        void Stop();
        IConfigurationManager ConfigurationManager { get; set; }
        //List<IModule> Modules { get; set; }
    }
}
