using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace example.thread.demo._10_DoubleCheckDemo
{
    /// <summary>
    /// 单例构造
    /// </summary>
    public class Singleton1
    {
        private static object s_lock = new object();

        private static Singleton1 instance = null;

        private Singleton1()
        {
            // 可以把单例对象的初始化放在这里
        }

        public static Singleton1 GetInstance()
        {
            // 已经创建，直接返回
            if (instance != null)
            {
                return instance;
            }
            // 进入临界区
            Monitor.Enter(s_lock); // 使一个线程创建对象
            // 创建对象
            Singleton1 temp = new Singleton1();
            // 使创建的对象，对其他线程“可见”
            Volatile.Write(ref instance, temp);
            Monitor.Exit(s_lock);

            return instance;
        }
    }
}
