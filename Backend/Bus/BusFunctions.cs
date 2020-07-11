using Bus.Logger;
using Bus.Sensors;
using Bus.WebInterface;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using System.Threading;

namespace Bus.Functions
{
    class BusFunctions
    {

        private static bool validConfig = false;
        public static void StartBackend()
        {
            LoadConfiguration();
            Console.Title = string.Format("{0}, CheckDelay: {1}s", Bus.Model.ToString(), Bus.checkDelay);

            BusCCTV.Start();
            BusLogger.Start();
            BusTracker.Start();
            BusWebInterfaceListener.Start();

            while (validConfig)
            {
                ConsoleOutput();
                Thread.Sleep(Bus.checkDelay * 1000);
            }
        }

        public static void LoadConfiguration()
        {
            string config;
            string configFile = string.Format("{0}", Assembly.GetEntryAssembly().Location.Replace("Bus.exe", "config.json"));

            if (!File.Exists(configFile))
                File.Create(configFile);

            using (StreamReader configReader = new StreamReader(configFile))
            {
                config = configReader.ReadToEnd();
                try
                {
                    Bus.Model = JsonConvert.DeserializeObject<BusConfig>(config);
                    validConfig = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(ConsoleFunctions.WriteWithColour(ConsoleColor.Red, "INVALID CONFIGURATION FILE"));
                    BusLogger.LogException(e);
                    validConfig = false;
                }
                finally
                {
                    configReader.Close();
                }
            }

            BusCCTV.CheckHDDUsage();
            BusCCTV.CheckDriveHealth();
        }

        public static void ConsoleOutput()
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);

            Console.WriteLine("Current Bus: {0}", (string.IsNullOrEmpty(Bus.Model.Name)) ? "NO_NAME" : Bus.Model.Name + Environment.NewLine);
            Console.WriteLine("Security Status: {0} Cameras, {1} Sensors", Bus.Model.Cameras.Count, (Bus.Model.Doors.Count + Bus.Model.Windows.Count + Bus.Model.Sensors.Count));
            Console.WriteLine("--------------------------------------");
            Console.WriteLine("Type    | Name         |  Status");

            SensorFunctions.SensorCheck();
        }


    }
}
