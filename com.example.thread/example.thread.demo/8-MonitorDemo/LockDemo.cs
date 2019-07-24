using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace example.thread.demo._8_MonitorDemo
{
    /// <summary>
    /// lock关键字用法 
    /// </summary>
    public class LockDemo
    {
        private void SomeMethod()
        {
            lock (this)
            {
                // 对数据独占访问
            }
        }

        private void SomeMethod1()
        {
            Boolean lockToken = false;
            try
            {
                // 可能异常：threadAbortException，在未获取锁之前就可能退出，finally会得到调用，但不应退出锁
                Monitor.Enter(this, ref lockToken);  // Enter方法成功获取锁，会将lockToken设为True
                // 对数据独占访问
            }
            finally
            {
                if (lockToken)
                {
                    Monitor.Exit(this);
                }
            }
        }

    }
}
