using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgaHackTools.Example.Shared.Structs;
using AgaHackTools.Main.Hook.WndProc;
using AgaHackTools.Main.Interfaces;
using ToolsSharp.Hooks.WndProc.Default;
using System.Runtime.InteropServices;

namespace AgaHackTools.Example.Shared
{
    public static class CSGOData
    {
        public static ISmartMemory Memory;
        
        public static Structs.Player LocalPlayer = new Structs.Player { m_iID = 0, m_iHealth = 0, m_iTeam = 0 };
        public static IntPtr LocalPlayerAddress = IntPtr.Zero;
        public static CSGOWeapon LocalPlayerWeapon = new CSGOWeapon { m_iWeaponID = 0 };
        public static EntityWrap[] Entity = new EntityWrap[1024 ];//* 2
        public static Options Config = new Options();
        public static SignOnState SignOnState;
        public static bool IsNotPlaying => SignOnState != SignOnState.SIGNONSTATE_FULL;
        public static IEngineTrace TraceRay;

        public static InternalDelegates.CreateInterfaceFn EngineCreateInterface;
        public static InternalDelegates.CreateInterfaceFn ClientCreateInterface;
        public static InternalDelegates.CreateInterfaceFn vgui2CreateInterface;
        public static InternalDelegates.CreateInterfaceFn vguimatsurfaceCreateInterface;
        public static InternalDelegates.CreateInterfaceFn ServerCreateInterface;

        public static unsafe InternalDelegates.CreateInterfaceFn GetInterface(ISmartMemory _memory, string szModule)
        {
            var createInterface = _memory[szModule]["CreateInterface"];
            return createInterface.GetDelegate<InternalDelegates.CreateInterfaceFn>();
        }
    }
}
