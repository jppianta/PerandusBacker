﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using PerandusBacker.Stash.Json;
using PerandusBacker.Stash;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PerandusBacker.Controls
{
  public sealed class IntemInfoPanel : Control
  {
    public StashItem Item
    {
      get => (StashItem)GetValue(ItemProperty);
      set => SetValue(ItemProperty, value);
    }

    public static readonly DependencyProperty ItemProperty =
      DependencyProperty.Register(nameof(Item), typeof(StashItem),
        typeof(IntemInfoPanel), new PropertyMetadata(null, new PropertyChangedCallback(OnItemChanged)));

    private StackPanel InfoStackPanel;
    public IntemInfoPanel()
    {
      this.DefaultStyleKey = typeof(IntemInfoPanel);
    }

    protected override void OnApplyTemplate()
    {
      InfoStackPanel = GetTemplateChild("InfoStackPanel") as StackPanel;

      CreatePanel();
    }

    private static void OnItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      IntemInfoPanel panel = (IntemInfoPanel)d;

      panel.CreatePanel();
    }

    private void CreatePanel()
    {
      if (InfoStackPanel != null)
      {
        InfoStackPanel.Children.Clear();

        (List<(ItemProperty[], Orientation)> propertiesList, List<string[]>  modsList) = GetPropertiesAndModifiers();

        InfoStackPanel.Children.Add(CreatePropertiesPanel(propertiesList, HasMods(modsList)));
        InfoStackPanel.Children.Add(CreateModsPanel(modsList));
      }
    }

    private bool HasMods(List<string[]> mods)
    {
      return mods.Count > 0;
    }

    private (List<(ItemProperty[], Orientation)>, List<string[]>) GetPropertiesAndModifiers()
    {
      List<(ItemProperty[], Orientation)> properties = new List<(ItemProperty[], Orientation)>();
      List<string[]> modifiers = new List<string[]>();

      if (Item != null)
      {
        if (Item.Properties != null)
        {
          properties.Add((Item.Properties, Orientation.Vertical));
        }
        if (Item.AdditionalProperties != null)
        {
          properties.Add((Item.AdditionalProperties, Orientation.Vertical));
        }
        if (Item.Requirements != null)
        {
          properties.Add((Item.Requirements, Orientation.Horizontal));
        }

        if (Item.ImplicitMods != null)
        {
          modifiers.Add(Item.ImplicitMods);
        }
        if (Item.ExplicitMods != null)
        {
          modifiers.Add(Item.ExplicitMods);
        }
      }

      return (properties, modifiers);
    }

    private SolidColorBrush GetPropertyColor(PropertyColor color)
    {
      SolidColorBrush brush = new SolidColorBrush();

      switch (color)
      {
        case PropertyColor.Magic: brush.Color = ColorHelper.FromArgb(255, 0, 78, 140); break;
        case PropertyColor.Cold: brush.Color = ColorHelper.FromArgb(255, 0, 183, 195); break;
        case PropertyColor.Fire: brush.Color = ColorHelper.FromArgb(255, 164, 38, 44); break;
        case PropertyColor.Lightning: brush.Color = ColorHelper.FromArgb(255, 193, 156, 0); break;
        default: brush.Color = ColorHelper.FromArgb(255, 50, 49, 48); break;
      }

      return brush;
    }

    private StackPanel CreatePropertyItem(ItemProperty property)
    {
      StackPanel panel = new StackPanel();
      panel.Orientation = Orientation.Horizontal;
      panel.HorizontalAlignment = HorizontalAlignment.Center;
      panel.Spacing = 4;

      bool hasValues = property.Values != null && property.Values.Length > 0;

      panel.Children.Add(
        new TextBlock() { Text = $"{property.Name}{(hasValues ? ":" : "")}" }
      );

      if (hasValues)
      {
        StackPanel properties = new StackPanel();
        properties.Orientation = Orientation.Horizontal;
        properties.Spacing = 4;

        foreach (PropertyValue value in property.Values)
        {
          properties.Children.Add(
            new TextBlock() { Text = value.Value, Foreground = GetPropertyColor(value.Color) }
          );
        }

        panel.Children.Add(properties);
      }

      return panel;
    }

    private StackPanel CreatePropertyList((ItemProperty[], Orientation) properties)
    {
      StackPanel panel = new StackPanel();
      panel.Orientation = properties.Item2;
      panel.HorizontalAlignment = HorizontalAlignment.Center;
      panel.Spacing = properties.Item2 == Orientation.Horizontal ? 4 : 0;

      for (int i = 0; i < properties.Item1.Length; i++)
      {
        ItemProperty property = properties.Item1[i];

        if (properties.Item2 == Orientation.Horizontal)
        {
          StackPanel propertyPanel = new StackPanel();
          propertyPanel.Orientation = Orientation.Horizontal;

          propertyPanel.Children.Add(
            CreatePropertyItem(property)
          );
          if (i != properties.Item1.Length - 1)
          {
            propertyPanel.Children.Add(
              new TextBlock() { Text = "," }
            );
          }

          panel.Children.Add(propertyPanel);
        }
        else
        {
          panel.Children.Add(
            CreatePropertyItem(property)
          );
        }
      }

      return panel;
    }

    private StackPanel CreatePropertiesPanel(List<(ItemProperty[], Orientation)> propertiesList, bool hasMods)
    {
      StackPanel panel = new StackPanel();
      panel.Orientation = Orientation.Vertical;

      for (int i = 0; i < propertiesList.Count; i++)
      {
        (ItemProperty[], Orientation) properties = propertiesList[i];

        panel.Children.Add(
          CreatePropertyList(properties)
        );

        if (i != propertiesList.Count - 1 || hasMods)
        {
          panel.Children.Add(
            new MenuFlyoutSeparator() { Margin = new Thickness(0, 4, 0, 4) }
          );
        }
      } 

      return panel;
    }

    private TextBlock CreateModItem(string mod)
    {
      return new TextBlock() { Text = mod, Foreground = GetPropertyColor(PropertyColor.Magic), HorizontalAlignment = HorizontalAlignment.Center };
    }

    private StackPanel CreateModsList(string[] mods)
    {
      StackPanel panel = new StackPanel();

      foreach (string mod in mods)
      {
        panel.Children.Add(CreateModItem(mod));
      }

      return panel;
    }

    private StackPanel CreateModsPanel(List<string[]> modsList) {
      StackPanel panel = new StackPanel();
      panel.Orientation = Orientation.Vertical;

      for (int i = 0; i < modsList.Count; i++)
      {
        string[] mods = modsList[i];

        panel.Children.Add(
          CreateModsList(mods)
        );

        if (i != modsList.Count - 1)
        {
          panel.Children.Add(
            new MenuFlyoutSeparator() { Margin = new Thickness(0, 4, 0, 4) }
          );
        }
      }

      return panel;
    }
  }
}
