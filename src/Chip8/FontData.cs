using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Chip8;

/// <summary>
/// Represents font data for hexadecimal digits (0x0 - 0xF).<br></br>
/// Used for rendering those characters on a graphical display.
/// </summary>
/// <remarks>
/// Each character is a 4×5 pixel sprite, stored as 5 bytes.<br></br>
/// Only the upper 4 bits of each byte are used to represent the pixels.
/// </remarks>
public class FontData : ReadOnlyCollection<byte>
{
  private FontData(IList<byte> list) : base(list)
  {
  }

  public static FontData Create()
  {
    var data = new List<byte>
    {
      // 0
      0b1111_0000,
      0b1001_0000,
      0b1001_0000,
      0b1001_0000,
      0b1111_0000,
      // 1
      0b0010_0000,
      0b0110_0000,
      0b0010_0000,
      0b0010_0000,
      0b0111_0000,
      // 2
      0b1111_0000,
      0b0001_0000,
      0b1111_0000,
      0b1000_0000,
      0b1111_0000,
      // 3
      0b1111_0000,
      0b0001_0000,
      0b1111_0000,
      0b0001_0000,
      0b1111_0000,
      // 4
      0b1001_0000,
      0b1001_0000,
      0b1111_0000,
      0b0001_0000,
      0b0001_0000,
      // 5
      0b1111_0000,
      0b1000_0000,
      0b1111_0000,
      0b0001_0000,
      0b1111_0000,
      // 6
      0b1111_0000,
      0b1000_0000,
      0b1111_0000,
      0b1001_0000,
      0b1111_0000,
      // 7
      0b1111_0000,
      0b0001_0000,
      0b0010_0000,
      0b0100_0000,
      0b0100_0000,
      // 8
      0b1111_0000,
      0b1001_0000,
      0b1111_0000,
      0b1001_0000,
      0b1111_0000,
      // 9
      0b1111_0000,
      0b1001_0000,
      0b1111_0000,
      0b0001_0000,
      0b1111_0000,
      // A
      0b1111_0000,
      0b1001_0000,
      0b1111_0000,
      0b1001_0000,
      0b1001_0000,
      // B
      0b1110_0000,
      0b1001_0000,
      0b1110_0000,
      0b1001_0000,
      0b1110_0000,
      // C
      0b1111_0000,
      0b1000_0000,
      0b1000_0000,
      0b1000_0000,
      0b1111_0000,
      // D
      0b1110_0000,
      0b1001_0000,
      0b1001_0000,
      0b1001_0000,
      0b1110_0000,
      // E
      0b1111_0000,
      0b1000_0000,
      0b1111_0000,
      0b1000_0000,
      0b1111_0000,
      // F
      0b1111_0000,
      0b1000_0000,
      0b1111_0000,
      0b1000_0000,
      0b1000_0000
    };

    return new FontData(data);
  }
}
