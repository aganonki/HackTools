using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AgaHackTools.Example.Shared.Math;

namespace AgaHackTools.Example.Shared
{
    [StructLayout(LayoutKind.Explicit)]
    public struct LocalPlayer
    {
        [FieldOffset(Offsets.m_dwIndex)]
        public int m_iID;

        [FieldOffset(Offsets.m_iTeamNum)]
        public int m_iTeam;

        [FieldOffset(Offsets.m_iHealth)]
        public int m_iHealth;

        [FieldOffset(Offsets.m_iCrossHairID)]
        public int m_iCrosshairId;

        public bool IsValid()
        {
            return m_iID != 0 && m_iHealth > 0 && (m_iTeam == 2 || m_iTeam == 3);
        }
    }
}
