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
        static ScanResult lastScan;
        private static ILog Logger;
        public static void ScanOffsets(ISmartMemory memUtils)
        {
            Logger = new Log("OffsetScanner");
            EntityOff(memUtils);
            LocalPlayer(memUtils);
            Jump(memUtils);
            FindAttack(memUtils);
            FindAttack2(memUtils);
            ClientState(memUtils);
            SetViewAngles(memUtils);
            SignOnState(memUtils);
            GlowManager(memUtils);
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
            lastScan = memUtils["client.dll"].Find(new byte[] { 0x05, 0x00, 0x00, 0x00, 0x00, 0xC1, 0xe9, 0x00, 0x39, 0x48, 0x04 }, "x????xx?xxx", 1,false,false);
            if (lastScan.Success)
            {
                var tmp = lastScan.Address;
                byte tmp2 = memUtils.Read<byte>((IntPtr)(lastScan.Offset + 7));
                Offsets.EntityList = tmp + tmp2;
                Logger.Info(
                    ObjectEx.GetName(()=> Offsets.EntityList)+"\n"+
                    Offsets.EntityList.ToString("X8")
                    );
            }
        }
        static void vMatrix(ISmartMemory memUtils)
        {
            lastScan = memUtils["client.dll"].Find(new byte[] {
                0x53, 0x8B, 0xDC, 0x83, 0xEC, 0x08, 0x83, 0xE4,
                0xF0, 0x83, 0xC4, 0x04, 0x55, 0x8B, 0x6B, 0x04,
                0x89, 0x6C, 0x24, 0x04, 0x8B, 0xEC, 0xA1, 0x00,
                0x00, 0x00, 0x00, 0x81, 0xEC, 0x98, 0x03, 0x00,
                0x00 }, "xxxxxxxxxxxxxxxxxxxxxxx????xxxxxx", 0x4EE, false, false);
            if (lastScan.Success)
            {
                //address += 0x80;
                Offsets.ViewMatrix = lastScan.Address+ 0x80;
                Logger.Info(
                ObjectEx.GetName(() => Offsets.ViewMatrix) + "\n" +
                Offsets.ViewMatrix.ToString("X8")
                );
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
            lastScan = memUtils["client.dll"].Find(pattern,2,false,false);
            if (lastScan.Success)
            {
                //int tmp = memUtils.Read<int>((IntPtr)(lastScan.Address.ToInt32() + 2));
                Offsets.Attack = lastScan.Address;
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
            lastScan = memUtils["client.dll"].Find(pattern,2,false,false);
            if (lastScan.Success)
            {
                //int tmp = memUtils.Read<int>((IntPtr)(lastScan.Address.ToInt32() + 2));
                Offsets.Attack2 = lastScan.Address;
                Logger.Info(
                ObjectEx.GetName(() => Offsets.Attack2) + "\n" +
                Offsets.Attack2.ToString("X8")
                );
            }
        }
        static void LocalPlayer(ISmartMemory memUtils)
        {
            lastScan = memUtils["client.dll"].Find(new byte[] { 0x8D, 0x34, 0x85, 0x00, 0x00, 0x00, 0x00, 0x89, 0x15, 0x00, 0x00, 0x00, 0x00, 0x8B, 0x41, 0x08, 0x8B, 0x48 }, "xxx????xx????xxxxx", 3, false, false);
            if (lastScan.Success)
            {
                var tmp = lastScan.Address;
                byte tmp2 = memUtils.Read<byte>((IntPtr)(lastScan.Offset + 18));
                Offsets.LocalPlayer = tmp + tmp2;
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
            lastScan = memUtils["client.dll"].Find(pattern,16,false,false);
            if (lastScan.Success)
            {
                //int tmp = memUtils.Read<int>((IntPtr)(lastScan.Address.ToInt32() + 16));
                Offsets.Server = lastScan.Address ;
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
            lastScan = memUtils["engine.dll"].Find(pattern,6, false,false);
            if (lastScan.Success)
            {
                //int tmp = memUtils.Read<int>((IntPtr)(lastScan.Address.ToInt32() + 6));
                Offsets.ServerMap = lastScan.Address;
                Logger.Info(ObjectEx.GetName(() => Offsets.ServerMap) + "\n" + Offsets.ServerMap.ToString("X8"));
            }
        }
        static void Jump(ISmartMemory memUtils)
        {
            lastScan = memUtils["client.dll"].Find(new byte[] { 0x89, 0x15, 0x00, 0x00, 0x00, 0x00, 0x8B, 0x15, 0x00, 0x00, 0x00, 0x00, 0xF6, 0xC2, 0x03, 0x74, 0x03, 0x83, 0xCE, 0x08, 0xA8, 0x08, 0xBF }, "xx????xx????xxxxxxxxxxx",2 ,false,false);
            if (lastScan.Success)
            {
                //int tmp = memUtils.Read<int>((IntPtr)(lastScan.Address.ToInt32() + 2));
                Offsets.Jump = lastScan.Address;
                Logger.Info(ObjectEx.GetName(() => Offsets.Jump) + "\n" + Offsets.Jump.ToString("X8"));
            }
        }
        static void ClientState(ISmartMemory memUtils)
        {
            lastScan = memUtils["engine.dll"].Find(new byte[] { 0xC2, 0x00, 0x00, 0xCC, 0xCC, 0x8B, 0x0D, 0x00, 0x00, 0x00, 0x00, 0x33, 0xC0, 0x83, 0xB9 }, "x??xxxx????xxxx", 7,false,false);
            if (lastScan.Success)
            {
                //int tmp = memUtils.Read<int>((IntPtr)(lastScan.Address.ToInt32() + 7));
                Offsets.EngineClientState = lastScan.Address;
                Logger.Info(ObjectEx.GetName(() => Offsets.EngineClientState) + "\n" + Offsets.EngineClientState.ToString("X8"));
            }
        }
        static void SetViewAngles(ISmartMemory memUtils)
        {
            lastScan = memUtils["engine.dll"].Find(new byte[] { 0x8B, 0x15, 0x00, 0x00, 0x00, 0x00, 0x8B, 0x4D, 0x08, 0x8B, 0x82, 0x00, 0x00, 0x00, 0x00, 0x89, 0x01, 0x8B, 0x82, 0x00, 0x00, 0x00, 0x00, 0x89, 0x41, 0x04 }, "xx????xxxxx????xxxx????xxx", 11,false,true);
            if (lastScan.Success)
            {
                //int tmp = memUtils.Read<int>((IntPtr)(lastScan.Address.ToInt32() + 11));
                Offsets.SetViewAngles = lastScan.Address;
                Logger.Info(ObjectEx.GetName(() => Offsets.SetViewAngles) + "\n" + Offsets.SetViewAngles.ToString("X8"));
            }
        }
        static void SignOnState(ISmartMemory memUtils)
        {
            lastScan = memUtils["engine.dll"].Find(
                new byte[] { 0x51, 0xA1, 0x00, 0x00, 0x00, 0x00, 0x8B, 0x51, 0x00, 0x83, 0xB8, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7C, 0x40, 0x3B, 0xD1 },
                "xx????xx?xx?????xxxx", 11, false,true);
            if (lastScan.Success)
            {
                //int tmp = memUtils.Read<int>((IntPtr)(lastScan.Address.ToInt32() + 11));
                Offsets.SignOnState = lastScan.Address;
                Logger.Info(ObjectEx.GetName(() => Offsets.SignOnState) + "\n" + Offsets.SignOnState.ToString("X8"));
            }
        }
        static void GlowManager(ISmartMemory memUtils)
        {
            var bytu = new byte[] { 0xA1, 0x00, 0x00, 0x00, 0x00, 0xC7, 0x04, 0x02, 0x00, 0x00, 0x00, 0x00, 0x89, 0x35, 0xE8, 0x8E, 0x64, 0x1F };
            //A1 ?? ?? ?? ?? A8 01 75 4E
            //new byte[] { 0x8D, 0x8F, 0x00, 0x00, 0x00, 0x00, 0xA1, 0x00, 0x00, 0x00, 0x00, 0xC7, 0x04, 0x02, 0x00, 0x00, 0x00, 0x00, 0x89, 0x35, 0x00, 0x00, 0x00, 0x00, 0x8B, 0x51 }
            lastScan = memUtils["client.dll"].Find(bytu, /*"xx????x????xxx????xx????xx"*/"x????xxx????xx????", 1,false,false);
            if (lastScan.Success)
            {
                //int tmp = memUtils.Read<int>((IntPtr)(lastScan.Address.ToInt32() + 1));
                Offsets.GlowObjectBase = lastScan.Address;
                Logger.Info(ObjectEx.GetName(() => Offsets.GlowObjectBase) + "\n" + Offsets.GlowObjectBase.ToString("X8"));
            }
        }
        static void WeaponTable(ISmartMemory memUtils)
        {
            lastScan = memUtils["client.dll"].Find(new byte[] { 0xA1, 0x00, 0x00, 0x00, 0x00, 0x0F, 0xB7, 0xC9, 0x03, 0xC9, 0x8B, 0x44, 0x00, 0x0C, 0xC3 }, "x????xxxxxxx?xx", 1,false,false);
            if (lastScan.Success)
            {
                //int tmp = memUtils.Read<int>((IntPtr)(lastScan.Address.ToInt32() + 1));
                Offsets.MiscWeaponInfo = lastScan.Address;
                Logger.Info(ObjectEx.GetName(() => Offsets.MiscWeaponInfo) + "\n" + Offsets.MiscWeaponInfo.ToString("X8"));
            }
        }
#endregion
        #region ENTITY
        static void EntityID(ISmartMemory memUtils)
        {
            lastScan = memUtils["client.dll"].Find(
                new byte[] { 0x74, 0x72, 0x80, 0x79, 0x00, 0x00, 0x8B, 0x56, 0x00, 0x89, 0x55, 0x00, 0x74, 0x17 },
                "xxxx??xx?xx?xx", 8,false,false);
            if (lastScan.Success)
            {
                //byte tmp = memUtils.Read<byte>((IntPtr)(lastScan.Address.ToInt32() + 8));
                Offsets.EntityId = lastScan.OriginalAddress;
                Logger.Info(ObjectEx.GetName(() => Offsets.EntityId) + "\n" + Offsets.EntityId.ToString("X8"));
            }
        }
        static void EntityHealth(ISmartMemory memUtils)
        {
            lastScan = memUtils["client.dll"].Find(
                new byte[] { 0x8B, 0x41, 0x00, 0x89, 0x41, 0x00, 0x8B, 0x41, 0x00, 0x89, 0x41, 0x00, 0x8B, 0x41, 0x00, 0x89, 0x41, 0x00, 0x8B, 0x4F, 0x00, 0x83, 0xB9, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x2E },
                "xx?xx?xx?xx?xx?xx?xx?xx????xxx", 23,false,false);
            if (lastScan.Success)
            {
                //int tmp = memUtils.Read<int>((IntPtr)(lastScan.Address.ToInt32() + 23));
                Offsets.Health = lastScan.OriginalAddress;
                Logger.Info(ObjectEx.GetName(() => Offsets.Health) + "\n" + Offsets.Health.ToString("X8"));
            }
        }
        static void EntityVecOrigin(ISmartMemory memUtils)
        {
            lastScan = memUtils["client.dll"].Find(
                new byte[] { 0x8A, 0x0E, 0x80, 0xE1, 0xFC, 0x0A, 0xC8, 0x88, 0x0E, 0xF3, 0x00, 0x00, 0x87, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x87, 0x00, 0x00, 0x00, 0x00, 0x9F },
                "xxxxxxxxxx??x??????x????x", 13,false,false);
            if (lastScan.Success)
            {
                //int tmp = memUtils.Read<int>((IntPtr)(lastScan.Address.ToInt32() + 13));
                Offsets.VecOrigin = lastScan.OriginalAddress;
                Logger.Info(ObjectEx.GetName(() => Offsets.VecOrigin) + "\n" + Offsets.VecOrigin.ToString("X8"));
            }
        }

        #endregion
        #region PLAYER
        static void PlayerTeamNum(ISmartMemory memUtils)
        {
            lastScan = memUtils["client.dll"].Find(
                new byte[] { 0xCC, 0xCC, 0xCC, 0x8B, 0x89, 0x00, 0x00, 0x00, 0x00, 0xE9, 0x00, 0x00, 0x00, 0x00, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0x8B, 0x81, 0x00, 0x00, 0x00, 0x00, 0xC3, 0xCC, 0xCC },
                "xxxxx????x????xxxxxxx????xxx", 5,false,false);
            if (lastScan.Success)
            {
                //int tmp = memUtils.Read<int>((IntPtr)(lastScan.Address.ToInt32() + 5));
                Offsets.Team = lastScan.OriginalAddress;
                Logger.Info(ObjectEx.GetName(() => Offsets.Team) + "\n" + Offsets.Team.ToString("X8"));
            }
        }
        static void PlayerBoneMatrix(ISmartMemory memUtils)
        {
            memUtils["client.dll"].Find(
                new byte[] { 0x83, 0x3C, 0xB0, 0xFF, 0x75, 0x15, 0x8B, 0x87, 0x00, 0x00, 0x00, 0x00, 0x8B, 0xCF, 0x8B, 0x17, 0x03, 0x44, 0x24, 0x0C, 0x50 },
                "xxxxxxxx????xxxxxxxxx", 8, false, false);
            if (lastScan.Success)
            {
                //int tmp = memUtils.Read<int>((IntPtr)(lastScan.Address.ToInt32() + 8));
                Offsets.BoneMatrix = lastScan.OriginalAddress;
                Logger.Info(ObjectEx.GetName(() => Offsets.BoneMatrix) + "\n" + Offsets.BoneMatrix.ToString("X8"));
            }
        }
        static void PlayerWeaponHandle(ISmartMemory memUtils)
        {
            memUtils["client.dll"].Find(
                new byte[] { 0x0F, 0x45, 0xF7, 0x5F, 0x8B, 0x8E, 0x00, 0x00, 0x00, 0x00, 0x5E, 0x83, 0xF9, 0xFF },
                "xxxxxx????xxxx", 6, false, false);
            if (lastScan.Success)
            {
                //int tmp = memUtils.Read<int>((IntPtr)(lastScan.Address.ToInt32() + 6));
                Offsets.WeaponHandle = lastScan.OriginalAddress;
                Logger.Info(ObjectEx.GetName(() => Offsets.WeaponHandle) + "\n" + Offsets.WeaponHandle.ToString("X8"));
            }
        }
        #endregion

    }
}
