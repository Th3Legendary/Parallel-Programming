using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace PriceScraper
{
    class Program
    {
        static List<string> productLinks = new List<string>
            {
                "https://ardes.bg/product/msi-geforce-rtx-3070-8gb-gaming-z-trio-lhr-912-v390-271-213251",
                "https://ardes.bg/product/apple-macbook-pro-13-2020-z11c0000g-204416",
                "https://ardes.bg/product/27-lg-27gn750-b-27gn750-b-175856",
                "https://ardes.bg/product/48-lg-oled48c11lb-oled48c11lb-207797"
            };
        static async Task Main(string[] args)
        {          
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            string[] result = await BeginAsync();
            stopwatch.Stop();
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            PrintResult(result);
            Console.WriteLine("Elapsed time: " + stopwatch.ElapsedMilliseconds);
            Console.WriteLine("----------------------------------");         
        }
        static async Task<string[]> BeginAsync()
        {
            List<Task<string>> tasks = new List<Task<string>>();
            foreach (string link in productLinks)
            {
                tasks.Add(Task.Run(() => Parser.Parse(link)));
            }
            var result = await Task.WhenAll(tasks);
            return result;
        }
        static void PrintResult(string[] array)
        {
            foreach (var item in array)
            {
                Console.WriteLine(item);
            }
        }
    }
}