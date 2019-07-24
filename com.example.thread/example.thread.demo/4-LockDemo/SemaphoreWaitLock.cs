using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace example.thread.demo.LockDemo
{
    /// <summary>
    /// 信号量实现同步锁
    /// </summary>
    public class SemaphoreWaitLock : IDisposable
    {
        private Semaphore available;

        public SemaphoreWaitLock()
        {
            available = new Semaphore(1, 1);
        }

        public void Enter()
        {
            // 在内核中阻塞直接资源可用
            available.WaitOne();
        }

        public void Leave()
        {
            // 释放资源，让其他线程访问
            available.Release(1);
        }

        public void Dispose()
        {
            available.Close();
        }
    }
}
