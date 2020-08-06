namespace Chip8
{
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
    /// Clears the pixel at row <paramref name="x"/>, column <paramref name="y"/>.
    /// </summary>
    /// <param name="x">Pixel row.</param>
    /// <param name="y">Pixel column.</param>
    void ClearPixel(byte x, byte y);

    /// <summary>
    /// Sets the pixel at row <paramref name="x"/>, column <paramref name="y"/>.
    /// </summary>
    /// <param name="x">Pixel row.</param>
    /// <param name="y">Pixel column.</param>
    void SetPixel(byte x, byte y);
  }
}
