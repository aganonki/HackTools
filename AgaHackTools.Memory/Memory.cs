using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using AgaHackTools.Main.Interfaces;
using AgaHackTools.Main.Memory;
using AgaHackTools.Main.Native;
using AgaHackTools.Main.Native.Structs;

namespace AgaHackTools.Memory
{
    /// <summary>
    /// Implementation of IMemory
    /// </summary>
    public unsafe class Memory :  Pointer, ISmartMemory
    {
        #region Constructor
        /// <summary>
        /// Creates memory class. Id name is null or empty the its internal memory class
        /// </summary>
        /// <param name="name"></param>
        /// <param name="openHande"></param>
        public Memory(string name = null) : base(string.IsNullOrEmpty(name))
        {
            Modules = new Dictionary<string, IProcessModule>();
            _name = name;
            Load();
        }

        /// <summary>
        /// Gets process from name and open handle.
        /// If Name is null or empty then consider it as internal and get current process.
        /// </summary>
        public void Load()
        {
            var isInternal = string.IsNullOrEmpty(_name) || Process.GetProcessesByName(_name).FirstOrDefault() == Process.GetCurrentProcess();
            _process = isInternal ? Process.GetCurrentProcess() :Process.GetProcessesByName(_name).FirstOrDefault();
            if(_process==null)
                return;
            Handle = isInternal ?  new SafeMemoryHandle(_process.MainWindowHandle):OpenHandle(ProcessAccessFlags.AllAccess) ;
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
        public IProcessModule this[string moduleName] => FetchModule(moduleName);

        public IDictionary<string, IProcessModule> Modules { get; }
        public bool IsRunning => Handle!=null&&_process!=null&&!Handle.IsInvalid && !Handle.IsClosed && !MainProcess.HasExited;

        public Process MainProcess => _process;

        #endregion

        #region Fields
        private string _name;
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
        public ProcessModule GetModule(string name)
        {
            try
            {
                foreach (ProcessModule module in MainProcess.Modules)
                    if (module.FileName.EndsWith(name))
                        return module;
            }
            catch
            {
                //TODO Log this
            }
            return null;

        }

        private IProcessModule FetchModule(string moduleName)
        {
            if (Modules.ContainsKey(moduleName))
                return (IProcessModule)Modules[moduleName];
            var modulebase = GetModule(moduleName);
            // return null if not found
            if (modulebase==null)
                return null;
            var newModule = new Module(Handle, modulebase,Internal);
            Modules.Add(moduleName, newModule);
            return newModule;
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
