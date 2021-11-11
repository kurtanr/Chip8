using System;
using System.Threading;
using System.Threading.Tasks;

namespace Chip8
{
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

    private bool _isDisposed;

    private readonly CancellationTokenSource _executeCycleCancellationTokenSource;
    private Task _executeCycleTask;

    /// <summary>
    /// True if application is running.
    /// False if application is paused (see <see cref="PauseApplication"/>) or it was never started.
    /// </summary>
    public bool IsApplicationRunning { get; private set; }

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

      application.CopyTo(_cpu.Memory, Cpu.MemoryAddressOfFirstInstruction);
      IsApplicationLoaded = true;
    }

    /// <summary>
    /// Runs the application and sets the <see cref="IsApplicationRunning"/> to true.
    /// </summary>
    /// <exception cref="InvalidOperationException">If <see cref="IsApplicationLoaded"/> is false.</exception>
    public void RunApplication()
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
    }

    /// <summary>
    /// Pauses the application if it is running.
    /// Unpauses the application if it is paused.
    /// </summary>
    public void PauseApplication()
    {
      if (IsApplicationRunning)
      {
        IsApplicationRunning = false;
      }
      if(!IsApplicationRunning && IsApplicationLoaded)
      {
        IsApplicationRunning = true;
      }
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
      ExecuteCycleCore();
    }

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
      if (_isDisposed)
      {
        return;
      }

      IsApplicationRunning = false;

      _executeCycleCancellationTokenSource.Cancel();
      if (_executeCycleTask != null)
      {
        try
        {
          _executeCycleTask.Wait();
        }
        catch (AggregateException e) when (e.InnerException is OperationCanceledException)
        {
        };
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
          ExecuteCycleCore();
          Thread.Sleep(16);
        }
      }
    }

    private void ExecuteCycleCore()
    {
      _instructionExecutor.ExecuteSingleInstruction();
    }
  }
}
