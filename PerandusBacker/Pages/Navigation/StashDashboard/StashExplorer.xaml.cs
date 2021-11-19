using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Collections.ObjectModel;

using PerandusBacker.Stash;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

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

    private async void LoadData() {
      if (!StashManager.HasLoaded) {
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

    private TabViewItem CreateTab(Tab tab)
    {
      TabViewItem newTab = new TabViewItem
      {
        Header = tab.Info.Name,
        IsClosable = false
      };

      newTab.Content = new TabView(tab.Info.Index);

      return newTab;
    }
  }
}
