using Microsoft.UI.Xaml.Data;
using PerandusBacker.Stash.Json;
using System;

namespace PerandusBacker.Utils
{
  public class RarityToColor : IValueConverter
  {
    #region IValueConverter Members

    public object Convert(object value, Type targetType, object parameter, string language)
    {
      // The value parameter is the data from the source object.
      ItemFrame rarity = (ItemFrame)value;

      switch (rarity)
      {
        case ItemFrame.Unique: return "#da3b01";
        case ItemFrame.Rare: return "#c19c00";
        case ItemFrame.Magic: return "#004e8c";
        case ItemFrame.Gem: return "#5c2e91";
        default: return "#323130";
      }
    }

    // ConvertBack is not implemented for a OneWay binding.
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
      throw new NotImplementedException();
    }

    #endregion
  }

  public class ImageItemSize : IValueConverter
  {
    #region IValueConverter Members

    public object Convert(object value, Type targetType, object parameter, string language)
    {
      // The value parameter is the data from the source object.
      int item = (int)value;
      int multiplier = Int32.Parse((string)parameter);

      return item * multiplier;
    }

    // ConvertBack is not implemented for a OneWay binding.
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
      throw new NotImplementedException();
    }

    #endregion
  }

  public class SocketItemSize : IValueConverter
  {
    #region IValueConverter Members

    public object Convert(object value, Type targetType, object parameter, string language)
    {
      // The value parameter is the data from the source object.
      int item = (int)value;
      item = item > 3 ? 3 : item;
      int multiplier = Int32.Parse((string)parameter);

      return item * multiplier;
    }

    // ConvertBack is not implemented for a OneWay binding.
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
      throw new NotImplementedException();
    }

    #endregion
  }

  public class SocketColourConverter : IValueConverter
  {
    #region IValueConverter Members

    public object Convert(object value, Type targetType, object parameter, string language)
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

    // ConvertBack is not implemented for a OneWay binding.
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
      throw new NotImplementedException();
    }

    #endregion
  }

  public class SocketDirection : IValueConverter
  {
    #region IValueConverter Members

    public object Convert(object value, Type targetType, object parameter, string language)
    {
      // The value parameter is the data from the source object.
      ItemProperty[] item = (ItemProperty[])value;

      if (item?[0].Name == "Chance to Block")
      {
        return "RightToLeft";
      }

      return "LeftToRight";
    }

    // ConvertBack is not implemented for a OneWay binding.
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
      throw new NotImplementedException();
    }

    #endregion
  }

  public class HasProperty : IValueConverter
  {
    #region IValueConverter Members

    public object Convert(object value, Type targetType, object parameter, string language)
    {
      // The value parameter is the data from the source object.
      ItemProperty property = (ItemProperty)value;
      string val = parameter == null ? property.Value : (string)parameter;

      return property.Value == "" ? "" : val;
    }

    // ConvertBack is not implemented for a OneWay binding.
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}
