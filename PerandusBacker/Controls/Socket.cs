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
