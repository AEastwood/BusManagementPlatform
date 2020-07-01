using System;
using System.Collections.Generic;
using System.IO;
using System.Management;

namespace Bus
{

    class CCTV
    {
        public class Drives
        {
            public string Drive { get; set; }
            public bool FailureDetected { get; set; } = false;
            public bool FailureStatus { get; set; } = false;
            public string Folder { get; set; }
            public bool Monitor { get; set; }
            public double Used { get; set; }
        }

        private static ManagementObject wmiDrive = new ManagementObject("Win32_LogicalDisk.DeviceID='C:'");

        internal static float CheckDrive(string path)
        {
            DriveInfo[] diskDrives = DriveInfo.GetDrives();
            float freeSpace = 0;

            foreach (DriveInfo diskDrive in diskDrives)
            {
                if (diskDrive.IsReady == true)
                {
                    if (diskDrive.Name == path)
                        freeSpace = 100 - ((diskDrive.AvailableFreeSpace / (float)diskDrive.TotalSize) * 100);

                    //wmiDrive = GetObject($"WinMgmts:Win32_LogicalDisk='{diskDrive.Name}'");
                }
            }

            return freeSpace;
        }
    }

    class BusCCTV
    {
        public static void Start()
        {
            SetCCTVStorageCalculations();
        }

        public static void SetCCTVStorageCalculations()
        {
            List<CCTV.Drives> drives = Bus.Model.CCTVPaths;

            drives.ForEach(drive =>
            {
                drive.Used = Math.Round(CCTV.CheckDrive(drive.Drive));
            });
        }

    }
}
