using System;
using System.ComponentModel;
using AgaHackTools.Main.Memory;
using AgaHackTools.Main.Native;
using AgaHackTools.Main.Native.Structs;

namespace AgaHackTools.Memory
{
    public class MemoryProtection : IDisposable
    {
        /// <summary>
        ///     Internal or external protection required
        /// </summary>
        public bool Internal { get; }        
        /// <summary>
        ///     The base address of the altered memory.
        /// </summary>
        public IntPtr BaseAddress { get; }        
        /// <summary>
        ///     The base address of the altered memory.
        /// </summary>
        public SafeMemoryHandle Handle { get; }

        /// <summary>
        ///     Defines the new protection applied to the memory.
        /// </summary>
        public MemoryProtectionFlags NewProtection { get; }
        /// <summary>
        ///     References the inital protection of the memory.
        /// </summary>
        public MemoryProtectionFlags OldProtection { get; }

        /// <summary>
        ///     The size of the altered memory.
        /// </summary>
        public int Size { get; }
        

        public MemoryProtection(SafeMemoryHandle handle, IntPtr address, int size,bool @internal = true, MemoryProtectionFlags protection = MemoryProtectionFlags.ExecuteReadWrite)
        {
            BaseAddress = address;
            NewProtection = protection;
            Size = size;
            Internal = @internal;
            Handle = handle;

            OldProtection = ChangeMemoryProtection(protection);
        }

        /// <summary>
        ///     Frees resources and perform other cleanup operations before it is reclaimed by garbage collection.
        /// </summary>
        ~MemoryProtection()
        {
                Dispose();
        }


        /// <summary>
        ///     Restores the initial protection of the memory.
        /// </summary>
        public virtual void Dispose()
        {
            // Restore the memory protection
            ChangeMemoryProtection(OldProtection);
            // Avoid the finalizer 
            GC.SuppressFinalize(this);
        }

        #region ChangeMemoryProtection

        /// <summary>
        ///     Changes the protection on a region of committed pages in the virtual address space of a specified process.
        /// </summary>
        /// <param name="processHandle">A handle to the process whose memory protection is to be changed.</param>
        /// <param name="address">
        ///     A pointer to the base address of the region of pages whose access protection attributes are to be
        ///     changed.
        /// </param>
        /// <param name="size">The size of the region whose access protection attributes are changed, in bytes.</param>
        /// <param name="protection">The memory protection option.</param>
        /// <returns>The old protection of the region in a structure.</returns>
        public MemoryProtectionFlags ChangeMemoryProtection(MemoryProtectionFlags protection)
        {
            // Create the variable storing the old protection of the memory page
            MemoryProtectionFlags oldProtection;

            bool result;
            // Change the protection in the target process
            if (Internal)
                result = NativeMethods.VirtualProtect(BaseAddress, Size, protection, out oldProtection);
            else
                result = NativeMethods.VirtualProtectEx(Handle, BaseAddress, Size, protection, out oldProtection);

            if (result)
            {
                // Return the old protection
                return oldProtection;
            }

            // Else the protection couldn't be changed, throws an exception
            throw new Win32Exception(
                string.Format("Couldn't change the protection of the memory at 0x{0} of {1} byte(s) to {2}.",
                    BaseAddress.ToString("X"), Size, protection));
        }

        #endregion
    }
}
