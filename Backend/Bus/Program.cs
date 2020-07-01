using Bus.Functions;
using System;
using System.Threading.Tasks;

namespace Bus
{
    class Program
    {
        static void Main()
        {
            Task busCommandTask = new Task(() => BusFunctions.StartBackend());
            busCommandTask.Start();
            Console.Read();
        }

    }
}
