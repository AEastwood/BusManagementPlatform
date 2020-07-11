using Bus.Commands;
using Bus.Functions;
using Bus.Logger;
using Bus.Monitor;
using Bus.Notifications;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Bus.CommandManager
{
    class CommandProcessor
    {
        public static string RunCommand(Command command)
        {
            Stopwatch timeTaken = new Stopwatch();
            timeTaken.Start();

            BusLogger.LogWebInterfaceCommand(command);
            Task<CommandCallback> response = Task<CommandCallback>.Factory.StartNew(() =>
            {
                switch (command.CommandName)
                {
                    case "get_command_log":
                        return CommandCallback(command.CommandName, BusLogger.CommandLog);

                    case "get_exception_log":
                        return CommandCallback(command.CommandName, BusLogger.ExceptionLog);

                    case "get_notifications":
                        return CommandCallback(command.CommandName, NotificationManager.NotificationList);

                    case "get_status":
                        return CommandCallback(command.CommandName, BusMonitor.GetHappiness());

                    case "initial_load":
                        return CommandCallback(command.CommandName, Bus.Model);

                    case "mark_read":
                        NotificationManager.MarkNotificationAsRead(command.CommandParamaters[0]);
                        return CommandCallback(command.CommandName, NotificationManager.NotificationList);

                    case "reload_config":
                        BusFunctions.LoadConfiguration();
                        return CommandCallback(command.CommandName, Bus.Model);

                    case "toggle_door":
                        BusConfig.UpdateDoorStatus(Bus.Model, command.CommandParamaters[0]);
                        return CommandCallback(command.CommandName, Bus.Model.Doors);

                    default:
                        return CommandCallback(null, null);
                }
            });

            timeTaken.Stop();
            
            response.Result.TimeTaken = new TimeTaken
            {
                Elapsed = timeTaken.Elapsed,
                ElapsedMilliseconds = timeTaken.ElapsedMilliseconds,
                ElapsedTicks = timeTaken.ElapsedTicks,
                IsRunning = timeTaken.IsRunning
            };

            return JsonConvert.SerializeObject(response.Result);
        }

        public static CommandCallback CommandCallback(string command, object data)
        {
            CommandCallback callback = new CommandCallback
            {
                Command = command,
                Data = data
            };

            return callback;
        }
    }
}
