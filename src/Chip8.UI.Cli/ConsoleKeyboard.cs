namespace Chip8.UI.Cli;

/// <summary>
/// Console implementation of <see cref="IKeyboard"/>.<br/>
/// Uses a background thread to read console key events.<br/>
/// A console has no key-up events - we only know when a key is pressed, never when it's released.<br/>
/// So we simulate "held down" with a timeout-based approach.<br/>
/// <br/>
/// <para>
/// CHIP-8     Physical position (QWERTY layout)<br/>
/// 1 2 3 C    Top row:    1 2 3 4<br/>
/// 4 5 6 D    Home row:   Q W E R<br/>
/// 7 8 9 E    Home row:   A S D F<br/>
/// A 0 B F    Bottom row: Z X C V
/// </para>
/// </summary>
internal sealed class ConsoleKeyboard : IKeyboard, IDisposable
{
  // Stores the last press time for each key to simulate "held down" behavior.
  private readonly long[] _keyLastPressedTicks = new long[16];
  private const long KeyHoldDurationTicks = TimeSpan.TicksPerMillisecond * 150;

  private readonly Thread _inputThread;
  private volatile bool _running = true;

  private bool _waitingForRelease;
  private byte? _pressedKey;

  /// <summary>
  /// Set to true when the Escape key is pressed, signaling the application to exit.
  /// </summary>
  public bool IsEscapePressed { get; private set; }

  private static readonly Dictionary<ConsoleKey, byte> KeyMapping = new()
  {
    { ConsoleKey.D1, 0x1 },
    { ConsoleKey.D2, 0x2 },
    { ConsoleKey.D3, 0x3 },
    { ConsoleKey.D4, 0xC },

    { ConsoleKey.Q, 0x4 },
    { ConsoleKey.W, 0x5 },
    { ConsoleKey.E, 0x6 },
    { ConsoleKey.R, 0xD },

    { ConsoleKey.A, 0x7 },
    { ConsoleKey.S, 0x8 },
    { ConsoleKey.D, 0x9 },
    { ConsoleKey.F, 0xE },

    { ConsoleKey.Z, 0xA },
    { ConsoleKey.Y, 0xA }, // QWERTZ keyboard support
    { ConsoleKey.X, 0x0 },
    { ConsoleKey.C, 0xB },
    { ConsoleKey.V, 0xF }
  };

  public ConsoleKeyboard()
  {
    _inputThread = new Thread(ReadInput) { IsBackground = true, Name = "Chip8KeyboardInput" };
    _inputThread.Start();
  }

  private void ReadInput()
  {
    while (_running)
    {
      try
      {
        if (Console.KeyAvailable)
        {
          var keyInfo = Console.ReadKey(true);

          if (keyInfo.Key == ConsoleKey.Escape)
          {
            IsEscapePressed = true;
            return;
          }

          if (KeyMapping.TryGetValue(keyInfo.Key, out byte chip8Key))
          {
            // On 32-bit runtimes, a long (64-bit) read/write is not atomic.
            // For details see: https://algomaster.io/learn/concurrency-interview/csharp-memory-model
            Interlocked.Exchange(ref _keyLastPressedTicks[chip8Key], DateTime.UtcNow.Ticks);
          }
        }
        else
        {
          Thread.Sleep(1);
        }
      }
      catch (InvalidOperationException)
      {
        return;
      }
    }
  }

  public bool IsKeyDown(byte key)
  {
    // A key is considered "down" for a short duration after the last press to simulate key-hold behavior.
    long lastPressed = Interlocked.Read(ref _keyLastPressedTicks[key]);
    return (DateTime.UtcNow.Ticks - lastPressed) < KeyHoldDurationTicks;
  }

  public byte? WaitForKeyPressAndRelease()
  {
    if (!_waitingForRelease)
    {
      for (byte i = 0; i < 16; i++)
      {
        if (IsKeyDown(i))
        {
          _waitingForRelease = true;
          _pressedKey = i;
          return null;
        }
      }
      return null;
    }
    else
    {
      if (_pressedKey.HasValue && !IsKeyDown(_pressedKey.Value))
      {
        _waitingForRelease = false;
        var key = _pressedKey;
        _pressedKey = null;
        return key;
      }
      return null;
    }
  }

  public void Dispose()
  {
    _running = false;
  }
}
