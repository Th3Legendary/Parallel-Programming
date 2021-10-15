using System;
using System.Diagnostics;

namespace Word_Analyser
{
    class Program
    {
        private static string[] TextSeparate(string text)
        {
            string[] stringSeparators = { "\n", " ", ",", ".", "?", "-", "!", ":", "...", "	",
                                          "\t", " —", "\"", "—", "\r", "_", "„", "“", ";", "*",
                                          "[", "]", "(", ")"};

            return text.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
        }

        static void Main(string[] args)
        {

            Console.OutputEncoding = System.Text.Encoding.UTF8;
            string[] words = TextSeparate(Properties.Resources.George_Orwell_1984);

            SingleThreaded st = new SingleThreaded();
            MultiThreaded mt = new MultiThreaded();

            var stopwatch1 = Stopwatch.StartNew();
            st.Begin(words);
            stopwatch1.Stop();

            Console.WriteLine("--------------------------------");
            Console.WriteLine("Time taken: " + stopwatch1.ElapsedMilliseconds);
            Console.WriteLine("--------------------------------");

            var stopwatch2 = Stopwatch.StartNew();
            mt.BeginAsync(words);
            stopwatch2.Stop();

            Console.WriteLine("--------------------------------");
            Console.WriteLine("Time taken: " + stopwatch2.ElapsedMilliseconds);
            Console.ReadLine();
          
        }
    }
}
