namespace Chip8.UI.Blazor;

/// <summary>
/// Blazor WebAssembly implementation of <see cref="IKeyboard"/>.
/// Key state is read from JavaScript as a 16-bit bitmask once per frame via <see cref="UpdateKeyState"/>.
/// </summary>
internal sealed class BlazorKeyboard : IKeyboard
{
  private int _keyMask;

  private bool _waitingForRelease;
  private byte? _pressedKey;

  /// <summary>
  /// Updates the keyboard state from a bitmask where bit N = 1 means CHIP-8 key N is pressed.
  /// Called once per frame from the game loop.
  /// </summary>
  public void UpdateKeyState(int keyMask)
  {
    _keyMask = keyMask;
  }

  public bool IsKeyDown(byte key) => (_keyMask & (1 << key)) != 0;

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
}
