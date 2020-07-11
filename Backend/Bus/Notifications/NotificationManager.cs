using System.Collections.Generic;

namespace Bus.Notifications
{
    class NotificationManager
    {
        public static List<Notification> NotificationList = new List<Notification>();

        public static void AddNotification(Notification notification)
        {
            NotificationList.Add(notification);
        }

        public static void MarkNotificationAsRead(string notificationId)
        {
            NotificationList.ForEach(notification =>
            {
                if (notification.Id == int.Parse(notificationId))
                    notification.Read = true;
            });
        }

        public static List<Notification> UnreadNotifications()
        {
            if (NotificationList.Count == 0)
                return null;

            List<Notification> notifications = new List<Notification>();

            NotificationList.ForEach(notification =>
            {
                if (!notification.Read)
                    notifications.Add(notification);
            });

            return notifications;
        }
    }
}
