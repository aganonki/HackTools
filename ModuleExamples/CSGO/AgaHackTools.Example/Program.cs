using AgaHackTools.Main;
using AgaHackTools.Main.Interfaces;
using CSGO.UI.ModuleManager.Metro;
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
            try
            {
                AllocConsole();
                Console.WriteLine("Main funct!");
                var csgoProcess = new CSGOProcess();
                csgoProcess.Load();

                //var uithread = new Thread(start);
                //uithread.Start();
                csgoProcess.Start();
                start();
            }
            catch (Exception e)
            {
                Console.WriteLine( e);
            }

            Thread.Sleep(Timeout.Infinite);
        }
        static void start()
        {
            CSGO.UI.ModuleManager.Metro.App.Main();
        }
    }
}
