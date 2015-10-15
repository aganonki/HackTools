using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RGiesecke.DllExport;

namespace DomainWrapper
{
    public static class Loader
    {

        [DllExport("Host", CallingConvention.Cdecl)]
        [STAThread]
        public static void Host([MarshalAs(UnmanagedType.LPStr)]string loadDir, [MarshalAs(UnmanagedType.LPStr)]string name)
        {
#if LAUNCH_MDA
            System.Diagnostics.Debugger.Launch();
#endif
            loadDir = Path.GetDirectoryName(loadDir);
            Trace.Assert(Directory.Exists(loadDir));

            Trace.Listeners.Add(new TextWriterTraceListener(Path.Combine(loadDir, @"Logs\", name + ".Loader.log")));

            try
            {
                using (var host = new PathedDomainHost(name, loadDir))
                {
                    host.Execute();
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
            }
        }
    }
}
