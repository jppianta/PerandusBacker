using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections.ObjectModel;

using PerandusBacker.Utils;
using PerandusBacker.Stash.Json;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PerandusBacker.Controls
{
  public sealed class PriceBar : Control
  {
    private ObservableCollection<string> Currencies = new ObservableCollection<string>(Data.CurrencyList);
    private StashItem SelectedItem;
    private ComboBox CurrencyComboBox;
    private NumberBox CurrencyCountBox;
    public PriceBar()
    {
      this.DefaultStyleKey = typeof(PriceBar);
      DataContext = this;

      Events.ItemSelectedHandler += OnItemSelected;
    }

    protected override void OnApplyTemplate()
    {
      CurrencyComboBox = GetTemplateChild("CurrencyComboBox") as ComboBox;
      CurrencyComboBox.ItemsSource = Currencies;
      CurrencyComboBox.SelectionChanged += OnCurrencySelected;

      CurrencyCountBox = GetTemplateChild("CurrencyCountBox") as NumberBox;
      CurrencyCountBox.ValueChanged += CurrencyCountChanged;
    }

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
      }
      else
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
