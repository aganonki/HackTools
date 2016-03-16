using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AgaHackTools.Example.Shared
{
    public static class Offsets
    {
        //LocalPlayer -> m_dwLocalPlayer: _______________ 0x00A9430C
        #region SignScanned
        public static IntPtr LocalPlayer = new IntPtr(0x00A9430C);
        public static IntPtr EntityList;

        public static IntPtr ViewMatrix;
        public static IntPtr Attack;
        public static IntPtr Attack2;
        public static IntPtr Server;
        public static IntPtr ServerMap;
        public static IntPtr Jump;
        public static IntPtr GlowObjectBase;        
        public static IntPtr MiscWeaponInfo;
        public static IntPtr EntityId;
        public static IntPtr Health;
        public static IntPtr WeaponHandle;
        public static IntPtr BoneMatrix;
        public static IntPtr Team;
        public static IntPtr VecOrigin;
        public static IntPtr EngineClientState;
        public static IntPtr SetViewAngles;
        public static IntPtr SignOnState;
        #endregion

        #region Entity
        public const int ListSize = 0x10;
        public const int m_iCrossHairID = 0x0000A950; //Tue Sep 29 21:26:06 2015
        public const int m_iHealth = 0x000000FC; //Tue Sep 29 21:26:06 2015
        public const int m_iTeamNum = 0x000000F0; //Tue Sep 29 21:26:06 2015
        public const int m_dwIndex = 0x00000064; //Tue Sep 29 21:26:06 2015
        public const int m_iFlags = 0x100;
        public const int m_vecViewOffset = 0x104;
        public const int m_vecVelocity = 0x110;
        public const int m_vecOrigin = 0x134;
        public const int m_hActiveWeapon = 0x2EE8;
        public const int m_vecPunch = 0x00003018;
        public const int m_iShotsFired = 0xA2B0;
        public const int m_iVirtualTable = 0x8;
        public const int m_iID=0x64;
        public const int m_iDormant =0xE9;
        public const int m_hOwner =0x148;
        public const int m_dwBoneMatrix= 0x2698;
        public const int m_bSpotted= 0x935;
        #endregion

        #region Weapon
        public const int m_iState = 0x31E8;
        public const int m_iClip1 = 0x31F4;
        public const int m_flNextPrimaryAttack = 0x000031C8;
        public const int m_bCanReload = 0x00003235;
        public const int m_fAccuracyPenalty = 0x000032B0;
        public const int m_iWeaponID = 0x32D4;
        public const int m_zoomLevel = 0x00003330; 
        #endregion



    }

}
