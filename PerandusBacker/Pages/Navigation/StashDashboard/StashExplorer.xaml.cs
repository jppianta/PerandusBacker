using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;

using PerandusBacker.Stash;
using PerandusBacker.Json;
using PerandusBacker.Utils;

namespace PerandusBacker.Pages.Navigation.StashDashboard
{
  /// <summary>
  /// An empty page that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class StashExplorer : Page
  {
    public Dictionary<string, (TabViewItem, int)> tabMap = new Dictionary<string, (TabViewItem, int)>();
    public StashExplorer()
    {
      this.InitializeComponent();

      Events.SearchHandler += SearchOnTabs;
      Events.ReloadItemsHandler += ReloadItems;
    }

    private async void LoadData(Dictionary<string, ItemPriceInfo> items, bool refresh = false)
    {
      if (!StashManager.HasLoaded || refresh)
      {
        LoadingControl.IsLoading = true;

        await StashManager.LoadStashes(items);

        LoadingControl.IsLoading = false;
      }

      InitializeTabs();
    }

    private async void InitializeBackgroundTask(Dictionary<string, CurrencyPriceInfo> currencies)
    {
      if (!BackgroundManager.IsJobRunning)
      {
        await BackgroundManager.StartBackgroundJob(currencies);
      }
    }

    private void InitializeTabs()
    {
      for (int i = 0; i < StashManager.Tabs.Length; i++)
      {
        Tab tab = StashManager.Tabs[i];

        if (!tabMap.ContainsKey(tab.Info.Id))
        {
          TabViewItem item = CreateTab(tab, i);

          if (!tab.IsEmpty)
          {
            TabControl.TabItems.Add(item);
          }
        }
      }

      TabControl.SelectedIndex = 0;
    }

    private void SearchOnTabs(object _, SearchEventArgs e)
    {
      StashManager.Tabs.AsParallel().ForAll(tab => tab.SearchItems(e.Query));
      foreach (Tab tab in StashManager.Tabs)
      {
        (TabViewItem tabViewItem, int index) = tabMap[tab.Info.Id];

        if (tab.IsEmpty)
        {
          TabControl.TabItems.Remove(tabViewItem);
        }
        else if (!TabControl.TabItems.Contains(tabViewItem))
        {
          TabControl.TabItems.Insert(index, tabViewItem);
        }

        (tabViewItem.Content as TabView).RefreshItems();
      }
    }

    private TabViewItem CreateTab(Tab tab, int index)
    {
      TabViewItem tabViewItem = new TabViewItem
      {
        Header = tab.Info.Name,
        IsClosable = false,
        HeaderTemplate = HeaderTemplate,
      };

      tabViewItem.Content = new TabView(tab);

      tabMap.Add(tab.Info.Id, (tabViewItem, index));

      return tabViewItem;
    }

    private async void ReloadItems(object _, EventArgs e)
    {
      LoadData((await Storage.LoadPrices()).Items, true);
    }

    private async void OnLoading(FrameworkElement sender, object args)
    {
      Prices prices = await Storage.LoadPrices();

      InitializeBackgroundTask(prices.Currencies);

      LoadData(prices.Items);
    }
  }
}
