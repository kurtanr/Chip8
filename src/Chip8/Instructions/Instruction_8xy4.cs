using System;

namespace Chip8.Instructions;

/// <summary>
/// Set Vx = Vx + Vy, set VF = carry.
/// </summary>
/// <remarks>
/// The values of Vx and Vy are added together. If the result is greater than 8 bits (i.e., > 255,) VF is set to 1, 
/// otherwise 0. Only the lowest 8 bits of the result are kept, and stored in Vx.
/// </remarks>
public class Instruction_8xy4 : CpuInstruction
{
  public Instruction_8xy4(DecodedInstruction decodedInstruction) : base(decodedInstruction)
  {
    Description = $"Set V{Decoded.x:X} = V{Decoded.x:X} + V{Decoded.y:X}, VF = carry.";
    Mnemonic = $"ADD V{Decoded.x:X}, V{Decoded.y:X}";
  }

  /// <inheritdoc/>
  public override void Execute(Cpu cpu, IDisplay display, IKeyboard keyboard)
  {
    if (Decoded.x == 0xF && !cpu.AllowQuirks)
    {
      throw new InvalidOperationException("Cannot use VF as Vx register of ADD operation. VF is already storing the carry flag so it cannot store the result also.");
    }

    var sum = cpu.V[Decoded.x] + cpu.V[Decoded.y];
    cpu.V[Decoded.x] = (byte)sum;
    cpu.V[0xF] = (sum > 0xFF) ? (byte)1 : (byte)0;
  }
}
