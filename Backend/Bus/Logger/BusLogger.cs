using Bus.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

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

        private static void CommitCommandLogToFile(Command command)
        {
            LogFiles["WebInterface"] = Assembly.GetEntryAssembly().Location.Replace("Bus.exe", $"logs\\WebInterfaceLog-{date}.{fileType}");

            try
            {
                TextWriter LogWriter = new StreamWriter(LogFiles["WebInterface"], append: true);
                LogWriter.WriteLine("Time: {0}    Command: {1}    Auth_Key: {2}", DateTime.Now.ToString(), command.CommandName, command.Token);
                LogWriter.Close();
            }
            catch (Exception e)
            {
                LogException(e);
            }
        }

        private static void CommitExceptionLogToFile(Exception exception)
        {
            LogFiles["Exceptions"] = Assembly.GetEntryAssembly().Location.Replace("Bus.exe", $"logs\\Exceptions-{date}.{fileType}");

            try
            {
                TextWriter LogWriter = new StreamWriter(LogFiles["Exceptions"], append: true);
                LogWriter.WriteLine("Time: {0}    Exception: {1}", DateTime.Now.ToString(), exception.Message);
                LogWriter.Close();
            }
            catch (Exception e)
            {
                LogException(e);
            }

        }

        public static void LogWebInterfaceCommand(Command command)
        {
            if (!command.Logged || string.IsNullOrEmpty(command.CommandName))
                return;

            CommandLog.Add(command);
            CommitCommandLogToFile(command);
        }

        public static void LogException(Exception e, [CallerMemberName] string callerName = "")
        {
            ExceptionLog.Add(e);

            if(callerName != "CommitExceptionLogToFile")
                CommitExceptionLogToFile(e);
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
