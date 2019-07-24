using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace example.thread.demo
{
    public class TaskFactoryDemo
    {
        public static void Test()
        {
            Task parent = new Task(() =>
            {
                // 初始化任务工厂
                var cts = new CancellationTokenSource();
                var tFactory = new TaskFactory<int>(cts.Token,
                                                    TaskCreationOptions.AttachedToParent,
                                                    TaskContinuationOptions.ExecuteSynchronously,
                                                    TaskScheduler.Default);
                // 创建任务，启动3个子任务
                var childTasks = new[]
                {
                    tFactory.StartNew(() => Sum(cts.Token,10000)),
                    tFactory.StartNew(() => Sum(cts.Token,20000)),
                    tFactory.StartNew(() => Sum(cts.Token,int.MaxValue))
                };
                // 任何子任务异常，取消子任务
                for (int task = 0; task < childTasks.Length; task++)
                {
                    childTasks[task].ContinueWith(t => cts.Cancel(), TaskContinuationOptions.OnlyOnFaulted);
                }
                // 子任务完成后，获取返回的最大值
                // 将最大值传给另一个任务显示最大结果,这个任务也是子任务（由tFactory创建）,不会被取消
                tFactory.ContinueWhenAll(childTasks,
                                         completeTasks => completeTasks.Where(w => !w.IsFaulted && !w.IsCanceled).Max(t => t.Result),
                                         CancellationToken.None)
                        // 创建任务,显示计算结果
                        .ContinueWith(t => Console.WriteLine("The max number is :" + t.Result),
                                       TaskContinuationOptions.ExecuteSynchronously);
            });

            // 显示未处理异常
            // 父任务可能和子任务并行执行
            parent.ContinueWith(p =>
            {
                StringBuilder sBuild = new StringBuilder();
                foreach (var e in p.Exception.Flatten().InnerExceptions)
                {
                    sBuild.AppendLine(e.GetType().ToString());
                }
                Console.WriteLine(sBuild.ToString());
            }, TaskContinuationOptions.OnlyOnFaulted);

            // 启动任务
            parent.Start();
        }

        private static int Sum(CancellationToken token, int value)
        {
            return value;
        }
    }

}
