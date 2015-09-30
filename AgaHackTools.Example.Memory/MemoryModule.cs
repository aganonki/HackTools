using System;
using System.Collections;
using System.ComponentModel;
using AgaHackTools.Example.Shared;
using AgaHackTools.Main.AbstractImplementations;
using AgaHackTools.Main.Interfaces;

namespace AgaHackTools.Example.MemoryModule
{
    public class MemoryModule : Module<CSGOCurrentData>
    {
        private ISmartMemory _memory;
        private bool offsetsLoaded;
        public MemoryModule(ISmartMemory memory, CSGOCurrentData csgo, Hashtable config) : base(csgo, config)
        {
            _memory= memory;
            offsetsLoaded = false;
        }

        public override string Name => this.GetType().Name;

        protected override void Process(object state)
        {
            var csgo = (CSGOCurrentData) state;
            if (!_memory.IsRunning) return;
            if (!offsetsLoaded)
            {
                LoadOffsets();
                return;
            }

            //Reading all data here
            try
            {
                csgo.LocalPlayer = _memory["client.dll"].Read<LocalPlayer>(Offsets.LocalPlayer);
            }
            catch (Win32Exception e)
            {
                Logger.Error(e);
            }
            //run base for OnUpdate event
            base.Process(state);
        }

        private void LoadOffsets()
        {
            //TODO Add scanning
            //Offsets.LocalPlayer = _memory.Pattern.
            offsetsLoaded = true;
        }
    }
}
