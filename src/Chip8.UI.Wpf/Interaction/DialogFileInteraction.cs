using System.Runtime.Versioning;
using System.Windows.Forms;

namespace Chip8.UI.Wpf.Interaction;

[SupportedOSPlatform("windows")]
internal class DialogFileInteraction : IFileInteraction
{
  #region Opening

  public string GetBinaryPathForOpening()
  {
    var openFileDialog = GetOpenFileDialog("Open Binary File");
    return openFileDialog.ShowDialog() == DialogResult.OK ? openFileDialog.FileName : string.Empty;
  }

  public bool ConfirmGetBinaryPathForOpening()
  {
    var result = MessageBox.Show(
      "Opening a new binary file will discard current binary and source code changes. Do you want to continue?",
      "Confirm Open",
      MessageBoxButtons.YesNo,
      MessageBoxIcon.Warning);
    return result == DialogResult.Yes;
  }

  public string GetCodePathForOpening()
  {
    var openFileDialog = GetOpenFileDialog("Open Source Code File");
    return openFileDialog.ShowDialog() == DialogResult.OK ? openFileDialog.FileName : string.Empty;
  }

  public bool ConfirmGetCodePathForOpening()
  {
    var result = MessageBox.Show(
      "Opening a new source code file will discard current binary and source code changes. Do you want to continue?",
      "Confirm Open",
      MessageBoxButtons.YesNo,
      MessageBoxIcon.Warning);
    return result == DialogResult.Yes;
  }

  private OpenFileDialog GetOpenFileDialog(string title)
  {
    return new OpenFileDialog
    {
      CheckFileExists = true,
      CheckPathExists = true,
      Multiselect = false,
      Title = title
    };
  }

  #endregion

  #region Saving

  public string GetBinaryPathForSaving(string? suggestedFileNameOrNull)
  {
    var saveFileDialog = GetSaveFileDialog("Save Binary File",
      "All files (*.*)|*.*", suggestedFileNameOrNull);
    return saveFileDialog.ShowDialog() == DialogResult.OK ? saveFileDialog.FileName : string.Empty;
  }

  public string GetCodePathForSaving(string? suggestedFileNameOrNull)
  {
    var saveFileDialog = GetSaveFileDialog("Save Source Code File",
      "Chip8 code files (*.c8)|*.c8|All files (*.*)|*.*", suggestedFileNameOrNull);
    return saveFileDialog.ShowDialog() == DialogResult.OK ? saveFileDialog.FileName : string.Empty;
  }

  private SaveFileDialog GetSaveFileDialog(string title, string filter, string? suggestedFileNameOrNull)
  {
    return new SaveFileDialog
    {
      CheckPathExists = true,
      FileName = suggestedFileNameOrNull,
      Filter = filter,
      Title = title,
    };
  }

  #endregion
}
