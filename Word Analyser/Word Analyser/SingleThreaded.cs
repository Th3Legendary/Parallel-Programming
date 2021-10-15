
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Word_Analyser
{
    class SingleThreaded
    {
        string[] words;
        public void Begin(string[] words)
        {
            this.words = words;
            NumberOfWords();
            ShortestWord();
            LongestWord();
            AverageWordLength();
            FiveMostCommon();
            FiveLeastCommon();
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

            foreach(var word in words)
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
