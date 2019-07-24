using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace example.thread.demo.LockDemo
{
    /// <summary>
    /// 使用AutoResetEvent(自动重置事件)实现线程同步锁
    /// </summary>
    public class SimpleWaitLock : IDisposable
    {
        private readonly AutoResetEvent autoResetEvent;

        public SimpleWaitLock()
        {
            autoResetEvent = new AutoResetEvent(true); // 开始自由使用 
        }

        public void Enter()
        {
            // 在内核中阻塞，直到资源可用
            autoResetEvent.WaitOne();
        }


        public void Leave()
        {
            // 让另一个线程访问资源
            autoResetEvent.Set();
        }


        public void Dispose()
        {
            autoResetEvent.Dispose();
        }
    }
}
