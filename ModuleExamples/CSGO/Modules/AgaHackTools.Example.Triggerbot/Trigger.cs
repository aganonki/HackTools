using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AgaHackTools.Example.Shared;
using AgaHackTools.Example.Shared.Structs;
using AgaHackTools.Main.AbstractImplementations;
using AgaHackTools.Main.Default;
using AgaHackTools.Main.Interfaces;

namespace AgaHackTools.Example.Triggerbot
{
    public class Trigger : Module<object>
    {
        private ISmartMemory _memory;

        public Trigger(ISmartMemory memory, object csgo, int tick = 60, ILog logger = null) : base(csgo, logger, tick)
        {
            _memory = memory;
            OnUpdate += Module_Update;
        }
        public Trigger(ISmartMemory memory, object csgo, int tick = 60) : this(memory, csgo, tick,Log.GetLogger(typeof(Trigger).Name))
        {
        }
        private void Module_Update(object sender, object csgo)
        {
            if (!_memory.IsRunning)
            {
                _memory.Load();
                return;
            }
            if (CSGOData.IsNotPlaying)
                return;

            //Do all stuff here
            try
            {
                TriggerBot();
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }


        public override string Name => this.GetType().Name;

        protected void TriggerBot()
        {
            if (!CSGOData.Config.Triggerbot)
                return;
            if (!CSGOData.LocalPlayer.IsValid())
                return;
            if (CSGOData.LocalPlayer.m_iCrosshairId < 1)
                return;
            try
            {

                Shared.Structs.Player target = CSGOData.Entity[CSGOData.LocalPlayer.m_iCrosshairId - 1].Player;
            if (!target.IsValid()&& target.m_iTeam == CSGOData.LocalPlayer.m_iTeam)
                return;
            attack();
            //DefaultInput.LeftMouseClick(50);       
            }
            catch (Exception e)
            {
                Logger.Error(Name+" module failed "+e);
            }
        }
        private void attack(int delay = 25)
        {
            if (!CSGOData.LocalPlayer.IsValid()) return;
            _memory["client.dll"].Write((Offsets.Attack), 5);
            Thread.Sleep(delay);
            _memory["client.dll"].Write(Offsets.Attack, 4);
        }
    }
}
