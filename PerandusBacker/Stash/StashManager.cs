using Microsoft.UI.Xaml.Data;
using PerandusBacker.Stash.Json;
using PerandusBacker.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;


namespace PerandusBacker.Stash
{
  internal class StashManager
  {
    public int NumberOfStashes;
    public List<StashItem> Items = new List<StashItem>();
    private readonly object ItemsLock = new object();
    public StashManager()
    {

    }

    public async Task LoadStashes()
    {
      Items.Clear();
      string output = await Network.Request($"character-window/get-stash-items?accountName={Data.Account.Name}&league={Data.League.Id}&tabs=1");
      StashInfo info = JsonSerializer.Deserialize<StashInfo>(output);

      NumberOfStashes = info.NumberOfTabs;
      Task[] tasks = new Task[NumberOfStashes];
      for (int i = 0; i < NumberOfStashes; i++)
      {
        int idx = i;
        tasks[idx] = Task.Run(() => LoadItems(info.Tabs[idx]));
      }

      await Task.WhenAll(tasks);
    }

    private async Task LoadItems(StashTab tab)
    {
      try
      {
        string output = await Network.Request($"character-window/get-stash-items?accountName={Data.Account.Name}&league={Data.League.Id}&tabs=0&tabIndex={tab.Index}");
        StashItem[] items = JsonSerializer.Deserialize<StashInfo>(output).Items;

        foreach (var item in items)
        {
          item.TabInfo = tab;

          if (item.Sockets != null)
          {
            if (item.Sockets.Length > 3)
            {
              (item.Sockets[2].Colour, item.Sockets[3].Colour) = (item.Sockets[3].Colour, item.Sockets[2].Colour);
            }
          }
        }

        lock (ItemsLock)
        {
          Items.AddRange(items);
        }
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
    }

    private IEnumerable<IOrderedEnumerable<StashItem>> SortColumnQuery(string columnName, bool ascending)
    {
      switch (columnName)
      {
        default:
          if (ascending)
          {
            return from item in Items
                   group item by item.TabInfo.Index into indexedItem
                   orderby indexedItem.Key
                   let g =
                     (from groupedItem in indexedItem
                      orderby groupedItem.FullName ascending
                      select groupedItem)
                   select g;
          }
          else
          {
            return from item in Items
                   group item by item.TabInfo.Index into indexedItem
                   orderby indexedItem.Key
                   let g =
                     (from groupedItem in indexedItem
                      orderby groupedItem.FullName descending
                      select groupedItem)
                   select g;
          }
      }
    }

    public CollectionViewSource GetGroupsSorted(string columnName, bool ascending)
    {
      var query = SortColumnQuery(columnName, ascending);

      CollectionViewSource groupedItems = new CollectionViewSource();
      groupedItems.IsSourceGrouped = true;
      groupedItems.Source = query;

      return groupedItems;
    }

    public CollectionViewSource GetGroups()
    {
      return GetGroupsSorted("FullName", true);
    }
  }
}
