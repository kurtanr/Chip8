using Microsoft.Win32;
using System;

namespace Chip8.UI.Wpf.Helpers;

public static class ThemeHelper
{
  public static bool IsDarkMode
  {
    get
    {
#if WINDOWS
      object? systemUsesLightTheme = Registry.GetValue(
        @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize",
        "SystemUsesLightTheme",
        0);

      // Safely unbox, default to 0 (dark mode) if null
      return Convert.ToInt32(systemUsesLightTheme ?? 0) == 0;
#else
      // Default to dark mode on non-Windows platforms
      return true;
#endif
    }
  }
}
