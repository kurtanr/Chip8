using System.Collections.Generic;
using System.Windows;

namespace Chip8.UI.Wpf.Interaction;

internal class DialogBuildInteraction : IBuildInteraction
{
  public void ShowBuildErrors(IEnumerable<string> errors)
  {
    MessageBox.Show("Build failed with the following errors:\n" + string.Join("\n", errors), "Build Errors", MessageBoxButton.OK, MessageBoxImage.Error);
  }
}
