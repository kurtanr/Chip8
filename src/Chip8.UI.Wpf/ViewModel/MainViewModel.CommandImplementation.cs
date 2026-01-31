using Chip8;
using Chip8.Instructions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Chip8.UI.Wpf.Interaction;

namespace Chip8.UI.Wpf.ViewModel;

public partial class MainViewModel
{
  private readonly Emulator _emulator;
  private readonly IFileInteraction _fileInteraction;
  private readonly IBuildInteraction _buildInteraction;
  private readonly ObservableCollection<InstructionViewModel> _instructions = new ObservableCollection<InstructionViewModel>();

  private const string _untitled = "Untitled";

  private string _binaryFileName = _untitled, _codeFileName = _untitled;
  private string? _binaryFilePath, _codeFilePath;
  private bool _isBinaryModified, _isCodeModified;

  #region Load/save commands

  private void LoadApplication()
  {
    if (_isBinaryModified || _isCodeModified)
    {
      if (!_fileInteraction.ConfirmGetBinaryPathForOpening())
      {
        return;
      }
    }

    var path = _fileInteraction.GetBinaryPathForOpening();
    if (string.IsNullOrWhiteSpace(path))
    {
      return;
    }

    _binaryFilePath = path;
    _binaryFileName = Path.GetFileName(_binaryFilePath);

    _codeFilePath = null;
    _codeFileName = _untitled;

    var application = File.ReadAllBytes(_binaryFilePath);
    LoadApplicationCore(application);

  }

  private void LoadApplicationCore(byte[] application)
  {
    _emulator.LoadApplication(application);
    _instructions.Clear();

    var instructionDecoder = new InstructionDecoder();
    ushort address = Cpu.MemoryAddressOfFirstInstruction;
    for (int i = 0; i < application.Length - 1; i += 2)
    {
      var instruction = GetInstruction(application[i], application[i + 1], address, instructionDecoder);
      _instructions.Add(instruction);
      address = (ushort)(address + 2);
    }

    // if length of the application is not an even number, that means that the last instruction
    // was skipped (in the preceding for loop) because it did not start on an even address
    if (application.Length % 2 != 0)
    {
      var instruction = GetInstruction(application[application.Length - 1], 0x0, address, instructionDecoder);
      _instructions.Add(instruction);
    }

    InstructionInMemory = string.Join(Environment.NewLine, _instructions.Select(x => x.AssemblyDisplay));
    InstructionInCode = string.Join(Environment.NewLine, _instructions.Select(x => x.SourceCodeDisplay));
    CpuRegisters = _emulator.GetValueOfCpuRegisters();

    _isBinaryModified = false;

    RefreshUI();
  }

  private void SaveApplication()
  {
    if (_binaryFilePath == null)
    {
      var suggestedFileNameOrNull = (_codeFilePath == null ? null : _codeFileName);
      var path = _fileInteraction.GetBinaryPathForSaving(suggestedFileNameOrNull);
      if (string.IsNullOrWhiteSpace(path))
      {
        return;
      }

      _binaryFilePath = path;
      _binaryFileName = Path.GetFileName(_binaryFilePath);
    }

    var application = _emulator.GetApplication();
    File.WriteAllBytes(_binaryFilePath, application);

    _isBinaryModified = false;

    RefreshUI();
  }

  private void LoadSourceCode()
  {
    if (_isBinaryModified || _isCodeModified)
    {
      if (!_fileInteraction.ConfirmGetCodePathForOpening())
      {
        return;
      }
    }

    var path = _fileInteraction.GetCodePathForOpening();
    if (string.IsNullOrWhiteSpace(path))
    {
      return;
    }

    _codeFilePath = path;
    _codeFileName = Path.GetFileName(_codeFilePath);

    ClearUIAfterLoadingExistingOrCreatingNewSourceCode();
  }

  private void SaveSourceCode()
  {
    if (_codeFilePath == null)
    {
      var suggestedFileNameOrNull = (_binaryFilePath == null ? null : _binaryFileName);
      var path = _fileInteraction.GetCodePathForSaving(suggestedFileNameOrNull);
      if (string.IsNullOrWhiteSpace(path))
      {
        return;
      }

      _codeFilePath = path;
      _codeFileName = Path.GetFileName(_codeFilePath);
    }

    File.WriteAllText(_codeFilePath, InstructionInCode);

    _isCodeModified = false;

    RefreshUI();
  }

  #endregion

  #region Application commands

  private void RunContinueApplication()
  {
    _emulator.RunContinueApplication();
    RefreshUI();
  }

  private void PauseApplication()
  {
    _emulator.PauseApplication();
    CpuRegisters = _emulator.GetValueOfCpuRegisters();
    RefreshUI();
  }

  private void StopApplication()
  {
    _emulator.StopApplication();
    CpuRegisters = _emulator.GetValueOfCpuRegisters();
    RefreshUI();
  }

  private void ExecuteSingleStep()
  {
    _emulator.ExecuteSingleCycle();
    CpuRegisters = _emulator.GetValueOfCpuRegisters();
    RefreshUI();
  }

  #endregion

  #region Source code commands

  private void NewSourceCode()
  {
    if (_isBinaryModified || _isCodeModified)
    {
      if (!_fileInteraction.ConfirmGetCodePathForOpening())
      {
        return;
      }
    }

    _codeFilePath = null;
    _codeFileName = _untitled;

    ClearUIAfterLoadingExistingOrCreatingNewSourceCode();
  }

  private void BuildSourceCode()
  {
    var decompiledApplication = InstructionInCode!
      .Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries)
      .Select(x => x.Trim())
      .ToArray();

    var instructionEncoder = new InstructionEncoder();
    var buildErrors = new List<string>();

    // Convert all lines of code into CPU instructions
    var cpuInstructions = new List<CpuInstruction>();
    for (int i = 0; i < decompiledApplication.Length; i++)
    {
      var cpuInstruction = instructionEncoder.GetCpuInstruction(decompiledApplication[i]);
      if (cpuInstruction.Decoded.InstructionCode == 0x0000)
      {
        buildErrors.Add($"Line {i + 1}: Unable to encode instruction: '{decompiledApplication[i]}'");
        continue;
      }

      cpuInstructions.Add(cpuInstruction);
    }

    if (buildErrors.Count > 0)
    {
      _buildInteraction.ShowBuildErrors(buildErrors);
      return;
    }

    var application = new List<byte>();
    foreach (var cpuInstruction in cpuInstructions)
    {
      // First add most significant byte
      application.Add((byte)((cpuInstruction.Decoded.InstructionCode & 0xFF00) >> 8));

      // Then least significant byte
      application.Add(cpuInstruction.Decoded.kk);
    }

    if (application.Count > 0 && application[application.Count - 1] == 0x0)
    {
      application.RemoveAt(application.Count - 1);
    }

    LoadApplicationCore(application.ToArray());

    RefreshUI();
  }

  private void ToggleBreakpoint()
  {
    // TODO
    RefreshUI();
  }

  #endregion

  #region Helper methods

  private void ClearUIAfterLoadingExistingOrCreatingNewSourceCode()
  {
    _binaryFilePath = null;
    _binaryFileName = _untitled;

    _emulator.Reset();
    _instructions.Clear();

    InstructionInMemory = string.Empty;
    InstructionInCode = _codeFilePath != null ? File.ReadAllText(_codeFilePath) : string.Empty;

    _isBinaryModified = false;
    _isCodeModified = false;

    RefreshUI();
  }

  private void RefreshUI()
  {
    CanLoadApplication = !_emulator.IsApplicationRunning && !_emulator.IsApplicationPaused;
    CanLoadSourceCode = !_emulator.IsApplicationRunning && !_emulator.IsApplicationPaused;
    CanSaveApplication = _emulator.IsApplicationLoaded && !_emulator.IsApplicationRunning && _isBinaryModified;
    CanSaveSourceCode = !_emulator.IsApplicationRunning && _isCodeModified;

    CanRunContinueApplication = _emulator.IsApplicationLoaded && !_emulator.IsApplicationRunning;
    CanPauseApplication = _emulator.IsApplicationLoaded && _emulator.IsApplicationRunning;
    CanStopApplication = _emulator.IsApplicationLoaded && (_emulator.IsApplicationRunning || _emulator.IsApplicationPaused);
    CanExecuteSingleStep = _emulator.IsApplicationLoaded && !_emulator.IsApplicationRunning;

    CanNewSourceCode = !_emulator.IsApplicationRunning && !_emulator.IsApplicationPaused;
    CanBuildSourceCode = !_emulator.IsApplicationRunning && !_emulator.IsApplicationPaused &&
      !string.IsNullOrWhiteSpace(InstructionInCode);

    CommandManager.InvalidateRequerySuggested();

    OnPropertyChanged(nameof(Title));
  }

  private InstructionViewModel GetInstruction(byte msb, byte lsb, ushort address, InstructionDecoder instructionDecoder)
  {
    var decodedInstruction = instructionDecoder.Decode(msb, lsb);
    var cpuInstruction = instructionDecoder.GetCpuInstruction(decodedInstruction);

    return new InstructionViewModel(cpuInstruction, address);
  }

  #endregion
}
