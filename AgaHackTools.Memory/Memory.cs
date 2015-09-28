using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AgaHackTools.Main.Interfaces;
using AgaHackTools.Main.Memory;
using AgaHackTools.Main.Native;
using AgaHackTools.Main.Native.Structs;

namespace AgaHackTools.Memory
{
    public unsafe class Memory :  Pointer,IMemory
    {
        #region Constructor
        public Memory(string name, bool openHande = false) : base(Process.GetProcessesByName(name).First())
        {
            Modules = new Dictionary<string, ISmartPointer>();
            _process = Process.GetProcessesByName(name).First();
            if (openHande)
                OpenHandle(ProcessAccessFlags.AllAccess);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Pattern implementation
        /// </summary>
        public IPattern Pattern { get; set; }

        /// <summary>
        /// Allows to read from modules of without manual BaseAdress. For ex.: memory["client.dll"].Read<int>(offset);
        /// </summary>
        /// <param name="moduleName">ModuleName</param>
        /// <returns>ISmartPonter implementation with BaseAddress set</returns>
        public ISmartPointer this[string moduleName] => FetchModule(moduleName);

        public IDictionary<string,ISmartPointer> Modules;

        public bool IsRunning => !Handle.IsInvalid && !Handle.IsClosed && !MainProcess.HasExited;

        public Process MainProcess => _process;
        private Process _process; 
        #endregion

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
            catch
            {
                //TODO Log this
            }
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
