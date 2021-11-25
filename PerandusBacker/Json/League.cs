using System.Text.Json.Serialization;

namespace PerandusBacker.Json
{
  internal class LeagueInfo
  {
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("realm")]
    public string Realm { get; set; }
  }
}
