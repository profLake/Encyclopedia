/*
 * creation date  25 oct 2021
 * last change    25 oct 2021
 * author         artur
 */
using System;
using System.Threading;

class __SystemThreadingThread
{
    static void Main()
    {
        Console.WriteLine("***** _ *****");

        SystemThreadingThread_Silent();

        Console.ReadLine();
    }
    static void SystemThreadingThread_Silent()
    {
        Console.WriteLine(">->->->->->->->->->->->->->->->->->->   SystemThreadingThread_Silent()\n");


        // class System.Threading.Thread
        //
        //   Это, собственно, - поток. Из него и растёт вся природа потоков в .NET. Как ты видел, он имеет как обычные, так и статические
        //     методы, некоторые из которых создают новые потоки в текущем домене. Есть ещё серия методов для приостановки, остановки и
        //     убийства своего потока. Вот список основных статических членов этого класса:
        //  
        //         CurrentContext  - свойство из .NET Classic (его нету в .NET Core и современном .NET)
        //
        //                           свойство, что выдаст текущей контекст твоего текущего потока (в виде экземпляра типа
        //                           System.Runtime.Remoting.Contexts.Context, не System.AppContext, который статический и предназначен для
        //                           другого. Сам по себе ..Contexts.Context небольшой класс)
        //         CurrentThread   - а это свойство выдаст тебе объект Thread с твоим текущим потоком. Уже видели
        //         GetDomain()     - статический метод, что выдаёт ссылку на текущий домен (типа System.AppDomain). Тоже видели
        //         GetDomainId()   - а этот выдаёт int, хранящий id текущего домена (да, можно просто сначала получить домен из GetDomain(),
        //                           а затем уже из него получить тот же id)
        //         Sleep()         - приостанавливает твой текущий поток (ничего не возвращает)
        //  
        //     А вот часть членов уровня экземпляра:
        //  
        //         IsAlive       - бодрствует ли поток (не спит и не остановлен)(это bool свойство)?
        //         IsBackground  - это фоновый поток (об этом позже)(тоже bool свойство)?
        //         Name          - это string свойство для задания или установки имени потока
        //
        //                              Да, это тоже есть, но не забывай о свойстве ManagedThreadId, что ты видел в методе
        //                              DelegateAsynchrony_..(). у доменов есть схожие свойства FriendlyName и Id
        //
        //                         По умолчанию null
        //
        //                             Это свойство примечательно тем, что способно сильно упростить отладку логики потоков, т.к. во время
        //                             сеанса VS отображает в своём окне Debug -> Windows -> Threads его (чтобы увидеть потоки там, нужен
        //                             breakpoint)
        //
        //         Priority      - свойство типа System.Threading.ThreadPriority для установки или получения приоритета потока. Вот как
        //                         выглядит всё перечисление:
        //                             public enum ThreadPriority
        //                             {
        //                                 Lowest = 0,
        //                                 BelowNormal = 1,
        //                                 Normal = 2,
        //                                 AboveNormal = 3,
        //                                 Highest = 4
        //                             }
        //                         По умолчанию потокам выставляется Normal. Если ты решишь выставить другой уровень приоритета этого
        //                         свойства, то не забывай о том, что для CLR это станет чем-то вроде подсказки о важности потока. Это значит,
        //                         что приоритет System.Threading.ThreadPriority.Highest не гаранитует, что поток получить наивысший
        //                         приоритет. Но всё-таки CLR проинструктирует планировщик о том, как
        //                         лучше выделять кванты времени (при подходящих условия). Потоки с одинаковыми уровнями приоритета должны
        //                         получать одинаковое количество времени на выполнение
        //  
        //                             На самом деле необходимость в изменении приоритетов потоков возникает крайне редко (если вообще
        //                             когда-то).
        //
        //                         Теоретически, неграмотное распределение приоритетов потокам может навредить общей работе приложения
        //         ThreadState   - { get; } свойство для получения состояния потока (в виде одного из значений System.Threading.ThreadState)
        //         Abort()       - метод для поднятия исключение System.Threading.ThreadAbortException, и этим (обычно) останавливают поток.
        //                         Опять же, это только укажет CLR о (очень большой) желательности остановить поток
        //         Interrupt()   - прерывает (приостанавливает) свой поток на подходящее время. msdn же говорит, что поток будет
        //                         прерван, как только он будет приостановлен в следующий раз (или прерван сразу, если он итак в состоянии
        //                         System.Threading.ThreadState.WaitSleepJoin. ****да, я тоже не очень понял как можно прервать уже прерванный
        //                         поток)
        //         Join()        - блокирует вызывающий поток до тех пор, пока не завершится поток этого метода (если вызвать из нашего потока
        //                         метод Join() этого другого объекта, наш поток будет ждать).
        //                         Если применить Join() к текущему потоку, начнётся бесконечный (или вечный) цикл
        //         Resume()      - возобновляет работу ранее приостановленного потока
        //         Start()       - указывает CLR о том, что нужно как можно скорее начать поток
        //         Supsend()     - приостанавливает поток. Если поток уже в этом состоянии, то эффекта метод Supsend() не даст
        //  
        //   Приостановка и прекращение работы (остановка) обычно считаются плохой идеей, т.к. есть шанс (хоть и небольшой), что поток
        //     допустит "утечку" своей рабочей нагрузки (поток забудет переменную или пропустит команду, я думаю)


        // CurrentThreadStatistic
        //
        //   Как мы знаем, у каждого домена всегда есть свой первичный поток. Давай же посмотрим разнообразные статистические данные о
        //     первичном потоке нашего домена!
        //
          Thread primaryThread = Thread.CurrentThread;
          primaryThread.Name = "ThePrimaryThread";
        //
          Console.WriteLine("Name of current AppDomain: {0}", Thread.GetDomain().FriendlyName);
        //Console.WriteLine("ID of current Context: {0}", Thread.CurrentContext.ContextID);  //****имеется лишь в .NET Classic
        //                                                                       // CurrentContext - как уже и говорилось, этим свойством
        //                                                                       //   мы получаем объекты класса
        //                                                                       //   System.Runtime.Remoting.Context.Context. Его члены
        //                                                                       //   можно пересчитать по пальцам одной руки (не считая
        //                                                                       //   стандартных, вроде ToString())
        //                                                                       // System.Threading.Thread.GetDomain() - На самом деле гораздо
        //                                                                       //   проще использовать свойство
        //                                                                       //   System.AppDomain.CurrentDomain
          Console.WriteLine("Thread name: {0}", primaryThread.Name);
          Console.WriteLine("Has thread started?: {0}", primaryThread.IsAlive);
          Console.WriteLine("Priority level: {0}({1})", primaryThread.Priority, (int)primaryThread.Priority);
          Console.WriteLine("Thread state: {0}\n", primaryThread.ThreadState);   // ThreadState - скорее всего это свойство прозвали так в
                                                                                 //   честь перечисления System.Threading.ThreadState
        //


        // ManualThreadsCreation
        //
        //   Вот небольшое пособие о том, как создать собсвтенный поток для выполнения определённой работы:
        //  
        //     > Создай метод, что будет служить точкой входа для нового потока
        //     > Создай новый экземпляр делегата ...ThreadStart (или ParametrizedThreadStart, если ты намерен отправить что-то новому потоку),
        //       передав его конструктору адрес метода, что был создан в пункте 1
        //     > Создай объект ...Thread, передав конструктору делегат ...ThreadStart/...PrametrizedThreadStart в качестве аргумента
        //     > Если нужно, установи начальные характеристики потока (имя, приоритет и т.д.)
        //     > Чтобы начать, вызови метод Start() своего потока
        //  
        //   Как ты понял, используя тип делегата ParametrizedThreadStart, ты можешь передать новому потоку всё, что угодно, т.к.
        //     System.Object может хранить в себе всё, что угодно
        //
        //
        // System.Threading.ThreadStart_Using
        //
        //   Довольно очевидно, что System.Threading.ThreadStart удобен для запуска какого-то дела в фоне без дальнейшего взаимодействия с
        //     новым потоком, а System.Threading.ParameterizedThreadStart подходит для единоразовой передачи ему чего-то. Т.к. различаются
        //     эти пути мало чем, здесь будет показан пример одного из них
        //   Давай посмотрим на использование этого делегата. Поток, что мы создадим здесь, просто будет выводить последовательность чисел
        //     на экран, делая на каждом шаге паузу в примерно 2 секунды
        //
            void PrintNumbers()
            {
                Console.WriteLine("{0} is executing PrintNumbers()", Thread.CurrentThread.Name);
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine(i);
                    Thread.Sleep(2000);
                }
            }
        //
            Thread.CurrentThread.Name = "Primary";
            Thread backgroundThread = new Thread(new ThreadStart(PrintNumbers));
            backgroundThread.Name = "Secondary";
            backgroundThread.Start();
                                                  // new Thread() - у этого класса есть 4 простых конструктора: ThreadStart или
                                                  //   ParameterizedThreadStart, или они же, но с указанием максимального размера стёка
                                                  // Start() - всего две версии: одна без параметров для запуска потока с делегатом ThreadStart
                                                  //   , вторая с параметрами, для запуска потока с делегатом ParameterizedThreadStart
        //
            Thread.Sleep(1000);                   // Thread.Sleep() - лучше подождать, пока второстепенный поток войдёт в строй, иначе с
            for (int _ = 0; _ < 4; _++)           //   большой вероятностью главный поток выведет своё сообщение быстрее
            {
                Console.WriteLine("{0} - I still exist!", Thread.CurrentThread.Name);
                Thread.Sleep(2000);
            }
        //
            Console.WriteLine();
        //
        //   Как видим, в cmd чередуется вывод от двух разных частей кода, а значит у нас одновременно действуют 2 отдельных потока
        //   На самом деле автор сделал демонстрацию делегата ...ThreadStart чуть более сложной - с опросом пользователя о его намерении
        //     выполнить метод в двухпоточном режиме и привлечением System.Windows.Forms.MessageBox. Демонстрация ...ParameterizedThreadStart у
        //     него была попроще
        //
        //
        // AutoResetEvent_Using
        //
        //   Помнишь, как в одном из первых примеров мы применяли простую булевую переменную в качестве индикатора завершённости асинхронного
        //     вызова метода (она звалась isDone, метод звался DelegateAsynchrony_...())?. Ещё там говорилось, что это довольно нехорошое
        //     решение, т.к. был шанс,
        //     что оба потока будут использовать её одновременно. Ну так вот, простой и безопасный способ заставить один поток ждать другого
        //     предусматривает применение класса System.Threading.AutoResetEvent (это класс, не event!)
        //
        //         Объект этого класса должен быть виден и из главного, и из второстепенного потока. Суть в том, что главный поток должен
        //           вызвать его метод WaitOne(). Поток застрянет в этом методе, пока из второстепенного метода не будет вызван метод Set()
        //           этого же объекта
        //
            AutoResetEvent waitHandle = new AutoResetEvent(false);
                                            // AutoResetEvent - не очень большой класс
                                            // false - единственный конструктор этого класса просто настраивает внутренний индикатор
        //
            void AddNumbers()
            {
                Console.WriteLine("{0} + {1} is {2}", 5, 4, 5 + 4);
        //
                Console.WriteLine("{0}: sleep time", Thread.CurrentThread.Name);
                Thread.Sleep(1000);
        //
                waitHandle.Set();           // waitHandle.Set() - этот метод переключает внутренний индикатор в true. Это приведёт к оповещению
            }                               //   всех потоков, что ждут в waitHandle.WaitOne(), что они могут продолжить свои дела

            Thread myNewThread = new Thread(new ThreadStart(AddNumbers));
            myNewThread.Name = "Secondary";
        //
            myNewThread.Start();
            waitHandle.WaitOne();           // waitHandle.WaitOne() - на самом деле он делает то же, что и метод WaitOne() у экземпляров типа
                                            //   System.Threading.WaitHandle (т.е. блокирует вызывающий поток до тех пор, пока не изменится
                                            //   внутренний индикатор в его waitHandle)
        //
            Console.WriteLine("{0}: ok, sleep time gone\n", Thread.CurrentThread.Name);
        //
        //
        // ForegroundAndBackgroundThreads
        //
        //   Теперь давай поговорим про разницу между "потоками переднего плана" и "фоновыми потоками". На самом деле всё снова просто:
        //       > "Потоки переднего плана" предохраняют текущее приложение от завершения. Среда CLR настроена на это
        //       > "Фоновые потоки" (иногда называемые потоками-доменами) воспринимаются CLR как расширяемые пути выполнения, которые
        //         легко можно проигнорировать. Если все потоки переднего плана завершены, то все фоновые потоки автоматически уничтожаются,
        //         и домен выгружается
        //   Как ты понял, потоки переднего плана и фоновые потоки не синонимы первичных и рабочих потоков. Конструктора класса Thread по
        //     умолчанию создают потоки переднего класса, но это можно изменить, переключив на true одно свойство. Вот как это делается:
        //
            void DeadthreadMethod()
            {
                Console.WriteLine("{0}: I will die!!!", Thread.CurrentThread.Name);
                Thread.Sleep(10_000);
                Console.WriteLine("{0}: Am I still alive!?", Thread.CurrentThread.Name);  // "..alive!?" - это сообщение таки не будет выведено
            }
        //
            Thread bgroundThread = new Thread(DeadthreadMethod);
            bgroundThread.Name = "The background Thread";
            bgroundThread.IsBackground = true;  // IsBackground - то самое свойство, через которое поток можно сделать фоновым
            bgroundThread.Start();              // bgroundThread.Start() - мы запустили фоновый поток. Это значит, что он прервётся при
                                                //   выгрузке домена (здесь это точно произойдёт, если ты, как конечный пользователь, нажмёшь
                                                //   Enter для прохождения метода Console.ReadLine() быстрее, чем за 10сек.)
        //
        //


        Console.WriteLine("<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<   SystemThreadingThread_Silent()");
    }
}