using Microsoft.UI.Xaml.Controls;

using PerandusBacker.Stash;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PerandusBacker.Pages.Navigation.StashDashboard
{
  /// <summary>
  /// An empty page that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class TabView : Page
  {
    private Tab StashTab;
    public TabView(Tab tab)
    {
      StashTab = tab;
      this.InitializeComponent();
    }

    public void RefreshItems()
    {
      Table.LoadData();
    }
  }
}
