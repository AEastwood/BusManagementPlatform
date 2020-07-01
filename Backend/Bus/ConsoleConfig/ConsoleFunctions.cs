using System;

namespace Bus
{
    class ConsoleFunctions
    {
        internal static string WriteWithColour(ConsoleColor color, string text)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = ConsoleColor.Gray;
            return string.Empty;
        }

    }
}
