using System.Text;

namespace Chip8.UI.Cli;

/// <summary>
/// Console implementation of <see cref="IDisplay"/> using ANSI escape codes and Unicode half-block characters.
/// Each character cell represents two vertical pixels, rendering the 64×32 display as 64×16 characters.
/// </summary>
internal sealed class ConsoleDisplay : IDisplay
{
  private const int ScreenWidth = 64;
  private const int ScreenHeight = 32;

  private readonly bool[,] _pixelBuffer = new bool[ScreenWidth, ScreenHeight];
  private readonly Lock _bufferLock = new();
  private volatile bool _dirty;

  // StringBuilder constructor's capacity parameter pre-allocates the internal buffer.
  // Since RenderIfDirty() is called 60 times per second, pre-sizing avoids repeated internal
  // buffer resizing/reallocation on the first few frames (default capacity is 16 chars).
  // +64 is added to the capacity to account for the:
  // - move cursor to home position (3 chars)
  // - newline characters added at the end of each line (16x2 chars = 32 chars)
  private readonly StringBuilder _frameBuffer = new(ScreenWidth * (ScreenHeight / 2) + 64);

  public void Clear()
  {
    lock (_bufferLock)
    {
      Array.Clear(_pixelBuffer);
    }
    _dirty = true;
  }

  public void ClearPixel(byte x, byte y)
  {
    lock (_bufferLock)
    {
      _pixelBuffer[x, y] = false;
    }
    _dirty = true;
  }

  public bool GetPixel(byte x, byte y)
  {
    lock (_bufferLock)
    {
      return _pixelBuffer[x, y];
    }
  }

  public void SetPixel(byte x, byte y)
  {
    lock (_bufferLock)
    {
      _pixelBuffer[x, y] = true;
    }
    _dirty = true;
  }

  public void RenderIfDirty()
  {
    if (!_dirty)
    {
      return;
    }

    _frameBuffer.Clear();
    _frameBuffer.Append("\x1b[H"); // Move cursor to home position

    lock (_bufferLock)
    {
      _dirty = false;

      for (int y = 0; y < ScreenHeight; y += 2)
      {
        for (int x = 0; x < ScreenWidth; x++)
        {
          bool top = _pixelBuffer[x, y];
          bool bottom = _pixelBuffer[x, y + 1];

          _frameBuffer.Append((top, bottom) switch
          {
            (true, true) => '█',
            (true, false) => '▀',
            (false, true) => '▄',
            _ => ' '
          });
        }
        _frameBuffer.Append("\r\n");
      }
    }

    Console.Out.Write(_frameBuffer);
  }
}
