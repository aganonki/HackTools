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
        [FieldOffset(0x64)]
        public int m_iID;

        [FieldOffset(0xF0)]
        public int m_iTeam;

        [FieldOffset(0xFC)]
        public int m_iHealth;

        [FieldOffset(0x8ce4)]
        public int m_iCrosshairId;

        public bool IsValid()
        {
            return this.m_iID != 0 && this.m_iHealth > 0 && (m_iTeam == 2 || m_iTeam == 3);
        }
    }
}
