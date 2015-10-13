using System;
using System.Collections;
using System.Reflection;
using System.Threading;
using AgaHackTools.Main.Default;
using AgaHackTools.Main.Interfaces;

namespace AgaHackTools.Main.AbstractImplementations
{
    /// <summary>
    /// IModule partial implementation
    /// </summary>
    /// <typeparam name="T">Update Object type</typeparam>
    public abstract class Module<T> : IModule<T>
    {
        #region Delegates
        private TimerCallback callback;
        #endregion

        #region Events
        public event EventHandler<T> OnUpdate; 
        #endregion

        #region Fields
        private Timer _timer;
        private int _interval;
        private bool _started;
        private T _state;
        #endregion

        #region Properties
        //public Hashtable Configuration { get; set; }
        public int Ticks {
            get { return 1000/_interval;}
            set { _interval = 1000/value;
                if (IsRunning)
                    _timer.Change(0, _interval);
            }
        }
        public ILog Logger { get; set; }
        public bool IsRunning => _started;
        public virtual string Name => this.GetType().Name;
        #endregion

        #region This
        #endregion

        #region Constructors/Destructor
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="updateData">Object that will be used in Process method</param>
        /// <param name="ticks">Ticks per second</param>
        public Module(T updateData,int ticks)
        {

            _state = updateData;
            _started = false;
            callback += Process;
            _timer = new Timer(callback, _state, Timeout.Infinite, Timeout.Infinite);
            if(Logger== null)
            Logger =  Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.ToString());
            Ticks = ticks;
        }
        /// <summary>
        /// Constructor for module
        /// </summary>
        /// <param name="updateData">Object that will be used in Process method</param>
        /// <param name="ticks">Ticks per second</param>
        /// <param name="logger">Logger instance</param>
        public Module(T updateData,ILog logger, int ticks = 60) : this(updateData, ticks)
        {         
            Logger = logger;
        }
        ///// <summary>
        ///// Constructor for module
        ///// </summary>
        ///// <param name="updateData">Object that will be used in Process method</param>
        ///// <param name="config"> Configuration</param>
        ///// <param name="ticks">Ticks per second</param>
        ///// <param name="logger">Logger instance</param>
        //public Module(T updateData,Hashtable config, int ticks = 60, ILog logger = null) : this(updateData,logger, ticks)
        //{         
        //    Configuration = config;
        //}

        ~Module()
        {
            Dispose();
            // Construct.
        }
        #endregion

        #region Methods
        #region Private
        /// <summary>
        /// Handle module logic
        /// </summary>
        /// <param name="state"></param>
        protected virtual void Process(object state)
        {
            //Do stuff here
            OnUpdate?.Invoke(this,(T)state);
        }

        public virtual void Dispose()
        {
            _timer.Change(int.MaxValue, _interval);
            _timer.Dispose();
        }
        #endregion
        #region Public
        /// <summary>
        /// Start module
        /// </summary>
        public virtual void Start()
        {
            _timer.Change(_interval, _interval);
            _started = true;
            //Logger.Info("Started: " + Name);
        }
        /// <summary>
        /// Stop module
        /// </summary>
        public virtual void Stop()
        {
            _timer.Change(int.MaxValue, _interval);
            _started = false;
        }
        #endregion
        #endregion
            
    }
}
