using System;

namespace Chip8.Instructions
{
  /// <summary>
  /// Set I = location of sprite for digit Vx.
  /// </summary>
  /// <remarks>
  /// The value of I is set to the location for the hexadecimal sprite corresponding to the value of Vx.
  /// </remarks>
  /// <exception cref="InvalidOperationException"> if value of Vx is not in range [0..0xF].</exception>
  public class Instruction_Fx29 : CpuInstruction
  {
    public Instruction_Fx29(DecodedInstruction decodedInstruction) : base(decodedInstruction)
    {
      Description = $"Set I = address of sprite for value of V{Decoded.x:X}.";
      Mnemonic = $"LD F, V{Decoded.x:X}";
    }

    /// <inheritdoc/>
    public override void Execute(Cpu cpu, IDisplay display, IKeyboard keyboard)
    {
      var vxValue = cpu.V[Decoded.x];
      if(vxValue > 0xF)
      {
        throw new InvalidOperationException($"Value of V{Decoded.x:X} (0x{vxValue:X}) must be in range [0..0xF]");
      }

      cpu.I = (ushort)(vxValue * 5);
    }
  }
}
