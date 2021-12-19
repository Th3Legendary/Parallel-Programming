using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static BarSim.Bar;

namespace BarSim
{ 
    class Student
    {
        enum NightlifeActivities { Walk, VisitBar, GoHome };
        enum BarActivities { Drink, Dance, Leave };

        Random random = new Random();
        bool staysOut = false;

        public string Name { get; set; }
        public int Age { get; set; }
        public double Budget { get; set; }
        public Bar Bar { get; set; }
        public volatile bool staysAtBar = false;

        public Student(string name, Bar bar, int age, double budget)
        {
            Name = name;
            Bar = bar;
            Age = age;
            Budget = budget;
        }

        private NightlifeActivities GetRandomNightlifeActivity()
        {
            int n = random.Next(10);
            if (n < 3) return NightlifeActivities.Walk;
            if (n < 8) return NightlifeActivities.VisitBar;
            return NightlifeActivities.GoHome;
        }

        private BarActivities GetRandomBarActivity()
        {
            int n = random.Next(10);
            if (n < 4) return BarActivities.Dance;
            if (n < 9) return BarActivities.Drink;
            return BarActivities.Leave;
        }

        private void WalkOut()
        {
            staysOut = true;
            Console.WriteLine($"{Name} is walking in the streets.");
            Thread.Sleep(100);
        }

        private void VisitBar()
        {
            Console.WriteLine($"{Name} is getting in the line to enter the bar.");      
            staysAtBar = false;
            Thread.Sleep(100);

            if (random.Next(10) == 6)
            {
                Console.WriteLine($"{Name} has decided to leave the queue and walk the streets.");
                WalkOut();
                return;
            }

            switch (Bar.Enter(this))
            {
                case EntranceStatus.BarClosed:
                    Console.WriteLine($"The bar has closed, {Name} cannot enter!");
                    return;
                case EntranceStatus.Underage:
                    Console.WriteLine($"{Name} is under 18 years of age, so he cannot enter!");
                    return;
                case EntranceStatus.Valid:
                    staysAtBar = true;
                    Console.WriteLine($"{Name} has successfully entered the bar!");
                    break;
            }

            while (staysAtBar)
            {
                var nextActivity = GetRandomBarActivity();
                switch (nextActivity)
                {
                    case BarActivities.Dance:
                        Console.WriteLine($"{Name} is dancing.");
                        Thread.Sleep(100);
                        break;

                    case BarActivities.Drink:
                        var drinkCount = Enum.GetNames(typeof(DrinkType)).Length;
                        Drink drink = Bar.GetDrink((DrinkType)random.Next(drinkCount));

                        if (drink.Price <= Budget)
                        {
                            if(drink.Quantity == 0)
                            {
                                Console.WriteLine($"{Name} wanted to purchase {drink.Name}, but it was out of stock.");
                                Thread.Sleep(100);
                                break;
                            }
                            Budget -= drink.Price;
                            Bar.PurchaseDrink(drink);
                            Console.WriteLine($"{Name} bought {drink.Name} and is ready to drink it.");
                            Thread.Sleep(100);
                            break;
                        }

                        Console.WriteLine($"{Name} does not have enough money to purchase {drink.Name}.");
                        Thread.Sleep(100);
                        break;

                    case BarActivities.Leave:                       
                        staysAtBar = false;
                        break;

                    default: throw new NotImplementedException();
                }
            }
            Console.WriteLine($"{Name} has left the bar.");
            Bar.Leave(this);
            staysOut = false;
            Thread.Sleep(100);
        }
       
        public void PaintTheTownRed()
        {
            WalkOut();
            while (staysOut)
            {
                var nextActivity = GetRandomNightlifeActivity();
                switch (nextActivity)
                {
                    case NightlifeActivities.Walk:
                        WalkOut();
                        break;
                    case NightlifeActivities.VisitBar:
                        VisitBar();
                        break;
                    case NightlifeActivities.GoHome:
                        staysOut = false;
                        break;
                    default: throw new NotImplementedException();
                }
            }
            Console.WriteLine($"{Name} is going back home.");
        }
    }
}
