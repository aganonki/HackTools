using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgaHackTools.Main.Default
{
    /// <summary>
    ///     Contains data regarding a pattern scan result.
    /// </summary>
    public struct ScanResult
    {
        /// <summary>
        ///     The address found.
        /// </summary>
        public IntPtr Address;

        /// <summary>
        ///     The offset found.
        /// </summary>
        public int Offset;

        /// <summary>
        ///     The original address found.
        /// </summary>
        public IntPtr OriginalAddress;

        /// <summary>
        ///     Indicates if scann was successful
        /// </summary>
        public bool Success;
    }
}
