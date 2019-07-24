using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace example.thread.demo
{

    /// <summary>
    /// 简单自旋锁
    /// </summary>
    public class SimpleSpinLock
    {
        private int resourceInUse; // 0=false,1=true

        public void Enter()
        {
            while (true)
            {
                // 资源正在使用
                // "未使用"变成"正在使用"才返回
                if (Interlocked.Exchange(ref resourceInUse, 1) == 0)
                {
                    return;
                }
            }
        }

        public void Leave()
        {
            // 标记资源"未使用"
            Volatile.Write(ref resourceInUse, 0);
        }

    }
}
