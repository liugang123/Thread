using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace example.thread.demo
{

    public class VolatileDemo
    {
        private static object m_lock = new object();

        private static int threadCount = 20;

        private static volatile int race = 0;

        private static void Increment()
        {
            Monitor.Enter(m_lock);
            race++;
            Monitor.Exit(m_lock);
        }

        public static void Run()
        {
            Thread[] threads = new Thread[threadCount];
            for (int i = 0; i < threadCount; i++)
            {
                threads[i] = new Thread(() =>
                {
                    for (int y = 0; y < 1000; y++)
                    {
                        Increment();
                    }
                });
                threads[i].Start();
            }

            while (true)
            {
                for (int i = 0; i < threadCount; i++)
                {
                    if (threads[i].ThreadState != ThreadState.Stopped)
                    {
                        continue;
                    }
                }
                break;
            }
            Console.WriteLine("累加结果:" + race);
        }
    }
}
