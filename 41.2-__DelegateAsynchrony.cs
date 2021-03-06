/*
 * creation date  24 oct 2021
 * last change    24 oct 2021
 * author         artur
 */
using System;
using System.Threading;

class __DelegateAsynchrony
{
    static void Main()
    {
        Console.WriteLine("***** _ *****");

        DelegateAsynchrony_Silent();

        Console.ReadLine();
    }
    static void DelegateAsynchrony_Silent()
    {
        Console.WriteLine(">->->->->->->->->->->->->->->->->->->   DelegateAsynchrony_Silent()\n");


        // RoleOfThreadSynchronization
        //
        //   Чтобы защитить ресурсы приложений (переменные, методы), разработчики .NET должны применять потоковые примитивы для управления
        //     доступом между выполняющимися потоками (таких, как блокировки, мониторы, атрибут [Synchronization] и поддержка языковых
        //     ключевых слов. Всё это ты увидишь в действии)(другими словами, для своих штук ты должен синхронизировать потоки)
        //   Хоть платформа .NET и не может полностью скрыть все сложности под ковёр, сам
        //     процес всё-таки вышел гораздо более простым, чем это было в C++. Используя типы пространства System.Threading, библиотеку TPL
        //     и ключевые слова async и await языка C#, можно работать со множеством потоков, прикладывая мизер от былых усилий
        //  
        //   Но прежде, давай посмотрим на то как асинхронность строилась в .NET приложениях раньше (т.е. как применять делегат для вызова
        //     метода в асинхронной манере). Хоть начиная с .NET 4.6
        //     применение ключевых слов async и await стали более простой альтернативой асинхронным делегатам, попрежнему рекомендуется
        //     знать старинный метод (ведь в производственной среде осталось огромное кол-во старого кода)
        //


        // BriefReviewOfDelegates
        //
        //   Как мы помним, делегат - это фактически аналог классического указателя на функцию (выполненный в безопасной
        //     объекто-оринетировачной манере .NET). Каждый делегат (благодаря своим предкам) способен хранить целый список методов. При
        //     компиляции объявления (и определения) каждого из делегатов компилятор выпустит по новому запечатанному классу. Вспомни, что
        //     сгенерированный метод Invoke() вызывает внутренние функции в синхронной манере (т.е. прежде, чем thread попытвается
        //     вызвать его, происходит проверка на то, не запущен ли он уже каким-то другим потоком)
        //   Давай посмотрим кусок приложения, что использует синхронный (т.е. блокирующий) вызов делегата
        //
          Console.WriteLine("BriefReviewOfDelegates_Silent() invoked on thread: {0}", Thread.CurrentThread.ManagedThreadId);
                                                          // WriteLine() - сначала просто выведем id текущего потока (это 1)(метод
                                                          //   ToString() у класса Thread не переопределён)
          Func<int, int, int> f = (int a, int b) =>       // Func<> - ты же не забыл про универсальные стандартные делегаты, верно?
          {
              Console.WriteLine("f() invoked on thread {0}", Thread.CurrentThread.ManagedThreadId);
              Thread.Sleep(5000);                         // WriteLine("f()..") - также мы посмотрим на id потока, что запустил этот
              return a + b;                               //   делегат. Выведет тот же id
          };                                              // Thread.Sleep() - заметь, что в этом лямбда-выражении поток засыпает на 5 секунд
          int answer = f(10, 10);                         // f() - можно было бы также написать f.Invoke()
          Console.WriteLine("10 + 10 is {0}\n", answer);  // Console.WriteLine() - эта строка кода не выполнится до тех пор, пока не
        //                                                //   завершится вызов f() (т.к. при синхронном вызове на него не выделяется
        //                                                //   отдельный поток)
        //
        //
        // AsynchronousNatureOfDelegate
        //
        //   f здесь очень напоминает делегат с методом какой-нибудь удалённой веб-службы. Как ты понял, при
        //     вызове хранящегося метода, приложение будет козаться зависшим, т.к. твой единственный на данный момент работник ушёл в эту
        //     коммандировку. Что делать? А ответ прост - временно нанять второго работника, что и отправится в коммандировку, т.е. вызвать
        //     этот метод в асинхронной манере. Твой основной поток просто передаст второстепенному аргументы, а сам продолжит работу у себя.
        //   К счастью, каждый делегат оснащён такой способностью. А ещё лучше то, что ты не обязан углублятся в детали работы типов
        //     System.Threading, чтобы уметь это делать. При компилировании типа делегата, csh.exe автоматически генерирует два методы с
        //     именами BegineInvoke() и EndInvoke(). Параметры в BegineInvoke() основаны на типе делегата, в котором этот метод
        //     определён, + последние два параметра System.AsyncCallBack и System.Object (мы позже поговорим о них, а пока
        //     что просто будем передавать им по null'у). Его возвращаемым типом всегда ставится System.IAsyncResult. EndInvoke() же
        //     принимает в качестве параметра System.IAsyncResult, а возвращает тот тип, что назначен как возвратный у его делегата
        //
        //
        // System.IAsyncResult
        //
        //   Как ты, может, понял, этот интерфейс - связующее звено между BeginInvoke() и EndInvoke(), и он позволяет вызывающему потоку
        //     получать результат асинхронного метода позже. Как? Всё просто - ты сразу получаешь совместимый с этим интерфейсом экземпляр, в
        //     котором есть индикатор того, завершён ли метод. Если тебе нужен результат ну прямо сейчас, то можешь спокойно послать этот
        //     экземпляр в метод EndInvoke(), и получить желанные данные (если метод не завершён, придётся подхождать). Вот полное определение
        //     интерфейса System.IAsyncReslut:
        //  
        //         public interface IAsyncResult
        //         {
        //             object? AsyncState { get; }
        //             WaitHandle AsyncWaitHandle { get; }
        //             bool CompletedSynchronously { get; }
        //             bool IsCompleted { get; }
        //         }
        //  
        //     В простейшем случае эти члены не трогаются (происходит так, как я описал выше). Как будет показано позже, эти члены могут 
        //     вызываться, когда нужна высокая вовлечённость в процесс. А пока что можешь просмотреть чисто демонстративный пример:
        //
          Console.WriteLine("DelegateAsynchrony_Silent() invoked on thread: {0}", Thread.CurrentThread.ManagedThreadId);
        //
          IAsyncResult fAsyncResult = f.BeginInvoke(10, 10, null, null);                     // f.BeginInvoke() - на этот раз мы используем
          Console.WriteLine("Doing another work of the method...");                          //   этот способ (и выведется id 3)
                                                                                             // "Doing another work.." - вывод этой строки
          int answer1 = f.EndInvoke(fAsyncResult);                                           //   сразу после вызова BeginInvoke() покажет
          Console.WriteLine("10 + 10 is {0}\n", answer1);                                    //   нам, что основной метод не прервался
                                                                                             //   (мы видим, что эта строка выводится раньше,
                                                                                             //   чем та, что должна быть выведена f())
        //   Как оказалось, метод EndInvoke() может быть вызван лишь раз для своего          // f.EndInvoke() - а здесь придётся подождать
        //     асинхронного вызова. При повторном использовании выдаёт исключение            //   (ведь врядли этот метод завершится быстрее
        //                                                                                   //   нашего кода между Begin..() и End..()
        //
        //
        //   На самом деле в реальных условиях не стоит допускать того, чтобы какой-нибудь поток простаивал так много времени (время -
        //     - деньги!). Если результат асинхронно вызванного метода только желателен, но не обязателен прямо сейчас, то можно просто
        //     сделать проверку на счёт того, что не завершился ли этот метод? В интерфейса IAsyncResult предусмотрено bool свойство
        //     IsCompleted, и это и есть индикатор завершённости
        //
          IAsyncResult fAsyncResult1 = f.BeginInvoke(20, 20, null, null);             // f.BeginInvoke() - на этот раз id будет 4
          while (!fAsyncResult1.IsCompleted)                                          // while (!..IsCompleted) - наш поток будет что-то
          {                                                                           //   делать (по большей части снова спать), пока не
              Console.WriteLine("Doing more work in the method...");                  //   завершится асинхронный вызов 
              Thread.Sleep(1000);
          }
          Console.WriteLine("20 + 20 is {0}\n", f.EndInvoke(fAsyncResult1));          // f.EndInvoke() - здесь мы уже не будем ждать лишнее
                                                                                      //   время
        //
        //   Выведется 5 сообщений из цикла - и это говорит о том, что в 5 секунд вызов делегата f() укладывается
        //
        //
        //   Также интерфейс System.IAsyncResult предлагает свойство AsyncWaitHandle, что предназначено для ещё более гибкой логики ожидания.
        //     Свойство AsyncWaitHandle возвращает экземпляр типа System.Threading.WaitHandler, что содержит метод WaitOne(). Преимуещство его
        //     использования в том, что можно указать максимальное время ожидания. Если оно истекает, метод возвратит false. Вот как это
        //     выглядит в действии:
        //
          IAsyncResult fAsyncResult2 = f.BeginInvoke(30, 30, null, null);
          if (fAsyncResult2.AsyncWaitHandle.WaitOne(3000) == false)  // ..WaitOne(3000) - если вызов не успеет завершится за 3 секунды, то
          {                                                           //   будет выведено сообщение "Too long.."
              Console.WriteLine("Too long time has gone");
          }
          Console.WriteLine();
        //
        //
        //   Хотя рассмотренные свойства IAsycnResult вполне справляеются со своей задачей (синхронизации кода), это обеспечивают не самый
        //     эффективный подход. Во многом они напоминают надоедлевого менеджера, постоянно спрашивающего, а не сделал ли ты уже это?. К
        //     счастью, в делегатах имеется несколько более элегантных приёмов получения результатов метода, что был вызван в
        //     асинхронной манере


        // Role of System.AsyncCallBack
        //
        //   Вместо опроса делегата о завершённости вызова можно просто заставить его самому сказать об этом. Для этого и был создан 3-й
        //     параметр метода BeginInvoke() у делегатов (это параметр типа делегата System.AsyncCallback, что имеет сигнатуру
        //     void (System.IAsyncResult)). Суть в том, что ты просто посылаешь функцию, что будет вызвана второстепенным потоком, как только
        //     асинхронный метод завершится
        //
          bool isDone = false;              // isDone - это будет нашим местным индикатором. На самом деле не очень хорошо, что он будет
                                            //   использоваться двумя потоками
        //
          void AddComplete(IAsyncResult _)  // AddComplete() - этот метод будет вызван второстепенным потоком после завершения своего
          {                                 //   асинхронного метода
              Console.WriteLine("AddComplete() invoked on thread: {0}", Thread.CurrentThread.ManagedThreadId);
              Console.WriteLine("Your addition is completed");
              isDone = true;                // isDone - заметь, что эта функция переключает индиктор, что постоянно проверяется главным
          }                                 //   потоком
        //
          Console.WriteLine("the method is invoked on thread: {0}", Thread.CurrentThread.ManagedThreadId);
          IAsyncResult ar = f.BeginInvoke(10, 10, new AsyncCallback(AddComplete), null);
          while (!isDone)
          {
              Console.WriteLine("Another work...");
              Thread.Sleep(1000);
          }
          Console.WriteLine("And finally we got {0}\n", f.EndInvoke(ar));  // Console.WriteLine("..") - почему бы просто не вставить эту строку
                                                                           //   кода в AddComplete()? На самом деле здесь это можно сделать
                                                                           //   (и всё вышло бы куда стройнее), но здесь случай исключительный
                                                                           //   (ведь AddComplete() у нас - локальная функция). Подробнее этот
                                                                           //   разбобран ниже
        //
        //   Заметь, что id стандартного потока неизменно на протяжении работы всего приложения, а id второстепенных меняется с каждым новым
        //     асинхронным вызовом


        // Role of System.Runtime.Remoting.Messaging.AsyncResult
        //
        //   Как ты видишь, финальный метод AddComplete() не может сам получить результат, т.к. самого объекта f в доступе у него нет (а
        //     метод EndInvoke() должен вызываться из него)(на самом деле AddComplete() до него дотягивается, но это только потому, что я
        //     использовал локальную функцию). Вместо того, чтобы как-то передвигать этот метод или объект, лучше воспользоваться более
        //     элегнатным способом - получить этот объект из принимаемого параметра. Методу, что содержит делегат System.AsyncCallBack, после
        //     завершения асинхронного вызова на самом деле посылается экземпляр типа System.Runtime.Remoting.Messaging.AsyncResult, а его
        //     свойство AsyncDelegate может вернуть ссылку на исходный асинхронный делегат
        //
        //         Важный момент - класс System.Runtime.Remoting.Messaging.AsyncResult
        //         поставлялся лишь в .NET до версии 4.8
        //
        //   Ниже показана реализация этой схемы
        //
        //void AddComplete1(IAsyncResult ar)                                           // ar - на этот раз мы намерены использовать
        //{                                                                            //   поступающие данные
        //    object startedAsyncDelegate = ((System.Runtime.Remoting.Messaging.AsyncResult)ar).AsyncDelegate;
        //                                                                             // AsyncDelegate - да, этот метод всё-таки возвращает
        //                                                                             //   то, что заявлено, но, правда, в виде object
        //    int result = ((Func<int, int, int>)startedAsyncDelegate).EndInvoke(ar);
        //    Console.WriteLine("AddComplete():\tThe final result is {0}", result);
        //}
        //
        //f.BeginInvoke(10, 10, AddComplete1, null);                                   // f.BeginInvoke(...) - ты же помнишь об этой
        //for (int i = 0; i < 12; i++)                                                 //   способности, верно?
        //{                                                                            // i < 12 - сообщение из AddComplete() выведится прямо
        //    Console.WriteLine("RoleOfAsyncResult() does some work...");              //   между сообщениями от RoleOfAsyncResult()
        //    Thread.Sleep(500);
        //}
        //Console.WriteLine("What!? Has the method really got it!?\n");
        //
        /////////after reading: BeginInvokeLastArgument//////////////////////////////////////////////
        //****почему бы просто не отправлять ссылку на f в метод AddComplete1() в качестве параметра
        //    @object?
        /////////////////////////////////////////////////////////////////////////////////////////////


        // BeginInvokeLastArgument()
        //
        //   Финальным аспектом асинхронных делегатов, что мы ещё не рассмотрели, является 4-й параметр метода BeginInvoke() (до сих пор мы
        //     посылали в него null). Он позволяет посылать дополнительную инфу о состоянии исходного делегата завершительному методу
        //   Например, вот как мы можем послать строку с текстом в AddComplete() (для демонстрации пойдёт и текст):
        //
          void AddComplete2(IAsyncResult ar)
          {
              Console.WriteLine("AddComplete():\t main thread is telling me this: {0}", ar.AsyncState);
          }                                                   // ar.AsyncState - это свойство (типа object, кстати) выдаст тебе ссылку на
                                                              //   4-й аргумент, что был отправлен в BeginInvoke()
        //
          f.BeginInvoke(10, 10, AddComplete2, "I thanks you for adding these numbers");
          Thread.Sleep(3 * 1000);
          Console.WriteLine();
        //
        //
         
        
        // Afterwords
        //
        //   Теперь ты знаешь, как можно запускать отдельные методы в отдельных потоках, юху! Теперь настало время погрузится в саму природу
        //     потоков
        //   На самом деле у объектов, тип которых реализует System.IAsyncResult,
        //     есть ещё одно нерассмотренное свойство - CompletedSynchronously
        //   Есть ещё одно интересное замечание - с каждым запущенным методом это приложение потребляет всё больше памяти (по 102кб на
        //     метод, под windows'ом и с .NET 4.7)!)


        Console.WriteLine("<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<   DelegateAsynchrony_Silent()");
    }
}