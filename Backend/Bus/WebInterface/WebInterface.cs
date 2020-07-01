using Bus.Exceptions;
using Bus.Logger;
using Bus.CommandManager;
using Newtonsoft.Json;
using SuperSocket.SocketBase.Config;
using SuperWebSocket;
using System.Collections.Generic;

namespace Bus.WebInterface
{
    class BusWebInterfaceListener
    {
        private static readonly Dictionary<string, string> serverCert = new Dictionary<string, string>() {
            { "path", @"E:\SteamLogin\cert.pfx" },
            { "secret", "42^kFvZtkf2?8pGza^Lm4mp9aSpL8bEGgcf99qwG*e55rwV=_hJJ$5CjS6f*3hSK" },
        };

        private static WebSocketServer webInterfaceEndpoint;
        public static void Start()
        {
            var webInterfaceServerConfig = new ServerConfig()
            {
                Ip = "any",
                Port = Bus.listenPort,
                Security = "TLS",
                Certificate = new CertificateConfig { FilePath = serverCert["path"], Password = serverCert["secret"] },
            };

            webInterfaceEndpoint = new WebSocketServer();
            webInterfaceEndpoint.Setup(new RootConfig(), webInterfaceServerConfig);
            webInterfaceEndpoint.NewMessageReceived += Command_Received;
            webInterfaceEndpoint.Start();
        }

        private static void Command_Received(WebSocketSession session, string value)
        {
            Command command = JsonConvert.DeserializeObject<Command>(value);
            CommandCallback commandCallback = new CommandCallback();

            if (command.Token != Bus.AuthToken)
            {
                BusLogger.LogException(new InvalidAuthTokenException("INVALID AUTHENTICATION TOKEN ERROR"));
                commandCallback.Callback = "errorMessage";
                commandCallback.Data = "An incorrect authentication key was provided";
                session.Send(JsonConvert.SerializeObject(commandCallback));
                return;
            }

            session.Send(CommandManager.CommandHandler.Run(command));
        }

    }
}
