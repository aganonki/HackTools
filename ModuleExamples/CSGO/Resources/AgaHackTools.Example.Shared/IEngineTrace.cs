using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AgaHackTools.Example.Shared.Math;
using AgaHackTools.Example.Shared.Structs;
using AgaHackTools.Main.Interfaces;
using AgaHackTools.Main.Memory;
using AgaHackTools.Native;

namespace AgaHackTools.Example.Shared
{
    public unsafe class IEngineTrace
    {
        #region Delegates

        public delegate void TraceRay(Ray_t ray, uint fMask, IntPtr** pTraceFilter, IntPtr pTrace);

        #endregion

        #region Fields

        public const uint RayMask = 0x4600400B;
        public const uint RayMask2 = 0x46004003;
        public VirtualClass Class;

        public TraceRay Trace;
        private ISmartMemory _memory;

        #endregion

        #region Constructors

        public IEngineTrace(TraceRay clas, ISmartMemory memory)
        {
            Trace = clas;
            OurCallBackDelegate = OurCallback;
            OurCallBackPointer = OurCallBackDelegate.GetFunctionPointer();
            _memory = memory;
        }

        #endregion

        #region Public methods

        public bool IsVisable(IntPtr local, Vector3 start, Vector3 end)
        {
            Ray_t ray =Ray_t.Init(start, end);

            CGameTrace tr = new CGameTrace();
            IntPtr* filter = default(IntPtr*);
            filter[0] = local;

            Trace(ray, RayMask, &filter, OurCallBackPointer);

            return tr.IsVisible();
        }

        #endregion

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr TrayCallback(IntPtr objectPointer, uint arg);
        private TrayCallback OurCallBackDelegate { get; }
        private IntPtr OurCallBackPointer { get; }

        private IntPtr OurCallback(IntPtr address, uint arg)
        {
            var tempObject = _memory.Read<CGameTrace>(address);
            Console.WriteLine(tempObject.ToString());
            return (IntPtr)1;
        }
    }
}
