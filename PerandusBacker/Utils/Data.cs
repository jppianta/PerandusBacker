using PerandusBacker.Json;

namespace PerandusBacker.Utils
{
  internal class AccountInfo
  {
    public string Name { get; set; }
    public string Image { get; set; }
  }

  static class Data
  {
    public static AccountInfo Account = new AccountInfo();
    public static LeagueInfo League = new LeagueInfo();
    public static string[] CurrencyList = new string[] {
      "Perandus Coin",
      "Chaos Orb",
      "Orb of Alchemy",
      "Chromatic Orb",
      "Exalted Orb",
      "Orb of Alteration",
      "Jeweller's Orb",
      "Orb of Chance",
      "Cartographer's Chisel",
      "Orb of Fusing",
      "Orb of Scouring",
      "Blessed Orb",
      "Orb of Regret",
      "Regal Orb",
      "Gemcutter's Prism",
      "Divine Orb",
      "Mirror of Kalandra",
      "Silver Coin",
    };
    public static string CurrencyMap(string name)
    {
      switch (name)
      {
        case ("Chromatic Orb"): return "chrom";
        case ("Orb of Alteration"): return "alt";
        case ("Jeweller's Orb"): return "jewel";
        case ("Orb of Chance"): return "chance";
        case ("Cartographer's Chisel"): return "chisel";
        case ("Orb of Fusing"): return "fuse";
        case ("Orb of Alchemy"): return "alch";
        case ("Orb of Scouring"): return "scour";
        case ("Blessed Orb"): return "blessed";
        case ("Orb of Regret"): return "regret";
        case ("Regal Orb"): return "regal";
        case ("Gemcutter's Prism"): return "gemcutter";
        case ("Divine Orb"): return "divine";
        case ("Exalted Orb"): return "exalted";
        case ("Mirror of Kalandra"): return "mirror";
        case ("Perandus Coin"): return "perandus";
        case ("Silver Coin"): return "silver";
        default: return "chaos";
      }
    }

    public static CurrencyPriceRequest CreateCurrencyPriceRequestObject(string currency)
    {
      return new CurrencyPriceRequest()
      {
        Exchange = new CurrencyPriceOptions()
        {
          Have = new[] { CurrencyMap(currency) },
          Want = new[] { CurrencyMap("Chaos Orb") },
          Status = new CurrencyPriceStatusOptions() {
            Option = "online"
          }
        }
      };
    }
  }
}
