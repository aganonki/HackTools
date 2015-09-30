﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AgaHackTools.Main.Interfaces
{
    public interface IModule<T> : IDisposable
    {
        string Name { get; }
        ILog Logger { get; set; }
        bool IsRunning { get; }
        /// <summary>
        /// Ticks module thread will repeat per second
        /// </summary>
        int Ticks { get; set; }
        /// <summary>
        /// Configurations
        /// </summary>
        Hashtable Configuration { get; set; }
        /// <summary>
        /// Start
        /// </summary>
        void Start();
        /// <summary>
        /// Stop module
        /// </summary>
        void Stop();
        /// <summary>
        /// On update event
        /// </summary>
        EventHandler<T> OnUpdate { get; set; }
    }
}