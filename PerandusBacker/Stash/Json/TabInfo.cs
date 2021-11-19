using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using System.Text.Json.Serialization;

namespace PerandusBacker.Stash.Json
{
  public class RgbColour
  {
    public byte r { get; set; }
    public byte g { get; set; }
    public byte b { get; set; }
    public SolidColorBrush ToBrush()
    {
      return new SolidColorBrush() { Color = ColorHelper.FromArgb(255, r, g, b) };
    }
  }
  public class TabInfo
  {
    [JsonPropertyName("n")]
    public string Name { get; set; }

    [JsonPropertyName("colour")]
    public RgbColour Colour { get; set; }

    [JsonPropertyName("i")]
    public int Index { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
  }
}
