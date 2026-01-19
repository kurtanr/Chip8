using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;

namespace Chip8.UI.Wpf.View;

/// <summary>
/// See: https://stackoverflow.com/a/57385064/15770755
/// </summary>
public class SynchronisedScroll
{
  public static SynchronisedScrollToken GetToken(ScrollViewer obj)
  {
    return (SynchronisedScrollToken)obj.GetValue(TokenProperty);
  }
  public static void SetToken(ScrollViewer obj, SynchronisedScrollToken value)
  {
    obj.SetValue(TokenProperty, value);
  }
  public static readonly DependencyProperty TokenProperty =
      DependencyProperty.RegisterAttached("Token", typeof(SynchronisedScrollToken), typeof(SynchronisedScroll), new PropertyMetadata(TokenChanged));

  private static void TokenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    var scroll = d as ScrollViewer;
    var oldToken = e.OldValue as SynchronisedScrollToken;
    var newToken = e.NewValue as SynchronisedScrollToken;

    if (scroll != null)
    {
      oldToken?.unregister(scroll);
      newToken?.register(scroll);
    }
  }
}

public class SynchronisedScrollToken
{
  List<ScrollViewer> registeredScrolls = new List<ScrollViewer>();

  internal void unregister(ScrollViewer scroll)
  {
    throw new NotImplementedException();
  }

  internal void register(ScrollViewer scroll)
  {
    scroll.ScrollChanged += ScrollChanged;
    registeredScrolls.Add(scroll);
  }

  private void ScrollChanged(object sender, ScrollChangedEventArgs e)
  {
    var sendingScroll = sender as ScrollViewer;
    if (sendingScroll == null)
    {
      return;
    }

    foreach (var potentialScroll in registeredScrolls)
    {
      if (potentialScroll == sendingScroll)
        continue;

      if (potentialScroll.VerticalOffset != sendingScroll.VerticalOffset)
        potentialScroll.ScrollToVerticalOffset(sendingScroll.VerticalOffset);

      if (potentialScroll.HorizontalOffset != sendingScroll.HorizontalOffset)
        potentialScroll.ScrollToHorizontalOffset(sendingScroll.HorizontalOffset);
    }
  }
}
