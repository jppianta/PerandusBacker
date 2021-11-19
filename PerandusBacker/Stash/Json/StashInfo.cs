using System.Text.Json.Serialization;

namespace PerandusBacker.Stash.Json
{
  public class StashInfo
  {
    [JsonPropertyName("numTabs")]
    public int NumberOfTabs { get; set; }

    [JsonPropertyName("tabs")]
    public TabInfo[] Tabs { get; set; }

    [JsonPropertyName("items")]
    public StashItem[] Items { get; set; }
  }
}
