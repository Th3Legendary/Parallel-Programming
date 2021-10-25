using System;
using System.Collections.Generic;
using System.Text;

namespace Online_Shop
{
    class Shop
    {
        public volatile Dictionary<string, int> stock;
        object stockLock = new object();

        public Shop(Dictionary<string, int> stock)
        {
            this.stock = stock;
        }

        public bool Buy(string product, int amount)
        {
            lock (stockLock)
            {
                if (stock.ContainsKey(product) && stock[product] >= amount)
                {
                    stock[product] -= amount;
                    return true;
                }
                return false;
            }
        }

        public void Supply(string product, int amount)
        {
            lock (stockLock)
            {
                if (stock.ContainsKey(product))
                {
                    stock[product] += amount;
                }
            }
        }
    }
}
