using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Word_Analyser
{
    class MultiThreaded
    {
        string[] words;
        public void BeginAsync(string[] words)
        {
            this.words = words;

            List<Thread> threads = new List<Thread>();

            Thread thread1 = new Thread(this.NumberOfWords);
            threads.Add(thread1);
            thread1.Start();

            Thread thread2 = new Thread(this.AverageWordLength);
            threads.Add(thread2);
            thread2.Start();

            Thread thread3 = new Thread(this.LongestWord);
            threads.Add(thread3);
            thread3.Start();

            Thread thread4 = new Thread(this.ShortestWord);
            threads.Add(thread4);
            thread4.Start();

            Thread thread5 = new Thread(this.FiveMostCommon);
            threads.Add(thread5);
            thread5.Start();

            Thread thread6 = new Thread(this.FiveLeastCommon);
            threads.Add(thread6);
            thread6.Start();

            foreach (Thread thread in threads) thread.Join();
        }
        private void NumberOfWords()
        {
            Console.WriteLine("Number of words: " + words.Length);
        }
        private void ShortestWord()
        {
            string shortestWord = words[0];
            foreach (var word in words)
            {
                if (shortestWord.Length == 1) { break; }
                if (word.Length < shortestWord.Length)
                {
                    shortestWord = word;
                }

            }
            Console.WriteLine("Shortest Word: " + shortestWord);
        }
        private void LongestWord()
        {
            string longestWord = words[0];
            foreach (string word in words)
            {
                if (word.Length > longestWord.Length)
                {
                    longestWord = word;
                }
            }
            Console.WriteLine("Longest Word: " + longestWord);
        }
        private void AverageWordLength()
        {
            double totalLength = 0;
            foreach (var word in words)
            {
                totalLength += word.Length;
            }
            totalLength /= words.Length;
            double avgLength = Math.Round(totalLength, 3);
            Console.WriteLine("Average Word length: " + avgLength);
        }

        private void FiveMostCommon()
        {
            Dictionary<string, int> repeatedWordCount = new Dictionary<string, int>();

            foreach (var word in words)
            {
                if (repeatedWordCount.ContainsKey(word))
                {
                    repeatedWordCount[word]++;
                    continue;
                }
                repeatedWordCount.Add(word, 1);
            }

            string text = "";
            int count = 0;

            foreach (var word in repeatedWordCount.OrderByDescending(l => l.Value).Take(5).Select(k => k.Key).ToArray())
            {
                if (count == 4)
                {
                    text += word + ";";
                }
                else
                {
                    text += word + ", ";
                    count++;
                }
            }
            Console.WriteLine("Five Most Common Words: " + text);
        }
        private void FiveLeastCommon()
        {
            Dictionary<string, int> repeatedWordCount = new Dictionary<string, int>();

            foreach (var word in words)
            {
                if (repeatedWordCount.ContainsKey(word))
                {
                    repeatedWordCount[word]++;
                    continue;
                }
                repeatedWordCount.Add(word, 1);
            }

            string text = "";
            int count = 0;

            foreach (var word in repeatedWordCount.OrderBy(l => l.Value).Take(5).Select(k => k.Key).ToArray())
            {
                if (count == 4)
                {
                    text += word + ";";
                }
                else
                {
                    text += word + ", ";
                    count++;
                }
            }
            Console.WriteLine("Five Least Common Words: " + text);
        }
    }
}

