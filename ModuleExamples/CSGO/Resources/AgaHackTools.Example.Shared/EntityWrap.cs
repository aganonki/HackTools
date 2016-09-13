using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgaHackTools.Example.Shared.Structs;

namespace AgaHackTools.Example.Shared
{
    public class EntityWrap
    {
        public ClassID ClassId;
        public Entity Entity;
        public Shared.Structs.Player Player;
        public CSGOWeapon Weapon;
        public int Address;
        public bool Bomberman;

        public bool IsWeapon => (ClassId == ClassID.AK47 ||
                                 ClassId == ClassID.DEagle ||
                                 ClassId == ClassID.WeaponAug ||
                                 ClassId == ClassID.WeaponAWP ||
                                 ClassId == ClassID.WeaponBizon ||
                                 ClassId == ClassID.WeaponElite ||
                                 ClassId == ClassID.WeaponFiveSeven ||
                                 ClassId == ClassID.WeaponG3SG1 ||
                                 ClassId == ClassID.WeaponGalilAR ||
                                 ClassId == ClassID.WeaponGlock ||
                                 ClassId == ClassID.WeaponHKP2000 ||
                                 ClassId == ClassID.WeaponM249 ||
                                 ClassId == ClassID.WeaponM4A1 ||
                                 ClassId == ClassID.WeaponMP7 ||
                                 ClassId == ClassID.WeaponMP9 ||
                                 ClassId == ClassID.WeaponMag7 ||
                                 ClassId == ClassID.WeaponNOVA ||
                                 ClassId == ClassID.WeaponNegev ||
                                 ClassId == ClassID.WeaponP250 ||
                                 ClassId == ClassID.WeaponP90 ||
                                 ClassId == ClassID.WeaponSCAR20 ||
                                 ClassId == ClassID.WeaponSG556 ||
                                 ClassId == ClassID.WeaponSSG08 ||
                                 ClassId == ClassID.WeaponTaser ||
                                 ClassId == ClassID.WeaponTec9 ||
                                 ClassId == ClassID.WeaponUMP45 ||
                                 ClassId == ClassID.WeaponXM1014 ||
                                 ClassId == ClassID.WeaponXM1014 ||
                                 ClassId == ClassID.WeaponMag7 ||
                                 ClassId == ClassID.WeaponG3SG1 ||
                                 ClassId == ClassID.WeaponElite ||
                                 ClassId == ClassID.WeaponBizon ||
                                 ClassId == ClassID.WeaponSCAR20 ||
                                 ClassId == ClassID.WeaponFamas ||
                                 ClassId == ClassID.WeaponGalil ||
                                 ClassId == ClassID.WeaponP228 ||
                                 ClassId == ClassID.WeaponTMP ||
                                 ClassId == ClassID.WeaponUSP ||
                                 ClassId == ClassID.WeaponSG550 ||
                                 ClassId == ClassID.SCAR17 ||
                                 ClassId == ClassID.WeaponScout ||
                                 ClassId == ClassID.WeaponMP5Navy ||
                                 ClassId == ClassID.WeaponMAC10 ||
                                 ClassId == ClassID.WeaponM3 ||
                                 ClassId == ClassID.WeaponSG552 ||
                                 ClassId == ClassID.WeaponSawedoff);

        public bool IsPlayer=> ClassId == ClassID.CSPlayer;
        
    }
}
