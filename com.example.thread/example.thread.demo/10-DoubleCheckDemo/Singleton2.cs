using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace example.thread.demo._10_DoubleCheckDemo
{
    /// <summary>
    /// 单例构造
    /// </summary>
    public class Singleton2
    {
        private static Singleton2 instance = new Singleton2();

        private Singleton2()
        {

        }

        public static Singleton2 GetInstance()
        {
            return instance;
        }

    }
}
