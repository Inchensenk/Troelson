using System;
/*стр 690*/
namespace SyncDelegateReview
{
    internal class Program
    {
        public delegate int BinaryOp(int x, int y);


        
        static void Main(string[] args)
        {
            Console.WriteLine("*****Synch Delegate Review * ****");

            // Вывести идентификатор выполняющегося потока.
            Console.WriteLine("Main() invoked on thread {0}.", Thread.CurrentThread.ManagedThreadId);

            // Вызвать Add() в синхронном режиме.
            BinaryOp b = new BinaryOp(Add);

            // Можно было бы также написать b.Invoke(10, 10);
            int answer = b(10, 10);

            // Эти строки кода не выполнятся до тех пор,
            // пока не завершится метод Add().
            Console.WriteLine("Doing more work m Main()'");
            Console.WriteLine("10 + 10 is {0}.", answer);
            Console.ReadLine();
        }

        /*
         Внутри метода Add () вызывается статический метод Thread. Sleep (), 
         чтобы приостановить вызывающий поток приблизительно на 5 секунд для моделирования длительно выполняющейся задачи. 
         Поскольку метод Add () вызывается в синхронной манере,
         метод Main () не будет выводить результат операции до тех пор, 
         пока не завершится метод Add ().
         */
        static int Add(int x, int y)
        {
            // Вывести идентификатор выполняющегося потока.
            Console.WriteLine("Add () invoked on thread {0}.",
            Thread.CurrentThread.ManagedThreadId);
            // Сделать паузу для моделирования длительной операции.
            Thread.Sleep(5000);
            return x + y;
        }

    }
}

/*
    Обратите внимание, что метод Main () получает доступ к текущему потоку (через
свойство Thread.CurrentThread) и выводит идентификатор потока посредством свойства ManagedThreadld. Та же самая логика повторяется в статическом методе Add ().
Как и можно было ожидать, учитывая, что вся работа в этом приложении выполняется исключительно в первичном потоке, на консоль выводится одно и то же значение
идентификатора:
    ***** Synch Delegate Review *****
    Main () invoked on thread 1.
    Add () invoked on thread 1.
    Doing more work in Main() !
    10 + 10 is 20.
    Press any key to continue . . .

    Запустив программу, вы должны заметить пятисекундную задержку перед тем, как
выполнится последний вызов Console .WriteLine () в методе Main (). Хотя многие (если
не большинство) методов могут вызываться синхронно без болезненных последствий,
при необходимости делегаты .NET можно инструктировать на вызов их методов асинхронным образом.

 */