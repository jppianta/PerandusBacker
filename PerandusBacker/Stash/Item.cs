using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using PerandusBacker.Utils;
using PerandusBacker.Json;

namespace PerandusBacker.Stash
{
  public class Item : INotifyPropertyChanged
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

    public TabInfo TabInfo;

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

    public int Width { get; set; }

    public int Height { get; set; }

    public string Icon { get; set; }

    public string Id { get; set; }

    public string Name { get; set; }

    public bool Corrupted { get; set; }

    public string FullName { get => Name == "" ? TypeLine : $"{Name} {TypeLine}"; }

    public string TypeLine { get; set; }

    public string BaseType { get; set; }

    public bool Identified { get; set; }

    public int ItemLevel { get; set; }

    public int X { get; set; }

    public int Y { get; set; }

    public ItemFrame FrameType { get; set; }

    public ItemSocket[] Sockets { get; set; }

    public int StackSize { get; set; }

    public int MaxStackSize { get; set; }

    public Dictionary<string, int> ValueablePropertyIndexes = new Dictionary<string, int>();

    public ItemProperty[] Properties { get; set; }

    public ItemProperty[] AdditionalProperties { get; set; }

    public ItemProperty[] Requirements { get; set; }

    public string[] ImplicitMods { get; set; }

    public string[] ExplicitMods { get; set; }

    public Item(StashItem item)
    {
      Width = item.Width;
      Height = item.Height;
      Icon = item.Icon;
      Id = item.Id;
      Name = item.Name;
      Corrupted = item.Corrupted;
      TypeLine = item.TypeLine;
      BaseType = item.BaseType;
      Identified = item.Identified;
      ItemLevel = item.ItemLevel;
      X = item.X;
      Y = item.Y;
      FrameType = item.FrameType;
      Sockets = item.Sockets;
      StackSize = item.StackSize;
      MaxStackSize = item.MaxStackSize;
      ImplicitMods = item.ImplicitMods;
      ExplicitMods = item.ExplicitMods;
      if (item.Properties != null)
        (ValueablePropertyIndexes, Properties) = ItemProperties.ParseItemProperties(item.Properties);
      if (item.AdditionalProperties != null)
        (_, AdditionalProperties) = ItemProperties.ParseItemProperties(item.AdditionalProperties);
      if (item.Requirements != null)
        (_, Requirements) = ItemProperties.ParseItemProperties(item.Requirements);
    }

    private T RetrieveProperty<T>(string propertyName, Func<string, T> parser, T defaultValue)
    {
      if (ValueablePropertyIndexes.ContainsKey(propertyName))
      {
        ItemProperty property = Properties[ValueablePropertyIndexes[propertyName]];

        return parser(property.Values[0].Value);
      }

      return defaultValue;
    }

    private string RetrieveProperty(string propertyName)
    {
      if (ValueablePropertyIndexes.ContainsKey(propertyName))
      {
        ItemProperty property = Properties[ValueablePropertyIndexes[propertyName]];

        return property.Values[0].Value;
      }

      return "";
    }

    public int Quality
    {
      get
      {
        return RetrieveProperty("Quality", (text) =>
        {
          return Int32.Parse(text.TrimStart('+').TrimEnd('%'));
        }, 0);
      }
    }

    public string QualityText { get => RetrieveProperty("Quality"); }

    public int PhysicalDamage
    {
      get
      {
        return RetrieveProperty("Physical Damage", (text) =>
        {
          string[] splitted = text.Split('-');
          return Int32.Parse(splitted[0]);
        }, 0);
      }
    }

    public string PhysicalDamageText { get => RetrieveProperty("Physical Damage"); }

    public string StackText { get => StackSize > 0 ? StackSize.ToString() : ""; }

    public string CorruptedText { get => Corrupted ? "C" : ""; }

    public bool IsShield { get => ValueablePropertyIndexes.ContainsKey("Chance to Block"); }
  }
}
