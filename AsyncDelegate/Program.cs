using System;
using System.Threading;
/*стр 693*/
namespace AsyncDelegate
{
    internal class Program
    {
        public delegate int BinaryOp(int x, int y);
        static void Main(string[] args)
        {
            Console.WriteLine("***** Async Delegate Invocation *****");

            // Вывести идентификатор выполняющегося потока.
            Console.WriteLine("Main() invoked on thread {0}.",Thread.CurrentThread.ManagedThreadId);

            // Вызвать Add() во вторичном потоке.
            BinaryOp b = new BinaryOp(Add);

            // После обработки следующего оператора вызывающий поток
            // блокируется, пока не будет завершен Beginlnvoke() .
            IAsyncResult ar = b.BeginInvoke(10, 10, null, null);


            // Выполнить другую работу в первичном потоке...
            Console.WriteLine("Doing more work in Main () ! ");// Этот вызов занимает намного меньше пяти секунд!

            // По готовности получить результат выполнения метода Add().
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


/*На заметку! Метод, возвращающий void, можно просто вызвать асинхронно и забыть. В таких
случаях нет необходимости сохранять совместимый с IAsyncResult объект или обращаться
к Endlnvoke (), т.к. нет возвращаемого значения, которое требуется получить.
Исключение, которое выдается, если возможность не работает на соответствующей платформе.*/

/*Чтобы проинструктировать делегат BinaryOp о вызове метода Add () асинхронным образом, модифицируем логику предыдущего проекта (можно добавить код к имеющемуся
проекту, но в коде примеров для главы доступен новый проект консольного приложения
по имени AsyncDelegate). Обновите предыдущий метод Main (), как показано ниже:
*/

/*
 Запуск приложения приводит к выводу на консоль двух уникальных идентификаторов потоков, поскольку в текущем домене приложения функционирует множество
 потоков:
 ***** Async Delegate Invocation *****
 Main() invoked on thread 1.
 Doing more work in Mam() !
 Add() invoked on thread 3.
 10 + 10 is 20.
 */