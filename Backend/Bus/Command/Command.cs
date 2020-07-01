namespace Bus.WebInterface
{
    class Command
    {
        public string CommandName { get; set; } = string.Empty;
        public string[] CommandParamaters { get; set; }
        public string Token { get; set; }
        public bool Logged { get; set; } = true;
    }
}
