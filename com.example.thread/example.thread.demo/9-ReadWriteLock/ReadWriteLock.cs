using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace example.thread.demo._9_ReadWriteLock
{
    /// <summary>
    /// 读写锁 
    /// </summary>
    public sealed class ReadWriteLock : IDisposable
    {
        // 不允许使用递归锁
        private readonly ReaderWriterLockSlim m_lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

        private DateTime dateTime;

        public void PerformTransaction()
        {
            m_lock.EnterWriteLock();
            // 独占访问资源
            dateTime = DateTime.Now;
            m_lock.ExitWriteLock();
        }

        public DateTime LastTransaction()
        {
            m_lock.EnterReadLock();
            // 拥有数据的共享访问权
            DateTime temp = dateTime;
            m_lock.ExitReadLock();
            return temp;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }


    }
}

