using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgaHackTools.Example.Shared;
using AgaHackTools.Main.Default;
using AgaHackTools.Main.Extensions;
using AgaHackTools.Main.Interfaces;

namespace AgaHackTools.Example.MemoryModule
{
    public static class OffsetScanner
    {
        static ScanResult scan;
        private static ILog Logger;
        static int clientDllBase;
        static int engineDllBase;
        public static void ScanOffsets(ISmartMemory memUtils)
        {
            Logger = new Log("OffsetScanner");
            clientDllBase = (int)memUtils["client.dll"].ImageBase;
            engineDllBase = (int)memUtils["engine.dll"].ImageBase;
            EntityOff(memUtils);
            LocalPlayer(memUtils);
            Jump(memUtils);
            FindAttack(memUtils);
            FindAttack2(memUtils);
            ClientState(memUtils);
            SetViewAngles(memUtils);
            SignOnState(memUtils);
            GlowManager(memUtils); //Broken need new sign
            WeaponTable(memUtils);
            EntityID(memUtils);
            EntityHealth(memUtils);
            EntityVecOrigin(memUtils);
            PlayerTeamNum(memUtils);
            PlayerBoneMatrix(memUtils);
            PlayerWeaponHandle(memUtils);
            vMatrix(memUtils);
            Server(memUtils);
            MapName(memUtils);
        }
        #region Misc
        static void EntityOff(ISmartMemory memUtils)
        {
            scan = memUtils["client.dll"].Find(new byte[] { 0x05, 0x00, 0x00, 0x00, 0x00, 0xC1, 0xe9, 0x00, 0x39, 0x48, 0x04 }, "x????xx?xxx");
            if (scan.Success)
            {
                var tmp = memUtils.Read<int>((IntPtr)(scan.Address + 1));
                byte tmp2 = memUtils.Read<byte>((IntPtr)(scan.Address + 7));
                Offsets.EntityList = (IntPtr) tmp + tmp2 - (int)scan.OriginalAddress;
                Logger.Info(
                    ObjectEx.GetName(()=> Offsets.EntityList)+"\n"+
                    Offsets.EntityList.ToString("X8")
                    );
            }
        }
        static void vMatrix(ISmartMemory memUtils)
        {
            var pattern = new byte[] {
                0xE8, 0x00, 0x00, 0x00, 0x00, 0x8D, 0x95, 0xE0 };
            scan = memUtils["client.dll"].Find(pattern);
            if (scan.Success)
            {
                int address = memUtils.Read<int>((IntPtr)(scan.Address.ToInt32() - 0x1a));
                address -= clientDllBase;
                address += 0x90;
                Offsets.ViewMatrix = new IntPtr(address);
            }
        }
        private static void FindAttack(ISmartMemory memUtils)
        {
            byte[] bytes = BitConverter.GetBytes(4294967293U);
            byte[] pattern = new byte[27]
            {
            137,21,0,0,0,0,139,21,0,0,0,0,246,194,3,
            116,3,131,206,4,168,4,191,bytes[0],bytes[1],bytes[2],bytes[3]
            };
            scan = memUtils["client.dll"].Find(pattern);
            if (scan.Success)
            {
                int tmp = memUtils.Read<int>((IntPtr)(scan.Address.ToInt32() + 2));
                Offsets.Attack = new IntPtr(tmp - clientDllBase);
                Logger.Info(ObjectEx.GetName(() => Offsets.Attack) + "\n" +Offsets.Attack.ToString("X8")
                );
            }
        }
        private static void FindAttack2(ISmartMemory memUtils)
        {
            byte[] int1 = BitConverter.GetBytes(0xFFFFFFFD);
            byte[] int2 = BitConverter.GetBytes(0x00002000);
            byte[] pattern = new byte[]{
                0x89, 0x15, 0x00, 0x00, 0x00, 0x00, //mov [client.dll+xxxx],edx
                0x8B, 0x15, 0x00, 0x00, 0x00, 0x00, //mov edx, [client.dll+????]
                0xF6, 0xC2, 0x03,                   //test dl, 03
                0x74, 0x06,                         //je client.dll+???? 
                0x81, 0xCE, int2[0], int2[1], int2[2], int2[3], //or esi,00002000
                0xA9,  int2[0], int2[1], int2[2], int2[3],      //test al,00002000
                0xBF, int1[0], int1[1], int1[2], int1[3]        //mov edi,FFFFFFFD
                };
            scan = memUtils["client.dll"].Find(pattern);
            if (scan.Success)
            {
                int tmp = memUtils.Read<int>((IntPtr)(scan.Address.ToInt32() + 2));
                Offsets.Attack2 = (IntPtr)tmp - clientDllBase;
                Logger.Info(
                ObjectEx.GetName(() => Offsets.Attack2) + "\n" +
                Offsets.Attack2.ToString("X8")
                );
            }
        }
        static void LocalPlayer(ISmartMemory memUtils)
        {
            scan = memUtils["client.dll"].Find(new byte[] { 0x8D, 0x34, 0x85, 0x00, 0x00, 0x00, 0x00, 0x89, 0x15, 0x00, 0x00, 0x00, 0x00, 0x8B, 0x41, 0x08, 0x8B, 0x48 }, "xxx????xx????xxxxx");
            if (scan.Success)
            {
                var tmp = memUtils.Read<int>((IntPtr)(scan.Address.ToInt32() + 3));
                byte tmp2 = memUtils.Read<byte>((IntPtr)(scan.Address.ToInt32() + 18));
                Offsets.LocalPlayer = (IntPtr) tmp + tmp2 - clientDllBase;
                Logger.Info(ObjectEx.GetName(() => Offsets.LocalPlayer) + "\n" + Offsets.LocalPlayer.ToString("X8"));
            }
        }
        static void Server(ISmartMemory memUtils)
        {
            byte[] pattern = new byte[]{
                0x81, 0xC6, 0x00, 0x00, 0x00, 0x00,
                0x81, 0xFE, 0x00, 0x00, 0x00, 0x00,
                0x7C, 0xEB,
                0x8B, 0x0D, 0x00, 0x00, 0x00, 0x00, //<<<<
                0x5F,
                0x5E,
                0x85, 0xC9,
                0x74, 0x0F,
                0x8B, 0x01,
                0xFF, 0x50, 0x04,
                0xC7, 0x05
                };
            scan = memUtils["client.dll"].Find(pattern);
            if (scan.Success)
            {
                int tmp = memUtils.Read<int>((IntPtr)(scan.Address.ToInt32() + 16));
                Offsets.Server = (IntPtr)tmp - clientDllBase;
                Logger.Info(ObjectEx.GetName(() => Offsets.Server) + "\n" + Offsets.Server.ToString("X8"));
            }
        }
        static void MapName(ISmartMemory memUtils)
        {
            byte[] pattern = new byte[]{
               0x72, 0xEF,
               0xC6, 0x00, 0x00,
               0xB8, 0x00, 0x00, 0x00, 0x00,
               0x80, 0x3D, 0x00, 0x00, 0x00, 0x00, 0x00,
               0x74, 0x15,
               0x8A, 0x08,
               0x80, 0xF9
                };
            scan = memUtils["engine.dll"].Find(pattern);
            if (scan.Success)
            {
                int tmp = memUtils.Read<int>((IntPtr)(scan.Address.ToInt32() + 6));
                Offsets.ServerMap = (IntPtr)tmp - engineDllBase;
                Logger.Info(ObjectEx.GetName(() => Offsets.ServerMap) + "\n" + Offsets.ServerMap.ToString("X8"));
            }
        }
        static void Jump(ISmartMemory memUtils)
        {
            scan = memUtils["client.dll"].Find(new byte[] { 0x89, 0x15, 0x00, 0x00, 0x00, 0x00, 0x8B, 0x15, 0x00, 0x00, 0x00, 0x00, 0xF6, 0xC2, 0x03, 0x74, 0x03, 0x83, 0xCE, 0x08, 0xA8, 0x08, 0xBF }, "xx????xx????xxxxxxxxxxx");
            if (scan.Success)
            {
                int tmp = memUtils.Read<int>((IntPtr)(scan.Address.ToInt32() + 2));
                Offsets.Jump = (IntPtr)tmp - clientDllBase;
                Logger.Info(ObjectEx.GetName(() => Offsets.Jump) + "\n" + Offsets.Jump.ToString("X8"));
            }
        }
        static void ClientState(ISmartMemory memUtils)
        {
            scan = memUtils["engine.dll"].Find(new byte[] { 0xC2, 0x00, 0x00, 0xCC, 0xCC, 0x8B, 0x0D, 0x00, 0x00, 0x00, 0x00, 0x33, 0xC0, 0x83, 0xB9 }, "x??xxxx????xxxx");
            if (scan.Success)
            {
                int tmp = memUtils.Read<int>((IntPtr)(scan.Address.ToInt32() + 7)); 
                Offsets.EngineClientState = (IntPtr)tmp - engineDllBase;
                Logger.Info(ObjectEx.GetName(() => Offsets.EngineClientState) + "\n" + Offsets.EngineClientState.ToString("X8"));
            }
        }
        //static void ClientState2(MemUtils memUtils)
        //{
        //    scan = memUtils.PerformSignatureScan(new byte[] { 0x68, 0, 0, 0, 0, 0xFF, 0x15, 0x98, 0x75, 0x1C, 0x0D, 0xA1, 0, 0, 0, 0, 0x83, 0xc4, 0x1c }, "x????xx????x????xxx", engineDll);
        //    if (scan.Success)
        //    {
        //        int tmp = memUtils.Read<int>((IntPtr)(scan.Address.ToInt32() + 12));
        //        Options.Engine = tmp - engineDllBase;
        //    }
        //}
        static void SetViewAngles(ISmartMemory memUtils)
        {
            scan = memUtils["engine.dll"].Find(new byte[] { 0x8B, 0x15, 0x00, 0x00, 0x00, 0x00, 0x8B, 0x4D, 0x08, 0x8B, 0x82, 0x00, 0x00, 0x00, 0x00, 0x89, 0x01, 0x8B, 0x82, 0x00, 0x00, 0x00, 0x00, 0x89, 0x41, 0x04 }, "xx????xxxxx????xxxx????xxx");
            if (scan.Success)
            {
                int tmp = memUtils.Read<int>((IntPtr)(scan.Address.ToInt32() + 11));
                Offsets.SetViewAngles = (IntPtr)tmp;
                Logger.Info(ObjectEx.GetName(() => Offsets.SetViewAngles) + "\n" + Offsets.SetViewAngles.ToString("X8"));
            }
        }
        static void SignOnState(ISmartMemory memUtils)
        {
            scan = memUtils["engine.dll"].Find(
                new byte[] { 0x51, 0xA1, 0x00, 0x00, 0x00, 0x00, 0x8B, 0x51, 0x00, 0x83, 0xB8, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7C, 0x40, 0x3B, 0xD1 },
                "xx????xx?xx?????xxxx");
            if (scan.Success)
            {
                int tmp = memUtils.Read<int>((IntPtr)(scan.Address.ToInt32() + 11));
                Offsets.SignOnState = (IntPtr)tmp;
                Logger.Info(ObjectEx.GetName(() => Offsets.SignOnState) + "\n" + Offsets.SignOnState.ToString("X8"));
            }
        }
        static void GlowManager(ISmartMemory memUtils)
        {
            var bytu = new byte[] { 0xE8, 0x0, 0x0, 0x0, 0x0, 0x83, 0xC4, 0x04, 0xB8, 0x0, 0x0, 0x0, 0x0, 0xC3, 0xcc };
            scan = memUtils["client.dll"].Find(bytu, "x????xxxx????xx");
            if (scan.Success)
            {
                int tmp = memUtils.Read<int>((IntPtr)(scan.Address.ToInt32() + 9));
                Offsets.GlowObjectBase = (IntPtr)tmp - clientDllBase;
                Logger.Info(ObjectEx.GetName(() => Offsets.GlowObjectBase) + "\n" + Offsets.GlowObjectBase.ToString("X8"));
            }
        }
        static void WeaponTable(ISmartMemory memUtils)
        {
            scan = memUtils["client.dll"].Find(new byte[] { 0xA1, 0x00, 0x00, 0x00, 0x00, 0x0F, 0xB7, 0xC9, 0x03, 0xC9, 0x8B, 0x44, 0x00, 0x0C, 0xC3 }, "x????xxxxxxx?xx");
            if (scan.Success)
            {
                int tmp = memUtils.Read<int>((IntPtr)(scan.Address.ToInt32() + 1));
                Offsets.MiscWeaponInfo = (IntPtr)tmp - clientDllBase;
                Logger.Info(ObjectEx.GetName(() => Offsets.MiscWeaponInfo) + "\n" + Offsets.MiscWeaponInfo.ToString("X8"));
            }
        }
#endregion
        #region ENTITY
        static void EntityID(ISmartMemory memUtils)
        {
            scan = memUtils["client.dll"].Find(
                new byte[] { 0x74, 0x72, 0x80, 0x79, 0x00, 0x00, 0x8B, 0x56, 0x00, 0x89, 0x55, 0x00, 0x74, 0x17 },
                "xxxx??xx?xx?xx");
            if (scan.Success)
            {
                //byte tmp = memUtils.Read<byte>((IntPtr)(lastScan.Address.ToInt32() + 8));
                Offsets.EntityId = (IntPtr)memUtils.Read<byte>((IntPtr)(scan.Address.ToInt32() + 8));
                Logger.Info(ObjectEx.GetName(() => Offsets.EntityId) + "\n" + Offsets.EntityId.ToString("X8"));
            }
        }
        static void EntityHealth(ISmartMemory memUtils)
        {
            scan = memUtils["client.dll"].Find(
                new byte[] { 0x8B, 0x41, 0x00, 0x89, 0x41, 0x00, 0x8B, 0x41, 0x00, 0x89, 0x41, 0x00, 0x8B, 0x41, 0x00, 0x89, 0x41, 0x00, 0x8B, 0x4F, 0x00, 0x83, 0xB9, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x2E },
                "xx?xx?xx?xx?xx?xx?xx?xx????xxx");
            if (scan.Success)
            {
                //int tmp = memUtils.Read<int>((IntPtr)(lastScan.Address.ToInt32() + 23));
                Offsets.Health = (IntPtr)memUtils.Read<int>((IntPtr)(scan.Address.ToInt32() + 23));
                Logger.Info(ObjectEx.GetName(() => Offsets.Health) + "\n" + Offsets.Health.ToString("X8"));
            }
        }
        static void EntityVecOrigin(ISmartMemory memUtils)
        {
            scan = memUtils["client.dll"].Find(
                new byte[] { 0x8A, 0x0E, 0x80, 0xE1, 0xFC, 0x0A, 0xC8, 0x88, 0x0E, 0xF3, 0x00, 0x00, 0x87, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x87, 0x00, 0x00, 0x00, 0x00, 0x9F },
                "xxxxxxxxxx??x??????x????x");
            if (scan.Success)
            {
                //int tmp = memUtils.Read<int>((IntPtr)(lastScan.Address.ToInt32() + 13));
                Offsets.VecOrigin = (IntPtr)memUtils.Read<int>((IntPtr)(scan.Address.ToInt32() + 13));
                Logger.Info(ObjectEx.GetName(() => Offsets.VecOrigin) + "\n" + Offsets.VecOrigin.ToString("X8"));
            }
        }

        #endregion
        #region PLAYER
        static void PlayerTeamNum(ISmartMemory memUtils)
        {
            scan = memUtils["client.dll"].Find(
                new byte[] { 0xCC, 0xCC, 0xCC, 0x8B, 0x89, 0x00, 0x00, 0x00, 0x00, 0xE9, 0x00, 0x00, 0x00, 0x00, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0x8B, 0x81, 0x00, 0x00, 0x00, 0x00, 0xC3, 0xCC, 0xCC },
                "xxxxx????x????xxxxxxx????xxx");
            if (scan.Success)
            {
                //int tmp = memUtils.Read<int>((IntPtr)(lastScan.Address.ToInt32() + 5));
                Offsets.Team = (IntPtr)memUtils.Read<int>((IntPtr)(scan.Address.ToInt32() + 5));
                Logger.Info(ObjectEx.GetName(() => Offsets.Team) + "\n" + Offsets.Team.ToString("X8"));
            }
        }
        static void PlayerBoneMatrix(ISmartMemory memUtils)
        {
            scan = memUtils["client.dll"].Find(
                new byte[] { 0x83, 0x3C, 0xB0, 0xFF, 0x75, 0x15, 0x8B, 0x87, 0x00, 0x00, 0x00, 0x00, 0x8B, 0xCF, 0x8B, 0x17, 0x03, 0x44, 0x24, 0x0C, 0x50 },
                "xxxxxxxx????xxxxxxxxx");
            if (scan.Success)
            {
                //int tmp = memUtils.Read<int>((IntPtr)(lastScan.Address.ToInt32() + 8));
                Offsets.BoneMatrix = (IntPtr)memUtils.Read<int>((IntPtr)(scan.Address.ToInt32() + 8));
                Logger.Info(ObjectEx.GetName(() => Offsets.BoneMatrix) + "\n" + Offsets.BoneMatrix.ToString("X8"));
            }
        }
        static void PlayerWeaponHandle(ISmartMemory memUtils)
        {
            scan = memUtils["client.dll"].Find(
                new byte[] { 0x0F, 0x45, 0xF7, 0x5F, 0x8B, 0x8E, 0x00, 0x00, 0x00, 0x00, 0x5E, 0x83, 0xF9, 0xFF },
                "xxxxxx????xxxx");
            if (scan.Success)
            {
                //int tmp = memUtils.Read<int>((IntPtr)(lastScan.Address.ToInt32() + 6));
                Offsets.WeaponHandle = (IntPtr)memUtils.Read<int>((IntPtr)(scan.Address.ToInt32() + 6));
                Logger.Info(ObjectEx.GetName(() => Offsets.WeaponHandle) + "\n" + Offsets.WeaponHandle.ToString("X8"));
            }
        }
        #endregion

    }
}
