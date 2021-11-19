using System.Text.Json.Serialization;

namespace PerandusBacker.Utils
{
  internal class AccountInfo
  {
    public string Name { get; set; }
    public string Image { get; set; }
  }

  internal class LeagueInfo
  {
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("realm")]
    public string Realm { get; set; }
  }
  static class Data
  {
    public static AccountInfo Account = new AccountInfo();
    public static LeagueInfo League = new LeagueInfo();
  }
}
