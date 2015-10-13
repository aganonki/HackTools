using System;
using System.Collections;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using AgaHackTools.Example.Shared;
using AgaHackTools.Main.AbstractImplementations;
using AgaHackTools.Main.Interfaces;
using static AgaHackTools.Example.Shared.InternalDelegates;
using AgaHackTools.Native;

namespace AgaHackTools.Example.MemoryModule
{
    public class MemoryModule : Module<CSGOCurrentData>
    {
        private ISmartMemory _memory;
        private bool offsetsLoaded;
        public MemoryModule(ISmartMemory memory, CSGOCurrentData csgo, int tick = 60, ILog logger = null) : base(csgo, logger, tick)
        {
            _memory= memory;
            offsetsLoaded = false;
        }

        public override string Name => this.GetType().Name;
       
        protected override void Process(object state)
        {
            //Console.ReadKey();
            Stop();
            var csgo = (CSGOCurrentData) state;
            if (!_memory.IsRunning)
            {
                _memory.Load();
                //Start();
                return;
            }
            
            if (!offsetsLoaded)
            {
                LoadOffsets();
                Start();
                return;
            }

            //Reading all data here
            try
            {
                //ReadMemory(csgo);

            }
            catch (Win32Exception e)
            {
                Logger.Error(e);
            }
            //run base for OnUpdate event
            base.Process(state);
            Start();
        }

        private void ReadMemory(CSGOCurrentData csgo)
        {
            csgo.LocalPlayer = _memory["client.dll"].Read<LocalPlayer>(Offsets.LocalPlayer);
        }

        private CreateInterfaceFn EngineCreateInterface;
        private void LoadOffsets()
        {
            //TODO Add scanning
            //Offsets.LocalPlayer = _memory.Pattern.
            Logger.Info("LoadOffsets");
            if (_memory.Internal)
            {
                var enginedll = _memory["engine.dll"];
                Logger.Info(enginedll.Path + "\nAddress: " +
                                enginedll.ThisModule.BaseAddress);
                var createInter=enginedll["CreateInterface"];
                Logger.Info("New Interface name: "+createInter.Name + "\nAddress" +createInter.BaseAddress);
                EngineCreateInterface = createInter.GetDelegate<CreateInterfaceFn>();
                var returncode = 0;
                var IEngineTracePtr = EngineCreateInterface(IEngineTraceVTable,ref returncode);
                var IEngineTrace = new VirtualClass(IEngineTracePtr,_memory);
                Logger.Info("New VClass name: " + IEngineTraceVTable + "\nAddress: " + IEngineTrace.ClassAddress.ToString());
            }
            offsetsLoaded = true;
        }
    }
}
