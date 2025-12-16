using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chip8;

/// <summary>
/// Emulator for the Chip8.<br></br>
/// Uses <see cref="Cpu"/>, <see cref="IKeyboard"/> and <see cref="IDisplay"/> to emulate a Chip8 system.<br></br>
/// Runs the emulation loop at around 60Hz. Emulation loop:<br></br>
/// - decodes single instruction using <see cref="InstructionDecoder"/><br></br>
/// - executes decoded instruction using <see cref="InstructionExecutor"/><br></br>
/// - decrements <see cref="Cpu.DT"/> and <see cref="Cpu.ST"/> timers<br></br>
/// - updates <see cref="IDisplay"/> if necessary<br></br>
/// - reads pressed key on the <see cref="IKeyboard"/><br></br>
/// </summary>
public class Emulator : IDisposable
{
  private readonly Cpu _cpu;
  private readonly IDisplay _display;
  private readonly IKeyboard _keyboard;
  private readonly InstructionExecutor _instructionExecutor;
  private readonly StringBuilder _stringBuilder = new StringBuilder();

  private bool _isDisposed;

  private readonly CancellationTokenSource _executeCycleCancellationTokenSource;
  private Task _executeCycleTask;

  private const int DefaultDelay = 15;

  /// <summary>
  /// True if application is running.
  /// False if application is paused (see <see cref="PauseApplication"/> and <see cref="IsApplicationPaused"/>), stopped (see <see cref="StopApplication"/>) or it was never started.
  /// </summary>
  public bool IsApplicationRunning { get; private set; }

  /// <summary>
  /// True if application is paused.
  /// </summary>
  public bool IsApplicationPaused { get; private set; }

  /// <summary>
  /// True if application is loaded.
  /// Before running the application, application must be loaded.
  /// </summary>
  public bool IsApplicationLoaded { get; private set; }

  public Emulator(Cpu cpu, IDisplay display, IKeyboard keyboard)
  {
    _cpu = cpu ?? throw new ArgumentNullException(nameof(cpu));
    _display = display ?? throw new ArgumentNullException(nameof(display));
    _keyboard = keyboard ?? throw new ArgumentNullException(nameof(keyboard));

    _instructionExecutor = new InstructionExecutor(cpu, display, keyboard);
    _executeCycleCancellationTokenSource = new CancellationTokenSource();
  }

  /// <summary>
  /// Resets the <see cref="Cpu"/> and clears the <see cref="IDisplay"/> used by the emulator.
  /// </summary>
  public void Reset()
  {
    _cpu.Reset();
    _display.Clear();

    IsApplicationLoaded = false;
    IsApplicationRunning = false;
    IsApplicationPaused = false;
  }

  /// <summary>
  /// Loads application from a byte array into <see cref="Cpu.Memory"/>.
  /// First byte of the application is set at <see cref="Cpu.MemoryAddressOfFirstInstruction"/>.
  /// </summary>
  /// <param name="application">Array of bytes which represents a CHIP-8 application.</param>
  /// <exception cref="ArgumentNullException">If <paramref name="application"/> is null.</exception>
  /// <exception cref="ArgumentException">If <paramref name="application"/> exceeds size of: <see cref="Cpu.MemorySizeInBytes"/> - <see cref="Cpu.MemoryAddressOfFirstInstruction"/>.</exception>
  public void LoadApplication(byte[] application)
  {
    if (application == null)
    {
      throw new ArgumentNullException(nameof(application), "Cannot set Cpu.Memory to null.");
    }

    Reset();

    application.CopyTo(_cpu.Memory, Cpu.MemoryAddressOfFirstInstruction);
    IsApplicationLoaded = true;
  }

  /// <summary>
  /// Runs the application and sets the <see cref="IsApplicationRunning"/> to true.<br></br>
  /// Continues with execution of the application if it is paused.
  /// </summary>
  /// <exception cref="InvalidOperationException">If <see cref="IsApplicationLoaded"/> is false.</exception>
  public void RunContinueApplication()
  {
    if (!IsApplicationLoaded)
    {
      throw new InvalidOperationException("Cannot run application because application is not loaded.");
    }
    if (IsApplicationRunning)
    {
      return;
    }

    if (_executeCycleTask == null)
    {
      _executeCycleTask = Task.Factory.StartNew(
        ExecuteCycle, _executeCycleCancellationTokenSource.Token,
        TaskCreationOptions.LongRunning, TaskScheduler.Current);
    }

    IsApplicationRunning = true;
    IsApplicationPaused = false;
  }

  /// <summary>
  /// Pauses the application if it is running.
  /// </summary>
  /// <exception cref="InvalidOperationException">If <see cref="IsApplicationLoaded"/> is false or application was loaded but never started.</exception>
  public void PauseApplication()
  {
    if (!IsApplicationLoaded)
    {
      throw new InvalidOperationException("Cannot pause application because application is not loaded.");
    }
    if (_executeCycleTask == null)
    {
      throw new InvalidOperationException("Cannot pause application because application was never started.");
    }
    if (IsApplicationRunning)
    {
      IsApplicationRunning = false;
      IsApplicationPaused = true;
    }
  }

  /// <summary>
  /// Stops the application if it is running.
  /// </summary>
  /// <exception cref="InvalidOperationException">If <see cref="IsApplicationLoaded"/> is false or application was loaded but never started.</exception>
  public void StopApplication()
  {
    if (!IsApplicationLoaded)
    {
      throw new InvalidOperationException("Cannot stop application because application is not loaded.");
    }
    if (_executeCycleTask == null && !IsApplicationPaused)
    {
      throw new InvalidOperationException("Cannot stop application because application was never started.");
    }
    if (IsApplicationRunning || IsApplicationPaused)
    {
      IsApplicationRunning = false;
      IsApplicationPaused = false;

      // Wait for the execution thread to stop before modifying CPU state
      _executeCycleTask?.Wait();

      Array temp = Array.CreateInstance(typeof(byte), Cpu.MemorySizeInBytes);
      Array.Copy(_cpu.Memory, temp, Cpu.MemorySizeInBytes);

      _cpu.Reset();
      temp.CopyTo(_cpu.Memory, 0);

      _display.Clear();
    }
  }

  /// <summary>
  /// Returns application as byte array.
  /// Application is <see cref="Cpu.Memory"/> content starting at <see cref="Cpu.MemoryAddressOfFirstInstruction"/>,
  /// and ending at the last non-zero byte.
  /// </summary>
  /// <returns>Application represented as byte array.</returns>
  public byte[] GetApplication()
  {
    var indexOfLastNonZeroByte = Cpu.MemoryAddressOfFirstInstruction;
    for (ushort i = (ushort)(Cpu.MemoryAddressOfLastInstruction + 1); i >= Cpu.MemoryAddressOfFirstInstruction; i--)
    {
      if (_cpu.Memory[i] != 0)
      {
        indexOfLastNonZeroByte = i;
        break;
      }
    }

    Array result = Array.CreateInstance(typeof(byte), indexOfLastNonZeroByte - Cpu.MemoryAddressOfFirstInstruction + 1);
    Array.Copy(_cpu.Memory, Cpu.MemoryAddressOfFirstInstruction, result, 0, result.Length);
    return (byte[])result;
  }

  /// <summary>
  /// Executes single CPU cycle (single instruction).
  /// Method can be called only if application is loaded and not running (either paused or not started).
  /// </summary>
  /// <exception cref="InvalidOperationException">If application is not loaded or is running.</exception>
  public void ExecuteSingleCycle()
  {
    if (!IsApplicationLoaded)
    {
      throw new InvalidOperationException("Cannot execute single cycle because application is not loaded.");
    }
    if (IsApplicationRunning)
    {
      throw new InvalidOperationException("Cannot execute single cycle because application is running.");
    }
    _instructionExecutor.ExecuteSingleInstruction();
    Thread.Sleep(DefaultDelay);
    IsApplicationPaused = true;
  }

  /// <summary>
  /// Returns a string containing value of all the <see cref="Cpu"/> registers.
  /// </summary>
  /// <returns>String containing value of all the <see cref="Cpu"/> registers.</returns>
  public string GetValueOfCpuRegisters()
  {
    _stringBuilder.Clear();

    // Hexadecimal value is padded with 5 characters on the right
    _stringBuilder.Append($"PC = {$"0x{_cpu.PC:X}",-5} ({_cpu.PC}){Environment.NewLine}");
    _stringBuilder.Append($"I  = {$"0x{_cpu.I:X}",-5} ({_cpu.I}){Environment.NewLine}");
    _stringBuilder.Append($"DT = {$"0x{_cpu.DT:X}",-5} ({_cpu.DT}){Environment.NewLine}");
    _stringBuilder.Append($"ST = {$"0x{_cpu.ST:X}",-5} ({_cpu.ST}){Environment.NewLine}");

    for (int i = 0; i <= 15; i++)
    {
      _stringBuilder.Append($"V{i:X} = {$"0x{_cpu.V[i]:X}",-5} ({_cpu.V[i]}){Environment.NewLine}");
    }

    return _stringBuilder.ToString();
  }

  /// <inheritdoc cref="IDisposable.Dispose"/>
  public void Dispose()
  {
    if (_isDisposed)
    {
      return;
    }

    IsApplicationRunning = false;
    IsApplicationPaused = false;

    _executeCycleCancellationTokenSource.Cancel();
    if (_executeCycleTask != null)
    {
      _executeCycleTask.Wait();
    }

    _executeCycleCancellationTokenSource.Dispose();
    _executeCycleTask?.Dispose();

    _isDisposed = true;
  }

  private void ExecuteCycle()
  {
    while (!_executeCycleCancellationTokenSource.IsCancellationRequested)
    {
      while (IsApplicationRunning)
      {
        // Execute 10 instructions per cycle
        for (int i = 0; i < 10; i++)
        {
          if (IsApplicationRunning)
          {
            _instructionExecutor.ExecuteSingleInstruction();
          }
        }
        if (IsApplicationRunning)
        {
          Thread.Sleep(DefaultDelay);
        }
      }
      return;
    }
  }
}
