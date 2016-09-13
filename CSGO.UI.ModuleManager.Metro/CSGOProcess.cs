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
using Json;

namespace AgaHackTools.Example
{
    public class CSGOProcess : IProcess
    {
        #region Fields

        ISmartMemory _memory;
        private Options Configs;
        private List<string> moduleList;
        IModuleManager moduleManager;

        private const string csgoSettings = "Settings.cfg";
        private const string modulesCfg = "Modules.cfg";

        #endregion

        #region Constructors

        /// <summary>
        /// CustomFormattedClass Constructor.
        /// </summary>
        public CSGOProcess()
        {
            moduleManager = new ModuleManager();
            Logger = Log.GetLogger(this.GetType().Name.ToString());
            Modules = new List<IModule<object>>();
            moduleList = new List<string>
            {
                "AgaHackTools.Example.MemoryReadingModule",
                "AgaHackTools.Example.CSGO.Misc",
                "AgaHackTools.Example.Triggerbot",
            };
            ConfigurationManagerStatic.DefaultConfig(modulesCfg, moduleList);
            // Construct.
            ConfigurationManager = new ConfigurationManager();
        }

        #endregion

        #region Interface members

        public void Load()
        {
            Logger.Info(AppDomain.CurrentDomain.BaseDirectory);
            Logger.Info("Getting startup modules!");
            Configs = ConfigurationManagerStatic.GetConfiguration<Options>(csgoSettings);
            moduleList = ConfigurationManagerStatic.GetConfiguration<List<string>>(modulesCfg);
            Logger.Info("Loading modules!");
            //Load IMemory
            var memoryModuleAssembly = moduleManager.GetInstance("","AgaHackTools.Memory.dll", "ISmartMemory");
            _memory = moduleManager.ActivateInstance<ISmartMemory>(memoryModuleAssembly, "csgo");

            Logger.Info("Loaded: Memory implementation lib");
            //Load ModuleResponsable for memoryUpdates
            foreach (var item in moduleList)
            {
                // for 
                Load(item);
            }   
        }

        public void Load(string item)
        {
            var newModuleType = moduleManager.GetInstance("",item, "IModule`1");
            var newModule = moduleManager.ActivateInstance<IModule<object>>(newModuleType, new object[] {_memory, new object(), 60});
            Modules.Add(newModule);
            Logger.Info("Loaded: " + newModule.Name);
        }

        public void Start()
        {
            Logger.Info("Starting modules!");
            Modules.ForEach(x=>x.Start());
            Logger.Info("Started all modules!");
        }

        public void Stop()
        {
            Logger.Info("Stoping modules!");
            Modules.ForEach(x => x.Stop());
            Logger.Info("Stopped all modules!");
        }

        #endregion

        #region Properties

        public IConfigurationManager ConfigurationManager { get; set; }
        public ILog Logger { get; set; }
        public bool IsRunning { get; }

        public List<IModule<object>> Modules { get; set; }

        #endregion

        #region Other

        ~CSGOProcess()
        {
            // Construct.
        }

        #endregion
    }
}
