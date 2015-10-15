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
        #region Fields

        private int _interval;
        private T _state;
        private Timer _timer;
        private TimerCallback callback;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="updateData">Object that will be used in Process method</param>
        /// <param name="ticks">Ticks per second</param>
        protected Module(T updateData,int ticks)
        {

            _state = updateData;
            IsRunning = false;
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
        protected Module(T updateData,ILog logger, int ticks = 60) : this(updateData, ticks)
        {         
            Logger = logger;
        }

        #endregion

        #region Interface members

        public virtual void Dispose()
        {
            _timer.Change(int.MaxValue, _interval);
            _timer.Dispose();
        }

        /// <summary>
        /// Start module
        /// </summary>
        public virtual void Start()
        {
            _timer.Change(_interval, _interval);
            IsRunning = true;
            //Logger.Info("Started: " + Name);
        }

        /// <summary>
        /// Stop module
        /// </summary>
        public virtual void Stop()
        {
            _timer.Change(int.MaxValue, _interval);
            IsRunning = false;
        }

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
        public bool IsRunning { get; private set; }

        public virtual string Name => this.GetType().Name;

        #endregion

        #region Private methods

        /// <summary>
        /// Handle module logic
        /// </summary>
        /// <param name="state"></param>
        protected virtual void Process(object state)
        {
            //Do stuff here
            OnUpdate?.Invoke(this,(T)state);
        }

        #endregion

        #region Other

        public event EventHandler<T> OnUpdate;
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
    }
}
