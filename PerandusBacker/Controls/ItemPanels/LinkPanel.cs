using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using PerandusBacker.Stash.Json;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PerandusBacker.Controls
{
  public sealed class LinkPanel : Control
  {
    public StashItem Item
    {
      get => (StashItem)GetValue(ItemProperty);
      set => SetValue(ItemProperty, value);
    }

    public static readonly DependencyProperty ItemProperty =
      DependencyProperty.Register(nameof(Item), typeof(StashItem),
        typeof(LinkPanel), new PropertyMetadata(null, new PropertyChangedCallback(OnItemChanged)));

    public LinkPanel()
    {
      this.DefaultStyleKey = typeof(LinkPanel);
    }

    private static void OnItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      LinkPanel linkPanel = (LinkPanel)d;

      linkPanel.DataContext = e.NewValue;
      Canvas LinkPanel = linkPanel.GetTemplateChild("LinkPanel") as Canvas;

      if (LinkPanel != null)
      {
        if (linkPanel.Item.Sockets != null)
        {
          int spacing = 10;
          int socketSize = 30;
          int rows = (linkPanel.Item.Sockets.Length % linkPanel.Item.Width == 0 ? linkPanel.Item.Sockets.Length : linkPanel.Item.Sockets.Length + 1) / linkPanel.Item.Width;
          int columns = linkPanel.Item.Width; 

          LinkPanel.Height = (rows * socketSize) + ((rows - 1) * spacing);
          LinkPanel.Width = (columns * socketSize) + ((columns - 1) * spacing);
        }

        linkPanel.DrawSocketLinks(LinkPanel);
      }
    }

    protected override void OnApplyTemplate()
    {
      DataContext = Item;
      Canvas LinkPanel = GetTemplateChild("LinkPanel") as Canvas;

      if (Item.Sockets != null)
      {
        int spacing = 10;
        int socketSize = 30;

        int rows = (Item.Sockets.Length % 2 == 0 ? Item.Sockets.Length : Item.Sockets.Length + 1) / Item.Width;
        int columns = Item.Width;

        LinkPanel.Height = (rows * socketSize) + ((rows - 1) * spacing);
        LinkPanel.Width = (columns * socketSize) + ((columns - 1) * spacing);
      }

      DrawSocketLinks(LinkPanel);
    }

    private void DrawSocketLinks(Canvas LinkPanel)
    {
      LinkPanel.Children.Clear();

      if (Item.Sockets?.Length > 0)
      {
        ItemSocket lastItem = Item.Sockets[0];
        for (int i = 1; i < Item.Sockets.Length; i++)
        {
          ItemSocket currentItem = Item.Sockets[i];
          if (lastItem.Group == currentItem.Group)
          {
            int startLevel = (i - 1) / Item.Width;
            int startPos = (i - 1) % Item.Width;

            int endLevel = i / Item.Width;
            int endPos = i % Item.Width;

            if (startLevel != endLevel)
            {
              // If ending level is odd and the sockets for that level are filled or the item is a shield,
              // link sockets from right to left
              if (endLevel % 2 != 0 && ((Item.Sockets.Length >= (endLevel + 1) * Item.Width) || Item.IsShield))
              {
                endPos = (endPos + 1) % Item.Width;
              }
              else
              {
                startPos = startPos == 0 ? 0 : startPos - 1;
              }
            }

            double spacing = 10;
            double socketHalf = 15;

            double socketSizePlusSpacing = socketHalf * 2 + spacing;

            double x1 = socketHalf + startPos * socketSizePlusSpacing;
            double y1 = socketHalf + startLevel * socketSizePlusSpacing;

            double x2 = socketHalf + endPos * socketSizePlusSpacing;
            double y2 = socketHalf + endLevel * socketSizePlusSpacing;

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

            LinkPanel.Children.Add(CreateLine(x1, y1, x2, y2));

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
  }
}
