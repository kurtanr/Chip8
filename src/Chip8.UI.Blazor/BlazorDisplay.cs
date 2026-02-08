namespace Chip8.UI.Blazor;

/// <summary>
/// Blazor WebAssembly implementation of <see cref="IDisplay"/>.
/// Maintains a pixel buffer; actual canvas rendering is driven externally via <see cref="GetPixelData"/>.
/// </summary>
internal sealed class BlazorDisplay : IDisplay
{
  private const int ScreenWidth = 64;
  private const int ScreenHeight = 32;

  private readonly bool[,] _pixelBuffer = new bool[ScreenWidth, ScreenHeight];
  private readonly byte[] _pixelData = new byte[ScreenWidth * ScreenHeight];
  private bool _dirty;

  public void Clear()
  {
    Array.Clear(_pixelBuffer);
    _dirty = true;
  }

  public void ClearPixel(byte x, byte y)
  {
    _pixelBuffer[x, y] = false;
    _dirty = true;
  }

  public bool GetPixel(byte x, byte y) => _pixelBuffer[x, y];

  public void SetPixel(byte x, byte y)
  {
    _pixelBuffer[x, y] = true;
    _dirty = true;
  }

  public void RenderIfDirty()
  {
    // No - op in Blazor.Rendering is handled by the game loop via GetPixelData.
  }

  /// <summary>
  /// Returns a flat byte array (64×32, row-major, 1=on 0=off) if the display changed since the last call.
  /// Returns null if nothing changed.
  /// </summary>
  public byte[]? GetPixelData()
  {
    if (!_dirty)
    {
      return null;
    }

    _dirty = false;

    for (int y = 0; y < ScreenHeight; y++)
    {
      for (int x = 0; x < ScreenWidth; x++)
      {
        _pixelData[y * ScreenWidth + x] = _pixelBuffer[x, y] ? (byte)1 : (byte)0;
      }
    }

    return _pixelData;
  }
}
