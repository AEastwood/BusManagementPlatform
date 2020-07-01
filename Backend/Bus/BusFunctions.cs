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
                ConsoleOutput(Bus.Model);
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
                    Console.Read();
                }
                finally
                {
                    configReader.Close();
                }
            }

            BusCCTV.SetCCTVStorageCalculations();

        }

        public static bool ReloadBusConfiguration()
        {
            validConfig = false;
            Bus.Model = null;
            StartBackend();
            return true;
        }

        public static void ConsoleOutput(BusConfig bus)
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);

            Console.WriteLine("Current Bus: {0}", (string.IsNullOrEmpty(bus.Name)) ? "NO_NAME" : bus.Name + Environment.NewLine);
            Console.WriteLine("Security Status: {0} Cameras, {1} Sensors", bus.Cameras.Count, (bus.Doors.Count + bus.Windows.Count + bus.Sensors.Count));
            Console.WriteLine("--------------------------------------");
            Console.WriteLine("Type    | Name         |  Status");

            SensorFunctions.SensorCheck(bus);
        }


    }
}
