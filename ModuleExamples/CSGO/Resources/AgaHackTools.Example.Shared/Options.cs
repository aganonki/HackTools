using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgaHackTools.Example.Shared
{
    public class Options
    {
        public Options()
        {
            Triggerbot = true;
            AutoPistol = true;
            KnifeBot = true;
            Aimbot = true;
            KnifeBotFrontStab = true;
            Glow=true;
            AimbotJumpNonTargetDist = 600;
            AimbotExtraSmoothDist = 600;
            AimbotFOV = 20;
            AimbotSmooth = 14;
            AimbotStopDelay = 5;
            KnifeBotDist = 65f;
            SmoothOther = 65f;
            m_bRenderWhenOccluded = true;
            m_bRenderWhenUnoccluded = false;
            m_bFullBloom = false;
            m_bBloomInside = true;
            AimkeyAutoShootFov = 4f;
            KnifeBotV2 = true;
            KnifeBot_CheckBack = true;
            KnifeBot_HullMaxs = 64f;
            KnifeBot_HullDim = 16f;
            AimbotBones = true;
        }

        public bool Triggerbot;
        public bool AutoPistol;
        public bool KnifeBot;
        public bool KnifeBotFrontStab;
        public float AimbotExtraSmoothDist;
        public float AimbotJumpNonTargetDist;
        public float AimbotFOV;
        public float AimbotSmooth;
        public float AimbotStopDelay;
        public bool Aimbot;
        public bool AimbotBones;
        public bool Glow;
        public float SmoothOther;
        public bool m_bRenderWhenOccluded;
        public bool m_bRenderWhenUnoccluded;
        public bool m_bFullBloom;
        public bool m_bBloomInside;
        public bool Bhop;
        public float AimkeyAutoShootFov;
        public bool KnifeBotV2;
        public bool KnifeBot_CheckBack;
        public float KnifeBot_HullMaxs;
        public float KnifeBot_HullDim;
        public float KnifeBotDist;
    }    
}
