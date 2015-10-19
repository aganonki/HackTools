using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgaHackTools.Example.Shared.Math;
using AgaHackTools.Example.Shared.Structs;
using AgaHackTools.Native;

namespace AgaHackTools.Example.Shared
{
    public unsafe class IEngineTrace
    {
        #region Delegates

        public delegate void TraceRay(Ray_t ray, uint fMask, IntPtr** pTraceFilter,ref  CGameTrace pTrace);

        #endregion

        #region Fields

        public const uint RayMask = 0x4600400B;
        public VirtualClass Class;

        public TraceRay Trace;

        #endregion

        #region Constructors

        public IEngineTrace(VirtualClass clas)
        {
            Class = clas;
        }

        #endregion

        #region Public methods

        public bool IsVisable(IntPtr local, Vector3 start, Vector3 end)
        {
            Ray_t ray =Ray_t.Init(start, end);

            CGameTrace tr = new CGameTrace();
            IntPtr* filter = default(IntPtr*);
            filter[0] = local;

            Trace(ray, RayMask, &filter, ref tr);
            return tr.IsVisible();
        }

        #endregion
    }
}
