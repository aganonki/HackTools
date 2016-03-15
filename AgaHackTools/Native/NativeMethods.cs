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

        /// <summary>
        /// The mouse_event function synthesizes mouse motion and button clicks.
        /// </summary>
        /// <param name="dwFlags">Controls various aspects of mouse motion and button clicking.</param>
        /// <param name="dx">The mouse's absolute position along the x-axis or its amount of motion since the last mouse event was generated, 
        /// depending on the setting of MOUSEEVENTF_ABSOLUTE. Absolute data is specified as the mouse's actual x-coordinate; 
        /// relative data is specified as the number of mickeys moved. A mickey is the amount that a mouse has to move for it to report that it has moved.</param>
        /// <param name="dy">The mouse's absolute position along the y-axis or its amount of motion since the last mouse event was generated, 
        /// depending on the setting of MOUSEEVENTF_ABSOLUTE. Absolute data is specified as the mouse's actual y-coordinate; 
        /// relative data is specified as the number of mickeys moved.</param>
        /// <param name="dwData ">If dwFlags contains MOUSEEVENTF_WHEEL, then dwData specifies the amount of wheel movement. A positive value indicates that the wheel was rotated forward, away from the user; a negative value indicates that the wheel was rotated backward, toward the user. One wheel click is defined as WHEEL_DELTA, which is 120.
        ///If dwFlags contains MOUSEEVENTF_HWHEEL, then dwData specifies the amount of wheel movement.A positive value indicates that the wheel was tilted to the right; a negative value indicates that the wheel was tilted to the left.
        ///If dwFlags contains MOUSEEVENTF_XDOWN or MOUSEEVENTF_XUP, then dwData specifies which X buttons were pressed or released. This value may be any combination of the following flags.
        ///If dwFlags is not MOUSEEVENTF_WHEEL, MOUSEEVENTF_XDOWN, or MOUSEEVENTF_XUP, then dwData should be zero.</param>
        /// <param name="dwExtraInfo">An additional value associated with the mouse event. An application calls GetMessageExtraInfo to obtain this extra information.</param>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(MouseEventFlags dwFlags, uint dx, uint dy, uint dwData, uint dwExtraInfo);

        #region ReadProcessMemory

        /// <summary>
        ///     Reads data from an area of memory in a specified process. The entire area to be read must be accessible or the
        ///     operation fails.
        /// </summary>
        /// <param name="hProcess">
        ///     A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ
        ///     access to the process.
        /// </param>
        /// <param name="lpBaseAddress">
        ///     A pointer to the base address in the specified process from which to read. Before any data transfer occurs,
        ///     the system verifies that all data in the base address and memory of the specified size is accessible for read
        ///     access,
        ///     and if it is not accessible the function fails.
        /// </param>
        /// <param name="lpBuffer">
        ///     [Out] A pointer to a buffer that receives the contents from the address space of the specified
        ///     process.
        /// </param>
        /// <param name="dwSize">The number of bytes to be read from the specified process.</param>
        /// <param name="lpNumberOfBytesRead">
        ///     [Out] A pointer to a variable that receives the number of bytes transferred into the specified buffer. If
        ///     lpNumberOfBytesRead is NULL, the parameter is ignored.
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is nonzero.
        ///     If the function fails, the return value is zero. To get extended error information, call
        ///     <see cref="Marshal.GetLastWin32Error" />.
        /// </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ReadProcessMemory(SafeMemoryHandle hProcess, IntPtr lpBaseAddress,
            [Out] byte[] lpBuffer, int dwSize, out IntPtr lpNumberOfBytesRead);

        #endregion

        #region WriteProcessMemory

        /// <summary>
        ///     Writes data to an area of memory in a specified process. The entire area to be written to must be accessible or the
        ///     operation fails.
        /// </summary>
        /// <param name="hProcess">
        ///     A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and
        ///     PROCESS_VM_OPERATION access to the process.
        /// </param>
        /// <param name="lpBaseAddress">
        ///     A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the
        ///     system verifies that
        ///     all data in the base address and memory of the specified size is accessible for write access, and if it is not
        ///     accessible, the function fails.
        /// </param>
        /// <param name="lpBuffer">
        ///     A pointer to the buffer that contains data to be written in the address space of the specified
        ///     process.
        /// </param>
        /// <param name="nSize">The number of bytes to be written to the specified process.</param>
        /// <param name="lpNumberOfBytesWritten">
        ///     A pointer to a variable that receives the number of bytes transferred into the specified process.
        ///     This parameter is optional. If lpNumberOfBytesWritten is NULL, the parameter is ignored.
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is nonzero.
        ///     If the function fails, the return value is zero. To get extended error information, call
        ///     <see cref="Marshal.GetLastWin32Error" />.
        /// </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool WriteProcessMemory(SafeMemoryHandle hProcess, IntPtr lpBaseAddress, byte[] lpBuffer,
            int nSize, out int lpNumberOfBytesWritten);

        #endregion
        #region LoadLibrary

        /// <summary>
        ///     Loads the specified module into the address space of the calling process. The specified module may cause other
        ///     modules to be loaded.
        /// </summary>
        /// <param name="lpFileName">
        ///     The name of the module. This can be either a library module (a .dll file) or an executable module (an .exe file).
        ///     The name specified is the file name of the module and is not related to the name stored in the library module
        ///     itself,
        ///     as specified by the LIBRARY keyword in the module-definition (.def) file.
        ///     If the string specifies a full path, the function searches only that path for the module.
        ///     If the string specifies a relative path or a module name without a path, the function uses a standard search
        ///     strategy to find the module; for more information, see the Remarks.
        ///     If the function cannot find the module, the function fails. When specifying a path, be sure to use backslashes (\),
        ///     not forward slashes (/).
        ///     For more information about paths, see Naming a File or Directory.
        ///     If the string specifies a module name without a path and the file name extension is omitted, the function appends
        ///     the default library extension .dll to the module name.
        ///     To prevent the function from appending .dll to the module name, include a trailing point character (.) in the
        ///     module name string.
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is a handle to the module.
        ///     If the function fails, the return value is NULL. To get extended error information, call
        ///     <see cref="Marshal.GetLastWin32Error" />.
        /// </returns>
        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr LoadLibrary(string lpFileName);

        #endregion
        #region FreeLibrary

        /// <summary>
        ///     Frees the loaded dynamic-link library (DLL) module and, if necessary, decrements its reference count.
        ///     When the reference count reaches zero, the module is unloaded from the address space of the calling process and the
        ///     handle is no longer valid.
        /// </summary>
        /// <param name="hModule">
        ///     A handle to the loaded library module. The <see cref="LoadLibrary" /> , LoadLibraryEx,
        ///     GetModuleHandle, or GetModuleHandleEx function returns this handle.
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is nonzero.
        ///     If the function fails, the return value is zero. To get extended error information, call
        ///     <see cref="Marshal.GetLastWin32Error" />.
        /// </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FreeLibrary(IntPtr hModule);

        #endregion
        #region GetProcAddress

        /// <summary>
        ///     Retrieves the address of an exported function or variable from the specified dynamic-link library (DLL).
        /// </summary>
        /// <param name="hModule">
        ///     A handle to the DLL module that contains the function or variable. The LoadLibrary, LoadLibraryEx,
        ///     LoadPackagedLibrary, or GetModuleHandle function returns this handle.
        ///     The GetProcAddress function does not retrieve addresses from modules that were loaded using the
        ///     LOAD_LIBRARY_AS_DATAFILE flag. For more information, see LoadLibraryEx.
        /// </param>
        /// <param name="procName">
        ///     The function or variable name, or the function's ordinal value.
        ///     If this parameter is an ordinal value, it must be in the low-order word; the high-order word must be zero.
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is the address of the exported function or variable.
        ///     If the function fails, the return value is NULL. To get extended error information, call
        ///     <see cref="Marshal.GetLastWin32Error" />.
        /// </returns>
        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        /// <summary>
        /// High resolution timing method
        /// </summary>
        /// <param name="lpPerformanceCount"></param>
        /// <returns></returns>
        [DllImport("Kernel32.dll")]
        public static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("user32.dll")]
        public static extern int GetKeyState(int vKey);

        public const int KEY_PRESSED = 0x8000;

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        public static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        /// <summary>
        ///     Sends the specified message to a window or windows.
        ///     The SendMessage function calls the window procedure for the specified window and does not return until the window
        ///     procedure has processed the message.
        /// </summary>
        /// <param name="hWnd">A processHandle to the window whose window procedure will receive the message.</param>
        /// <param name="msg">The message to be sent.</param>
        /// <param name="wParam">Additional message-specific information.</param>
        /// <param name="lParam">Additional message-specific information.</param>
        /// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, UIntPtr wParam, IntPtr lParam);

        /// <summary>
        ///     Passes message information to the specified window procedure..
        /// </summary>
        /// <param name="lpPrevWndFunc">
        ///     The previous window procedure. If this value is obtained by calling the GetWindowLong
        ///     function with the nIndex parameter set to GWL_WNDPROC or DWL_DLGPROC, it is actually either the address of a window
        ///     or dialog box procedure, or a special internal value meaningful only to CallWindowProc
        /// </param>
        /// <param name="hWnd">A Process Handle to the window procedure to receive the message.</param>
        /// <param name="msg">The message.</param>
        /// <param name="wParam">
        ///     Additional message-specific information. The contents of this parameter depend on the value of the
        ///     Msg parameter.
        /// </param>
        /// <param name="lParam">
        ///     Additional message-specific information. The contents of this parameter depend on the value of the
        ///     Msg parameter.
        /// </param>
        /// <returns>Type: LRESULT</returns>
        [DllImport("user32.dll")]
        public static extern int CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, int msg, int wParam, int lParam);
        #endregion
    }
}
