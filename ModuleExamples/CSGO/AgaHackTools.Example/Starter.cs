using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AgaHackTools.Example;
using System.Threading;

namespace Managed
{

    public unsafe class Execution
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AllocConsole();
        [STAThread]
        private static int EntryMain(string pwzArgument)
        {
            try
            {
                AllocConsole();
                Console.WriteLine("Starter funct!");
                var csgoProcess = new CSGOProcess();
                csgoProcess.Load();
                csgoProcess.Start();
                Thread t = new Thread(start);
                t.SetApartmentState(ApartmentState.STA);

                t.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return 0;
        }

        static void start()
        {
            CSGO.UI.ModuleManager.Metro.App.Main();
        }
    }
}
