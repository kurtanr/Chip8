using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;

namespace Chip8.UI.Wpf.View;

/// <summary>
/// See: https://stackoverflow.com/a/57385064/15770755
/// </summary>
public static class SynchronisedScroll
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
      oldToken?.Unregister(scroll);
      newToken?.Register(scroll);
    }
  }
}

public class SynchronisedScrollToken
{
  private readonly List<ScrollViewer> _registeredScrolls = new List<ScrollViewer>();

  internal void Unregister(ScrollViewer scroll)
  {
    if (scroll == null)
    {
      return;
    }

    scroll.ScrollChanged -= ScrollChanged;
    _registeredScrolls.Remove(scroll);
  }

  internal void Register(ScrollViewer scroll)
  {
    scroll.ScrollChanged += ScrollChanged;
    _registeredScrolls.Add(scroll);
  }

  private void ScrollChanged(object sender, ScrollChangedEventArgs e)
  {
    var sendingScroll = sender as ScrollViewer;
    if (sendingScroll == null)
    {
      return;
    }

    foreach (var potentialScroll in _registeredScrolls)
    {
      if (potentialScroll == sendingScroll)
      {
        continue;
      }

      if (potentialScroll.VerticalOffset != sendingScroll.VerticalOffset)
      {
        potentialScroll.ScrollToVerticalOffset(sendingScroll.VerticalOffset);
      }

      if (potentialScroll.HorizontalOffset != sendingScroll.HorizontalOffset)
      {
        potentialScroll.ScrollToHorizontalOffset(sendingScroll.HorizontalOffset);
      }
    }
  }
}
