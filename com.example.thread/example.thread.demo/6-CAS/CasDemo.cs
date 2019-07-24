using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace example.thread.demo
{

    public delegate int Morpher<TResult, TArg>(int startValue, TArg arg, out TResult morphResult);

    /// <summary>
    /// 乐观并发
    /// </summary>
    public class CasDemo
    {

        public static int Maximum(ref int target, int value)
        {
            int currentVal = target;
            int startVal;
            int desiredVal;

            // 不在循环中修改taget的值
            do
            {
                // 循环迭代的起始值
                startVal = currentVal;

                // 基于startVal 和 value 计算desiredVal
                desiredVal = Math.Max(startVal, value);

                // 线程可能被占用，代码需要原子性
                //if (target == startVal)
                //{
                //    target = desiredVal;
                //}
                currentVal = Interlocked.CompareExchange(ref target, desiredVal, startVal);

                // terget 值被修改，就进行重复 
            } while (startVal != currentVal);

            return desiredVal;
        }

        public static TResult Morph<TResult, TArg>(ref int target, TArg arg, Morpher<TResult, TArg> morpher)
        {
            TResult morphResult;
            int currentVal = target;
            int startVal;
            int desiredVal;
            do
            {
                startVal = currentVal;
                desiredVal = morpher(startVal, arg, out morphResult);
                currentVal = Interlocked.CompareExchange(ref target, desiredVal, startVal);
            } while (startVal != currentVal);
            return morphResult;
        }

    }
}
