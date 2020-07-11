using Bus.CommandManager;
using Bus.Commands;
using Bus.Exceptions;
using Bus.Logger;
using Newtonsoft.Json;
using SuperSocket.SocketBase.Config;
using SuperWebSocket;

namespace Bus.WebInterface
{

    class BusWebInterfaceListener
    {
        public static void Start()
        {
            var webInterfaceServerConfig = new ServerConfig()
            {
                Ip = "any",
                Port = Bus.listenPort,
                Security = "TLS",
                Certificate = new CertificateConfig { FilePath = Bus.serverCert["path"], Password = Bus.serverCert["secret"] },
            };

            WebSocketServer webInterfaceEndpoint = new WebSocketServer();
            webInterfaceEndpoint.Setup(new RootConfig(), webInterfaceServerConfig);
            webInterfaceEndpoint.NewMessageReceived += ProcessCommand;
            webInterfaceEndpoint.Start();
        }

        private static void ProcessCommand(WebSocketSession session, string value)
        {
            Command command = JsonConvert.DeserializeObject<Command>(value);

            if (command.Token != Bus.AuthToken)
            {
                BusLogger.LogException(new InvalidAuthTokenException("INVALID AUTHENTICATION TOKEN ERROR"));
                session.Send(JsonConvert.SerializeObject(CommandProcessor.CommandCallback("error_message", "An incorrect authentication key was provided")));
                return;
            }

            session.Send(CommandProcessor.RunCommand(command));
            session.Close();
        }

    }
}
