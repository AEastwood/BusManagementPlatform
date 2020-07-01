namespace Bus.Sensors
{
    class Camera
    {
        public string Name { get; set; }
        public string IPAddress { get; set; }
        public int Port { get; set; }
        public int State { get; set; } = 3;
        public string Username { get; set; }
        public string Password { get; set; }

        public Camera() { }

    }
}
