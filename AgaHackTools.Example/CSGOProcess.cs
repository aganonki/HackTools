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

        #region Delegates
        #endregion

        #region Events
        #endregion

        #region Fields
        ModuleManager moduleManager;
        IMemory _memory;
        private List<string> moduleList;
        private Hashtable Configs;
        public CSGOCurrentData CSGO;
        #endregion

        #region Properties
        public ILog Logger { get; set; }
        public bool IsRunning { get; }
        public List<IModule<CSGOCurrentData>> Modules { get; set; }
        #endregion

        #region This
        #endregion

        #region Constructors/Destructor
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
        ~CSGOProcess()
        {
            // Construct.
        }
        #endregion

        #region Methods
        #region Private
        #endregion
        #region Public
        public void Load()
        {

            Logger.Info("Getting startup modules!");
            //TODO REad modules and Configs from cfg file
            //Only for now
            moduleList.Add("AgaHackTools.Example.Triggerbot");
            moduleList.Add("AgaHackTools.Example.MemoryModule");
            Configs.Add("Triggerbot",true);
            //Only for now
            Logger.Info("Loading modules!");
            //Load IMemory
            var memoryModuleAssembly = moduleManager.GetInstance("AgaHackTools.Memory.dll", "ISmartMemory");
            _memory = moduleManager.ActivateInstance<ISmartMemory>(memoryModuleAssembly, false);

            Logger.Info("Loaded: Memory implementation lib");
            //Load ModuleResponsable for memoryUpdates
            foreach (var item in moduleList)
            {
                // for 
                var newModuleType = moduleManager.GetInstance(item, "IModule`1");
                var newModule = moduleManager.ActivateInstance<IModule<CSGOCurrentData>>(newModuleType, new object[] {_memory, CSGO, Configs});
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
        #endregion




    }
}
