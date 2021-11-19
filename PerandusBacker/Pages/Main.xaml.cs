using Microsoft.UI.Xaml.Controls;

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
      Events.ResizeWindow(800, 600);
    }
  }
}
