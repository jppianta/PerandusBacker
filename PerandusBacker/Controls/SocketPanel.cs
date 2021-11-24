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

using PerandusBacker.Stash.Json;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PerandusBacker.Controls
{
  public sealed class SocketPanel : Control
  {
    public StashItem Item
    {
      get => (StashItem)GetValue(ItemProperty);
      set => SetValue(ItemProperty, value);
    }

    public static readonly DependencyProperty ItemProperty =
      DependencyProperty.Register(nameof(Item), typeof(StashItem),
        typeof(SocketPanel), new PropertyMetadata(null, new PropertyChangedCallback(OnItemChanged)));

    public SocketPanel()
    {
      this.DefaultStyleKey = typeof(SocketPanel);
    }

    private static void OnItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      SocketPanel socketPanel = (SocketPanel)d;

      Grid SocketGrid = socketPanel.GetTemplateChild("SocketGrid") as Grid;

      if (SocketGrid != null)
      {
        socketPanel.SetDefinitions(SocketGrid);
        socketPanel.CreateSockets(SocketGrid);
      }
    }

    private void SetDefinitions(Grid SocketGrid)
    {
      SocketGrid.RowDefinitions.Clear();
      SocketGrid.ColumnDefinitions.Clear();

      if (Item.Sockets != null)
      {
        int levels = (Item.Sockets.Length % Item.Width == 0 ? Item.Sockets.Length : Item.Sockets.Length + 1) / Item.Width;

        for (int i = 0; i < Item.Width; i++)
        {
          SocketGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
        }
        for (int i = 0; i < levels; i++)
        {
          SocketGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
        }
      }
    }

    private void CreateSockets(Grid SocketGrid)
    {
      SocketGrid.Children.Clear();

      for (int i = 0; i < Item.Sockets?.Length; i++)
      {
        int level = i / Item.Width;
        int position = level % 2 == 0 ? i % Item.Width : (i + 1) % Item.Width;

        Binding binding = new Binding();
        binding.Mode = BindingMode.OneWay;
        binding.Source = Item.Sockets[i];

        Socket socket = new Socket();

        socket.SetBinding(Socket.SocketInstanceProperty, binding);

        socket.SetValue(Grid.RowProperty, level);
        socket.SetValue(Grid.ColumnProperty, position);

        SocketGrid.Children.Add(socket);
      }
    }

    protected override void OnApplyTemplate()
    {
      Grid SocketGrid = GetTemplateChild("SocketGrid") as Grid;

      SetDefinitions(SocketGrid);
      CreateSockets(SocketGrid);
    }
  }
}
