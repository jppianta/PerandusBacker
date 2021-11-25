using System.Text.Json.Serialization;

namespace PerandusBacker.Json
{
  public enum SocketColour
  {
    Red,
    Green,
    Blue,
    White
  }

  public enum ItemFrame
  {
    Normal,
    Magic,
    Rare,
    Unique,
    Gem
  }

  public class ItemSocket
  {
    private SocketColour ToColour(string s)
    {
      switch (s)
      {
        case "R": return SocketColour.Red;
        case "G": return SocketColour.Green;
        case "B": return SocketColour.Blue;
        default: return SocketColour.White;
      }
    }

    [JsonPropertyName("group")]
    public int Group { get; set; }

    public SocketColour Colour { get; set; }

    public int SocketIndex { get; set; }

    [JsonPropertyName("sColour")]
    public string sColour { get => ""; set => Colour = ToColour(value); }
  }

  public class ItemPropertyJson
  {
    [JsonPropertyName("values")]
    public object[][] Values { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("type")]
    public int? Type { get; set; }

    [JsonPropertyName("displayMode")]
    public int DisplayMode { get; set; }
  }

  public class StashItem
  {
    [JsonPropertyName("w")]
    public int Width { get; set; }

    [JsonPropertyName("h")]
    public int Height { get; set; }

    [JsonPropertyName("icon")]
    public string Icon { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("inventoryId")]
    public string InventoryId { get; set; }

    [JsonPropertyName("corrupted")]
    public bool Corrupted { get; set; }

    [JsonPropertyName("typeLine")]
    public string TypeLine { get; set; }

    [JsonPropertyName("baseType")]
    public string BaseType { get; set; }

    [JsonPropertyName("identified")]
    public bool Identified { get; set; }

    [JsonPropertyName("ilvl")]
    public int ItemLevel { get; set; }

    [JsonPropertyName("x")]
    public int X { get; set; }

    [JsonPropertyName("y")]
    public int Y { get; set; }

    [JsonPropertyName("frameType")]
    public ItemFrame FrameType { get; set; }

    [JsonPropertyName("sockets")]
    public ItemSocket[] Sockets { get; set; }

    [JsonPropertyName("stackSize")]
    public int StackSize { get; set; }

    [JsonPropertyName("maxStackSize")]
    public int MaxStackSize { get; set; }

    [JsonPropertyName("properties")]
    public ItemPropertyJson[] Properties { get; set; }

    [JsonPropertyName("additionalProperties")]
    public ItemPropertyJson[] AdditionalProperties { get; set; }

    [JsonPropertyName("requirements")]
    public ItemPropertyJson[] Requirements { get; set; }

    [JsonPropertyName("implicitMods")]
    public string[] ImplicitMods { get; set; }

    [JsonPropertyName("explicitMods")]
    public string[] ExplicitMods { get; set; }
  }
}
