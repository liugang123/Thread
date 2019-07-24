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
    public class Singleton3
    {
        private static Singleton3 instance = null;

        private Singleton3()
        {

        }

        public static Singleton3 GetInstance()
        {
            if (instance != null)
            {
                return instance;
            }
            // 创建对象，并固定下来
            Singleton3 temp = new Singleton3();
            Interlocked.CompareExchange(ref instance, temp, null);

            // 如果线程竞争失败，新建的第二个单例对象会被垃圾回收
            return instance;
        }

    }
}
