using AgaHackTools.Main;
using AgaHackTools.Main.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgaHackTools.Example
{
    class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AllocConsole();
        [STAThread]
        static void Main(string[] args)
        {
            AllocConsole();
            var csgoProcess = new CSGOProcess();
            csgoProcess.Load();
            csgoProcess.Start();
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
