using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _14_1
{
    class MyThreads
    {
        Stopwatch stopeWatch = new Stopwatch();

        public void ThreadTimer()
        {
            int count = 10;
            for (int i = 1; i <= count; i++)
            {
                Console.WriteLine($"Поток {i}");

                using (var countdownEvent = new CountdownEvent(i))
                {
                    stopeWatch.Restart();
                    for (int j = 0; j < i; j++)
                    {
                        new Thread(delegate () {countdownEvent.Signal();}).Start();
                    }
                    countdownEvent.Wait();
                    Console.WriteLine($"Запущен {i} поток {stopeWatch.Elapsed}\n");
                }

                Thread.Sleep(1);

                Console.WriteLine($"Поток в пуле {i}");

                using (var countdownEvent = new CountdownEvent(i))
                {
                    stopeWatch.Restart();
                    for (int j = 0; j < i; ++j)
                    {
                        ThreadPool.QueueUserWorkItem(delegate (object obj){countdownEvent.Signal();});
                    }

                    countdownEvent.Wait();
                    Console.WriteLine($"Запущен {i} поток из пула {stopeWatch.Elapsed}\n");
                }

            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            MyThreads myThreads = new MyThreads();
            myThreads.ThreadTimer();
        }
    }
}
