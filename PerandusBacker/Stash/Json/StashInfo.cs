using System.Text.Json.Serialization;

namespace PerandusBacker.Stash.Json
{
  internal class StashInfo
  {
    [JsonPropertyName("numTabs")]
    public int NumberOfTabs { get; set; }

    [JsonPropertyName("tabs")]
    public StashTab[] Tabs { get; set; }

    [JsonPropertyName("items")]
    public StashItem[] Items { get; set; }
  }
}
