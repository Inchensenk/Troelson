
/*695*/
namespace AsyncCallbackDelegate
{
    public delegate int BinaryOp(int x, int y);
    class Program
    {
        private static bool isDone = false;
        static void Main(string[] args)
        {
            Console.WriteLine("***** AsyncCallbackDelegate Example *****");
            Console.WriteLine("Main() invoked on thread {0}.", Thread.CurrentThread.ManagedThreadId);
            BinaryOp b = new BinaryOp(Add);
            IAsyncResult ar = b.BeginInvoke(10, 10, new AsyncCallback(AddComplete), null);

            // Предположим, что здесь делается какая-то другая работа...
            while (!isDone)
            {
                Console.WriteLine("Working....");
                Thread.Sleep(1000);
            }
            Console.ReadLine();
        }
        static int Add(int x, int y)
        {
            Console.WriteLine("Add() invoked on thread {0}.", Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(5000);
            return x + y;
        }


        static void AddComplete(IAsyncResult iar)
        {
            Console.WriteLine("AddComplete() invoked on thread {0}.", Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("Your addition is complete");
            isDone = true;
        }
    }
}

/*На заметку! Строго говоря, использование булевской переменной в данном примере не является
безопасным в отношении потоков, поскольку к ее значению имеют доступ два разных потока.
В текущем примере подобное допустимо; тем не менее, запомните в качестве очень важного
эмпирического правила: вы должны обеспечивать блокировку данных, разделяемых между несколькими потоками. Вы увидите, как это делается, далее в главе.*/

/*И снова статический метод AddComplete () будет вызван делегатом AsyncCallback
после завершения метода Add (). Запустив программу, можно убедиться, что именно
вторичный поток вызывает AddComplete ():

***** AsyncCallbackDelegate Example *****
Main() invoked on thread 1.
Add () invoked on thread 3.
Working....
Working....
Working....
Working....
Working....
AddComplete () invoked on thread 3.
Your addition is complete

 Подобно другим примерам, приведенным в настоящей главе, вывод может несколько отличаться. На самом деле вполне возможно, что после завершения работы метода
AddComplete () появится только одно финальное сообщение Working.... Это просто побочный эффект односекундной задержки в Main ().
 */