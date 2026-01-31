using Chip8.Instructions;
using System.ComponentModel;

namespace Chip8.UI.Wpf.ViewModel;

public class InstructionViewModel : INotifyPropertyChanged
{
  public event PropertyChangedEventHandler? PropertyChanged;

  protected virtual void OnPropertyChanged(string propertyName)
  {
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
  }

  public string AssemblyDisplay
  {
    get;
    set
    {
      field = value;
      OnPropertyChanged(nameof(AssemblyDisplay));
    }
  }

  public string SourceCodeDisplay
  {
    get;
    set
    {
      field = value;
      OnPropertyChanged(nameof(SourceCodeDisplay));
    }
  }

  public InstructionViewModel(CpuInstruction cpuInstruction, ushort address)
  {
    AssemblyDisplay = $"[0x{address:X3}] 0x{cpuInstruction.Decoded.InstructionCode:X4}  // {cpuInstruction.Description}";
    SourceCodeDisplay = cpuInstruction.Mnemonic;
  }

  public override string ToString()
  {
    return SourceCodeDisplay;
  }
}
