using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AgaHackTools.Example.Shared;
using AgaHackTools.Main.AbstractImplementations;
using AgaHackTools.Main.Default;
using AgaHackTools.Main.Interfaces;

namespace AgaHackTools.Example.CSGO.Misc
{
    public class Misc: Module<object>
    { 
        #region Fields

        private ISmartMemory _memory;

        private InternalDelegates.CreateInterfaceFn EngineCreateInterface;
        private bool offsetsLoaded;

        #endregion

        #region Constructors

        public Misc(ISmartMemory memory, object csgo, int tick = 100, ILog logger = null) : base(csgo, logger, tick)
        {
            _memory = memory;
            offsetsLoaded = false;
            OnUpdate += Module_Update;
        }
        public Misc(ISmartMemory memory, object csgo, int tick = 100) : base(csgo, Log.GetLogger(typeof(Misc).Name), tick)
        {
            _memory = memory;
            offsetsLoaded = false;
            OnUpdate += Module_Update;
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
                bhop();
                AutoPistol();
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
        private void AutoPistol()
        {
            if (CSGOData.Config.AutoPistol)
                if (DefaultInput.GetKeyDown(Keys.LButton) && CSGOData.LocalPlayerWeapon.IsPistol())
                    attack(15);
        }

        private void attack(int delay = 25)
        {
            if (!CSGOData.LocalPlayer.IsValid()) return;
            _memory["client.dll"].Write((Offsets.Attack), 5);
            Thread.Sleep(delay);
            _memory["client.dll"].Write(Offsets.Attack, 4);
        }
        private void attack2(int zoom = 50)
        {
            //if (!CSGOCurrentData.LocalPlayer.IsValid() || !LocalPlayerWeapon.IsSniper()) return;
            //if (!LocalPlayerWeapon.IsZoomed())
            //{
            //    _memory["client.dll"].Write(Offsets.Attack2, 5);
            //    Thread.Sleep(25);
            //    _memory["client.dll"].Write(Offsets.Attack2, 4);
            //    Thread.Sleep(zoom);
            //}
        }

        public void bhop()
        {
            if (DefaultInput.GetKeyDown(Keys.Space))
            {
            if ((CSGOData.LocalPlayer.m_iFlags & 1) == 1) //Stands (FL_ONGROUND)
                _memory["client.dll"].Write(Offsets.Jump, 5);
            else
                _memory["client.dll"].Write(Offsets.Jump, 4);                
            }
        }
        #endregion
    }

}
