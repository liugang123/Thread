using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace example.thread.demo
{
    public class AsyncCoordinator
    {
        private int oPCount = 1;
        private int statusReported = 0; // 0=false,1=true
        private Action<CoordinationStatus> callback;
        private Timer timer;

        /// <summary>
        /// 发起操作之前调用
        /// </summary>
        public void AboutToBegin(int opsToAdd = 1)
        {
            Interlocked.Add(ref oPCount, opsToAdd);
        }

        /// <summary>
        /// 处理好一个操作结果之后调用
        /// </summary>
        public void JustEnded()
        {
            if (Interlocked.Decrement(ref oPCount) == 0)
            {
                ReportStatus(CoordinationStatus.AllDone);
            }
        }

        /// <summary>
        /// 所有操作方法发起后调用
        /// </summary>
        public void AllBegun(Action<CoordinationStatus> callback, int timeout = Timeout.Infinite)
        {
            this.callback = callback;
            if (timeout != Timeout.Infinite)
            {
                this.timer = new Timer(TimeExpired, null, timeout, Timeout.Infinite);
            }
            JustEnded();
        }


        public void TimeExpired(Object state)
        {
            ReportStatus(CoordinationStatus.Timeout);
        }

        public void Cancel()
        {
            ReportStatus(CoordinationStatus.Cancel);
        }

        private void ReportStatus(CoordinationStatus status)
        {
            // 如果状态未报告过，就报告，否则忽略
            if (Interlocked.Exchange(ref statusReported, 1) == 0)
            {
                callback(status);
            }
        }
    }

    /// <summary>
    /// 任务状态
    /// </summary>
    public enum CoordinationStatus
    {
        AllDone,
        Timeout,
        Cancel
    }

}
