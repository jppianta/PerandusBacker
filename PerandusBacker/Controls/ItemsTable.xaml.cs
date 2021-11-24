using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.UI.Xaml.Media;
using System;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml.Data;
using CommunityToolkit.WinUI.UI;
using Microsoft.UI;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Data;

using PerandusBacker.Utils;
using PerandusBacker.Stash;
using PerandusBacker.Stash.Json;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PerandusBacker.Controls
{
  /// <summary>
  /// An empty window that can be used on its own or navigated to within a Frame.
  /// </summary>
  public partial class ItemsTable : UserControl
  {
    private ObservableCollection<StashItem> Items = new ObservableCollection<StashItem>();
    public int StashTabIndex
    {
      get { return (int)GetValue(StashTabIndexProperty); }
      set
      {
        SetValue(StashTabIndexProperty, value);
        LoadData();
      }
    }

    public static readonly DependencyProperty StashTabIndexProperty =
      DependencyProperty.Register(nameof(StashTabIndex), typeof(int),
        typeof(ItemsTable), new PropertyMetadata(0));

    private StashItem currentSelectedItem = null;

    private Tab StashTab { get => StashManager.Tabs[StashTabIndex]; }

    public ItemsTable()
    {
      this.InitializeComponent();
    }

    private void LoadData()
    {
      StashGrid.ItemsSource = StashTab.GetItems().View;

      StashGrid.SelectedItem = currentSelectedItem;
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
