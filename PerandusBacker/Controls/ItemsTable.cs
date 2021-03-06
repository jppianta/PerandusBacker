using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Controls;
using CommunityToolkit.WinUI.UI.Controls;

using PerandusBacker.Stash;
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
    private Item currentSelectedItem;

    public ItemsTable()
    {
      this.DefaultStyleKey = typeof(ItemsTable);
    }

    private static void OnItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      //ItemsTable table = (ItemsTable)d;

      //table.LoadData();
    }

    protected override void OnApplyTemplate()
    {
      StashGrid = GetTemplateChild("StashGrid") as DataGrid;

      StashGrid.SelectionChanged += OnSelectionChanged;
      StashGrid.Sorting += SortColumn;

      LoadData();
    }

    public void LoadData()
    {
      if (StashGrid != null)
      {
        StashGrid.ItemsSource = StashTab.CurrentView;

        //StashGrid.SelectedItem = currentSelectedItem;
      }
    }

    private void SortColumn(object sender, DataGridColumnEventArgs e)
    {
      if (e.Column.SortDirection == null)
      {
        StashTab.LoadItemsSorted(e.Column.Tag.ToString(), true);
        e.Column.SortDirection = DataGridSortDirection.Ascending;
      }
      else if (e.Column.SortDirection == DataGridSortDirection.Ascending)
      {
        StashTab.LoadItemsSorted(e.Column.Tag.ToString(), false);
        e.Column.SortDirection = DataGridSortDirection.Descending;
      }
      else
      {
        StashTab.LoadItems();
        e.Column.SortDirection = null;
      }

      LoadData();
      StashGrid.SelectedItem = currentSelectedItem;
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      currentSelectedItem = StashGrid.SelectedItem as Item;
      Events.ItemSelected(currentSelectedItem);
    }
  }
}
