using System;

namespace Chip8.Instructions;

/// <summary>
/// Set Vx = Vy, then set Vx = Vx SHL 1. VF = MSB.
/// </summary>
/// <remarks>
/// If the most-significant bit of Vx is 1, then VF is set to 1, otherwise to 0. Then Vx is multiplied by 2 (1 bit shift to left).
/// </remarks>
public class Instruction_8xyE : CpuInstruction
{
  public Instruction_8xyE(DecodedInstruction decodedInstruction) : base(decodedInstruction)
  {
    Description = $"Set V{Decoded.x:X} = V{Decoded.y:X}, V{Decoded.x:X} = V{Decoded.x:X} SHL 1. VF = MSB.";
    Mnemonic = $"SHL V{Decoded.x:X}, V{Decoded.y:X}";
  }

  /// <inheritdoc/>
  public override void Execute(Cpu cpu, IDisplay display, IKeyboard keyboard)
  {
    if (Decoded.x == 0xF)
    {
      throw new InvalidOperationException("Cannot use VF as Vx register of SHL operation. VF is already storing the most-significant bit of Vx so it cannot store the result also.");
    }

    cpu.V[Decoded.x] = cpu.V[Decoded.y];
    cpu.V[0xF] = ((cpu.V[Decoded.x] & 0x8) > 0) ? (byte)1 : (byte)0;
    cpu.V[Decoded.x] &= 0x7F; // set MSB to 0
    cpu.V[Decoded.x] <<= 1;
  }
}
