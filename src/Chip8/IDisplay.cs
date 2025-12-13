namespace Chip8;

/// <summary>
/// Abstraction of Chip-8 screen.
/// Screen is a 64x32-pixel monochrome display with the origin (0,0) in the top left corner.
/// </summary>
public interface IDisplay
{
  /// <summary>
  /// Clears the entire screen.
  /// </summary>
  void Clear();

  /// <summary>
  /// Clears the pixel at coordinates (<paramref name="x"/>, <paramref name="y"/>).
  /// </summary>
  /// <param name="x">Pixel column.</param>
  /// <param name="y">Pixel row.</param>
  void ClearPixel(byte x, byte y);

  /// <summary>
  /// Gets the pixel at coordinates (<paramref name="x"/>, <paramref name="y"/>).
  /// </summary>
  /// <param name="x">Pixel column.</param>
  /// <param name="y">Pixel row.</param>
  /// <returns>True if pixel is set, false otherwise.</returns>
  bool GetPixel(byte x, byte y);

  /// <summary>
  /// Sets the pixel at coordinates (<paramref name="x"/>, <paramref name="y"/>).
  /// </summary>
  /// <param name="x">Pixel column.</param>
  /// <param name="y">Pixel row.</param>
  void SetPixel(byte x, byte y);
}
