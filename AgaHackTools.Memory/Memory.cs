using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AgaHackTools.Interfaces;
using AgaHackTools.Native;
using AgaHackTools.Native.Structs;

namespace AgaHackTools.Memory
{
    public unsafe class Memory :  Pointer,IMemory
    {
        public IPattern Pattern { get; set; }

        public Memory(string name, bool openHande= false):base(Process.GetProcessesByName(name).First())
        {
            Modules = new Hashtable();
            _process = Process.GetProcessesByName(name).First();
            if (openHande)
                OpenHandle(ProcessAccessFlags.AllAccess);
        }

        public ISmartPointer this[string moduleName] => FetchModule(moduleName);

        public Hashtable Modules;

        public bool IsRunning => !Handle.IsInvalid && !Handle.IsClosed && !MainProcess.HasExited;

        public Process MainProcess => _process;
        private Process _process;
        #region Methods
        /// <summary>
        /// Opens a handle to a process
        /// </summary>
        /// <param name="id">ID of the process</param>
        /// <param name="flags">ProcessAccessFlags to use</param>
        /// <returns>A handle to the process</returns>
        public SafeMemoryHandle OpenHandle(ProcessAccessFlags flags)
        {
            return NativeMethods.OpenProcess(flags, false, _process.Id);
        }

        /// <summary>
        /// Retrieves the process-module with the given name, returns null if not found
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IntPtr GetModuleBase(string name)
        {
            try
            {
                foreach (ProcessModule module in MainProcess.Modules)
                    if (module.FileName.EndsWith(name))
                        return module.BaseAddress;
            }
            catch { }
            return IntPtr.Zero;
        }

        private ISmartPointer FetchModule(string moduleName)
        {
            if (Modules.ContainsKey(moduleName))
                return (ISmartPointer)Modules[moduleName];
            else
            {
                var newModule = new Pointer(Handle, GetModuleBase(moduleName));
                Modules.Add(moduleName, newModule);
                return newModule;
            }
        }         
        
        /// <summary>
        ///     Releases all resources used by the object.
        /// </summary>
        public override void Dispose()
        {
            // Raise the event OnDispose
            OnDispose?.Invoke(this, new EventArgs());

            //// Close the process handle
            //Handle.Close();

            // Avoid the finalizer
            GC.SuppressFinalize(this);
        }
        #endregion
        #region Public Events

        /// <summary>
        ///     Raises when the object is disposed.
        /// </summary>
        public override event EventHandler OnDispose;

        #endregion

    }
}
