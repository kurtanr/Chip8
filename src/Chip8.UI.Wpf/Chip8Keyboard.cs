using ICSharpCode.AvalonEdit.Editing;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Chip8.UI.Wpf;

/// <summary>
/// CHIP-8     Physical position (QWERTY layout)
/// 1 2 3 C    Top row:    1 2 3 4
/// 4 5 6 D    Home row:   Q W E R
/// 7 8 9 E    Home row:   A S D F
/// A 0 B F    Bottom row: Z X C V
/// </summary>
internal class Chip8Keyboard : IKeyboard
{
  private readonly bool[] _keyState = new bool[16];

  // Used by WaitForKeyPressAndRelease
  private bool _waitingForRelease = false;
  private byte? _pressedKey = null;

  private static readonly Dictionary<int, byte> KeyMapping = new()
  {
    { 0x31, 0x1 }, // '1' key
    { 0x32, 0x2 },
    { 0x33, 0x3 },
    { 0x34, 0xC },

    { 0x51, 0x4 }, // Q position (physical)
    { 0x57, 0x5 },
    { 0x45, 0x6 },
    { 0x52, 0xD },

    { 0x41, 0x7 }, // A position
    { 0x53, 0x8 },
    { 0x44, 0x9 },
    { 0x46, 0xE },

    { 0x59, 0xA }, // Z position (on QWERTY), Y on QWERTZ
    { 0x58, 0x0 },
    { 0x43, 0xB },
    { 0x56, 0xF }
  };

  public void OnKeyDown(KeyEventArgs e)
  {
    if (e.OriginalSource is TextArea)
    {
      return;
    }

    if (!KeyMapping.TryGetValue(KeyInterop.VirtualKeyFromKey(e.Key), out byte key))
    {
      return;
    }

    _keyState[key] = true;
    e.Handled = true;
  }

  public void OnKeyUp(KeyEventArgs e)
  {
    if (e.OriginalSource is TextArea)
    {
      return;
    }

    if (!KeyMapping.TryGetValue(KeyInterop.VirtualKeyFromKey(e.Key), out byte key))
    {
      return;
    }

    _keyState[key] = false;
    e.Handled = true;
  }

  public bool IsKeyDown(byte key)
  {
    return _keyState[key];
  }

  public byte? WaitForKeyPressAndRelease()
  {
    if (!_waitingForRelease)
    {
      // Get first pressed key
      var pressedKeyIndex = _keyState.ToList().FindIndex(k => k);
      if (pressedKeyIndex >= 0)
      {
        _waitingForRelease = true;
        _pressedKey = (byte)pressedKeyIndex;
        return null;
      }
      return null;
    }
    else
    {
      // Check if the pressed key has been released
      if (_pressedKey.HasValue && !_keyState[_pressedKey.Value])
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