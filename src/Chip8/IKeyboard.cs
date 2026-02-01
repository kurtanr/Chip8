namespace Chip8;

/// <summary>
/// Abstraction of Chip-8 keyboard.
/// </summary>
public interface IKeyboard
{
  /// <summary>
  /// Returns true if the given Chip-8 key (0x0–0xF) is currently held down.
  /// </summary>
  bool IsKeyDown(byte key);

  /// <summary>
  /// Blocks until any key is pressed and released and then returns it.
  /// While blocking, timers should continue functioning.
  /// Used only by <see cref="Instructions.Instruction_Fx0A"/>.
  /// </summary>
  /// <returns>Pressed and released key (0x0–0xF) or null if no key was pressed and released.</returns>
  byte? WaitForKeyPressAndRelease();
}
