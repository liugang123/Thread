
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace example.thread.demo
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1.ThreadPool
            // ThreadPoolDemo.Test();

            // 2.TaskFactory
            // TaskFactoryDemo.Test();

            // 3.Timer
            // TimerDemo.Test();

            // 4.Lock
            // StrangeBehavior.Test();

            VolatileDemo.Run();

            Console.WriteLine("Main Run Finished");

            Console.ReadLine();
        }

    }
}
