using Bus.Notifications;
using System;
using System.IO;
using System.Linq;
using System.Management;

namespace Bus
{
    class CCTVManager
    {
        public class Drive
        {
            public string DriveLetter { get; set; }
            public string DriveStatus { get; set; }
            public bool ErrorDetected { get; set; } = false;
            public string Folder { get; set; }
            public bool Monitor { get; set; }
            public double Used { get; set; }
        }

        public static float CheckDriveStorage(Drive drive)
        {
            DriveInfo[] diskDrives = DriveInfo.GetDrives();
            float freeSpace = 0;

            foreach (DriveInfo diskDrive in diskDrives)
            {
                if (diskDrive.IsReady == true && diskDrive.Name == drive.DriveLetter)
                    freeSpace = 100 - ((diskDrive.AvailableFreeSpace / (float)diskDrive.TotalSize) * 100);
            }

            return freeSpace;
        }

        public static void CheckDriveHealth(Drive drive)
        {
            ManagementScope managementScope = new ManagementScope(string.Format("\\\\{0}\\root\\cimv2", Bus.EnvironmentName));
            ManagementObjectSearcher managementScopeSearcher = new ManagementObjectSearcher("SELECT * FROM WIN32_DiskDrive");
            string[] failureCodes = { "Error", "Degraded", "Unknown", "Pred Fail", "Stressed", "NonRecover", "No Contact", "Lost Comm" };

            managementScope.Connect();
            managementScopeSearcher.Scope = managementScope;

            foreach (ManagementObject queryObj in managementScopeSearcher.Get())
            {
                string status = queryObj["Status"].ToString();
                drive.DriveStatus = status;

                if (failureCodes.Contains(status))
                {
                    drive.ErrorDetected = true;
                    Notification notification = new Notification()
                    {
                        Name = "HDD Error",
                        Contents = string.Format("{0} has experienced an error, Error: {1}", drive.DriveLetter, drive.DriveStatus),
                        Priority = 3
                    };

                    if (!NotificationManager.NotificationList.Contains(notification))
                        NotificationManager.AddNotification(notification);
                }
            }

            managementScopeSearcher.Dispose();
        }
    }

    class BusCCTV
    {
        public static void Start()
        {
            CheckDriveHealth();
            CheckHDDUsage();
        }

        public static void CheckDriveHealth()
        {
            Bus.Model.CCTVDrives.ForEach(drive =>
            {
                if (drive.Monitor)
                    CCTVManager.CheckDriveHealth(drive);
            });
        }

        public static void CheckHDDUsage()
        {
            Bus.Model.CCTVDrives.ForEach(drive =>
            {
                if (drive.Monitor)
                    drive.Used = Math.Round(CCTVManager.CheckDriveStorage(drive));
            });
        }
    }
}
