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
            Console.WriteLine(_currentTime + " Error: " + message);
        }

        public void Warn(object message)=>Warn(message.ToString());
        public void Warn(string message)
        {
            Console.WriteLine(_currentTime + " Warn: " + message);
        }

        public void Warning(object message)=>Warning(message.ToString());
        public void Warning(string message)
        {
            Console.WriteLine(_currentTime + " Warning: " + message);
        }

        public void Info(object message)=>Info(message.ToString());
        
        public void Info(string message)
        {
            Console.WriteLine(_currentTime + " Info: " + message);
        }

        public void Debug(object message)=>Debug(message.ToString());
        
        public void Debug(string message)
        {
            Console.WriteLine(_currentTime + " Debug: " + message);
        }

        public void Fatal(object message) => Fatal(message.ToString());
        public void Fatal(string message)
        {
            Console.WriteLine(_currentTime+" Fatal error: " +message);
        }

        private string _currentTime => DateTime.Now.ToShortTimeString();

        public static ILog GetLogger(string sender)
        {
            return new Log(sender);
        }
    }
}
