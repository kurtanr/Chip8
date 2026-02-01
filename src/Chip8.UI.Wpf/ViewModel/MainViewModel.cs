using Chip8.UI.Wpf.Interaction;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Win32;

namespace Chip8.UI.Wpf.ViewModel;

public partial class MainViewModel : INotifyPropertyChanged
{
  public event PropertyChangedEventHandler? PropertyChanged;

  protected virtual void OnPropertyChanged(string propertyName)
  {
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
  }

  private class RelayCommand : ICommand
  {
    private readonly Action _execute;
    private readonly Func<bool>? _canExecute;

    public RelayCommand(Action execute, Func<bool>? canExecute = null)
    {
      _execute = execute ?? throw new ArgumentNullException(nameof(execute));
      _canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged
    {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }

    public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

    public void Execute(object? parameter) => _execute();
  }

  // Load/save commands
  public ICommand LoadApplicationCommand { get; private set; }
  public ICommand SaveApplicationCommand { get; private set; }
  public ICommand LoadSourceCodeCommand { get; private set; }
  public ICommand SaveSourceCodeCommand { get; private set; }

  // Can execute load/save commands
  public bool CanLoadApplication { get; internal set; }
  public bool CanSaveApplication { get; internal set; }
  public bool CanLoadSourceCode { get; internal set; }
  public bool CanSaveSourceCode { get; internal set; }

  // Application commands
  public ICommand RunContinueApplicationCommand { get; private set; }
  public ICommand PauseApplicationCommand { get; private set; }
  public ICommand StopApplicationCommand { get; private set; }
  public ICommand ExecuteSingleStepCommand { get; private set; }

  // Can execute application commands
  public bool CanRunContinueApplication { get; private set; }
  public bool CanPauseApplication { get; private set; }
  public bool CanStopApplication { get; private set; }
  public bool CanExecuteSingleStep { get; private set; }

  // Source code commands
  public ICommand NewSourceCodeCommand { get; private set; }
  public ICommand BuildSourceCodeCommand { get; private set; }
  public ICommand ToggleBreakpointCommand { get; private set; }

  // Can execute source code commands
  public bool CanNewSourceCode { get; private set; }
  public bool CanBuildSourceCode { get; private set; }
  public bool CanToggleBreakpoint { get; private set; }

  public string? InstructionInMemory
  {
    get;
    set
    {
      field = value;
      _isBinaryModified = true;

      OnPropertyChanged(nameof(InstructionInMemory));
      OnPropertyChanged(nameof(Title));
    }
  }

  public string? InstructionInCode
  {
    get;
    set
    {
      field = value;
      _isCodeModified = true;

      OnPropertyChanged(nameof(InstructionInCode));
      RefreshUI();
    }
  }

  public string? CpuRegisters
  {
    get;
    set
    {
      field = value;

      OnPropertyChanged(nameof(CpuRegisters));
    }
  }

  public string? FpsIpsStats
  {
    get;
    set
    {
      field = value;

      OnPropertyChanged(nameof(FpsIpsStats));
    }
  }

  private const string _isModifiedString = "*";
  public string Title
  {
    get
    {
      string isBinaryModified = _isBinaryModified ? _isModifiedString : "";
      string isCodeModified = _isCodeModified ? _isModifiedString : "";

      return $"Binary: {isBinaryModified}{_binaryFileName}, Code: {isCodeModified}{_codeFileName} - Chip-8";
    }
  }

  public bool IsDarkMode
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

  public MainViewModel(Emulator emulator, IFileInteraction fileInteraction, IBuildInteraction buildInteraction)
  {
    _emulator = emulator;
    _fileInteraction = fileInteraction;
    _buildInteraction = buildInteraction;

    // Load/save commands
    LoadApplicationCommand = new RelayCommand(
      LoadApplication, () => CanLoadApplication);

    SaveApplicationCommand = new RelayCommand(
      SaveApplication, () => CanSaveApplication);

    LoadSourceCodeCommand = new RelayCommand(
      LoadSourceCode, () => CanLoadSourceCode);

    SaveSourceCodeCommand = new RelayCommand(
      SaveSourceCode, () => CanSaveSourceCode);

    // Application commands
    RunContinueApplicationCommand = new RelayCommand(
      RunContinueApplication, () => CanRunContinueApplication);

    PauseApplicationCommand = new RelayCommand(
      PauseApplication, () => CanPauseApplication);

    StopApplicationCommand = new RelayCommand(
      StopApplication, () => CanStopApplication);

    ExecuteSingleStepCommand = new RelayCommand(
      ExecuteSingleStep, () => CanExecuteSingleStep);

    // Source code commands
    NewSourceCodeCommand = new RelayCommand(
      NewSourceCode, () => CanNewSourceCode);

    BuildSourceCodeCommand = new RelayCommand(
      BuildSourceCode, () => CanBuildSourceCode);

    ToggleBreakpointCommand = new RelayCommand(
      ToggleBreakpoint, () => CanToggleBreakpoint);

    RefreshUI();
  }

  public void NotifyThemeChanged()
  {
    OnPropertyChanged(nameof(IsDarkMode));
  }
}
