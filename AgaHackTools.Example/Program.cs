using AgaHackTools.Main;
using AgaHackTools.Main.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AgaHackTools.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var csgoProcess = new CSGOProcess();
            csgoProcess.Load();
            csgoProcess.Start();
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
