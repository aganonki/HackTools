﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgaHackTools.Example.Shared.Structs;
using AgaHackTools.Main.Hook.WndProc;
using AgaHackTools.Main.Interfaces;
using ToolsSharp.Hooks.WndProc.Default;

namespace AgaHackTools.Example.Shared
{
    public class CSGOData
    {
        public static ISmartMemory Memory;
        public CSGOData()
        {
        }
        public static Player LocalPlayer = new Player { m_iID = 0, m_iHealth = 0, m_iTeam = 0 };
        public static IntPtr LocalPlayerAddress = IntPtr.Zero;
        public static CSGOWeapon LocalPlayerWeapon = new CSGOWeapon { m_iWeaponID = 0 };
        public static EntityWrap[] Entity = new EntityWrap[1024 * 2];
        public Options Config = new Options();
        public SignOnState SignOnState;
        public bool IsNotPlaying => SignOnState != SignOnState.SIGNONSTATE_FULL;
        public IEngineTrace TraceRay;

        public InternalDelegates.CreateInterfaceFn EngineCreateInterface;
    }
}
