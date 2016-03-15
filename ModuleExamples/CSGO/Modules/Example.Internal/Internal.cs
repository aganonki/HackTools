using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgaHackTools.Example.Shared;
using AgaHackTools.Example.Shared.Structs;
using AgaHackTools.Main.AbstractImplementations;
using AgaHackTools.Main.Default;
using AgaHackTools.Main.Hook.WndProc;
using AgaHackTools.Main.Interfaces;
using ToolsSharp.Hooks.WndProc.Default;

namespace Example.Internal
{
    public class Internal : Module<CSGOData>
    {
        private ISmartMemory _memory;

        public Internal(ISmartMemory memory, CSGOData csgo, int tick = 60, ILog logger = null) : base(csgo, logger, tick)
        {
            _memory = memory;
            OnUpdate += Module_Update;
            WndEngine = new WindowHookEngine();
            WndEngine.RegisterCallback(new Stuff(csgo));
            
        }
        public Internal(ISmartMemory memory, CSGOData csgo, int tick = 60) : this(memory, csgo, tick,Log.GetLogger(typeof(Internal).Name))
        {
        }
        private void Module_Update(object sender, CSGOData csgo)
        {
            if (!_memory.Internal)
            {
                Stop();
                return;
            }
            if (!_memory.IsRunning)
            {
                _memory.Load();
                return;
            }
            else
            {
                InsatalWndHook(_memory.Handle.DangerousGetHandle());
            }
            if (csgo.IsNotPlaying)
                return;

            //Do all stuff here
            try
            {
                Pulse(csgo);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }


        public override string Name => this.GetType().Name;

        protected void Pulse(CSGOData csgo)
        {
            wndHook.SendUserMessage(UserMessage.StartUp);
        }
        public WindowHook wndHook;
        public WindowHookEngine WndEngine;

        public void InsatalWndHook(IntPtr handle)
        {
            if (wndHook==null||!wndHook.IsEnabled)
                return;
            wndHook = new WindowHook(handle, "wndHook", WndEngine);
            wndHook.Enable();
        }
    }
}
