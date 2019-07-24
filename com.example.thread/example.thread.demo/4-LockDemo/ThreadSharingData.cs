using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace example.thread.demo.LockDemo
{
    public class ThreadSharingData
    {
        private int flag = 0;
        private int value = 0;

        public void Thread1()
        {
            value = 5;
            //flag = 1;
            Volatile.Write(ref flag, 1);
        }

        public void Thread2()
        {
            //if (flag == 1)
            if (Volatile.Read(ref flag) == 1)
            {
                Console.WriteLine(value);
            }
        }

    }
}
