using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PerandusBacker.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PerandusBacker.Pages
{
  /// <summary>
  /// An empty page that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class Login : Page
  {
    private ObservableCollection<LeagueInfo> Leagues = new ObservableCollection<LeagueInfo>();
    public Login()
    {
      this.InitializeComponent();


      Events.ResizeWindow(400, 300);
    }

    async void LoadLeagues()
    {
      Leagues.Clear();

      string output = await Network.RequestApi("leagues");
      List<LeagueInfo> leagues = JsonSerializer.Deserialize<List<LeagueInfo>>(output);

      foreach (LeagueInfo league in leagues)
      {
        Leagues.Add(league);
      }

      LeaguesComboBox.SelectedIndex = 0;
    }

    private void OnLeagueSelected(object sender, SelectionChangedEventArgs e)
    {
      Data.League = LeaguesComboBox.SelectedItem as LeagueInfo;
    }

    void OnLoading(FrameworkElement el, object e)
    {
      LoadLeagues();
    }

    async void AttemptLogin(object sender, RoutedEventArgs e)
    {
      if (await Network.AttemptLogin(PoeSessionIdBox.Password))
      {
        if (SaveCredentialsCheckBox.IsChecked is bool Checked && Checked)
        {
          Storage.SaveUserInfo(PoeSessionIdBox.Password, Data.League);
        }

        Events.SwitchPage("Main");
      }
    }
  }
}
