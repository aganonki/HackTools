using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgaHackTools.Example.Shared;
using AgaHackTools.Main.AbstractImplementations;
using AgaHackTools.Main.Default;
using AgaHackTools.Main.Interfaces;

namespace AgaHackTools.Example.Triggerbot
{
    public class Trigger: Module<CSGOCurrentData>
    {
        private ISmartMemory _memory;
        public Trigger(ISmartMemory memory, CSGOCurrentData data,Hashtable config):base(data,config)
        {
            _memory = memory;
        }

        public override string Name => this.GetType().Name;

        protected override void Process(object state)
        {
            var csgoData = (CSGOCurrentData) state;
            if (!_memory.IsRunning) return;
            if (Configuration["Triggerbot"].Equals(true))
            if (csgoData.LocalPlayer.IsValid())
            {
                if(csgoData.LocalPlayer.m_iCrosshairId<1)
                    return;
                    DefaultInput.LeftMouseClick(50);
            }
            base.Process(state);
        }
    }
}
