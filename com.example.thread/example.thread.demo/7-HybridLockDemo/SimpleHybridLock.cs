using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace example.thread.demo.HybridLockDemo
{
    /// <summary>
    /// 混合线程同步锁
    /// </summary>
    public sealed class SimpleHybridLock : IDisposable
    {
        // int由基元用户模式构造使用（InterLocked的方法）
        private int waiters = 0;

        // 基元内核模式构造
        private AutoResetEvent waitLock = new AutoResetEvent(false);

        /// <summary>
        /// 进入临界区 
        /// </summary>
        public void Enter()
        {
            // 指出线程想要获取的锁
            if (Interlocked.Increment(ref waiters) == 1)
            {
                // 锁可以自由使用，无竞争，直接返回
                return;
            }

            // 另一个线程拥有锁（发生竞争），使线程等待
            waitLock.WaitOne(); // 产生较大的性能影响
            // waitOne返回后，当前线程获取到锁
        }

        /// <summary>
        /// 退出临界区
        /// </summary>
        public void Leave()
        {
            // 线程准备释放锁
            if (Interlocked.Decrement(ref waiters) == 0)
            {
                // 没有线程等待，直接返回
                return;
            }
            // 有其他线程在等待，唤醒其中一个
            waitLock.Set(); // 产生较大的性能影响
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            waitLock.Dispose();
        }

    }
}
