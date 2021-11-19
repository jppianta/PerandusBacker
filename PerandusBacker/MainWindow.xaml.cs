using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Animation;
using PerandusBacker.Pages;
using PerandusBacker.Utils;
using System;
using System.Collections.Generic;
using Windows.Graphics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PerandusBacker
{
  /// <summary>
  /// An empty window that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class MainWindow : Window
  {
    private AppWindow appWindow;
    private Dictionary<string, Type> Pages = new Dictionary<string, Type>();

    public MainWindow()
    {
      this.InitializeComponent();
      appWindow = GetAppWindowForCurrentWindow();
      appWindow.SetIcon("Assets/Logos/PerandusBacker.ico");

      this.Title = "Perandus Backer";

      this.InitializePages();
      Events.SwitchPageHandler += SwitchPage;
      Events.ResizeWindowHandler += ResizeWindow;

      AttemptLogin();
    }

    private async void AttemptLogin()
    {
      try
      {
        if (Storage.LoadUserInfo() && (await Network.AttemptLogin()))
        {
          SwitchPage("Main");
        }
        else
        {
          SwitchPage("Login");
        }
      }
      catch
      {
        SwitchPage("Login");
      }
      finally
      {
        this.Activate();
      }
    }

    private void InitializePages()
    {
      Pages["Login"] = typeof(Login);
      Pages["Main"] = typeof(Main);
    }

    void SwitchPage(object sender, SwitchPageEventArgs args)
    {
      SwitchPage(args.PageName);
    }

    private void SwitchPage(string page)
    {
      if (Pages.ContainsKey(page))
      {
        RootFrame.Navigate(Pages[page], null, new DrillInNavigationTransitionInfo());
      }
    }

    void ResizeWindow(object sender, ResizeEventArgs args)
    {
      ResizeWindow(args.Width, args.Heigth);
    }

    private void ResizeWindow(int width, int height)
    {
      appWindow.Resize(new SizeInt32 { Height = height, Width = width });

    }

    private AppWindow GetAppWindowForCurrentWindow()
    {
      IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
      WindowId myWndId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
      return AppWindow.GetFromWindowId(myWndId);
    }
  }
}
