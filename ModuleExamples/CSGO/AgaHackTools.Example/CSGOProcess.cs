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
        private Modules moduleList;
        IModuleManager moduleManager;
        
        private const string modulesCfg = "Modules.cfg";
        private const string settingsCfg = "Settings.cfg";

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
            moduleList = new Modules();
            ConfigurationManager = new ConfigurationManager();
            ConfigurationManager.SaveDefault(AppDomain.CurrentDomain.BaseDirectory + "\\" + modulesCfg, moduleList);
            ConfigurationManager.SaveDefault(AppDomain.CurrentDomain.BaseDirectory + "\\" + settingsCfg, new Options());
            // Construct.
        }

        #endregion

        #region Interface members

        public void   Load()
        {
            Logger.Info(AppDomain.CurrentDomain.BaseDirectory);
            Logger.Info("Getting startup modules! " + modulesCfg);
            moduleList = ConfigurationManager.Read<Modules>(AppDomain.CurrentDomain.BaseDirectory +"\\"+ modulesCfg);
            CSGOData.Config = ConfigurationManager.Read<Options>(AppDomain.CurrentDomain.BaseDirectory +"\\"+ settingsCfg);
            Logger.Info("Loading modules!");
            //Load IMemory
            var memoryModuleAssembly = moduleManager.GetInstance(moduleList.Path,"AgaHackTools.Memory.dll", "ISmartMemory");
            _memory = moduleManager.ActivateInstance<ISmartMemory>(memoryModuleAssembly, "");

            Logger.Info("Loaded: Memory implementation lib");
            //Load ModuleResponsable for memoryUpdates
            foreach (var item in moduleList.ModuleNames)
            {
                // for 
                var newModuleType = moduleManager.GetInstance(moduleList.Path, item, "IModule`1");
                var newModule = moduleManager.ActivateInstance<IModule<object>>(newModuleType, new object[] {_memory, new object(),60});
                Modules.Add(newModule);
                Logger.Info("Loaded: "+ newModule.Name);
            }   
        }

        public void Start()
        {
            Logger.Info("Starting modules!");
            Modules.ForEach(x=>x.Start());
            Logger.Info("Started "+Modules.Count+" modules!");
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

    public class Modules
    {
        public Modules()
        {
            Path = @"C:\temp\Modules\";
            ModuleNames = new List<string> {};
        }
        public string Path;
        public List<string> ModuleNames;
    }
}
