using Bus.WebInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Bus.Logger
{
    class BusLogger
    {

        public static List<Exception> ExceptionLog = new List<Exception>();
        public static List<Command> CommandLog = new List<Command>();
        
        private static readonly string date = DateTime.Now.ToString("d_MM_yyyy");
        private static readonly string fileType = "log";

        private static readonly Dictionary<string, string> LogFiles = new Dictionary<string, string>()
        {
            { "Exceptions", string.Format("{0}", Assembly.GetEntryAssembly().Location.Replace("Bus.exe", $"logs\\Exceptions-{date}.{fileType}")) },
            { "WebInterface", string.Format("{0}", Assembly.GetEntryAssembly().Location.Replace("Bus.exe", $"logs\\WebInterfaceLog-{date}.{fileType}")) }
        };

        private static void CommitCommandLogToFile()
        {

            TextWriter LogWriter = new StreamWriter(LogFiles["WebInterface"]);

            foreach (var logEntry in CommandLog)
                LogWriter.WriteLine("Time: {0}    Command: {1}    Auth_Key: {2}", DateTime.Now.ToString(), logEntry.CommandName, logEntry.Token);

            LogWriter.Close();

        }

        private static void CommitExceptionLogToFile()
        {

            TextWriter LogWriter = new StreamWriter(LogFiles["Exceptions"]);

            foreach (var logEntry in ExceptionLog)
                LogWriter.WriteLine("Time: {0}    Exception: {1}", DateTime.Now.ToString(), logEntry.Message);

            LogWriter.Close();

        }

        public static void LogWebInterfaceCommand(Command command)
        {
            if (string.IsNullOrEmpty(command.CommandName))
                return;

            if (!command.Logged)
                return;

            CommandLog.Add(command);
            CommitCommandLogToFile();
        }

        public static void LogException(Exception e)
        {
            ExceptionLog.Add(e);
            CommitExceptionLogToFile();
        }

        public static void Start()
        {
            foreach (KeyValuePair<string, string> LogFile in LogFiles)
            {
                if (!File.Exists(LogFile.Value))
                {
                    var logFile = File.Create(LogFile.Value);
                    logFile.Close();
                }
            }

        }

    }
}
