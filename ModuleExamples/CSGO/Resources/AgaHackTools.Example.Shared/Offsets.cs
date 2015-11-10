using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgaHackTools.Example.Shared
{
    public static class Offsets
    {
        //LocalPlayer -> m_dwLocalPlayer: _______________ 0x00A9430C
        public static IntPtr LocalPlayer = new IntPtr(0x00A9430C);
        public static IntPtr EntityList;
        public const int m_iCrossHairID = 0x00008CE4; //Tue Sep 29 21:26:06 2015
        public const int m_iHealth = 0x000000FC; //Tue Sep 29 21:26:06 2015
        public const int m_iTeamNum = 0x000000F0; //Tue Sep 29 21:26:06 2015
        public const int m_dwIndex = 0x00000064; //Tue Sep 29 21:26:06 2015
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
    }
}
