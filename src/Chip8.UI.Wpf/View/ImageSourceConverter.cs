using System;
using System.Globalization;
using System.Windows.Data;

namespace Chip8.UI.Wpf.View;

public class ImageSourceConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    bool isDark = (bool)value;
    string baseName = (string)parameter;
    string suffix = isDark ? "_dark" : "_light";
    return $"/images/{baseName}{suffix}.png";
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}