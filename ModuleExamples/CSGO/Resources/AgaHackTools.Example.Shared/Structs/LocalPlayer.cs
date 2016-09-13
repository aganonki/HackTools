using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AgaHackTools.Example.Shared.Math;
using AgaHackTools.Example.Shared.Structs;
using AgaHackTools.Main.Interfaces;

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

        [FieldOffset(Offsets.m_iFlags)]
        public int m_iFlags;

        [FieldOffset(Offsets.m_vecViewOffset)]
        public Vector3 m_vecViewOffset;

        [FieldOffset(Offsets.m_vecVelocity)]
        public Vector3 m_vecVelocity;

        [FieldOffset(Offsets.m_vecOrigin)]
        public Vector3 m_vecOrigin;

        [FieldOffset(Offsets.m_hActiveWeapon)]
        public uint m_hActiveWeapon;

        [FieldOffset(Offsets.m_vecPunch)]
        public Vector3 m_vecPunch;

        [FieldOffset(Offsets.m_iShotsFired)]
        public int m_iShotsFired;

        public bool IsValid()
        {
            return m_iID != 0 && m_iHealth > 0 && (m_iTeam == 2 || m_iTeam == 3);
        }

        public CSGOWeapon GetActiveWeapon(ISmartMemory memUtils)
        {
            try
            {
                if (this.m_hActiveWeapon == 0xFFFFFFFF)
                    return new CSGOWeapon() { m_iWeaponID = 0 };

                uint handle = this.m_hActiveWeapon & 0xFFF;
                if (handle < 1 || handle > 2048 || CSGOData.Entity[handle - 1].Address == null || CSGOData.Entity[handle - 1].Address == 0)
                    return new CSGOWeapon() { m_iWeaponID = 0 };
                int weapAddress = CSGOData.Entity[handle - 1].Address;
                return memUtils.Read<CSGOWeapon>((IntPtr)weapAddress);
            }
            catch
            {
                //Console.WriteLine("");
                return new CSGOWeapon() { m_iWeaponID = 0 };
            }
        }
    }
}
