using System;

namespace Chip8.Instructions;

/// <summary>
/// Set Vx = Vx - Vy, set VF = NOT borrow.
/// </summary>
/// <remarks>
/// If Vx >= Vy, then VF is set to 1, otherwise 0.<br></br>
/// Then Vy is subtracted from Vx, and the results stored in Vx.<br></br>
/// NOTE: Subtraction is byte-wise subtraction with wrap-around (e.g., 0 - 1 = 255).
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
    if (Decoded.x == 0xF && !cpu.AllowQuirks)
    {
      throw new InvalidOperationException("Cannot use VF as Vx register of SUB operation. VF is already storing the !borrow flag so it cannot store the result also.");
    }

    // Read as ints to avoid byte arithmetic surprises
    int vx = cpu.V[Decoded.x];
    int vy = cpu.V[Decoded.y];

    // Store result modulo 256 (byte cast wraps)
    cpu.V[Decoded.x] = (byte)(vx - vy);

    // VF = 1 when no borrow (Vx >= Vy), otherwise 0
    cpu.V[0xF] = (byte)(vx >= vy ? 1 : 0);
  }
}
