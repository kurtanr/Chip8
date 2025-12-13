namespace Chip8;

/// <summary>
/// Abstraction of Chip-8 keyboard.
/// </summary>
public interface IKeyboard
{
  /// <summary>
  /// Returns last key pressed.
  /// Returns null if no key was pressed since the last check.
  /// </summary>
  /// <returns>Numeric representation of pressed key, or null if no key was pressed.</returns>
  byte? GetPressedKey();
}
