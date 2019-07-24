using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace example.thread.demo._11_ConcurrentDemo
{
    /// <summary>
    /// 线程安全队列
    /// </summary>
    public sealed class SynchronziedQueue<T>
    {
        private readonly object m_lock = new object();
        private readonly Queue<T> m_queue = new Queue<T>();

        /// <summary>
        /// 入队
        /// </summary>
        public void Enqueue(T item)
        {
            Monitor.Enter(m_lock);
            // 数据项入读，唤起所有等待的线程
            m_queue.Enqueue(item);
            Monitor.PulseAll(m_lock);
        }

        /// <summary>
        /// 出队
        /// </summary>
        /// <returns></returns>
        public T Dequeue()
        {
            // 进入临界区
            Monitor.Enter(m_lock);
            // 队列为空，一直循环
            while (m_queue.Count == 0)
            {
                Monitor.Wait(m_lock);
            }
            // 队列元素出队
            T item = m_queue.Dequeue();
            Monitor.Exit(m_lock);
            return item;
        }

    }
}
