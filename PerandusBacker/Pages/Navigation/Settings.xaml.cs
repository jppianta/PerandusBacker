using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

using PerandusBacker.Utils;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PerandusBacker.Pages.Navigation
{
  /// <summary>
  /// An empty page that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class Settings : Page
  {
    private int SelectedTheme = (int)Application.Current.RequestedTheme;
    private string ThreadId = Data.ThreadId;
    private string AccountName = Data.Account.Name;
    private string AccountImage = Data.Account.Image;

    private Timer timer;
    public Settings()
    {
      this.InitializeComponent();
    }

    private void SignOut(object sender, RoutedEventArgs e)
    {
      Network.UpdatePoeSessionId("");
      Data.ThreadId = "";

      Storage.SaveUserInfo("", Data.League);
      Events.SwitchPage("Login");
    }

    private void OnThreadIdChanged(object sender, TextChangedEventArgs e)
    {
      TextBox threadIdBox = (TextBox)sender;

      Data.ThreadId = threadIdBox.Text;

      // Implement a debounce of 2 seconds so that we don't write the info to the disk at every key press
      if (timer == null) {
        timer = new Timer((object _) =>
        {
          Storage.SaveUserInfo();
          timer.Dispose();
        }, null, 2000, 0);
      } else 
      {
        timer.Change(2000, 0);
      }
    }

    private void OnThemeCheck(object sender, SelectionChangedEventArgs e)
    {
      RadioButtons themeRadio = sender as RadioButtons;
      Frame rootFrame = themeRadio.XamlRoot.Content as Frame;
      if (themeRadio.SelectedIndex == 0)
      {
        rootFrame.RequestedTheme = ElementTheme.Light;
      }
      else
      {
        rootFrame.RequestedTheme = ElementTheme.Dark;
      }
    }
  }
}
