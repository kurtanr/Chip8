using System;

namespace Chip8.Instructions
{
  /// <summary>
  /// Set Vx = Vy - Vx, set VF = NOT borrow.
  /// </summary>
  /// <remarks>
  /// If Vy >= Vx, then VF is set to 1, otherwise 0. Then Vx is subtracted from Vy, and the results stored in Vx.
  /// </remarks>
  public class Instruction_8xy7 : CpuInstruction
  {
    public Instruction_8xy7(DecodedInstruction decodedInstruction) : base(decodedInstruction)
    {
      Description = $"Set V{Decoded.x:X} = V{Decoded.y:X} - V{Decoded.x:X}, VF = NOT borrow.";
      Mnemonic = $"SUBN V{Decoded.x:X}, V{Decoded.y:X}";
    }

    /// <inheritdoc/>
    public override void Execute(Cpu cpu, IDisplay display)
    {
      if(Decoded.x == 0xF)
      {
        throw new InvalidOperationException("Cannot use VF as Vx register of SUB operation. VF is already storing the !borrow flag so it cannot store the result also.");
      }

      var sum = Math.Abs(cpu.V[Decoded.y] - cpu.V[Decoded.x]);
      cpu.V[0xF] = (cpu.V[Decoded.y] >= cpu.V[Decoded.x]) ? (byte)1 : (byte)0;
      cpu.V[Decoded.x] = (byte)sum;
    }
  }
}
