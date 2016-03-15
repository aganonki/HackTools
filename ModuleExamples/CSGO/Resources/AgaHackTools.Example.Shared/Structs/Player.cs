using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AgaHackTools.Example.Shared.Math;
using AgaHackTools.Main.Interfaces;

namespace AgaHackTools.Example.Shared.Structs
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Player
    {
        [FieldOffset(Offsets.m_iVirtualTable)]
        public uint m_iVirtualTable;

        [FieldOffset(Offsets.m_iID)]
        public int m_iID;

        [FieldOffset(Offsets.m_iDormant)]
        public byte m_iDormant;

        [FieldOffset(Offsets.m_hOwner)]
        public short m_hOwner;

        [FieldOffset(Offsets.m_dwBoneMatrix)]
        public int m_pBoneMatrix;

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

        [FieldOffset(0x935)]
        public bool m_bSpotted;

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

        public int GetBoneAddress(int boneIndex)
        {
            return m_pBoneMatrix + boneIndex * 0x30;
        }

        public Vector3 GetBoneVector(Bones bone)
        {
            if (IsValid())
                return new Vector3(
                    CSGOData.Memory.Read<float>((IntPtr)GetBoneAddress((int)bone) +0x0c),
                    CSGOData.Memory.Read<float>((IntPtr)GetBoneAddress((int)bone) + 0x1C),
                    CSGOData.Memory.Read<float>((IntPtr)GetBoneAddress((int)bone) + 0x2C) 
                    );
            else return Vector3.Zero;
        }
        public static Player NullPlayer => new Player() { m_iID = 0, m_iHealth = 0, m_iTeam = 0 };
    }

    public enum Bones : int
    {
        Head = 6,
        Neck = 5,
        Spine1 = 0,
        Spine2 = 1,
        Spine3 = 2,
        Spine4 = 3,
        Spine5 = 4,
        HandLeft = 10,
        ElbowLeft = 9,
        ShoulderLeft = 8,
        ShoulderRight = 36,
        ElbowRight = 37,
        HandRight = 38,
        FootLeft = 65,
        KneeLeft = 64,
        HipLeft = 67,
        HipRight = 73,
        KneeRight = 70,
        FootRight = 71,

        CTHandLeft = 9,
        CTElbowLeft = 10,
        CTShoulderLeft = 11,
        CTShoulderRight = 39,
        CTElbowRight = 40,
        CTHandRight = 41,
        CTFootLeft = 70,
        CTKneeLeft = 69,
        CTHipLeft = 68,
        CTHipRight = 76,
        CTKneeRight = 75,
        CTFootRight = 74
    }
}
