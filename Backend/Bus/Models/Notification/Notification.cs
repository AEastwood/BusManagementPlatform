namespace Bus.Notifications
{
    class Notification
    {
        public int Id { get; } = NotificationManager.NotificationList.Count + 1;
        public string Name { get; set; }
        public string Contents { get; set; }
        public int Priority { get; set; } = 0;
        public bool Read { get; set; } = false;
        public bool RequiresAction { get; set; }
    }
}
