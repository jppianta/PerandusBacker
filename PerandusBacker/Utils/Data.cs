using System;
using System.Collections.Generic;

using PerandusBacker.Json;

namespace PerandusBacker.Utils
{
  internal class AccountInfo
  {
    public string Name { get; set; }
    public string Image { get; set; }
  }

  internal class PriceInfo
  {
    public double ChaosPrice { get; set; }

    public DateTime Time { get; set; }
  }

  internal class Currency
  {
    public string Name { get; set; }

    public string ShortName { get; set; }

    public PriceInfo Price { get; set; }
  }

  static class Data
  {
    public static string ThreadId = "";
    public static string PoeSessionId { get; set; }
    public static AccountInfo Account = new AccountInfo();
    public static LeagueInfo League = new LeagueInfo();

    public static Dictionary<string, Currency> CurrencyMap = new Dictionary<string, Currency>(new[] {
      new KeyValuePair<string, Currency>("Perandus Coin", new Currency() { Name = "Perandus Coin", ShortName = "perandus" }),
      new KeyValuePair<string, Currency>("Chaos Orb", new Currency() { Name = "Chaos Orb", ShortName = "chaos", Price = new PriceInfo() { ChaosPrice = 1 } }),
      new KeyValuePair<string, Currency>("Orb of Alchemy", new Currency() { Name = "Orb of Alchemy", ShortName = "alch" }),
      new KeyValuePair<string, Currency>("Chromatic Orb", new Currency() { Name = "Chromatic Orb", ShortName = "chrom" }),
      new KeyValuePair<string, Currency>("Exalted Orb", new Currency() { Name = "Exalted Orb", ShortName = "exalted" }),
      new KeyValuePair<string, Currency>("Orb of Alteration", new Currency() { Name = "Orb of Alteration", ShortName = "alt" }),
      new KeyValuePair<string, Currency>("Jeweller's Orb", new Currency() { Name = "Jeweller's Orb", ShortName = "jewellers" }),
      new KeyValuePair<string, Currency>("Orb of Chance", new Currency() { Name = "Orb of Chance", ShortName = "chance" }),
      new KeyValuePair<string, Currency>("Cartographer's Chisel", new Currency() { Name = "Cartographer's Chisel", ShortName = "chisel" }),
      new KeyValuePair<string, Currency>("Orb of Fusing", new Currency() { Name = "Orb of Fusing", ShortName = "fuse" }),
      new KeyValuePair<string, Currency>("Orb of Scouring", new Currency() { Name = "Orb of Scouring", ShortName = "scour" }),
      new KeyValuePair<string, Currency>("Blessed Orb", new Currency() { Name = "Blessed Orb", ShortName = "blessed" }),
      new KeyValuePair<string, Currency>("Orb of Regret", new Currency() { Name = "Orb of Regret", ShortName = "regret" }),
      new KeyValuePair<string, Currency>("Regal Orb", new Currency() { Name = "Regal Orb", ShortName = "regal" }),
      new KeyValuePair<string, Currency>("Gemcutter's Prism", new Currency() { Name = "Gemcutter's Prism", ShortName = "gemcutter" }),
      new KeyValuePair<string, Currency>("Divine Orb", new Currency() { Name = "Divine Orb", ShortName = "divine" }),
      new KeyValuePair<string, Currency>("Mirror of Kalandra", new Currency() { Name = "Mirror of Kalandra", ShortName = "mirror" }),
      new KeyValuePair<string, Currency>("Silver Coin", new Currency() { Name = "Silver Coin", ShortName = "silver" }),
    });

    public static CurrencyPriceRequest CreateCurrencyPriceRequestObject(Currency currency)
    {
      return new CurrencyPriceRequest()
      {
        Exchange = new CurrencyPriceOptions()
        {
          Have = new[] { currency.ShortName },
          Want = new[] { CurrencyMap["Chaos Orb"].ShortName },
          Status = new CurrencyPriceStatusOptions()
          {
            Option = "online"
          }
        }
      };
    }

    public static Action Debounce(System.Threading.TimerCallback callback, TimeSpan period)
    {
      System.Threading.Timer timer = null;

      return () =>
      {
        if (timer == null)
        {
          timer = new System.Threading.Timer(callback, null, (int)period.TotalMilliseconds, 0);
        }
        else
        {
          timer.Change((int)period.TotalMilliseconds, 0);
        }
      };
    }
  }
}
