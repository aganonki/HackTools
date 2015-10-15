using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AgaHackTools.Example.Shared;
using AgaHackTools.Main;
using AgaHackTools.Main.Default;
using AgaHackTools.Main.Interfaces;

namespace AgaHackTools.Example
{
    public class CSGOProcess : IProcess
    {
        #region Fields

        ISmartMemory _memory;
        private Hashtable Configs;
        public CSGOCurrentData CSGO;
        private List<string> moduleList;
        ModuleManager moduleManager;

        #endregion

        #region Constructors

        /// <summary>
        /// CustomFormattedClass Constructor.
        /// </summary>
        public CSGOProcess()
        {
            moduleManager = new ModuleManager();
            Logger = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.ToString());
            Modules = new List<IModule<CSGOCurrentData>>();
            moduleList = new List<string>();
            Configs = new Hashtable();
            CSGO = new CSGOCurrentData();
            // Construct.
        }

        #endregion

        #region Interface members

        public void Load()
        {
            Logger.Info(AppDomain.CurrentDomain.BaseDirectory);
            Logger.Info("Getting startup modules!");
            //TODO REad modules and Configs from cfg file
            //Only for now
            //moduleList.Add("AgaHackTools.Example.Triggerbot");
            moduleList.Add("AgaHackTools.Example.MemoryModule");
            Configs.Add("Triggerbot",true);
            //Only for now
            Logger.Info("Loading modules!");
            //Load IMemory
            var memoryModuleAssembly = moduleManager.GetInstance("AgaHackTools.Memory.dll", "ISmartMemory");
            _memory = moduleManager.ActivateInstance<ISmartMemory>(memoryModuleAssembly, "");

            Logger.Info("Loaded: Memory implementation lib");
            //Load ModuleResponsable for memoryUpdates
            foreach (var item in moduleList)
            {
                // for 
                var newModuleType = moduleManager.GetInstance(item, "IModule`1");
                var newModule = moduleManager.ActivateInstance<IModule<CSGOCurrentData>>(newModuleType, new object[] {_memory, CSGO,60,Logger});
                Modules.Add(newModule);
                Logger.Info("Loaded: "+ newModule.Name);
            }   
        }

        public void Start()
        {
            Logger.Info("Starting modules!");
            Modules.ForEach(x=>x.Start());
        }

        public void Stop()
        {
            Logger.Info("Stoping modules!");
            Modules.ForEach(x => x.Stop());
        }

        #endregion

        #region Properties

        public ILog Logger { get; set; }
        public bool IsRunning { get; }

        public List<IModule<CSGOCurrentData>> Modules { get; set; }

        #endregion

        #region Other

        ~CSGOProcess()
        {
            // Construct.
        }

        #endregion
    }
}
