using System;

namespace Chip8.Instructions;

/// <summary>
/// Display n-byte sprite starting at memory location I at (Vx, Vy), set VF = collision.
/// </summary>
/// <remarks>
/// The interpreter reads n bytes from memory, starting at the address stored in I.
/// These bytes are then displayed as sprites on screen at coordinates (Vx, Vy).
/// Sprites are XORed onto the existing screen. If this causes any pixels to be erased,
/// VF is set to 1, otherwise it is set to 0.
/// If the sprite is positioned so part of it is outside the coordinates of the display,
/// it wraps around to the opposite side of the screen.
/// </remarks>
public class Instruction_Dxyn : CpuInstruction
{
  public Instruction_Dxyn(DecodedInstruction decodedInstruction) : base(decodedInstruction)
  {
    Description = $"Draw at (V{Decoded.x:X}, V{Decoded.y:X}) {Decoded.n}-byte sprite from I. VF = collision.";
    Mnemonic = $"DRW V{Decoded.x:X}, V{Decoded.y:X}, 0x{Decoded.n:X}";
  }

  /// <inheritdoc/>
  public override void Execute(Cpu cpu, IDisplay display, IKeyboard keyboard)
  {
    if (Decoded.n == 0)
    {
      return;
    }

    var spriteData = new byte[Decoded.n];
    Array.Copy(cpu.Memory, cpu.I, spriteData, 0, Decoded.n);

    var xStart = (byte)(cpu.V[Decoded.x] % 64);
    var yStart = (byte)(cpu.V[Decoded.y] % 32);

    cpu.V[0xF] = 0;

    for (byte i = 0; i < spriteData.Length; i++)
    {
      var y = (byte)((yStart + i) % 32);

      var sprite = spriteData[i];

      SetPixel(display, cpu, xStart, 0, y, sprite, 0x80);
      SetPixel(display, cpu, xStart, 1, y, sprite, 0x40);
      SetPixel(display, cpu, xStart, 2, y, sprite, 0x20);
      SetPixel(display, cpu, xStart, 3, y, sprite, 0x10);

      SetPixel(display, cpu, xStart, 4, y, sprite, 0x08);
      SetPixel(display, cpu, xStart, 5, y, sprite, 0x04);
      SetPixel(display, cpu, xStart, 6, y, sprite, 0x02);
      SetPixel(display, cpu, xStart, 7, y, sprite, 0x01);
    }
  }

  private void SetPixel(IDisplay display, Cpu cpu, byte xStart, byte xOffset, byte y, byte sprite, byte mask)
  {
    var x = (byte)((xStart + xOffset) % 64);
    var isPixelSetOnScreen = display.GetPixel(x, y);
    var isPixelSetInSprite = ((sprite & mask) > 0);

    // XOR operation: sprite XOR screen
    if (isPixelSetInSprite)
    {
      if (isPixelSetOnScreen)
      {
        // 1 XOR 1 = 0: erase pixel (collision)
        display.ClearPixel(x, y);
        cpu.V[0xF] = 1;
      }
      else
      {
        // 1 XOR 0 = 1: set pixel
        display.SetPixel(x, y);
      }
    }
    // else: sprite bit is 0, so XOR doesn't change screen (0 XOR 0 = 0, 0 XOR 1 = 1)
  }
}
