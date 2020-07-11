using System;

namespace Bus.Commands
{
    class Command
    {
        public string CommandName { get; set; } = string.Empty;
        public string[] CommandParamaters { get; set; }
        public string Token { get; set; }
        public bool Logged { get; set; } = true;
    }

    public class CommandCallback
    {
        public string Command { get; set; }
        public object Data { get; set; }
        public TimeTaken TimeTaken { get; set; }

    }

    public class TimeTaken
    {
        public TimeSpan Elapsed { get; set; }
        public long ElapsedMilliseconds { get; set; }
        public long ElapsedTicks { get; set; }
        public bool IsRunning { get; set; }
    }
}
