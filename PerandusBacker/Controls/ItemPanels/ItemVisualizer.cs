using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using PerandusBacker.Stash.Json;
using PerandusBacker.Utils;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PerandusBacker.Controls
{
  public sealed class ItemVisualizer : Control
  {
    private StashItem Item = null;
    private LinkPanel linkPanel;
    private SocketPanel socketPanel;
    private ItemInfoPanel infoPanel;
    public ItemVisualizer()
    {
      this.DefaultStyleKey = typeof(ItemVisualizer);
      DataContext = null;

      this.Visibility = Visibility.Collapsed;

      Events.ItemSelectedHandler += OnItemSelected;
    }

    private void OnItemSelected(object sender, ItemSelectedEventArgs e)
    {
      Item = e.Item;
      DataContext = Item;
      this.Visibility = Visibility.Visible;

      if (linkPanel != null && socketPanel != null && infoPanel != null)
      {
        linkPanel.Item = Item;
        socketPanel.Item = Item;
        infoPanel.Item = Item;
      }
    }

    protected override void OnApplyTemplate()
    {
      linkPanel = GetTemplateChild("LinkPanel") as LinkPanel;
      socketPanel = GetTemplateChild("SocketPanel") as SocketPanel;
      infoPanel = GetTemplateChild("ItemInfoPanel") as ItemInfoPanel;

      if (linkPanel != null && socketPanel != null)
      {
        linkPanel.Item = Item;
        socketPanel.Item = Item;
        infoPanel.Item = Item;
      }
    }
  }
}
