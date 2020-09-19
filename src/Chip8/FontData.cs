using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Chip8
{
  internal class FontData
  {
    public ReadOnlyCollection<byte> Data { get; private set; }

    public FontData()
    {
      var data = new List<byte>
      {
        // 0
        0b0110,
        0b1001,
        0b1001,
        0b1001,
        0b0110,
        // 1
        0b0110,
        0b0010,
        0b0010,
        0b0010,
        0b0111,
        // 2
        0b1110,
        0b0001,
        0b0011,
        0b0110,
        0b1111,
        // 3
        0b1110,
        0b0001,
        0b0110,
        0b0001,
        0b1110,
        // 4
        0b1010,
        0b1010,
        0b1110,
        0b0010,
        0b0010,
        // 5
        0b1111,
        0b1000,
        0b1111,
        0b0001,
        0b1111,
        // 6
        0b1000,
        0b1000,
        0b1111,
        0b1001,
        0b1111,
        // 7
        0b1111,
        0b0001,
        0b0010,
        0b0010,
        0b0010,
        // 8
        0b1111,
        0b1001,
        0b1111,
        0b1001,
        0b1111,
        // 9
        0b1111,
        0b1001,
        0b1111,
        0b0001,
        0b0001,
        // A
        0b0110,
        0b1001,
        0b1111,
        0b1001,
        0b1001,
        // B
        0b1000,
        0b1000,
        0b1111,
        0b1001,
        0b1111,
        // C
        0b1111,
        0b1000,
        0b1000,
        0b1000,
        0b1111,
        // D
        0b1110,
        0b1001,
        0b1001,
        0b1001,
        0b1110,
        // E
        0b1111,
        0b1000,
        0b1110,
        0b1000,
        0b1111,
        // F
        0b1111,
        0b1000,
        0b1110,
        0b1000,
        0b1000
      };

      for(int i = 0; i < data.Count; i++)
      {
        data[i] <<= 4;
      }

      Data = new ReadOnlyCollection<byte>(data);
    }
  }
}
