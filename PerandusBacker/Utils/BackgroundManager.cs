using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using PerandusBacker.Json;

namespace PerandusBacker.Utils
{
  internal static class BackgroundManager
  {
    public static bool IsJobRunning = false;
    public static async Task StartBackgroundJob(Dictionary<string, CurrencyPriceInfo> prices)
    {
      IsJobRunning = true;
      if (prices != null)
      {
        ApplyInitialPrices(prices);
      }

      await Task.Run(async () =>
      {
        while (true)
        {
          // From 10 to 10 minutes, update currency prices
          await UpdateCurrencyPrices();
          Thread.Sleep(TimeSpan.FromMinutes(10));
        }
      });
    }

    private static void ApplyInitialPrices(Dictionary<string, CurrencyPriceInfo> prices)
    {
      foreach ((string currencyName, CurrencyPriceInfo price) in prices)
      {
        Data.CurrencyMap[currencyName].Price = new PriceInfo() { ChaosPrice = price.ChaosPrice, Time = DateTime.FromBinary(price.TimeStamp) };
      }
    }

    private static async Task UpdateCurrencyPrices()
    {
      foreach (Currency currency in Data.CurrencyMap.Values)
      {
        if (currency.ShortName != "chaos" && currency.ShortName != "perandus")
        {
          // Just update the currency price if the the last update was 30 minutes ago or more
          if (currency.Price != null) {
            TimeSpan diff = DateTime.Now - currency.Price.Time;
            if (diff.TotalMinutes < 30) {
              continue;
            }
          }

          CurrencyPriceResponse response = await Network.GetCurrencyPrices(currency);

          double total = 0;
          foreach (var resp in response.Result)
          {
            ItemPrice price = resp.Listing.Price;
            total += price.Item.Amount / price.Exchange.Amount;
          }

          if (currency.Price == null)
          {
            currency.Price = new PriceInfo();
          }

          // Price is set to the mean of the 20 cheapest offers for the currency 
          currency.Price.ChaosPrice = total / response.Result.Length;
          currency.Price.Time = DateTime.Now;
          
          // Wait 5 minutes before trying to update the next currency price
          Thread.Sleep(TimeSpan.FromMinutes(5));
        }
      }
    }
  }
}
