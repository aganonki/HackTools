using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AgaHackTools.Example.Shared.Math;

namespace AgaHackTools.Example.Shared.Structs
{
    //-----------------------------------------------------------------------------
    // A ray...
    //-----------------------------------------------------------------------------
    [StructLayout(LayoutKind.Sequential)]
    public class Ray_t
    {
        VectorAligned m_Start;  // starting point, centered within the extents
        VectorAligned m_Delta;  // direction + length of the ray
        VectorAligned m_StartOffset;    // Add this to m_Start to get the actual ray start
        VectorAligned m_Extents;    // Describes an axis aligned box extruded along a ray
        //IntPtr m_pWorldAxisTransform; //matrix3x4_t
        [MarshalAs(UnmanagedType.U1)]
        bool m_IsRay;   // are the extents zero?
        [MarshalAs(UnmanagedType.U1)]
        bool m_IsSwept; // is delta != 0?

        public static Ray_t Ray(bool intit =true) 
        {
           return new Ray_t();
        }


        public static Ray_t Init(Vector3 start, Vector3  end)
	    {
            var ray = new Ray_t();
            if (end == null)
                return ray;
            var m_Delta = (end - start);
            ray.m_Delta = m_Delta.Aligned();
            ray.m_IsSwept = (m_Delta.LengthSqr() != 0);

            ray.m_Extents = Vector3.Zero.Aligned();
            ray.m_IsRay = true;

            // Offset m_Start to be in the center of the box...
            ray.m_StartOffset = start.Aligned();
            return ray;
	    }

//    void Init(Vector const& start, Vector const& end, Vector const& mins, Vector const& maxs)
//    {
//        Assert(&end);
//        VectorSubtract(end, start, m_Delta);
//
//        m_pWorldAxisTransform = NULL;
//        m_IsSwept = (m_Delta.LengthSqr() != 0);
//
//        VectorSubtract(maxs, mins, m_Extents);
//        m_Extents *= 0.5f;
//        m_IsRay = (m_Extents.LengthSqr() < 1e-6);
//
//        // Offset m_Start to be in the center of the box...
//        VectorAdd(mins, maxs, m_StartOffset);
//        m_StartOffset *= 0.5f;
//        VectorAdd(start, m_StartOffset, m_Start);
//        m_StartOffset *= -1.0f;
//    }

    // compute inverse delta
//    Vector InvDelta() const
//	{
//		Vector vecInvDelta;
//		for (int iAxis = 0; iAxis< 3; ++iAxis)
//		{
//			if (m_Delta[iAxis] != 0.0f)
//			{
//				vecInvDelta[iAxis] = 1.0f / m_Delta[iAxis];
//			}
//			else
//			{
//				vecInvDelta[iAxis] = FLT_MAX;
//			}
//		}
//		return vecInvDelta;
//	}

}
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct csurface_t
    {
        public char* name;
        public short surfaceProps;
        public ushort flags;
        public string ToString()
        {
            var text = "csurface_t\n";
            //text += name.ToString();
            text += "\n" + surfaceProps;
            text += "\n" + flags;
            return text;
        }
    };
    [StructLayout(LayoutKind.Sequential)]
    public struct cplane_t
    {
        [MarshalAs(UnmanagedType.Struct)]
        public Vector3 normal;
        public float dist;
        public byte type;
        public byte signbits;
        public byte pad1;
        public byte pad2;
        public string ToString()
        {
            var text = "cplane_t\n";
            text +=normal.ToString();
            text += "\n" + dist;
            text += "\n" + type;
            text += "\n" + signbits;
            text += "\n" + pad1;
            text += "\n" + pad2;
            return text;
        }
    };
    //[NativeCppClass()]

    [StructLayout(LayoutKind.Sequential)]
    public struct CBaseTrace
    {        
        public Vector3 startpos;
        public Vector3 endpos;
        public cplane_t plane;
        public float fraction;
        public int contents;
        public ushort dispFlags;
        [MarshalAs(UnmanagedType.U1)]
        public bool allsolid;
        [MarshalAs(UnmanagedType.U1)]
        public bool startsolid;

        public string ToString()
        {
            var text = "CBaseTrace\n";
            text += "\n" + startpos.ToString();
            text += "\n" + endpos.ToString();
            text += "\n" + plane.ToString();
            text += "\n" + fraction.ToString();
            text += "\n" + contents.ToString();
            text += "\n" + dispFlags.ToString();
            text += "\n" + allsolid.ToString();
            text += "\n" + startsolid.ToString();
            return text;
        }
    };
    //[NativeCppClass()]

    [StructLayout(LayoutKind.Sequential)]
    public struct CGameTrace 
    {
        public CBaseTrace basetrace;
        public float fractionleftsolid;
        
        public csurface_t surface;
        public int hitgroup;
        public short physicsbone;
        public ushort worldSurfaceIndex;
        public IntPtr m_pEnt;
        public int hitbox;

        public string ToString()
        {
            var text = "CGameTrace\n";
            text += "\n" + basetrace.ToString();
            text += "\n" + fractionleftsolid.ToString();
            text += "\n" + surface.ToString();
            text += "\n" + hitgroup.ToString();
            text += "\n" + physicsbone.ToString();
            text += "\n" + m_pEnt.ToString();
            text += "\n" + hitbox.ToString();
            return text;
        }

    };
    [StructLayout(LayoutKind.Sequential)]
    public class trace_t
    {
        [MarshalAs(UnmanagedType.Struct)]
        public Vector3 startpos;
        [MarshalAs(UnmanagedType.Struct)]
        public Vector3 endpos;
        [MarshalAs(UnmanagedType.Struct)]
        public cplane_t plane;
            public float fraction;
            public int contents;
            public ushort dispFlags;
        [MarshalAs(UnmanagedType.U1)]
        public byte allsolid;
        [MarshalAs(UnmanagedType.U1)]
        public byte startsolid;
        public float fractionleftsolid;
        [MarshalAs(UnmanagedType.Struct)]
        public csurface_t surface;
        public int hitgroup;
        public short physicsbone;
        public ushort worldSurfaceIndex;
        public IntPtr m_pEnt;
        public int hitbox;
    }
    [StructLayout(LayoutKind.Sequential)]
    public class CBaseFilter
    {
        public IntPtr pSelf;
        public IntPtr pIgnore;
    }
    public static class TraceHelp
    {
        public static bool DidHit(this CGameTrace a)
        {
            return a.basetrace.fraction < 1 || a.basetrace.allsolid || a.basetrace.startsolid;
        }
        public static bool IsVisible(this CGameTrace a)
        {
            return a.basetrace.fraction > 0.97f;
        }
    }
}
