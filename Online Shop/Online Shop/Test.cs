using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Online_Shop
{
    class Test
    {
        static Random rand = new Random();
        static Shop shop = new Shop(InitialiseStock(rand.Next(1, 20)));       
        static int buyers = rand.Next(1, 100);
        static int suppliers = rand.Next(1, 5);

        static void Main(string[] args)
        {           
            StartTest();

            //Console.WriteLine(buyers.ToString());
            //Console.WriteLine(suppliers.ToString());
            //shop.stock.Select(i => $"{i.Key}: {i.Value}").ToList().ForEach(Console.WriteLine);
        }
        static void StartTest()
        {
            List<Thread> threads = new List<Thread>();

            for (int i = 0; i < suppliers; i++)
            {
                Thread t = new Thread(SupplyWorker);
                string threadName = "Supplier" + i.ToString();
                t.Start(threadName);
                threads.Add(t);
            }

            for (int i = 0; i < buyers; i++)
            {
                Thread t = new Thread(BuyWorker);
                string threadName = "Buyer" + i.ToString();
                t.Start(threadName);
                threads.Add(t);
            }

            foreach (var t in threads) t.Join();

            Console.WriteLine("Test Finished");
        }
        static void SupplyWorker(object obj)
        {
            string threadName = (string)obj;
            int itemsToSupply = rand.Next(1, shop.stock.Count);

            Console.WriteLine($"{threadName} will try to stock {itemsToSupply} items");

            while (itemsToSupply != 0)
            {
                int amount = rand.Next(1, 100);
                string item = shop.stock.ToList()[rand.Next(0, shop.stock.Count - 1)].Key;

                shop.Supply(item, amount);
                Console.WriteLine($"{threadName} added {amount} of {item}");
                
                --itemsToSupply;
            }
        }
        static void BuyWorker(object obj)
        {
            string threadName = (string)obj;
            int itemsToBuy = rand.Next(1, shop.stock.Count);

            Console.WriteLine($"{threadName} will try to buy {itemsToBuy} items");

            while (itemsToBuy != 0)
            {
                int amount = rand.Next(1, 20);
                string item = shop.stock.ToList()[rand.Next(0, shop.stock.Count - 1)].Key;
                bool success = shop.Buy(item, amount);

                if (success)
                {
                    Console.WriteLine($"{threadName} purchased {amount} of {item}");
                }
                else
                {
                    Console.WriteLine($"{threadName} wanted to get {amount} of {item}, but there weren't enough");
                }

                --itemsToBuy;
            }
        }
        static Dictionary<string, int> InitialiseStock(int amount)
        {
            Dictionary<string, int> stock = new Dictionary<string, int>();

            for (int i = 0; i < amount; i++)
            {
                stock.Add("Item" + i, rand.Next(1, 200));
            }
            return stock;
        }
    }
}
