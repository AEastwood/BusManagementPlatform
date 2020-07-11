using System;
using System.Collections.Generic;

namespace Bus
{
    class Bus
    {
        internal static readonly string AuthToken = "cb4c4f5720755aa656526c31891b60f7f80f97daaf0e91a329b9823ce0db91a0a356e53377fe9aa8be69e98aedad28e3263f8a42258dfc5ddc4f8d27b01e9b85";
        internal static readonly int checkDelay = 1;
        internal static readonly string EnvironmentName = System.Environment.MachineName;
        internal static readonly string TrackerAPIKey = "cb4c4f5720755aa656526c31891b60f7f80f97daaf0e91a329b9823ce0d";
        internal static BusConfig Model { get; set; }
        
        internal static readonly Dictionary<string, string> serverCert = new Dictionary<string, string>() {
            { "path", @"E:\SteamLogin\cert.pfx" },
            { "secret", "42^kFvZtkf2?8pGza^Lm4mp9aSpL8bEGgcf99qwG*e55rwV=_hJJ$5CjS6f*3hSK" },
        };

        internal static int listenPort = 9287;
    }
}
