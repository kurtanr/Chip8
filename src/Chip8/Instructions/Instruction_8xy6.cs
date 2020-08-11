using System;

namespace Chip8.Instructions
{
  /// <summary>
  /// Set Vx = Vy, then set Vx = Vx SHR 1. VF = LSB.
  /// </summary>
  /// <remarks>
  /// If the least-significant bit of Vx is 1, then VF is set to 1, otherwise to 0. Then Vx is divided by 2 (1 bit shift to right).
  /// </remarks>
  public class Instruction_8xy6 : CpuInstruction
  {
    public Instruction_8xy6(DecodedInstruction decodedInstruction) : base(decodedInstruction)
    {
      Description = $"Set V{Decoded.x:X} = V{Decoded.y:X}, V{Decoded.x:X} = V{Decoded.x:X} SHR 1. VF = LSB.";
      Mnemonic = $"SHR V{Decoded.x:X}, V{Decoded.y:X}";
    }

    /// <inheritdoc/>
    public override void Execute(Cpu cpu, IDisplay display)
    {
      if(Decoded.x == 0xF)
      {
        throw new InvalidOperationException("Cannot use VF as Vx register of SHR operation. VF is already storing the least-significant bit of Vx so it cannot store the result also.");
      }

      cpu.V[Decoded.x] = cpu.V[Decoded.y];
      cpu.V[0xF] = (byte)(cpu.V[Decoded.x] & 0x1);
      cpu.V[Decoded.x] >>= 1;
    }
  }
}
