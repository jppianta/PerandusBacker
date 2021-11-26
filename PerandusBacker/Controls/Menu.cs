using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;
using System.ComponentModel;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Core;

using PerandusBacker.Utils;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PerandusBacker.Controls
{
  public sealed class Menu : Control
  {
    private TextBox SearchBox;
    private Button ReloadButton;

    private Action UpdateDebounce;
    public Menu()
    {
      this.DefaultStyleKey = typeof(Menu);

      UpdateDebounce = Data.Debounce((object _) => Search(), TimeSpan.FromSeconds(1));
    }

    protected override void OnApplyTemplate()
    {
      SearchBox = GetTemplateChild("SearchBox") as TextBox;
      ReloadButton = GetTemplateChild("ReloadButton") as Button;

      if (SearchBox != null)
      {
        SearchBox.TextChanged += (object _, TextChangedEventArgs e) => UpdateDebounce();
      }

      if (ReloadButton != null)
      {
        ReloadButton.Click += (object _, RoutedEventArgs e) => Events.ReloadItems();
      }
    }

    private void Search()
    {
      if (SearchBox != null)
      {
        DispatcherQueue.TryEnqueue(() => Events.Search(SearchBox.Text));
      }
    }
  }
}
