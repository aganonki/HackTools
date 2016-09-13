namespace AgaHackTools.Main.Hook.WndProc
{
    /// <summary>
    ///     Interface that defines the methods to be called when the user messages <see cref="UserMessage.StartUp" /> and
    ///     <see cref="UserMessage.ShutDown" /> are sent to the <see cref="WindowHook" /> class.
    /// </summary>
    public interface IWindowEngine
    {
        /// <summary>
        ///     The method that is called when the <see cref="UserMessage.StartUp" /> message is sent to the
        ///     <see cref="WindowHook" /> class.
        /// </summary>
        void StartUp();

        /// <summary>
        ///     The method that is called when the <see cref="UserMessage.ShutDown" /> message is sent to the
        ///     <see cref="WindowHook" /> class.
        /// </summary>
        void ShutDown();
    }
}