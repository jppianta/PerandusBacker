using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using PerandusBacker.Json;

namespace PerandusBacker.Stash
{
  public enum PropertyColor
  {
    Normal,
    Magic,
    Cold,
    Lightning,
    Fire,
    Chaos,
  }
  public class PropertyValue
  {
    public string Value { get; set; }

    public PropertyColor Color { get; set; }
  }

  public class ItemProperty
  {
    public string Name { get; set; }

    public PropertyValue[] Values { get; set; }
  }

  public static class ItemProperties
  {
    public static (Dictionary<string, int>, ItemProperty[]) ParseItemProperties(ItemPropertyJson[] properties)
    {
      ItemProperty[] itemProperties = new ItemProperty[properties.Length];
      Dictionary<string, int> valueablePropertyIndexes = new Dictionary<string, int>();

      for (int i = 0; i < properties.Length; i++)
      {
        ItemPropertyJson property = properties[i];
        ItemProperty itemProperty = new ItemProperty();

        switch (property.DisplayMode)
        {
          case 3:
            itemProperty.Name = Regex.Replace(property.Name, "{.*?}", new MatchEvaluator((Match match) =>
            {
              ;
              int idx = Int32.Parse(match.Value.TrimStart('{').TrimEnd('}'));

              return (property.Values[idx][0]).ToString();
            }));
            break;
          default:
            itemProperty.Name = property.Name;
            break;
        }

        itemProperty.Values = property.Values.Select(value =>
          new PropertyValue() { Value = (value[0]).ToString(), Color = (PropertyColor)Int32.Parse((value[1]).ToString()) }
        ).ToArray();
        valueablePropertyIndexes.Add(itemProperty.Name, i);

        itemProperties[i] = itemProperty;
      }

      return (valueablePropertyIndexes, itemProperties);
    }
  }
}
