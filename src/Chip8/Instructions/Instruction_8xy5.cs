using System;

namespace Chip8.Instructions;

/// <summary>
/// Set Vx = Vx - Vy, set VF = NOT borrow.
/// </summary>
/// <remarks>
/// If Vx >= Vy, then VF is set to 1, otherwise 0. Then Vy is subtracted from Vx, and the results stored in Vx.
/// </remarks>
public class Instruction_8xy5 : CpuInstruction
{
  public Instruction_8xy5(DecodedInstruction decodedInstruction) : base(decodedInstruction)
  {
    Description = $"Set V{Decoded.x:X} = V{Decoded.x:X} - V{Decoded.y:X}, VF = NOT borrow.";
    Mnemonic = $"SUB V{Decoded.x:X}, V{Decoded.y:X}";
  }

  /// <inheritdoc/>
  public override void Execute(Cpu cpu, IDisplay display, IKeyboard keyboard)
  {
    if (Decoded.x == 0xF)
    {
      throw new InvalidOperationException("Cannot use VF as Vx register of SUB operation. VF is already storing the !borrow flag so it cannot store the result also.");
    }

    var sum = Math.Abs(cpu.V[Decoded.x] - cpu.V[Decoded.y]);
    cpu.V[0xF] = (cpu.V[Decoded.x] >= cpu.V[Decoded.y]) ? (byte)1 : (byte)0;
    cpu.V[Decoded.x] = (byte)sum;
  }
}
