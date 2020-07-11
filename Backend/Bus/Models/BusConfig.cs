using Bus.Monitor;
using Bus.Sensors;
using System.Collections.Generic;
using System.Linq;

namespace Bus
{
    class BusConfig
    {
        public List<Camera> Cameras { get; set; }
        public List<CCTVManager.Drive> CCTVDrives { get; set; }
        public bool CCTVEnabled { get; set; }
        public int CCTVMode { get; set; }
        public string DoorCode { get; set; }
        public List<Door> Doors { get; set; }
        public int DoorPin { get; set; }
        public Happiness Happiness { get; set; }
        public int Height { get; set; }
        public bool LowFuel { get; set; } = false;
        public string Name { get; set; }
        public string Registration { get; set; }
        public List<Sensor> Sensors { get; set; }
        public string TrackerAPIKey { get; set; }
        public int Weight { get; set; }
        public List<Window> Windows { get; set; }

        public override string ToString() => string.Format("Bus: {0}, Height: {1}ft Weight: {2}t", Name, (Height * 0.08), (Weight / 1000));

        public static void UpdateDoorStatus(BusConfig bus, string name)
        {
            var selectedDoor = bus.Doors.First(door => door.Name == name);

            if (selectedDoor != null)
                selectedDoor.State = (selectedDoor.State == 1) ? 2 : 1;
        }

        public static void UpdateWindowsStatus(BusConfig bus, string name)
        {
            var selectedWindow = bus.Windows.First(window => window.Name == name);

            if (selectedWindow != null)
                selectedWindow.State = (selectedWindow.State == 1) ? 2 : 1;
        }
    }
}
