using System;
using Microsoft.UI.Xaml.Controls;

using PerandusBacker.Pages.Navigation;
using PerandusBacker.Utils;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PerandusBacker.Pages
{
  /// <summary>
  /// An empty page that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class Main : Page
  {
    public Main()
    {
      this.InitializeComponent();
      Events.ResizeWindow(1400, 800);

      NavBar.SelectedItem = StashViewItem;
    }

    private void OnSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
      if (args.IsSettingsSelected)
      {
        contentFrame.Navigate(typeof(Settings));
      }
      else
      {
        var selectedItem = (NavigationViewItem)args.SelectedItem;
        if (selectedItem != null)
        {
          string selectedItemTag = ((string)selectedItem.Tag);
          string pageName = "PerandusBacker.Pages.Navigation." + selectedItemTag;
          Type pageType = Type.GetType(pageName);
          contentFrame.Navigate(pageType);
        }
      }
    }
  }
}
