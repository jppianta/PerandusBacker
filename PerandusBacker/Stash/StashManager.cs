using Microsoft.UI.Xaml.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using PerandusBacker.Json;
using PerandusBacker.Utils;

namespace PerandusBacker.Stash
{
  public static class StashManager
  {
    public static bool HasLoaded = false;
    public static int NumberOfStashes;
    public static Tab[] Tabs;

    public static async Task LoadStashes(Dictionary<string, ItemPriceInfo> itemPrices = null)
    {
      string output = await Network.Request($"character-window/get-stash-items?accountName={Data.Account.Name}&league={Data.League.Id}&tabs=1");
      StashInfo info = JsonSerializer.Deserialize<StashInfo>(output);

      NumberOfStashes = info.NumberOfTabs;
      Tabs = info.Tabs.Select(inf => new Tab(inf)).ToArray();
      var loadTabs = Tabs.Select(tab => tab.LoadItems(itemPrices));

      await Task.WhenAll(loadTabs);
      HasLoaded = true;
    }
  }
}
