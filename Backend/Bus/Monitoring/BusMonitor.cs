using Bus.Logger;
using System;

namespace Bus.Monitor
{
    class Happiness
    {
        public double Factor { get; set; }
        public double Max { get; set; }
        public double Percentage { get; set; }
        public string Status { get; set; }


        internal static void CalcMax()
        {
            Bus.Model.Happiness.Max += 2;
            Bus.Model.Happiness.Max += Bus.Model.Cameras.Count;
            Bus.Model.Happiness.Max += Bus.Model.CCTVDrives.Count;
            Bus.Model.Happiness.Max += Bus.Model.Doors.Count;
            Bus.Model.Happiness.Max += Bus.Model.Windows.Count;
        }

        internal static void CalcPercentage() =>
            Bus.Model.Happiness.Percentage = Math.Round((Bus.Model.Happiness.Factor / Bus.Model.Happiness.Max) * 100, 2);
    }

    class BusMonitor
    {
        private static int CheckHappiness()
        {
            int happiness = 0;

            if (BusLogger.ExceptionLog.Count == 0)
                happiness++;

            if (!Bus.Model.LowFuel)
                happiness++;

            Bus.Model.Cameras.ForEach(camera =>
            {
                if (camera.State != 0)
                    happiness++;
            });

            Bus.Model.CCTVDrives.ForEach(drive =>
            {
                if (drive.Used < 75)
                    happiness++;
            });

            Bus.Model.Doors.ForEach(door =>
            {
                if (door.State == 2)
                    happiness++;
            });

            Bus.Model.Windows.ForEach(window =>
            {
                if (window.State == 2)
                    happiness++;
            });

            return happiness;
        }        

        public static object GetHappiness()
        {
            Happiness happiness = new Happiness
            {
                Factor = CheckHappiness()
            };

            Bus.Model.Happiness = happiness;

            Happiness.CalcMax();
            Happiness.CalcPercentage();
            
            return Bus.Model.Happiness;
        }
    }
}
