using System;
using System.Collections.Generic;
using System.Threading;

namespace BarSim
{
    class Bar
    {

        public enum DrinkType {Vodka, Whiskey, Gin, Beer, Wine, Rum, Cognac, Brandy, Vermouth, Cider, Spirit};
        public enum EntranceStatus {Valid, Underage, BarClosed};
        Random r = new Random();
        List<Drink> drinks = new List<Drink>();
        List<Student> students = new List<Student>();
        Semaphore semaphore = new Semaphore(10, 10);

        public Bar()
        {
            GenerateDrinks();
        }

        private void GenerateDrinks()
        {
            foreach (string drink in Enum.GetNames(typeof(DrinkType)))
            {
                Drink d = new Drink();
                d.Name = drink;
                d.Quantity = r.Next(25, 35);
                d.Price = r.NextDouble() * 10.0;
                drinks.Add(d);
            }
        }
        public void Enter(Student student)
        {
            semaphore.WaitOne();
            lock (students)
            {
                students.Add(student);
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