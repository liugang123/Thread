using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace example.thread.demo.SpinLockDemo
{
    public sealed class SomeResource
    {
        private SimpleSpinLock spinLock = new SimpleSpinLock();

        public void AccessResource()
        {
            spinLock.Enter();

            // 一次一个线程访问资源

            spinLock.Leave();
        }
    }

}
