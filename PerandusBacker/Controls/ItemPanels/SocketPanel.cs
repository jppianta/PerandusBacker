using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;

using PerandusBacker.Stash;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PerandusBacker.Controls
{
  public sealed class SocketPanel : Control
  {
    public Item Item
    {
      get => (Item)GetValue(ItemProperty);
      set => SetValue(ItemProperty, value);
    }

    public static readonly DependencyProperty ItemProperty =
      DependencyProperty.Register(nameof(Item), typeof(Item),
        typeof(SocketPanel), new PropertyMetadata(null, new PropertyChangedCallback(OnItemChanged)));

    private Grid SocketGrid;

    public SocketPanel()
    {
      this.DefaultStyleKey = typeof(SocketPanel);
    }

    private static void OnItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      SocketPanel socketPanel = (SocketPanel)d;

      socketPanel.CreatePanel();
    }

    private void CreatePanel()
    {
      if (SocketGrid != null)
      {
        if (Item == null)
        {
          ClearPanel();
        } else{
          SetDefinitions();
          CreateSockets();
        }
      }
    }

    private void SetDefinitions()
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

    private void ClearPanel()
    {
      SocketGrid.Children.Clear();
    }

    private void CreateSockets()
    {
      ClearPanel();

      for (int i = 0; i < Item.Sockets?.Length; i++)
      {
        int level = i / Item.Width;
        int position = i % Item.Width;

        // If level is odd and the sockets for that level are filled or the item is a shield,
        // fill the sockets from right to left
        if (level % 2 != 0 && ((Item.Sockets.Length >= (level + 1) * Item.Width) || Item.IsShield))
        {
          position = (i + 1) % Item.Width;
        }

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
      SocketGrid = GetTemplateChild("SocketGrid") as Grid;

      CreatePanel();
    }
  }
}
