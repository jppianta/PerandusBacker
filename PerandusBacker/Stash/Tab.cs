using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;
using System.Collections.ObjectModel;
using Microsoft.UI.Xaml.Data;

using PerandusBacker.Utils;
using PerandusBacker.Json;

namespace PerandusBacker.Stash
{
  public sealed class Tab
  {
    public ObservableCollection<Item> Items = new ObservableCollection<Item>();
    public TabInfo Info;
    public Tab(TabInfo info)
    {
      Info = info;
    }

    public async Task LoadItems(Dictionary<string, ItemPriceInfo> itemsPrice = null)
    {
      string output = await Network.Request($"character-window/get-stash-items?accountName={Data.Account.Name}&league={Data.League.Id}&tabs=0&tabIndex={Info.Index}");
      StashItem[] items = JsonSerializer.Deserialize<StashInfo>(output).Items;

      foreach (var itemJson in items)
      {
        Item item = new Item(itemJson);
        item.TabInfo = Info;

        if (itemsPrice != null && itemsPrice.ContainsKey(item.Id) && itemsPrice[item.Id].Amount > 0)
        {
          item.PriceCount = itemsPrice[item.Id].Amount;
          item.PriceCurrency = itemsPrice[item.Id].Currency;
        }

        Items.Add(item);
      }
    }

    private IOrderedEnumerable<Item> SortColumnQuery(string columnName, bool ascending)
    {
      switch (columnName)
      {
        case "Stack":
          if (ascending)
          {
            return from item in Items
                   orderby item.StackSize ascending
                   select item;
          }
          else
          {
            return from item in Items
                   orderby item.StackSize descending
                   select item;
          }
        case "Corrupted":
          if (ascending)
          {
            return from item in Items
                   orderby item.Corrupted ascending
                   select item;
          }
          else
          {
            return from item in Items
                   orderby item.Corrupted descending
                   select item;
          }
        case "Quality":
          if (ascending)
          {
            return from item in Items
                   orderby item.Quality ascending
                   select item;
          }
          else
          {
            return from item in Items
                   orderby item.Quality descending
                   select item;
          }
        case "Physical Damage":
          if (ascending)
          {
            return from item in Items
                   orderby item.PhysicalDamage ascending
                   select item;
          }
          else
          {
            return from item in Items
                   orderby item.PhysicalDamage descending
                   select item;
          }
        default:
          if (ascending)
          {
            return from item in Items
                   orderby item.FullName ascending
                   select item;
          }
          else
          {
            return from item in Items
                   orderby item.FullName descending
                   select item;
          }
      }
    }

    public CollectionViewSource GetItemsSorted(string columnName, bool ascending)
    {
      IOrderedEnumerable<Item> query = SortColumnQuery(columnName, ascending);

      CollectionViewSource itemsCollection = new CollectionViewSource();
      itemsCollection.Source = query;

      return itemsCollection;
    }

    public IOrderedEnumerable<Item> GetItemsQuery()
    {
      return SortColumnQuery("FullName", true);
    }

    public CollectionViewSource GetItems()
    {
      return GetItemsSorted("FullName", true);
    }
  }
}
