using System;

namespace Bus.Sensors
{
    class SensorFunctions
    {
        private static readonly string TableHeader = "--------------------------------------";
        public static void SensorCheck()
        {
            Console.WriteLine(TableHeader);
            CheckCameras();
            Console.WriteLine(TableHeader);
            CheckDoors();
            Console.WriteLine(TableHeader);
            CheckSensors();
            Console.WriteLine(TableHeader);
            CheckWindows();
            Console.WriteLine(TableHeader);
        }

        private static readonly string[] cameraStatuses = { "UNKNOWN", "RECORDING", "IDLE        ", "INITIALISING" };
        private static readonly string[] openingStatuses = { "UNKNOWN", "OPEN   ", "CLOSED" };

        private static void CheckCameras()
        {
            Bus.Model.Cameras.ForEach(camera =>
            {
                Console.Write(string.Format("{0, -3} | {1,-12} |  ", ConsoleFunctions.WriteWithColour(ConsoleColor.Cyan, "CCTV"), camera.Name));
                ConsoleFunctions.WriteWithColour(StateColour(camera.State), $"{cameraStatuses[camera.State]}\n");
            });
        }

        private static void CheckDoors()
        {
            Bus.Model.Doors.ForEach(door =>
            {
                Console.Write(string.Format("{0, -3} | {1,-12} |  ", ConsoleFunctions.WriteWithColour(ConsoleColor.Cyan, "Door"), door.Name));
                ConsoleFunctions.WriteWithColour(StateColour(door.State), $"{openingStatuses[door.State]}\n");
            });

        }

        private static void CheckSensors()
        {
            Bus.Model.Sensors.ForEach(sensor =>
            {
                Console.Write(string.Format("{0, -1} | {1,-12} |  ", ConsoleFunctions.WriteWithColour(ConsoleColor.Cyan, "Sensor"), sensor.Name));
                ConsoleFunctions.WriteWithColour(StateColour(sensor.State), $"{openingStatuses[sensor.State]}\n");
            });
        }

        private static void CheckWindows()
        {
            Bus.Model.Windows.ForEach(window =>
            {
                Console.Write(string.Format("{0, -1} | {1,-12} |  ", ConsoleFunctions.WriteWithColour(ConsoleColor.Cyan, "Window"), window.Name));
                ConsoleFunctions.WriteWithColour(StateColour(window.State), $"{openingStatuses[window.State]}\n");
            });
        }

        private static ConsoleColor StateColour(int state)
        {
            state = (state.ToString() == null) ? 0 : state;

            switch (state)
            {
                case 1:
                    return ConsoleColor.Red;

                case 2:
                    return ConsoleColor.Green;

                default:
                    return ConsoleColor.DarkCyan;
            }
        }

    }
}
