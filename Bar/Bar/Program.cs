using System;
using System.Collections.Generic;
using System.Threading;

namespace BarSim
{
    class Program
    {
        static void Main(string[] args)
        {
            Random r = new Random();
            Bar bar = new Bar();
            List<Thread> studentThreads = new List<Thread>();
            for (int i = 0; i < 100; i++)
            {
                var student = new Student(i.ToString(), bar, r.Next(15,75), r.NextDouble() * 100);
                var thread = new Thread(student.PaintTheTownRed);
                thread.Start();
                studentThreads.Add(thread);
            }

            foreach (var t in studentThreads) t.Join();
            Console.WriteLine();
            Console.WriteLine("The party is over.");
            bar.GetDailyIncome();
            Console.ReadLine();
        }
    }
}