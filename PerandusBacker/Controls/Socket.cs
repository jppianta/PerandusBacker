using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using PerandusBacker.Json;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PerandusBacker.Controls
{
  public sealed class Socket : Control
  {
    public ItemSocket SocketInstance
    {
      get => (ItemSocket)GetValue(SocketInstanceProperty);
      set => SetValue(SocketInstanceProperty, value);
    }

    public static readonly DependencyProperty SocketInstanceProperty =
      DependencyProperty.Register(nameof(SocketInstance), typeof(ItemSocket),
        typeof(Socket), new PropertyMetadata(null));

    public Socket()
    {
      this.DefaultStyleKey = typeof(Socket);
    }

    protected override void OnApplyTemplate()
    {
      DataContext = SocketInstance;
    }
  }
}
