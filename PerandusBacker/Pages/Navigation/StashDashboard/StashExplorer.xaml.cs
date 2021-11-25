using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.Collections.Generic;

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
    ObservableCollection<TabViewItem> Tabs = new ObservableCollection<TabViewItem>();
    public StashExplorer()
    {
      this.InitializeComponent();
    }

    private async void LoadData(Dictionary<string, ItemPriceInfo> items)
    {
      if (!StashManager.HasLoaded)
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
      foreach (Tab tab in StashManager.Tabs)
      {
        Tabs.Add(CreateTab(tab));
      }

      TabControl.SelectedIndex = 0;
    }

    private TabViewItem CreateTab(Tab tab)
    {


      TabViewItem newTab = new TabViewItem
      {
        Header = tab.Info.Name,
        IsClosable = false,
        HeaderTemplate = HeaderTemplate
      };

      newTab.Content = new TabView(tab);

      return newTab;
    }

    private async void OnLoading(FrameworkElement sender, object args)
    {
      Prices prices = await Storage.LoadPrices();

      InitializeBackgroundTask(prices.Currencies);

      LoadData(prices.Items);
    }
  }
}
