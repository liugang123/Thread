using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace example.thread.demo
{
    /// <summary>
    /// 计数线程
    /// </summary>
    public class StrangeBehavior
    {
        private static bool stopWorker = false;

        public static void Test()
        {
            Console.WriteLine("计数线程运行5秒....");
            Thread t = new Thread(Worker);
            t.Start();
            Thread.Sleep(1000 * 5);
            stopWorker = true;
            Console.WriteLine("等待计数线程结束");
            // join等待worker线程终止
            t.Join();
        }

        /// <summary>
        /// 计数线程
        /// </summary>
        /// <param name="o"></param>
        private static void Worker(Object o)
        {
            int value = 0;
            while (stopWorker)
            {
                value++;
            }
            Console.WriteLine("计数线程停止，value=" + value);
        }
    }
}
