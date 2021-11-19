using Microsoft.UI.Xaml.Data;
using PerandusBacker.Stash.Json;
using PerandusBacker.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;


namespace PerandusBacker.Stash
{
  public static class StashManager
  {
    public static bool HasLoaded = false;
    public static int NumberOfStashes;
    public static Tab[] Tabs;

    public static async Task LoadStashes()
    {
      string output = await Network.Request($"character-window/get-stash-items?accountName={Data.Account.Name}&league={Data.League.Id}&tabs=1");
      StashInfo info = JsonSerializer.Deserialize<StashInfo>(output);

      NumberOfStashes = info.NumberOfTabs;
      Tabs = info.Tabs.Select(inf => new Tab(inf)).ToArray();
      var loadTabs = Tabs.Select(tab => tab.LoadItems());

      await Task.WhenAll(loadTabs);
      HasLoaded = true;
    }
  }
}
