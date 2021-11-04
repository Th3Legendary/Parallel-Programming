using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace PriceScraper
{
    class Parser
    {
        public static string Parse(string link)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(GetHTML(link));
            string productTitle = htmlDoc.DocumentNode.QuerySelector(".product-title h1")?.InnerText.Trim();
            string mainPrice = htmlDoc.DocumentNode.QuerySelector("#price-tag")?.InnerText.Trim();
            string coins = htmlDoc.DocumentNode.QuerySelector(".after-decimal")?.InnerText.Trim();
            string currency = htmlDoc.DocumentNode.QuerySelector(".currency")?.InnerText.Trim();
            return $"The price for {productTitle} is: {mainPrice}{coins} {currency}";
        }
        static string GetHTML(string link)
        {
            HttpClient client = new HttpClient();
            return client.GetStringAsync(link).Result;
        }
    }
}
