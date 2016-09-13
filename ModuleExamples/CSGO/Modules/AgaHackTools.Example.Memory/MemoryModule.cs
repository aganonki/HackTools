using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using AgaHackTools.Example.Shared;
using AgaHackTools.Example.Shared.Structs;
using AgaHackTools.Main.AbstractImplementations;
using AgaHackTools.Main.Default;
using AgaHackTools.Main.Interfaces;
using static AgaHackTools.Example.Shared.InternalDelegates;
using AgaHackTools.Native;
using AgaHackTools.Memory;
using System.Threading.Tasks;

namespace AgaHackTools.Example.MemoryModule
{
    public class MemoryModule : Module<object>
    {
        #region Fields

        private ISmartMemory _memory;

        private bool offsetsLoaded;

        #endregion

        #region Constructors

        public MemoryModule(ISmartMemory memory, object csgo, int tick = 60, ILog logger = null) : base(csgo, logger, tick)
        {
            _memory= memory;
            offsetsLoaded = false;
            OnUpdate += MemoryModule_Update;
            CSGOData.Memory = _memory;
        }
        public MemoryModule(ISmartMemory memory, object csgo, int tick = 60) : this(memory, csgo, tick, Log.GetLogger(typeof(MemoryModule).Name))
        {
        }
        
        private void MemoryModule_Update(object sender, object csgo)
        {
            
            if (!_memory.IsRunning)
            {
                _memory.Load();
                return;
            }
            
            if (!offsetsLoaded)
            {
                LoadOffsets();
                offsetsLoaded = true;
                return;
            }

            //Reading all data here
            try
            {
                LocalPlayer();
                Entities();

            }
            catch (Win32Exception e)
            {
                Logger.Error(e);
            }
        }

        #endregion

        #region Properties

        public override string Name => this.GetType().Name;

        #endregion

        #region Private methods

        private void LocalPlayer()
        {
            CSGOData.LocalPlayerAddress = _memory["client.dll"].Read<IntPtr>(Offsets.LocalPlayer);
            CSGOData.LocalPlayer = _memory["client.dll"].Read<Shared.Structs.Player>(CSGOData.LocalPlayerAddress);
            Console.WriteLine(CSGOData.LocalPlayer);
            CSGOData.LocalPlayerWeapon = CSGOData.LocalPlayer.GetActiveWeapon(_memory);
        }
        private void Entities()
        {
            byte[] data;
            data = _memory["client.dll"].ReadBytes(Offsets.EntityList, Offsets.ListSize * CSGOData.Entity.Length);
            for (int i = 0; i < data.Length/16; i++)
            {
                int address = BitConverter.ToInt32(data, i*16);
                CSGOData.Entity[i].Address = address;
                if (address == 0)
                {
                    CSGOData.Entity[i].Entity = Entity.NullEntity;
                    CSGOData.Entity[i].Player = Shared.Structs.Player.NullPlayer;
                    CSGOData.Entity[i].Weapon = CSGOWeapon.NullWeapon;
                    continue;
                }
                CSGOData.Entity[i].Entity = _memory.Read<Entity>(address);
                if (!CSGOData.Entity[i].Entity.IsValid())
                {
                    //CSGOData.Entity[i].Entity = Entity.NullEntity;
                    CSGOData.Entity[i].Player = Shared.Structs.Player.NullPlayer;
                    CSGOData.Entity[i].Weapon = CSGOWeapon.NullWeapon;
                    continue;
                }
                CSGOData.Entity[i].ClassId = CSGOData.Entity[i].Entity.GetClassID(_memory);
                switch (CSGOData.Entity[i].ClassId)
                {
                    case ClassID.CSPlayer:
                        CSGOData.Entity[i].Player = _memory.Read<Shared.Structs.Player>((IntPtr)address);
                        break;
                    default:
                        break;

                }
                if (CSGOData.Entity[i].IsWeapon)
                    CSGOData.Entity[i].Weapon = _memory.Read<CSGOWeapon>((IntPtr)address);
            }
        }
        public void GetSignOnState()
        {
            var clientStateAddress = _memory["engine.dll"].Read<int>(Offsets.EngineClientState);
            CSGOData.SignOnState = (SignOnState)_memory.Read<int>(clientStateAddress + (int)Offsets.SignOnState);
        }

        private unsafe void LoadOffsets()
        {
            Logger.Info("LoadOffsets");
            OffsetScanner.ScanOffsets(_memory);

            if (_memory.Internal)
            {
                Logger.Info("Loading internal stuff");
                var createInter= _memory["engine.dll"]["CreateInterface"];
                Logger.Info("New Interface name: "+createInter.Name + "\nAddress" +createInter.BaseAddress);
                //csgo.EngineCreateInterface = createInter.GetDelegate<CreateInterfaceFn>();
                //var returncode = 0;
                //var IEngineTracePtr = csgo.EngineCreateInterface(IEngineTraceVTable,ref returncode);
                
            }
        }

        #endregion
    }
}
