using System.Threading;
using System.Threading.Tasks;

using PerandusBacker.Stash;
using PerandusBacker.Json;

namespace PerandusBacker.Utils
{
  internal static class BackgroundManager
  {
    public static async Task StartBackgroundJob()
    {
      await Task.Run(async () =>
      {
        while (true)
        {
          // From 10 to 10 minutes, update currency prices
          await UpdateCurrencyPrices();
          Thread.Sleep(60000);
        }
      });
    }

    public static async Task UpdateCurrencyPrices()
    {
      foreach (Currency currency in Data.CurrencyMap.Values)
      {
        if (currency.ShortName != "chaos" && currency.ShortName != "perandus")
        {
          CurrencyPriceResponse response = await Network.GetCurrencyPrices(currency);

          double total = 0;
          foreach (var resp in response.Result)
          {
            ItemPrice price = resp.Listing.Price;
            total += price.Item.Amount / price.Exchange.Amount;
          }

          // Price is set to the mean of the 20 cheapest offers for the currency 
          Data.CurrencyMap[currency.Name].ChaosPrice = total / response.Result.Length;
        }

        Thread.Sleep(6000);
      }
    }
  }
}
