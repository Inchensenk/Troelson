using System;
/*694*/
namespace AsyncDelegate2
{
    internal class Program
    {
        public delegate int BinaryOp(int x, int y);
        static void Main(string[] args)
        {
            Console.WriteLine("***** Async Delegate Invocation *****");

            // Вывести идентификатор выполняющегося потока.
            Console.WriteLine("Main() invoked on thread {0}.", Thread.CurrentThread.ManagedThreadId);

            // Вызвать Add() во вторичном потоке.
            BinaryOp b = new BinaryOp(Add);

            // После обработки следующего оператора вызывающий поток
            // блокируется, пока не будет завершен Beginlnvoke() .
            IAsyncResult ar = b.BeginInvoke(10, 10, null, null);

            /*
            while (!ar.IsCompleted)
            {
                // Это сообщение продолжит выводиться до тех пор,
                // пока не будет завершен метод Add() .
                Console.WriteLine("Doing more work in Main () ! ");
                Thread.Sleep(1000);
            }*/

            //цикл while, в котором больше не применяется вызов Thread. Sleep():
            while (!ar.AsyncWaitHandle.WaitOne(1000, true))
            {
                Console.WriteLine(" Doing more work in Main() '");
            }

            // Теперь известно, что метод Add() завершен,
            int answer = b.EndInvoke(ar);// Снова происходит ожидание завершения другого потока!
            Console.WriteLine("10 + 10 is {0}.", answer);
            Console.ReadLine();

        }

        static int Add(int x, int y)
        {
            // Вывести идентификатор выполняющегося потока.
            Console.WriteLine("Add () invoked on thread {0}.", Thread.CurrentThread.ManagedThreadId);

            // Сделать паузу для моделирования длительной операции.
            Thread.Sleep(5000);
            return x + y;
        }
    }
}
/*
    на строчкеIAsyncResult ar = b.BeginInvoke(10, 10, null, null);
    Ошибка   System.PlatformNotSupportedException: "Operation is not supported on this platform."
    Исключение, которое выдается, если возможность не работает на соответствующей платформе.
 */


/*
 Здесь организован цикл, который продолжает выполнять оператор Console.
WriteLine () до тех пор, пока не завершится вторичный поток. Когда это произойдет,
можно получить результат выполнения метода Add (), точно зная, что он закончил работу. 
Вызов Thread. Sleep (1000) не обязателен для корректного функционирования
данного отдельно взятого приложения; однако, вынуждая первичный поток ожидать
приблизительно секунду на каждой итерации, мы предотвращаем вывод на консоль
слишком большого количества одного и того же сообщения. Ниже показан вывод (в
зависимости от быстродействия машины и времени запуска потоков он может слегка
варьироваться):

***** Async Delegate Invocation *****
Main() invoked on thread 1.
Doing more work in Main() !
Add() invoked on thread 3.
Doing more work in Main() !
Doing more work in Main() !
Doing more work in Main() 1
Doing more work in Main() !
Doing more work in Main() !
10 + 10 is 20.

Помимо свойства IsCompleted интерфейс IAsyncResult предлагает свойство AsyncWaitHandle, предназначенное для реализации более гибкой логики ожидания. Свойство AsyncWaitHandle возвращает экземпляр типа WaitHandle, который открывает доступ к методу по имени WaitOne (). Преимущество использования
WaitHandle. WaitOne () заключается в том, что можно указывать максимальное время ожидания. По истечении заданного времени метод WaitOne () возвратит false.
Взгляните на следующий измененный цикл while, в котором больше не применяется
вызов Thread. Sleep ():
while (!ar.AsyncWaitHandle.WaitOne(1000, true))
{
Console . WnteLine (" Doing more work in Main() '");
}
Хотя рассмотренные свойства интерфейса IAsyncResult предоставляют способ синхронизации вызывающего потока, все же они не обеспечивают самый эффективный
подход. Во многих отношениях свойство IsCompleted похоже на надоедливого менеджера (или коллегу), постоянно спрашивающего: “Вы уже сделали это?”. К счастью, делегаты предлагают несколько дополнительных (и более элегантных) приемов получения
результата из метода, который был вызван асинхронным образом.

 
 */

