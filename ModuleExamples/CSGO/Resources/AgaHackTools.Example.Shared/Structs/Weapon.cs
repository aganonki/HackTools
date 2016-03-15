using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AgaHackTools.Example.Shared.Structs
{
    [StructLayout(LayoutKind.Explicit)]
    public struct CSGOWeapon
    {
        //[FieldOffset(0x1358)]
        //public int m_iItemDefinitionIndex;

        [FieldOffset(Offsets.m_iState)]
        public int m_iState;

        [FieldOffset(Offsets.m_iClip1)]
        public int m_iClip1;

        [FieldOffset(Offsets.m_flNextPrimaryAttack)]
        public float m_flNextPrimaryAttack;

        [FieldOffset(Offsets.m_bCanReload)]
        public bool m_bCanReload;

        //[FieldOffset(0x162C)]
        //public int m_iWeaponTableIndex;

        [FieldOffset(Offsets.m_fAccuracyPenalty)]
        public float m_fAccuracyPenalty;

        [FieldOffset(Offsets.m_iWeaponID)]
        public int m_iWeaponID;
        [FieldOffset(Offsets.m_zoomLevel)]
        public int m_zoomLevel;


        public bool IsValid()
        {
            return this.m_iWeaponID > 0;// && this.m_iItemDefinitionIndex > 0;
        }

        public bool IsFullAuto()
        {
            return this.IsAssaultRifle() || this.IsMachinePistol() || this.IsMachineGun();
        }
        public bool IsNonAim()
        {
            return
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_knifegg ||
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_knife ||
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_flashbang ||
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_hegrenade ||
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_smokegrenade ||
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_molotov ||
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_decoy ||
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_incgrenade ||
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_c4;
        }
        public bool IsPistol()
        {
            return
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_deagle ||
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_elite ||
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_fiveseven ||
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_glock ||
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_p228 ||
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_usp ||
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_tec9 ||
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_taser ||
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_hkp2000;
        }
        public bool IsMachinePistol()
        {
            return
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_bizon ||
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_mac10 ||
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_mp5navy ||
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_mp7 ||
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_mp9 ||
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_p90 ||
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_tec9 ||
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_tmp ||
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_ump45;
        }
        public bool IsAssaultRifle()
        {
            return
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_ak47 ||
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_aug ||
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_famas ||
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_galil ||
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_galilar ||
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_m4a1 ||
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_sg556;
        }
        public bool IsMachineGun()
        {
            return
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_negev ||
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_m249;
        }
        public bool IsPumpGun()
        {
            return
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_m3 ||
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_mag7 ||
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_nova ||
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_sawedoff ||
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_xm1014;
        }
        public bool IsSniper()
        {
            return
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_awp ||
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_scout ||
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_scar20 ||
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_ssg08 ||
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_sg550 ||
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_g3sg1;
        }
        public bool IsSpecial()
        {
            return
                this.m_iWeaponID == (int)CSGO_Weapon_ID.weapon_deagle;
        }
        public bool IsZoomed()
        {
            return m_zoomLevel == 1 ||
                   m_zoomLevel == 2;
        }
        public enum CSGO_Weapon_ID
        {
            weapon_none = 0,
            weapon_deagle,
            weapon_elite,
            weapon_fiveseven,
            weapon_glock,
            weapon_p228,
            weapon_usp,
            weapon_ak47,
            weapon_aug,
            weapon_awp,
            weapon_famas,
            weapon_g3sg1,
            weapon_galil,
            weapon_galilar,
            weapon_m249,
            weapon_m3,
            weapon_m4a1,
            weapon_mac10,
            weapon_mp5navy,
            weapon_p90,
            weapon_scout,
            weapon_sg550,
            weapon_sg552,
            weapon_tmp,
            weapon_ump45,
            weapon_xm1014,
            weapon_bizon,
            weapon_mag7,
            weapon_negev,
            weapon_sawedoff,
            weapon_tec9,
            weapon_taser,
            weapon_hkp2000,
            weapon_mp7,
            weapon_mp9,
            weapon_nova,
            weapon_p250,
            weapon_scar17,
            weapon_scar20,
            weapon_sg556,
            weapon_ssg08,
            weapon_knifegg,
            weapon_knife,
            weapon_flashbang,
            weapon_hegrenade,
            weapon_smokegrenade,
            weapon_molotov,
            weapon_decoy,
            weapon_incgrenade,
            weapon_c4
        };

        public static CSGOWeapon NullWeapon => new CSGOWeapon { m_iWeaponID = 0};
}
}
