using AgaHackTools.Main.Interfaces;
using AgaHackTools.Main.Memory;
using AgaHackTools.Main.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AgaHackTools.Main.Default;

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
        internal readonly IDictionary<string, Function> CachedFunctions = new Dictionary<string, Function>();

        /// <summary>
        ///     The field for storing the modules data once dumped.
        /// </summary>
        private byte[] _moduleData;

        #endregion
        #region ModuleData

        /// <summary>
        ///     A dump of the modules data as a byte array.
        /// </summary>
        public byte[] ModuleData => _moduleData ?? (_moduleData = ReadBytes(ImageBase, Size));

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
            //Console.WriteLine("Looking for function "+functionName+" in "+ThisModule.FileName);
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
                    Console.WriteLine("Loading libary to local memory "+ ThisModule.FileName);
                    localModule = NativeHelper.LoadLibrary(ThisModule.FileName);
                }

                // Get the offset of the function
                var offset = NativeHelper.GetProcAddress(localModule, functionName).ToInt64() -
                             localModule.BaseAddress.ToInt64();
                Console.WriteLine("Function "+functionName+" Address: "+ offset);
                var functionAddress = new IntPtr(ThisModule.BaseAddress.ToInt64() + offset);
                // Rebase the function with the remote module
                var function = new Function(functionAddress, functionName);
                lock (CachedFunctions)
                {
                    // Store the function in the cache
                    CachedFunctions.Add(functionName, function);
                }
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
        #region Pattern

        /// <summary>
        ///     Performs a pattern scan.
        /// </summary>
        /// m>
        /// <param name="pattern">The <see cref="Pattern" /> Instance containing the data to use.</param>
        /// <returns>A new <see cref="ScanResult" /></returns>
        public ScanResult Find(Pattern pattern)
        {
            var bytes = pattern.GetBytesFromDwordPattern();
            var mask = pattern.GetMaskFromDwordPattern();
            return Find(bytes, mask);
        }

        /// <summary>
        ///     Performs a pattern scan.
        /// </summary>
        /// m>
        /// <param name="patternText">
        ///     The dword formatted text of the pattern.
        ///     <example>A2 5B ?? ?? ?? A2</example>
        /// </param>
        /// <param name="offsetToAdd">The offset to add to the offset result found from the pattern.</param>
        /// <param name="isOffsetMode">If the address is found from the base address + offset or not.</param>
        /// <param name="reBase">If the address should be rebased to this <see cref="RemoteModule" /> Instance's base address.</param>
        /// <returns>A new <see cref="ScanResult" /></returns>
        public ScanResult Find(string patternText)
        {
            var bytes = Pattern.GetBytesFromDwordPattern(patternText);
            var mask = Pattern.GetMaskFromDwordPattern(patternText);
            return Find(bytes, mask);
        }

        /// <summary>
        /// Preformpattern scan from byte[]
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="offsetToAdd"></param>
        /// <param name="isOffsetMode"></param>
        /// <param name="reBase"></param>
        /// <returns></returns>
        public ScanResult Find(byte[] pattern)
        {
            var bytes = pattern;
            var mask = Pattern.MaskFromPattern(pattern);
            return Find(bytes, mask);
        }

        /// <summary>
        ///     Performs a pattern scan.
        /// </summary>
        /// <param name="myPattern">The patterns bytes.</param>
        /// <param name="mask">The mask of the pattern. ? Is for wild card, x otherwise.</param>
        /// <param name="offsetToAdd">The offset to add to the offset result found from the pattern.</param>
        /// <param name="isOffsetMode">If the address is found from the base address + offset or not.</param>
        /// <param name="reBase">If the address should be rebased to this <see cref="RemoteModule" /> Instance's base address.</param>
        /// <returns>A new <see cref="ScanResult" /></returns>
        public ScanResult Find(byte[] myPattern, string mask)
        {
            var patternData = ModuleData;
            var patternBytes = myPattern;
            var patternMask = mask;
            var result = new ScanResult();
            for (var offset = 0; offset < patternData.Length; offset++)
            {
                if (patternMask.Where((m, b) => m == 'x' && patternBytes[b] != patternData[b + offset]).Any()) continue;
                // If this area is reached, the pattern has been found.
                result.Success = true;
                result.OriginalAddress = ImageBase;
                result.Address = IntPtr.Add(ImageBase, offset);
                result.Offset = offset;
                return result;
            }
            // If this is reached, the pattern was not found.
            result.Success = false;
            return result;
        } 
        #endregion
    }
}
