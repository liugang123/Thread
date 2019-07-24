using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace example.thread.demo
{
    /// <summary>
    /// 定时器
    /// </summary>
    public class TimerDemo
    {
        private static Timer timer;

        public static void Test()
        {
            Console.WriteLine("每两秒执行一次");
            // 创建但不启动计时器
            timer = new Timer(Status, null, Timeout.Infinite, Timeout.Infinite);
            // 启动计时器
            timer.Change(0, Timeout.Infinite);
        }

        /// <summary>
        /// TimerCallback委托匹配
        /// </summary>
        /// <param name="state"></param>
        private static void Status(object state)
        {
            Console.WriteLine("invoke method status at:" + DateTime.Now);
            Thread.Sleep(1000);
            // 让timer在两秒后再触发
            timer.Change(2000, Timeout.Infinite);
            // 线程回归池中，等待下一个工作项
        }
    }

}
