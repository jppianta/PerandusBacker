using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

using PerandusBacker.Stash;
using PerandusBacker.Json;

namespace PerandusBacker.Utils
{
  public class Converter : IValueConverter {
    #region IValueConverter Members

    public virtual object Convert(object value, Type targetType, object parameter, string language)
    {
      throw new NotImplementedException();
    }

    // ConvertBack is not implemented for a OneWay binding.
    public virtual object ConvertBack(object value, Type targetType, object parameter, string language)
    {
      throw new NotImplementedException();
    }

    #endregion
  }

  public class RarityToColor : Converter
  {
    public override object Convert(object value, Type targetType, object parameter, string language)
    {
      // The value parameter is the data from the source object.
      ItemFrame rarity = (ItemFrame)value;

      switch (rarity)
      {
        case ItemFrame.Unique: return "#da3b01";
        case ItemFrame.Rare: return "#8F7200";
        case ItemFrame.Magic: return "#004e8c";
        case ItemFrame.Gem: return "#5c2e91";
        default: return "#323130";
      }
    }
  }

  public class IsTabEmptyVisibility : Converter
  {
    public override object Convert(object value, Type targetType, object parameter, string language)
    {
      // The value parameter is the data from the source object.
      bool IsTabEmpty = (bool)value;

      return IsTabEmpty ? Visibility.Collapsed : Visibility.Visible;
    }
  }

  public class PropertyTypeToColor : Converter
  {
    public override object Convert(object value, Type targetType, object parameter, string language)
    {
      // The value parameter is the data from the source object.
      PropertyColor color = (PropertyColor)value;

      switch (color)
      {
        case PropertyColor.Magic: return "#004e8c";
        case PropertyColor.Cold: return "#00b7c3";
        case PropertyColor.Fire: return "#a4262c";
        case PropertyColor.Lightning: return "#c19c00";
        default: return "#323130";
      }
    }
  }

  public class ImageItemSize : Converter
  {
    public override object Convert(object value, Type targetType, object parameter, string language)
    {
      // The value parameter is the data from the source object.
      int item = (int)value;
      int multiplier = Int32.Parse((string)parameter);

      return item * multiplier;
    }
  }

  public class SocketColourConverter : Converter
  {
    public override object Convert(object value, Type targetType, object parameter, string language)
    {
      // The value parameter is the data from the source object.
      SocketColour item = (SocketColour)value;

      switch (item)
      {
        case SocketColour.Red: return "#750b1c";
        case SocketColour.Blue: return "#004e8c";
        case SocketColour.Green: return "#0b6a0b";
        default: return "#faf9f8";
      }
    }
  }

  public class HasProperty : Converter
  {
    public override object Convert(object value, Type targetType, object parameter, string language)
    {
      // The value parameter is the data from the source object.
      ItemProperty property = (ItemProperty)value;
      return property.Values.Length > 0 ?
         (parameter == null ? property.Values[0].Value : (string)parameter) :
         "";
    }
  }
}
