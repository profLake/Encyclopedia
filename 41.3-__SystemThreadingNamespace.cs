/*
 * creation date  25 oct 2021
 * last change    25 oct 2021
 * author         artur
 */
using System;
using System.Threading;

class __SystemThreadingNamespace
{
    static void Main()
    {
        Console.WriteLine("***** _ *****");

        SystemThreadingNamespace_Silent();

        Console.ReadLine();
    }
    static void SystemThreadingNamespace_Silent()
    {
        Console.WriteLine(">->->->->->->->->->->->->->->->->->->   SystemThreadingNamespace_Silent()\n");


        // Namespace System.Threading
        //
        //   (как ты уже понял) Пространство System.Threading предоставляет типы потоков и всё, что с ними связано.
        //     Вот некоторые важные члены этого пространства:
        //  
        //         Interlocked                - этот статический класс поставляет набор атомарных операций, что пригодятся для разделённых
        //                                      между несколькими потоками переменных
        //         Monitor                    - этот небольшой статический класс хранит инструменты для сихнронизации потоков при доступе к
        //                                      общим объектам.
        //                                      Он поставляет блокировки и методы ожидания/сигналов (т.е. ты будешь расстовлять
        //                                      светофоры). Ключевое слово lock языка C# (о котором позже) за кулисами применяет этот тип
        //         Mutex                      - этот примитив синхронизации может быть применён для синхронизации между границами доменов
        //                                      приложения (в Си mutex'ы делали немного другую вещь)
        //         ParameterizedThreadStart   - этот делегат позволяет потоку начатся в методе, что принимает параметр.
        //
        //                                          Поток может начатся только с метода, что хранится либо в ...ParameterizedThreadStart, либо
        //                                          в ...ThreadStart (о нём ниже)
        //
        //                                      Вариант с этим делегатом (...ParameterizedThreadStart) даёт
        //                                      возможность основному потоку передать новому второстепенному потоку какие-то данные. Имеет
        //                                      сигнатуру void (object)
        //         Semaphore                  - этот тип позволяет ограничить количество потоков, что могут одновременно ухватится за ресурс
        //                                      или определённый тип. Находится в System.dll
        //         Thread                     - этот класс представляет поток. Мы уже видели его несколько раз
        //         ThreadPool                 - этот статический класс создан для управления пулом потоков (да, CLR это поддерживает)
        //         ThreadPriority             - это перечисление хранит в себе уровни приоритета, что должены быть в каждом потоке
        //         ThreadStart                - делегат, созданный для хранения в себе метода, что должен быть запущен новым потоком. Этот
        //                                      делегат может хранить только те методы, что имеют сигнатуру void ..()
        //         ThreadState                - это перечисление указывает допустимые состояния потока (Running, Aborted и т.д.)
        //         Timer                      - этот класс имеет механизм для выполнения метода через указаные интервалы времени
        //         TimerCallback              - этот делегат используется в сочетании с типом Timer (т.к. Timer'ы хранят TimerCallback'и).
        //                                      Может хранить методы с сигнатурой void (object)
        //  


        Console.WriteLine("<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<   SystemThreadingNamespace_Silent()");
    }
}