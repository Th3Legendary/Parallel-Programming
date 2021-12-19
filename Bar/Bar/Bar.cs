using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BarSim
{
    class Bar
    {

        public enum DrinkType { Vodka, Whiskey, Gin, Beer, Wine, Rum, Cognac, Brandy, Vermouth, Cider, Spirit };
        public enum EntranceStatus { Valid, Underage, BarClosed };
        Random r = new Random();
        List<Drink> drinks = new List<Drink>();
        Dictionary<Drink, double> soldDrinks = new Dictionary<Drink, double>();
        List<Student> students = new List<Student>();
        Semaphore semaphore = new Semaphore(10, 10);
        bool closed = false;
        private readonly object drinkLocker = new object();
        private readonly object barLocker = new object();

        public Bar()
        {
            StockDrinks();
        }

        private void StockDrinks()
        {
            foreach (string drink in Enum.GetNames(typeof(DrinkType)))
            {
                Drink d = new Drink();
                d.Name = drink;
                d.Quantity = r.Next(25, 35);
                d.Price = r.NextDouble() * 6.0;
                drinks.Add(d);
                soldDrinks.Add(d, 0);
            }
        }

        public void PurchaseDrink(Drink drink)
        {
            lock (drinkLocker)
            {
                drink.Quantity -= 1;
                soldDrinks[drink] += drink.Price;
            }
        }

        public Drink GetDrink(DrinkType drink)
        {
            lock (drinkLocker)
            {
                Drink dr = drinks.FirstOrDefault(d => d.Name == drink.ToString());
                return dr;
            }
        }

        public void GetDailyIncome()
        {
            double sum = 0;
            foreach (KeyValuePair<Drink, double> entry in soldDrinks)
            {
                sum += entry.Value;
                Console.WriteLine($"{entry.Key.Name}: Quantity sold {Math.Round(entry.Value / entry.Key.Price)}, Money earned: {Math.Round(entry.Value, 2)}, In stock: {entry.Key.Quantity}");
            }
            Console.WriteLine($"Total daily income : {Math.Round(sum, 2)}");
        }

        public void CloseBar()
        {
            Console.WriteLine($"{students.Count} people have started leaving the bar.");

            lock (students)
            {
                foreach (Student student in students)
                {
                    student.staysAtBar = false;
                }
            }

            for (int i = 0; i < 10; i++)
            {
                semaphore.WaitOne();
            }

            if (students.Count == 0)
            {
                Console.WriteLine("------------------------The bar has closed.----------------------");
                return;
            }
        }

        public EntranceStatus Enter(Student student)
        {
            lock (barLocker)
            {
                if (closed)
                {
                    return EntranceStatus.BarClosed;
                }

                if (r.Next(100) == 1)
                {
                    closed = true;
                    Console.WriteLine("---------------------The bar has begun closing.---------------------");
                    CloseBar();
                    return EntranceStatus.BarClosed;
                }

                if (student.Age < 18)
                {
                    return EntranceStatus.Underage;
                }

                students.Add(student);
                semaphore.WaitOne();
                return EntranceStatus.Valid;
            }
        }

        public void Leave(Student student)
        {
            lock (students)
            {
                students.Remove(student);
            }
            semaphore.Release();
        }
    }
}