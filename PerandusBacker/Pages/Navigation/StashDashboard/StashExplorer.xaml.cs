using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;

using PerandusBacker.Stash;

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

      LoadData();
    }

    private async void LoadData()
    {
      if (!StashManager.HasLoaded)
      {
        LoadingControl.IsLoading = true;

        await StashManager.LoadStashes();

        LoadingControl.IsLoading = false;
      }

      InitializeTabs();
    }

    private void InitializeTabs()
    {
      foreach (Tab tab in StashManager.Tabs)
      {
        Tabs.Add(CreateTab(tab));
      }

      TabControl.SelectedIndex = 0;
    }

    private SymbolIconSource GetIcon(Tab tab)
    {
      switch (tab.Info.Type)
      {
        case "CurrencyStash": return new SymbolIconSource() { Symbol = Symbol.Placeholder };
        default: return new SymbolIconSource() { Symbol = Symbol.Placeholder };
      }
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
  }
}
