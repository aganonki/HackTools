using System;
using AgaHackTools.Main.Interfaces;

namespace AgaHackTools.Main.Default
{
    public class Log : ILog
    {
        private string _sender;
        public Log(string sender)
        {
            _sender = sender;
        }

        public void Error(object message)=>Error(message.ToString());

        public void Error(string message)
        {
            LogMessage(" Error: " + message);
        }

        public void Warn(object message)=>Warn(message.ToString());
        public void Warn(string message)
        {
            LogMessage(" Warn: " + message);
        }

        public void Warning(object message)=>Warning(message.ToString());
        public void Warning(string message)
        {
            LogMessage(" Warning: " + message);
        }

        public void Info(object message)=>Info(message.ToString());
        
        public void Info(string message)
        {
            LogMessage(" Info: " + message);
        }

        public void Debug(object message)=>Debug(message.ToString());
        
        public void Debug(string message)
        {
            LogMessage(" Debug: " + message);
        }

        public void Fatal(object message) => Fatal(message.ToString());
        public void Fatal(string message)
        {
            LogMessage(" Fatal error: " +message);
        }

        private void LogMessage(string message)
        {
            Console.WriteLine(_currentTime + " " + _sender + " " + message );
        }
        private string _currentTime => DateTime.Now.ToShortTimeString();

        public static ILog GetLogger(string sender)
        {
            return new Log(sender);
        }
    }
}
