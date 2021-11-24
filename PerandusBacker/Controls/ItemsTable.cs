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
using CommunityToolkit.WinUI.UI.Controls;

using PerandusBacker.Stash;
using PerandusBacker.Stash.Json;
using PerandusBacker.Utils;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PerandusBacker.Controls
{
  public sealed class ItemsTable : Control
  {
    public Tab StashTab
    {
      get => (Tab)GetValue(StashTabProperty);
      set => SetValue(StashTabProperty, value);
    }

    public static readonly DependencyProperty StashTabProperty =
      DependencyProperty.Register(nameof(StashTab), typeof(Tab),
        typeof(ItemsTable), new PropertyMetadata(null, new PropertyChangedCallback(OnItemChanged)));

    private DataGrid StashGrid;
    private StashItem currentSelectedItem;

    public ItemsTable()
    {
      this.DefaultStyleKey = typeof(ItemsTable);
    }

    private static void OnItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ItemsTable table = (ItemsTable)d;

      table.LoadData();
    }

    protected override void OnApplyTemplate()
    {
      StashGrid = GetTemplateChild("StashGrid") as DataGrid;

      StashGrid.SelectionChanged += OnSelectionChanged;
      StashGrid.Sorting += SortColumn;

      LoadData();
    }

    private void LoadData()
    {
      if (StashGrid != null)
      {
        StashGrid.ItemsSource = StashTab.GetItems().View;

        StashGrid.SelectedItem = currentSelectedItem;
      }
    }

    private void SortColumn(object sender, DataGridColumnEventArgs e)
    {
      if (e.Column.SortDirection == null)
      {
        StashGrid.ItemsSource = StashTab.GetItemsSorted(e.Column.Tag.ToString(), true).View;
        e.Column.SortDirection = DataGridSortDirection.Ascending;
      }
      else if (e.Column.SortDirection == DataGridSortDirection.Ascending)
      {
        StashGrid.ItemsSource = StashTab.GetItemsSorted(e.Column.Tag.ToString(), false).View;
        e.Column.SortDirection = DataGridSortDirection.Descending;
      }
      else
      {
        StashGrid.ItemsSource = StashTab.GetItems().View;
        e.Column.SortDirection = null;
      }

      StashGrid.SelectedItem = currentSelectedItem;
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      currentSelectedItem = StashGrid.SelectedItem as StashItem;
      Events.ItemSelected(currentSelectedItem);
    }
  }
}
