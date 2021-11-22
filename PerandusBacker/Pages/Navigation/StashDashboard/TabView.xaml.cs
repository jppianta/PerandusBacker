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
using PerandusBacker.Stash.Json;
using PerandusBacker.Utils;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PerandusBacker.Pages.Navigation.StashDashboard
{
  /// <summary>
  /// An empty page that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class TabView : Page
  {
    private ObservableCollection<string> Currencies = new ObservableCollection<string>(Data.CurrencyList);
    private StashItem SelectedItem;
    private int StashTabIndex;
    public TabView(int tabIndex)
    {
      StashTabIndex = tabIndex;
      this.InitializeComponent();

      Events.ItemSelectedHandler += OnItemSelected;
    }

    private Tab Tab { get => StashManager.Tabs[StashTabIndex]; }

    private void OnCurrencySelected(object sender, SelectionChangedEventArgs e)
    {
      if (SelectedItem != null)
      {
        SelectedItem.PriceCurrency = (string)CurrencyComboBox.SelectedItem;
      }
    }

    private void OnItemSelected(object sender, ItemSelectedEventArgs e)
    {
      SelectedItem = e.Item;
      if (e.Item.PriceCurrency != null)
      {
        CurrencyComboBox.SelectedItem = e.Item.PriceCurrency;
        CurrencyCountBox.Value = e.Item.PriceCount;
      } else
      {
        CurrencyComboBox.SelectedItem = null;
        CurrencyCountBox.Value = 0;
      }
    }

    private void CurrencyCountChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
    {
      if (SelectedItem != null)
      {
        SelectedItem.PriceCount = (int)CurrencyCountBox.Value;
      }
    }
  }
}
