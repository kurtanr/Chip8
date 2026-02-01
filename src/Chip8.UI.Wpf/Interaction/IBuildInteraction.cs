using System.Collections.Generic;

namespace Chip8.UI.Wpf.Interaction;

public interface IBuildInteraction
{
  void ShowBuildErrors(IEnumerable<string> errors);
}
