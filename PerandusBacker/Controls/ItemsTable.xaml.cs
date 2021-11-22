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
      DependencyProperty.Register("TabIndex", typeof(int),
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

    void OnLoaded(object sender, RoutedEventArgs e)
    {
      GridView socketPanel = (GridView)StashGrid.FindDescendant("SocketPanel");
      Canvas linkPanel = (Canvas)StashGrid.FindDescendant("LinkPanel");
      StashItem stashItem = StashGrid.SelectedItem as StashItem;

      DrawSocketLinks(socketPanel, linkPanel, stashItem);
    }

    private void DrawSocketLinks(GridView socketPanel, Canvas linkPanel, StashItem stashItem)
    {
      linkPanel.Children.Clear();

      if (socketPanel.Items.Count > 0)
      {
        ItemSocket lastItem = socketPanel.Items[0] as ItemSocket;
        for (int i = 1; i < socketPanel.Items.Count; i++)
        {
          ItemSocket currentItem = socketPanel.Items[i] as ItemSocket;
          if (lastItem.Group == currentItem.Group)
          {
            int startLevel = (i - 1) / stashItem.Width;
            int startPos = (i - 1) % stashItem.Width;

            int endLevel = i / stashItem.Width;
            int endPos = i % stashItem.Width;

            if (startLevel != endLevel)
            {
              if (endLevel % 2 != 0 && socketPanel.Items.Count >= (endLevel + 1) * stashItem.Width)
              {
                endPos = (endPos + 1) % stashItem.Width;
              }
              else
              {
                startPos = startPos == 0 ? 0 : startPos - 1;
              }
            }

            double margin = 10;
            double socketHalf = 15;

            double space = margin + socketHalf;

            double x1 = space + (startPos * space * 2);
            double y1 = space + (startLevel * space * 2);

            double x2 = space + (endPos * space * 2);
            double y2 = space + (endLevel * space * 2);

            if (startLevel != endLevel)
            {
              y1 += socketHalf;
              y2 -= socketHalf;
            }
            else
            {
              x1 += socketHalf;
              x2 -= socketHalf;
            }

            linkPanel.Children.Add(CreateLine(x1, y1, x2, y2));

            lastItem = currentItem;
          }
        }
      }
    }

    private Grid CreateLine(double x1, double y1, double x2, double y2)
    {
      Grid grid = new Grid();

      Line linkBorder = new Line()
      {
        X1 = x1,
        Y1 = y1,
        X2 = x2,
        Y2 = y2,
        StrokeThickness = 6,
        Stroke = new SolidColorBrush() { Color = Colors.Gold }
      };

      Line link = new Line()
      {
        X1 = x1,
        Y1 = y1,
        X2 = x2,
        Y2 = y2,
        StrokeThickness = 4,
        Stroke = new SolidColorBrush() { Color = Colors.DarkGoldenrod }
      };

      grid.Children.Add(linkBorder);
      grid.Children.Add(link);

      return grid;
    }

    private void OnDetailsVisibility(object sender, DataGridRowDetailsEventArgs e)
    {
      if (StashGrid.SelectedItem != e.Row.DataContext)
      {
        currentSelectedItem = e.Row.DataContext as StashItem;

        Canvas linkPanel = (Canvas)e.DetailsElement.FindDescendant("LinkPanel");
        GridView socketPanel = (GridView)e.DetailsElement.FindDescendant("SocketPanel");
        StashItem stashItem = e.Row.DataContext as StashItem;

        DrawSocketLinks(socketPanel, linkPanel, stashItem);

        Events.ItemSelected(currentSelectedItem);
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
  }
}
