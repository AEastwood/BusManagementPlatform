namespace Bus.Notifications
{
    class Notification
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Contents { get; set; }
        public bool Read { get; set; }
        public bool RequiresAction { get; set; }
    }
}
