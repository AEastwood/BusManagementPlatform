using Bus.Functions;
using Bus.Logger;
using Bus.Notifications;
using Bus.WebInterface;
using Newtonsoft.Json;
using System;

namespace Bus.CommandManager
{
    public class CommandCallback
    {
        public string Callback { get; set; }
        public object Data { get; set; }
    }

    class CommandHandler
    {
        public static string Run(Command command)
        {
            CommandCallback commandCallback = new CommandCallback();
            BusLogger.LogWebInterfaceCommand(command);

            switch (command.CommandName)
            {
                case "get_command_log":
                    commandCallback.Callback = command.CommandName;
                    commandCallback.Data = BusLogger.CommandLog;
                    return JsonConvert.SerializeObject(commandCallback);

                case "get_exception_log":
                    commandCallback.Callback = command.CommandName;
                    commandCallback.Data = BusLogger.ExceptionLog;
                    return JsonConvert.SerializeObject(commandCallback);

                case "get_notifications":
                    commandCallback.Callback = command.CommandName;
                    commandCallback.Data = NotificationManager.NotificationList;
                    return JsonConvert.SerializeObject(commandCallback);

                case "initial_load":
                    commandCallback.Callback = command.CommandName;
                    commandCallback.Data = Bus.Model;
                    return JsonConvert.SerializeObject(commandCallback);

                case "reload_config":
                    Console.Clear();
                    BusFunctions.LoadConfiguration();
                    commandCallback.Callback = command.CommandName;
                    commandCallback.Data = Bus.Model;
                    return JsonConvert.SerializeObject(commandCallback);

                case "toggle_door":
                    BusConfig.UpdateDoorStatus(Bus.Model, command.CommandParamaters[0]);
                    commandCallback.Callback = command.CommandName;
                    commandCallback.Data = Bus.Model.Doors;
                    return JsonConvert.SerializeObject(commandCallback);
            }

            return string.Empty;
        }
    }
}
