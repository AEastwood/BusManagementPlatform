using System.Threading.Tasks;
namespace Bus
{
    class BusTracker
    {
        private static bool Authenticated { get; set; }

        public static void Start()
        {
            string apiKey = Bus.TrackerAPIKey;

            if (Authenticated)
                Task.Run(() => UpdateTrackerAPI());
            else
                Task.Run(() => GetAuthentication());
        }

        private static void GetAuthentication()
        {
            Authenticated = true;

            if (Authenticated)
                Start();
        }

        private static void UpdateTrackerAPI()
        {

        }
    }
}
