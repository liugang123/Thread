using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace example.thread.demo.MonitorDemo
{

    /// <summary>
    /// 私有锁同步
    /// </summary>
    public sealed class TransactionDemo
    {
        // 每个Tansaction都有私有锁
        private readonly Object lockObj = new Object();

        // 临界资源
        private DateTime timeOfLastTrans;

        /// <summary>
        /// 同步访问资源
        /// </summary>
        public void PerformTransaction()
        {
            // 进入私有锁
            Monitor.Enter(lockObj);
            // 对资源独占访问
            timeOfLastTrans = DateTime.Now;
            // 退出私有锁
            Monitor.Exit(lockObj);
        }

        public DateTime LastTransaction()
        {
            // 进入私有锁
            Monitor.Enter(lockObj);
            // 对数据独占访问
            DateTime temp = timeOfLastTrans;
            // 退出私有锁
            Monitor.Exit(lockObj);
            return temp;
        }


    }
}
