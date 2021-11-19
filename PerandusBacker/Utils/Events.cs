using System;

namespace PerandusBacker.Utils
{
  public class ResizeEventArgs : EventArgs
  {
    public int Heigth { get; set; }
    public int Width { get; set; }
  }

  public class SwitchPageEventArgs : EventArgs
  {
    public string PageName { get; set; }
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
  }
}
