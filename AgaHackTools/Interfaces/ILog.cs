namespace AgaHackTools.Main.Interfaces
{
    public interface ILog
    {
        void Error(object message);
        void Error(string message);
        void Warn(object message);
        void Warn(string message);
        void Warning(object message);
        void Warning(string message);
        void Info(object message);
        void Info(string message);
        void Debug(object message);
        void Debug(string message);
        void Fatal(string message);
        void Fatal(object message);

    }
}
