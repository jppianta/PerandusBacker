using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

using PerandusBacker.Utils;

namespace PerandusBacker.Stash.Json
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

  public class ItemProperty
  {
    public string Value { get; set; }
    [JsonPropertyName("values")]
    public object[][] _Value
    {
      get => new object[0][];
      set => Value = value.Length > 0 ? value[0][0].ToString() : "";
    }

    [JsonPropertyName("name")]
    public string Name { get; set; }
  }

  public class StashItem : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;
    private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      if (propertyName == "PriceCount" || propertyName == "PriceCurrency")
      {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FullPrice"));
      }
    }

    public TabInfo TabInfo { get; set; }

    private int _priceCount = 0;
    public int PriceCount
    {
      get => _priceCount;
      set
      {
        _priceCount = value;
        NotifyPropertyChanged();
      }
    }

    private string _priceCurrency = null;
    public string PriceCurrency
    {
      get => _priceCurrency;
      set
      {
        _priceCurrency = value;
        NotifyPropertyChanged();
      }
    }

    public string FullPrice { get => PriceCount > 0 && PriceCurrency != null ? $"{PriceCount} {Data.CurrencyMap(PriceCurrency)}" : ""; }

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

    public string FullName { get => Name == "" ? TypeLine : $"{Name} {TypeLine}"; }

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
    public ItemProperty[] Properties { get; set; }

    [JsonPropertyName("additionalProperties")]
    public ItemProperty[] AdditionalProperties { get; set; }

    [JsonPropertyName("requirements")]
    public ItemProperty[] Requirements { get; set; }

    [JsonPropertyName("implicitMods")]
    public string[] ImplicitMods { get; set; }

    [JsonPropertyName("explicitMods")]
    public string[] ExplicitMods { get; set; }
  }
}
