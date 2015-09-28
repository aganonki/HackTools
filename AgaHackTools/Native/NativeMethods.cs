using System;
using System.Runtime.InteropServices;
using AgaHackTools.Main.Memory;
using AgaHackTools.Main.Native.Structs;

namespace AgaHackTools.Main.Native
{
    public unsafe static class NativeMethods
    {
        #region CloseHandle

        /// <summary>
        ///     Closes an open object handle.
        /// </summary>
        /// <param name="hObject">A valid handle to an open object.</param>
        /// <returns>
        ///     If the function succeeds, the return value is nonzero.
        ///     If the function fails, the return value is zero. To get extended error information, call
        ///     <see cref="Marshal.GetLastWin32Error" />.
        /// </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);

        #endregion

        #region GetForegroundWindow

        /// <summary>
        ///     Retrieves a handle to the foreground window (the window with which the user is currently working).
        ///     The system assigns a slightly higher priority to the thread that creates the foreground window than it does to
        ///     other threads.
        /// </summary>
        /// <returns>
        ///     The return value is a handle to the foreground window. The foreground window can be NULL in certain
        ///     circumstances, such as when a window is losing activation.
        /// </returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        #endregion

        #region OpenProcess

        /// <summary>
        ///     Opens an existing local process object.
        /// </summary>
        /// <param name="dwDesiredAccess">
        ///     [Flags] he access to the process object. This access right is checked against the security descriptor for the
        ///     process. This parameter can be one or more of the process access rights.
        ///     If the caller has enabled the SeDebugPrivilege privilege, the requested access is granted regardless of the
        ///     contents of the security descriptor.
        /// </param>
        /// <param name="bInheritHandle">
        ///     If this value is TRUE, processes created by this process will inherit the handle.
        ///     Otherwise, the processes do not inherit this handle.
        /// </param>
        /// <param name="dwProcessId">The identifier of the local process to be opened.</param>
        /// <returns>
        ///     If the function succeeds, the return value is an open handle to the specified process.
        ///     If the function fails, the return value is NULL. To get extended error information, call
        ///     <see cref="Marshal.GetLastWin32Error" />.
        /// </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern SafeMemoryHandle OpenProcess(ProcessAccessFlags dwDesiredAccess,
            [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);

        #endregion

        #region VirtualProtectEx

        /// <summary>
        ///     Changes the protection on a region of committed pages in the virtual address space of a specified process.
        /// </summary>
        /// <param name="hProcess">
        ///     A handle to the process whose memory protection is to be changed. The handle must have the PROCESS_VM_OPERATION
        ///     access right.
        ///     For more information, see Process Security and Access Rights.
        /// </param>
        /// <param name="lpAddress">
        ///     A pointer to the base address of the region of pages whose access protection attributes are to be changed.
        ///     All pages in the specified region must be within the same reserved region allocated when calling the VirtualAlloc
        ///     or VirtualAllocEx function using MEM_RESERVE.
        ///     The pages cannot span adjacent reserved regions that were allocated by separate calls to VirtualAlloc or
        ///     <see cref="VirtualAllocEx" /> using MEM_RESERVE.
        /// </param>
        /// <param name="dwSize">
        ///     The size of the region whose access protection attributes are changed, in bytes.
        ///     The region of affected pages includes all pages containing one or more bytes in the range from the lpAddress
        ///     parameter to (lpAddress+dwSize).
        ///     This means that a 2-byte range straddling a page boundary causes the protection attributes of both pages to be
        ///     changed.
        /// </param>
        /// <param name="flNewProtect">
        ///     The memory protection option. This parameter can be one of the memory protection constants.
        ///     For mapped views, this value must be compatible with the access protection specified when the view was mapped (see
        ///     MapViewOfFile, MapViewOfFileEx, and MapViewOfFileExNuma).
        /// </param>
        /// <param name="lpflOldProtect">
        ///     A pointer to a variable that receives the previous access protection of the first page in the specified region of
        ///     pages.
        ///     If this parameter is NULL or does not point to a valid variable, the function fails.
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is nonzero.
        ///     If the function fails, the return value is zero. To get extended error information, call
        ///     <see cref="Marshal.GetLastWin32Error" />.
        /// </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool VirtualProtectEx(SafeMemoryHandle hProcess, IntPtr lpAddress, int dwSize,
            MemoryProtectionFlags flNewProtect, out MemoryProtectionFlags lpflOldProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool VirtualProtect(IntPtr lpAddress, int dwSize,
            MemoryProtectionFlags flNewProtect, out MemoryProtectionFlags lpflOldProtect);

        #endregion

        #region MoveMemory
        /// <summary>
        /// Moves a block of memory from one location to another.
        /// </summary>
        /// <param name="dest">A pointer to the starting address of the move destination.</param>
        /// <param name="src">A pointer to the starting address of the block of memory to be moved.</param>
        /// <param name="size">The size of the block of memory to move, in bytes.</param>
        [DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory", SetLastError = false)]
        public static extern void MoveMemory(void* dest, void* src, int size);
        #endregion
    }
}
