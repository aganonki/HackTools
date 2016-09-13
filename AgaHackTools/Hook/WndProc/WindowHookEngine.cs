using System;
using System.Collections.Generic;
using AgaHackTools.Main.Hook.WndProc;

namespace ToolsSharp.Hooks.WndProc.Default
{
    /// <summary>
    ///     Default Class for adding elements to be executed inside of the thread the window <see cref="WindowHook" /> class is
    ///     attached to.
    /// </summary>
    public class WindowHookEngine : IWindowEngine
    {
        #region Public Delegates/Events
        /// <summary>
        ///     Occurs when [begin pulsing all pulses].
        /// </summary>
        public event EventHandler<UpdatePulseArgs> StartOfPulse;

        /// <summary>
        ///     Occurs when [all pulses complete].
        /// </summary>
        public event EventHandler<UpdatePulseArgs> EndOfPulse;
        #endregion

        #region Fields, Private Properties
        /// <summary>
        ///     Gets the linked list of <code>IPulsableElement</code>'s.
        /// </summary>
        /// <value>The linked list of IPulsableElement's.</value>
        private LinkedList<IPulsableElement> Pulsables { get; }
        #endregion

        #region Constructors, Destructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="WindowHookEngine" /> class.
        /// </summary>
        public WindowHookEngine()
        {
            Pulsables = new LinkedList<IPulsableElement>();
        }
        #endregion

        #region Interface Implementations
        /// <summary>
        ///     Shuts the engine down.
        /// </summary>
        public void ShutDown()
        {
            Pulsables.Clear();
        }

        /// <summary>
        ///     Starts the engine up.
        /// </summary>
        public void StartUp()
        {
            StartOfPulse?.Invoke(this, new UpdatePulseArgs());

            if (Pulsables == null)
            {
                return;
            }

            if (Pulsables.Count == 0)
            {
                return;
            }

            foreach (var pulsable in Pulsables)
            {
                pulsable.Pulse();
            }

            EndOfPulse?.Invoke(this, new UpdatePulseArgs());
        }
        #endregion

        /// <summary>
        ///     Adds a <code>IPulseableElement</code> member to the linked list. All elements in the list will have their
        ///     <code>Pulse()</code> method called when the <see cref="UserMessage.StartUp" /> is message is invoked.
        /// </summary>
        /// <param name="windowEngine">The window engine.</param>
        public void RegisterCallback(IPulsableElement windowEngine)
        {
            Pulsables.AddLast(windowEngine);
        }

        /// <summary>
        ///     Adds multiple <code>IPulseableElement</code> member to the linked list. All elements in the list will have their
        ///     <code>Pulse()</code> method called when the <see cref="UserMessage.StartUp" /> is message is invoked.
        /// </summary>
        /// <param name="pulsableElements">The window engine.</param>
        public void RegisterCallbacks(params IPulsableElement[] pulsableElements)
        {
            foreach (var pulsable in pulsableElements)
            {
                RegisterCallback(pulsable);
            }
        }

        /// <summary>
        ///     Removes an element from the <code>IPulseableElement</code> linked list contained in this instance.
        /// </summary>
        /// <param name="windowEngine">The window engine.</param>
        public void RemoveCallback(IPulsableElement windowEngine)
        {
            if (Pulsables.Contains(windowEngine))
            {
                Pulsables.Remove(windowEngine);
            }
        }

        // private static Lazy<Updater> LazyUpdater => new Lazy<Updater>((() => new Updater("Updater", 1000)));
    }
}