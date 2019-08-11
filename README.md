- #### 线程的开销
```
  1.线程内核对象
  2.线程环境块
  3.用户模式栈
  4.内核模式栈
  5.Dll线程链接和线程分离通知
  6.线程上下文切换
```

- #### 使用线程的理由
```
1.可响应性（对于客户端GUI应用程序）
以浪费系统资源和损害性能为代价，增强应用程序总体使用体验
2.性能（客户端和服务器应用程序）
```
 
- #### 创建线程
```
 构造Thread对象是轻量级的操作，因为并不实际创建一个操作系统线程
 真正创建操作系统线程，并开始执行回调方法，必须调用Thread的Start方法
```

- #### 前、后台线程
```
CLR将每个线程要么视为前台线程，要么视为后台线程
一个进程的所有前台线程停止运行时，CLR会强制终止仍在运行的任何后台线程
创建新线程默认为前台线程，可以设置线程对象的IsBackground属性为true使其成为后台线程
```

- #### 线程池
```
1.创建和销毁线程是昂贵操作，耗费大量时间，另外，太多的线程会浪费内存资源
操作系统必须调度可运行的线程并执行上下文的切换，太多的线程还对性能不利
2.每个CLR都有自己的线程池,线程池是应用程序能使用的线程集合
3.CLR初始化时，线程池中是没有线程的，线程池在内部维护一个操作请求队列
线程池没有线程就会创建一个新线程，当线程池线程完成任务后，线程不会被销毁
进入空闲状态，等待响应另一个请求，销毁线程也要产生额外的性能损失
4.当线程池收到多个请求任务，会尝试只用一个线程服务所有请求
如果应用程序发起的请求速度超过了线程池线程处理的速度，就会创建额为的线程
所有的请求都有少量的线程处理，线程池不必创建大量的线程
5.如果应用程序停止向线程池发起请求，就会产生大量空闲线程，
线程会自己醒来终止自己以释放资源=
```

- #### 执行上下文
```
1.每个线程都关联了一个执行上下文数据结构
2.执行上下文包括：
  安全设置（压缩栈、Thread的Principal属性和Windows身份）
  宿主设置（HostExceptionContextManager）
  逻辑调用上下文数据（logicalSetData、logicalGetData）
3.CLR自动造成初始线程的执行上下文“流向”任何辅助线程
  阻止执行上下文流动可以提升应用程序的性能
```

- #### 任务（Task）
```
1.Task相比较ThreadPool，可以知道线程操作什么时候完成，在操作完成时获得返回值
2.Task的创建：
  new Task(Action，Object).Start()
  new Task(Func<TArg,TResult>,Object).Start()
  Task.Run(Action)
3.调用Wait方法可以等待任务完成并获取结果
4.ContinueWith方法在任务完成时会自动启动新任务，主线程不会进入阻塞状态等待任务完成
5.任务支持父子关系，调用ContinueWith创建Task时，可指定TaskContinuationOptions.AttachedToParent标志将延续任务指定完成子任务
```
- #### 任务状态
```
1.Created：任务已显式创建，可以手动Start()这个任务
2.WaitingForActivation：任务已隐式创建，会自动开始
3.WaitingToRun：任务已调度，但尚未运行
4.Running：任务正在运行
5.WaitingForChildrenToComplete：任务正在等待子任务完成，子任务完成后才完成
6.RunToComplete：运行完成
7.Canceled：任务取消
8.Faulted：任务出错
```

- #### 任务工厂
```
创建一组共享相同配置的Task对象，可以通过创建一个任务工厂来封装通用的配置
TaskFactory：创建一组返回Void的任务
TaskFactory<TTresult>：创建一组具有特定返回类型的任务
```

- #### 任务执行者（TaskScheduler）
```
TaskScheduler对象负责执行被调度的任务 
1.池任务调度器(hread pool task scheduler),应用程序默认的任务调度器
2.上下文任务调度器(ynchronization context task scheduler),适合提供图形用户界面的应用程序，将所有任务调度给应用程序的GUI线程，
使所有任务都能更新UI组件，该调度器不使用线程池；程池线程更新UI组件时，也会
抛出InvalidOperationException
```
```
如果有特殊的任务调度需求，可以定义自己的TaskScheduler派生类
Parallel Extensions Extras包中提供了大量的任务调度器
1.IOTaskScheduler：将任务排队给线程池的I/O线程而不是工作者线程
2.LimitedConcurrencyLevelTaskScheduler：不允许超过n个任务同时执行
3.OrderedTaskScheduler：一次只允许一个任务执行
4.PrioritizingTaskScheduler：将任务送入CLR线程池队列
5.ThreadPerTaskScheduler：为每个任务创建并启动一个单独的线程，完全不使用线程池
```

- #### 执行定时器
```
1.System.Threading.Trimer
在一个线程池线程上执行定时的后台任务
线程池内部为所有的Timer对象只使用一个线程，这个线程知道下一个Timer对象什么时候到期，下一个timer对象到期，线程就会唤醒，在内部调用ThreabPool的QueueUserWorkItem，
将一个工作项添加到线程池的队列中
Trimer的change方法可以重新指定dueTime参数
2.System.Windows.Forms.Timer
当这个计时器触发时，winForm应用程序将一条计时器消息注入线程的消息队列中
线程必须执行消息泵来提取这些消息，并派发给需要的回调方法
这些工作都只由一个线程成年，计时器方法不会由多个线程并发执行
3.System.Windows.Threading.DispatcherTimer
Silverlight和WPF应用程序相较于WinForm的等价物
4.Windows.UI.Xaml.DispatcherTimer
Windows Store应用中的等价物
```

- #### 异步函数
```
《1》将方法标记为async，编译器会将方法的代码转换为实现了状态机的一个类型
允许线程执行状态机中的代码并返回，不需要一直执行到结束，支持挂起和恢复
《2》await操作符内部会分配一个Task对象执行任务，并在该Task对象上调用ContinueWith
传递用于恢复状态机的方法
```

- #### 线程同步
```
锁的全部意义在于一次只允许一个线程访问资源
Framework Class Libary保证所有的静态方法都是线程安全的 
```

```
《1》基元用户模式构造：速度快，使用特殊CPU指令来协调线程，协调是在硬件中发生的
用户模式中运行的线程可能被系统抢占，但线程会以最快的速度再次调度
想要取得资源但暂时取不到的线程会一直在用户模式“自旋”
《2》基元内核模式构造：线程通过内核模式的构造获取其他线程拥有的资源，windows会阻塞线程以避免浪费CPU时间。当资源可用时，Windows恢复线程，允许访问资源
《3》混合构造:在没有竞争的情况下，构造（线程）快而且不会被阻塞
存在构造竞争，构造（线程）被操作系统内核阻塞  
```

- #### 用户模式构造
```
两种基元用户模式线程同步构造
《1》易变构造
在特定的时间，在包含一个简单数据类型的变量上执行原子性的读或者写操作
public static class Volatile{
    public static void Write(ref int location,int value);
    public static Read(ref int location);
}
volatile.Write强迫location中的值在调用时写入
vlatile.Read强迫location中的值调用时读取
volatile关键字用于任何类型的静态或实列字段
volatile告诉C#和JIT编译器对易变字段的访问，都是以易变读取或写入的方式执行
不将字段缓存到CPU的寄存器，确保字段的所有读写操作都在RAM中进行

《2》互锁构造
在特定的时间，在包含一个简单数据类型的变量上执行原子性的读并且写操作
public static class Interlocked{
    public static int Increment(ref int location);
    public static int Decrement(ref int location);
    public static int Exchange(ref int location,int value);
}
Interlocked类中的每个方法都执行一次原子读取以及写入操作
Interlocked的所有方法建立了完整的内存栅栏
调用Interloked方法前任何变量写入都在方法调用之前执行，调用之后的任何变量读取都在调用之后读取
```

- #### 自旋锁
```
《1》Interlocked方法主要操作Int32值，如果需要原子性的操作类对象中的一组字段，
需要采取办法阻止所有线程，只允许其中一个进入对字段进行操作的代码区域
《2》可用Interlocked的方法构造一个线程同步块
存在对锁竞争的情况，会造成其他线程“自旋”，会浪费宝贵的CPU时间，阻止CPU做更有用的工作，自旋锁适用于执行的非常快的代码区域
《3》如果占用锁的线程的优先级低于想要获取锁的线程（自旋线程），由于占用锁线程的优先级较低，根本没机会运行，无法释放占用的锁，自旋线程有得不到锁，造成“活锁”情形
```

- #### 内核模式构造
```
《1》内核模式构造同步线程，要比用户模式构造慢多
内核对象上调用的每个方法都造成调用线程从托管代码转化为本机（native）用户模式代码
在转换为本机（native）内核模式代码，并且还要朝相反的方向一路返回
《2》检测到一个资源的竞争时，windows会阻塞等待的线程，使它不占着CPU的“自旋”，无谓的浪费处理资源
《3》可以实现本机（native）和托管（managed）线程相互之间的同步
《4》可以同步在同一机器的不同进程中运行的线程
《5》线程可以一直阻塞，直到集合中的任何内核模式构造可用
《6》阻塞线程可以设置超时值，指定时间内访问不到希望的资源，线程可以解除阻塞并执行其他任务
《7》事件和信号量是两种基元内核模式线程同步构造
```
- #### 事件
```
System.Threading.WaitHandle事件的抽象基类，唯一作用包装一个Windows内核对象句柄
在内核模式的构造上调用的每一个方法都代表一个完整的内存栅栏
《1》WaitOne：让调用线程等待底层内核对象收到信号，如果对象收到信号，返回true，获取资源占用权，超时返回false
《2》WaitAll：让调用线程等待WaitHandle【】中指定的所有内核对象都收到信号，所有对象都收到信号返回true，超时返回false
《3》WaitAny：让调用线程等待WaitHandle【】中指定的任何内核对象收到信号，返回int是与收到的信号内核对象对应的数组元素索引，没有收到信号，返回WaitTimeout
《4》Dispose：关闭底层内核对象句柄
《5》AutoResetEvent、ManualResetEvent、Semaphore和Mutex类都派生自WaitHandle
《6》内核模式构造的一个用途是在任何时刻只允许一个应用实例在运行
```

- #### Event 构造
```
public class EventWaitHandle : WaitHandle{
    public Boolean Set();
    public Boolean Reset();
}
```
```
事件构造只是由内核维护的Boolean变量
事件为false，在事件上等待的线程就阻塞，事件为true，就解除阻塞
```
```
自动重置事件：当事件为true时，只唤醒一个阻塞的线程，在解除第一个线程的阻塞后，内核将事件自动重置回false，其余线程继续阻塞
手动重置线程：当事件为true时，解除所有正在等待的线程的阻塞，内核不将事件手动重置回false
可使用自动重置事件轻松创建线程同步锁
```

- #### Semaphone 构造
```
 public sealed class Semaphore: WaitHandle{
    public Semaphore(int initialCount,int maxNumCount);
    public int Release(); // Release(1) 返回上一个计数
    public int Release(int releaseCount); //返回上一个计数 
 }
```
```
《1》信号量是内核维护的Int32变量
《2》信号为0时，在信号量上等待的线程会阻塞，信号量大于0时解除阻塞
《3》信号量上等待的线程解除阻塞，内核自动从信号量计数中减1
《5》信号量的当前计数绝不允许超过最大计数
《6》在信号量上连续多次调用release，会使它的内部计数一直递增，这可能解除大量线程的阻塞
《7》如果在一个信号量上多次调用Release，会导致它的计数超过最大计数
Release会抛出一个SemaphoreFullException
《8》自动重置事件在行为上和最大计数为1的信号量非常相似
```
```
三种内核模式基元的行为：
- 多个线程在一个自动重置事件上等待，设置事件只导致一个线程被解除阻塞
- 多个线程在一个手动重置事件上等待，设置事件导致所有线程解除阻塞
- 多个线程在一个信号量上等待时，释放信号量导致releaseCount个线程被解除阻塞
```

- #### Mutex 构造
```
public sealed class Mutex : WaitHandle{
    public Mutex();
    public void RealeaseMutex();
}
```
```
《1》互斥体（mutex）代表一个互斥锁，工作方式和AutoResetEvent或者计数为1的Semaphore相似，都是一次只释放一个正在等待的线程
《2》Mutex对象会查询调用线程的Int32ID，记录那个线程获取它，当线程调用ReleaseMutex释放互斥体时，Mutex确保调用线程就是获取Mutex的那个线程
《3》拥有Mutex的线程因为任何原因而终止，在Mutex上等待的某个线程会捕获到AbondonedMutexException异常而被唤醒，该异常通常会成为未处理的异常，从而终止整个进程，否则，如果获取Mutex的线程是在更新Mutex保护的数据之前终止的，其他被唤醒的线程可能试图访问损坏的数据，造成无法预料的结果和安全隐患
《4》Mutex对象维护这一个递归计数，指出获取Mutex的线程拥有的‘锁’次数，如果线程在拥有了Mutex之后，而后线程再次在Mutex上等待，计数器就会递增，这个线程允许继续运行。线程调用RelesaeMutex将导致计数器递减，只有计数器变为0时，另一个线程才能获取Mutex对象
《5》Mutex对象需要更多的内存来容纳额外的线程Id和计数休息，同时，Mutex代码必须维护这些信息，使锁变得更慢，多数情况下避免使用Mutex对象
```

- #### 混合线程同步构造
```
混合线程同步构造：合并了基元用户模式和内核模式。
没有线程竞争时，混合构造提供了基元用户模式所具有的性能优势
多个线程竞争一个构造时，混合构造通过基元内核模式的构造提供不“自旋”的优势
```

- #### FCL中的混合构造
```
FCL中自带了许多构造，有的直到首次有线程在一个构造上发生竞争时，才会创建内核模式的构造
许多构造还支持CancellationToken，使一个线程强迫解除可能在构造上等待的其他线程阻塞
```

```
ManualResetEventSlim和SemaphoreSlim构造的工作方式和对应的内核模式构造完全一致，
只是它们都在用户模式中“自旋”，而且都推迟到第一次发生竞争时，才创建内核模式的构造
```

- #### Monitor类和同步块
```
public static class Monitor {
    public static void Enter(Object obj);
    public static void Exit(Object obj);
}
```
```
同步的定义：当两个或更多个线程需要存取共同放入资源时，
          必须确定在同一时间点只有一个线程能存取共同的资源，实现这个目标的过程为“同步”
```
```
《1》最常用的混合型线程同步构造，提供自旋、线程所有权和递归的互斥锁。
《2》堆中的每个对象都可以关联一个“同步块”的数据机构。
    同步块包括：内核对象、拥有线程的ID、递归计数以及等待的线程计数等字段
《3》Monitor是静态类，它的方法可以对指定对象的同步块字段进行修改
《4》CLR初始化时在堆中分配一个同步块数组
    调用Monitor.Enter时，CLR在数组中找到一个空白同步块，并设置该对象的同步块索引，同步块和对象就动态关联起来类。
    调用Monitor.Exit时，会检查是否有其他任何线程正在等待使用对象的同步块，
    如果没有线程等待，同步块就自由了，Exit将对象的同步块索引设回-1，自由的同步块可以和另一个对象关联
《5》Monitor方法获取一个Object，传递值类型会导致值类型被装箱，
    造成线程在一装箱对象上获取锁，每次Enter都会在完全不同的对象上获取锁，无法实现线程的同步
《6》在一个方法中经常会获取一个锁，做一些工作，然后释放锁，
    C#通过lock关键字提供了简化的语法
```

- #### ReaderWriterLockSlim类
```
《1》如果多个线程同时以只读的方式访问数据，没不要阻塞线程，允许并发的访问数据。
如果线程希望修改数据，线程就需要对数据的独占式访问
《2》一个线程向数据写入时，请求访问的其他所有线程被阻塞
《3》一个线程从数据读取时，请求读取的线程允许继续执行，请求写入的线程被阻塞
《4》向线程写入锁结束时，要么解除一个写入线程的阻塞，要么解除所有读取线程的阻塞
《5》读取数据的所有线程结束后，一个Write线程被解除阻塞
《6》reader-writer锁通常比Monitor慢
```

- #### 双检锁
```
延时初始化：将单实例对象的构造推迟到应用程序首次请求该对象时进行，
当多个线程同时请求对象时，必须使用一些线程同步机制确保单实例对象只被构造一次
```
```
FCL提供了System.Lazy类来实现延迟加载
public class Lazy<T>{
    public Lazy(Fun<T> valueFactory,LazyThreadSafetyMode mode);
    public Boolean IsValueCreated {get;}
    public T Value {get;}
}
```
- LazyThreadSafetyMode 标志

标志项 | 说明
---|---
None | 完全没有线程安全支持
ExcutionAndPublication | 使用双检锁技术
PublicationOnly | 使用Interlocked.CompareExchange技术

- #### 条件变量模式
```
一个线程希望在一个复合条件为True时执行一些代码，这个模式称为条件变量模式
Monitor定义了方法来使用该模式
```
```
public static class Monitor{
    public static Boolean Wait(Object obj);
    public static Boolean Wait(Object obj,Int timeout);
    public static void Pulse(Object obj);
    public static void PulseAll(Object obj);
}
pulse只解除一个等待最久的线程
pulseAll解除所有正在等待的线程阻塞
```
- #### 并发集合类
```
《1》FCL提供了4个线程安全的集合类，这些集合类是“非阻塞”的
如果一个线程试图获取一个不存在的元素，线程会立即返回，不会阻塞，等着一个线程的出现
《2》ConcurrentDicitionary类内部使用Monitor，对集合操作时，锁只占用极短的时间
《3》ConcurrentQueue和ConcurrentStack不需要锁，内部使用InterLocked的方法操纵集合
《4》ConcurrentBag由大量迷你集合对象构成，每个线程一个。
线程将元素添加到bag时，就用Interlocked的方法将数据项添加到调用线程的迷你集合中。
线程从bag中获取元素时，bag检查通用线程的迷你集合，尝试获取数据。
如果数据在当前线程的迷你集合中，就用Interlocked提取数据
如果数据不存在，就在内部获取一个Monitor，以便从另一个线程的迷你集合中获取元素。这时一个线程从另一个线程的“窃取”
《5》ConcurrentStack、Queue、Bag通过GetEnumerator方法都是获取集合内容的一个“快照”，
实际集合中的数据可能在使用快照进行遍历时已经发生了改变
《6》ConcurrentDictionary的GetEnumeratorh获的不是bag的快照，在集合遍历时，字典中的数据可能改变了
《7》Count属性查询bag中元素的数量，如果其他线程在集合中增删元素，这个计数也不准确了
《8》ConcurrentStack、Queue、Bag都实现了IProducerConsumerCollection接口
实现这个接口的任何类都变成一个阻塞集合
如果集合已满，负责生产数据项的线程会阻塞
如果集合已空，负责消费数据项的线程会阻塞
```
