using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System;
using System.Windows;
using System.Xml;

namespace Chip8.UI.Wpf;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
  public App()
  {
    LoadAvalonEditHighlightingDefinition();
  }

  private static void LoadAvalonEditHighlightingDefinition()
  {
    const string languageName = "CHIP-8";
    string resourceName = $"{languageName}.xshd";
    var type = typeof(App);
    var fullName = type.Namespace + "." + resourceName;

    using var stream = type.Assembly.GetManifestResourceStream(fullName)
      ?? throw new InvalidOperationException($"Could not find embedded resource '{fullName}'.");

    using var reader = new XmlTextReader(stream);
    HighlightingManager.Instance.RegisterHighlighting(
      languageName, [], HighlightingLoader.Load(reader, HighlightingManager.Instance));
  }
}