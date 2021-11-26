using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.RegularExpressions;
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

    private string SortingColumn = "FullName";
    private bool SortingAscending = true;

    private object Query;
    public ICollectionView CurrentView
    {
      get {
        if (Query == null)
          return null;

        CollectionViewSource itemsCollection = new CollectionViewSource();

        itemsCollection.Source = Query;

        return itemsCollection.View;
      }
    }

    public bool IsEmpty = false;
    public Tab(TabInfo info)
    {
      Info = info;
    }

    public async Task LoadItems(Dictionary<string, ItemPriceInfo> itemsPrice = null)
    {
      Items.Clear();

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

      LoadItems();
    }

    private IOrderedEnumerable<Item> SortColumnQuery()
    {
      IEnumerable<Item> items = (Query == null ? Items : Query) as IEnumerable<Item>;
      switch (SortingColumn)
      {
        case "Stack":
          if (SortingAscending)
          {
            return from item in items
                   orderby item.StackSize ascending
                   select item;
          }
          else
          {
            return from item in items
                   orderby item.StackSize descending
                   select item;
          }
        case "Corrupted":
          if (SortingAscending)
          {
            return from item in items
                   orderby item.Corrupted ascending
                   select item;
          }
          else
          {
            return from item in items
                   orderby item.Corrupted descending
                   select item;
          }
        case "Quality":
          if (SortingAscending)
          {
            return from item in items
                   orderby item.Quality ascending
                   select item;
          }
          else
          {
            return from item in items
                   orderby item.Quality descending
                   select item;
          }
        case "Physical Damage":
          if (SortingAscending)
          {
            return from item in items
                   orderby item.PhysicalDamage ascending
                   select item;
          }
          else
          {
            return from item in items
                   orderby item.PhysicalDamage descending
                   select item;
          }
        default:
          if (SortingAscending)
          {
            return from item in items
                   orderby item.FullName ascending
                   select item;
          }
          else
          {
            return from item in items
                   orderby item.FullName descending
                   select item;
          }
      }
    }

    public void SearchItems(string search)
    {
      Query = Items.Where(item => Regex.IsMatch(item.FullName, search, RegexOptions.IgnoreCase));

      Query = SortColumnQuery();

      IsEmpty = (Query as IEnumerable<Item>).Count() == 0;
    }

    public void LoadItemsSorted(string columnName, bool ascending)
    {
      SortingColumn = columnName;
      SortingAscending = ascending;

      Query = SortColumnQuery();

      IsEmpty = (Query as IOrderedEnumerable<Item>).Count() == 0;
    }

    public void LoadItems()
    {
      LoadItemsSorted("FullName", true);
    }
  }
}
