using System;

using PerandusBacker.Stash;

namespace PerandusBacker.Utils
{
  internal class ResizeEventArgs : EventArgs
  {
    public int Heigth { get; set; }
    public int Width { get; set; }
  }

  internal class SwitchPageEventArgs : EventArgs
  {
    public string PageName { get; set; }
  }

  internal class ItemSelectedEventArgs : EventArgs
  {
    public Item SelectedItem { get; set; }
  }

  internal class SearchEventArgs : EventArgs
  {
    public string Query { get; set; }
  }

  static class Events
  {
    public static event EventHandler<SwitchPageEventArgs> SwitchPageHandler;
    public static void SwitchPage(string pageName)
    {
      SwitchPageHandler?.Invoke(null, new SwitchPageEventArgs() { PageName = pageName });
    }

    public static event EventHandler<ResizeEventArgs> ResizeWindowHandler;
    public static void ResizeWindow(int width, int height)
    {
      ResizeWindowHandler?.Invoke(null, new ResizeEventArgs() { Heigth = height, Width = width });
    }

    public static event EventHandler<ItemSelectedEventArgs> ItemSelectedHandler;
    public static void ItemSelected(Item item)
    {
      ItemSelectedHandler?.Invoke(null, new ItemSelectedEventArgs() { SelectedItem = item });
    }

    public static event EventHandler<SearchEventArgs> SearchHandler;
    public static void Search(string query)
    {
      SearchHandler?.Invoke(null, new SearchEventArgs() { Query = query });
    }

    public static event EventHandler ReloadItemsHandler;
    public static void ReloadItems()
    {
      ReloadItemsHandler?.Invoke(null, EventArgs.Empty);
    }
  }
}
