using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.UI.Xaml.Data;
using System.Collections.ObjectModel;

using PerandusBacker.Utils;
using PerandusBacker.Stash.Json;

namespace PerandusBacker.Stash
{
  public class Tab
  {
    public ObservableCollection<StashItem> Items = new ObservableCollection<StashItem>();
    public TabInfo Info;
    public Tab(TabInfo info)
    {
      Info = info;
    }

    public async Task LoadItems()
    {
      string output = await Network.Request($"character-window/get-stash-items?accountName={Data.Account.Name}&league={Data.League.Id}&tabs=0&tabIndex={Info.Index}");
      StashItem[] items = JsonSerializer.Deserialize<StashInfo>(output).Items;

      foreach (var item in items)
      {
        item.TabInfo = Info;

        Items.Add(item);
      }
    }

    private IOrderedEnumerable<StashItem> SortColumnQuery(string columnName, bool ascending)
    {
      switch (columnName)
      {
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
      IOrderedEnumerable<StashItem> query = SortColumnQuery(columnName, ascending);

      CollectionViewSource itemsCollection = new CollectionViewSource();
      itemsCollection.Source = query;

      return itemsCollection;
    }

    public IOrderedEnumerable<StashItem> GetItemsQuery()
    {
      return SortColumnQuery("FullName", true);
    }

    public CollectionViewSource GetItems()
    {
      return GetItemsSorted("FullName", true);
    }
  }
}
