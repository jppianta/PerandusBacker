using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.UI.Xaml.Data;

using PerandusBacker.Utils;
using PerandusBacker.Stash.Json;

namespace PerandusBacker.Stash
{
  public class Tab
  {
    public StashItem[] Items { get; set; }
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

        if (item.Sockets != null)
        {
          if (item.Sockets.Length > 3)
          {
            (item.Sockets[2].Colour, item.Sockets[3].Colour) = (item.Sockets[3].Colour, item.Sockets[2].Colour);
          }
        }
      }

      this.Items = items;
    }

    private IOrderedEnumerable<StashItem> SortColumnQuery(string columnName, bool ascending)
    {
      switch (columnName)
      {
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
      var query = SortColumnQuery(columnName, ascending);

      CollectionViewSource itemsCollection = new CollectionViewSource();
      itemsCollection.Source = query;

      return itemsCollection;
    }

    public CollectionViewSource GetItems() {
      return GetItemsSorted("FullName", true); 
    }
  }
}
