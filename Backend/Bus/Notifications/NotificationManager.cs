using System.Collections.Generic;

namespace Bus.Notifications
{
    class NotificationManager
    {
        public static List<Notification> NotificationList = new List<Notification>();

        public static void Add(Notification notification)
        {
            NotificationList.Add(notification);
        }
    }
}
