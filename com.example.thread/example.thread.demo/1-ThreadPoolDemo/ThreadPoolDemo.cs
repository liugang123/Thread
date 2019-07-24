using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace example.thread.demo
{
    public class ThreadPoolDemo
    {
        /// <summary>
        /// 入口函数
        /// </summary>
        public static void Test()
        {
            ThreadPool.QueueUserWorkItem(ComputeBoundOp, 5);
            Console.WriteLine("main thread doing other work here ....");
            Thread.Sleep(1000 * 5);
        }

        /// <summary>
        /// WaitCallBack委托
        /// </summary>
        private static void ComputeBoundOp(Object state)
        {
            Console.WriteLine("线程池执行方法中 ....,state :" + state);
            Thread.Sleep(1000);
            Console.WriteLine("线程池执行方法结束，等待请求任务");
        }

    }
}
