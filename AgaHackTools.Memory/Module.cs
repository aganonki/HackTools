using AgaHackTools.Main.Interfaces;
using AgaHackTools.Main.Memory;
using AgaHackTools.Main.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AgaHackTools.Memory
{
    public class Module : Pointer ,IProcessModule
    {
        public Module(SafeMemoryHandle handle, ProcessModule module, bool internalMem) : base(handle,module.BaseAddress, internalMem)
        {
            ThisModule = module;
        }

        #region This

        /// <summary>
        ///     Gets the specified function in the remote module.
        /// </summary>
        /// <param name="functionName">The name of the function.</param>
        /// <returns>A new instance of a <see cref="IProcessFunction" /> class.</returns>
        public IProcessFunction this[string functionName] => FindFunction(functionName);

        #endregion

        #region Native

        /// <summary>
        ///     The native <see cref="ProcessModule" /> object corresponding to this module.
        /// </summary>
        public ProcessModule ThisModule { get; }

        #endregion
        #region Fields

        /// <summary>
        ///     The dictionary containing all cached functions of the module.
        /// </summary>
        internal static readonly IDictionary<string, Function> CachedFunctions = new Dictionary<string, Function>();
        #endregion
        #region FindFunction

        /// <summary>
        ///     Finds the specified function in the remote module.
        /// </summary>
        /// <param name="functionName">The name of the function (case sensitive).</param>
        /// <returns>A new instance of a <see cref="RemoteFunction" /> class.</returns>
        /// <remarks>
        ///     Interesting article on how DLL loading works: http://msdn.microsoft.com/en-us/magazine/bb985014.aspx
        /// </remarks>
        public IProcessFunction FindFunction(string functionName)
        {

            // Check if the function is already cached
            if (CachedFunctions.ContainsKey(functionName))
                return CachedFunctions[functionName];

            // If the function is not cached
            // Check if the local process has this module loaded
            var localModule =
                Process.GetCurrentProcess()
                    .Modules.Cast<ProcessModule>()
                    .FirstOrDefault(m => m.FileName.ToLower() == Path.ToLower());
            var isManuallyLoaded = false;

            try
            {
                // If this is not the case, load the module inside the local process
                if (localModule == null)
                {
                    isManuallyLoaded = true;
                    localModule = NativeHelper.LoadLibrary(ThisModule.FileName);
                }

                // Get the offset of the function
                var offset = NativeHelper.GetProcAddress(localModule, functionName).ToInt64() -
                             localModule.BaseAddress.ToInt64();
                var functionAddress = new IntPtr(ThisModule.BaseAddress.ToInt64() + offset);
                // Rebase the function with the remote module
                var function = new Function(functionAddress, functionName);

                // Store the function in the cache
                CachedFunctions.Add(functionName, function);

                // Return the function rebased with the remote module
                return function;
            }
            finally
            {
                // Free the module if it was manually loaded
                if (isManuallyLoaded)
                    NativeHelper.FreeLibrary(localModule);
            }
        }

        #endregion
        #region Size

        /// <summary>
        ///     The size of the module in the memory of the remote process.
        /// </summary>
        public int Size => ThisModule.ModuleMemorySize;

        #endregion
        #region Path

        /// <summary>
        ///     The full path of the module.
        /// </summary>
        public string Path => ThisModule.FileName;

        #endregion

    }
}
