namespace Chip8.Instructions;

/// <summary>
/// Set Vx = Vx AND Vy.
/// </summary>
/// <remarks>
/// Performs a bitwise AND on the values of Vx and Vy, then stores the result in Vx.
/// A bitwise AND compares the corresponding bits from two values, and if both bits are 1,
/// then the same bit in the result is also 1. Otherwise, it is 0.
/// </remarks>
public class Instruction_8xy2 : CpuInstruction
{
  public Instruction_8xy2(DecodedInstruction decodedInstruction) : base(decodedInstruction)
  {
    Description = $"Set V{Decoded.x:X} = V{Decoded.x:X} AND V{Decoded.y:X}.";
    Mnemonic = $"AND V{Decoded.x:X}, V{Decoded.y:X}";
  }

  /// <inheritdoc/>
  public override void Execute(Cpu cpu, IDisplay display, IKeyboard keyboard)
  {
    cpu.V[Decoded.x] &= cpu.V[Decoded.y];

    if (cpu.AllowQuirks)
    {
      cpu.V[0xF] = 0;
    }
  }
}
