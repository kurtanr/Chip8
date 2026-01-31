namespace Chip8.UI.Wpf.Interaction;

public interface IFileInteraction
{
  string GetBinaryPathForOpening();

  bool ConfirmGetBinaryPathForOpening();

  string GetCodePathForOpening();

  bool ConfirmGetCodePathForOpening();

  string GetBinaryPathForSaving(string? suggestedFileNameOrNull);

  string GetCodePathForSaving(string? suggestedFileNameOrNull);
}
