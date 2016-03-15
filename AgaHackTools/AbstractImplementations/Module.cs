using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AgaHackTools.Main.Default;
using AgaHackTools.Main.Interfaces;
using AgaHackTools.Main.Native;

namespace AgaHackTools.Main.AbstractImplementations
{
    /// <summary>
    /// IModule partial implementation
    /// </summary>
    /// <typeparam name="T">Update Object type</typeparam>
    public abstract class Module<T> : IModule<T>
    {
        #region Fields

        private float _interval;
        private T _state;
        //private Timer _timer;
        private Task Loop;
        //private TimerCallback callback;
        private double LastFrameTick = 0;
        private double TimeBetweenFrame = 0;
        private double WaitTilNextFrame = 0;


        #endregion

        #region Constructors
        /// <summary>
        /// Constructor for module
        /// </summary>
        /// <param name="updateData">Object that will be used in Process method</param>
        /// <param name="ticks">Ticks per second</param>
        /// <param name="logger">Logger instance</param>
        protected Module(T updateData,ILog logger = null, int ticks = 60)
        {
            _state = updateData;
            IsRunning = false;
            //callback += Process;
            //_timer = new Timer(callback, _state, Timeout.Infinite, Timeout.Infinite);
            Loop = new Task(() => Process(_state));
            
            if (logger == null)
                Logger = Log.GetLogger(this.Name.ToString());
            else
                Logger = logger;
            Ticks = ticks;
        }

        #endregion

        #region Interface members

        public virtual void Dispose()
        {
            //_timer.Change(long.MaxValue, (long)_interval);
            //_timer.Dispose();
            Loop.Dispose();
        }

        /// <summary>
        /// Start module
        /// </summary>
        public virtual void Start()
        {
            //_timer.Change(_interval, _interval);
            Loop.Start();
            IsRunning = true;
            //Logger.Info("Started: " + Name);
        }

        /// <summary>
        /// Stop module
        /// </summary>
        public virtual void Stop()
        {
            IsRunning = false;
        }

        #endregion

        #region Properties

        //public Hashtable Configuration { get; set; }
        public float Ticks {
            get { return 1000/_interval;}
            set { _interval = 1000f/value;
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
            while (IsRunning)
            {
                //Do stuff here
                OnUpdate?.Invoke(this, (T) state);
                PreciseThreadStop();
                CalculateFramerate();
                //Thread.Sleep((int)_interval - 6);
                //Thread.Sleep((int)_interval);
            }
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

        private void PreciseThreadStop()
        {
            if (LastFrameTick != 0)
            {               
                TimeBetweenFrame = (QueryPerformanceCounter() - LastFrameTick);
                if (TimeBetweenFrame < _interval)
                {
                    //WaitTilNextFrame = QueryPerformanceCounter() + (_interval - TimeBetweenFrame);
                    //while (QueryPerformanceCounter() <= WaitTilNextFrame) ;
                    Thread.Sleep((int)(_interval - TimeBetweenFrame));
                }
                
            }
            LastFrameTick = QueryPerformanceCounter();
        }
        private static double QueryPerformanceCounter()
        {
            long tmp;
            NativeMethods.QueryPerformanceCounter(out tmp);
            return tmp / (double)1000;
        }
        private double lastTick;
        private int lastFrameRate;
        private int frameRate;

        public int FrameRate
        {
            get
            {
                return lastFrameRate;
            }
        }

        private Stopwatch fps = new Stopwatch();
        private void CalculateFramerate()
        {
            if (!fps.IsRunning)
                fps.Start();
            else
            if (fps.Elapsed.TotalMilliseconds >= 1000)
            {
                fps.Reset();
                //Logger.Info(FrameRate);
                lastFrameRate = 0;
            }
            lastFrameRate++;
        }

        #endregion
    }
}
