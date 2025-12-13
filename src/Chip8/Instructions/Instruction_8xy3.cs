namespace Chip8.Instructions;

/// <summary>
/// Set Vx = Vx XOR Vy.
/// </summary>
/// <remarks>
/// Performs a bitwise exclusive OR on the values of Vx and Vy, then stores the result in Vx.
/// An exclusive OR compares the corresponding bits from two values,
/// and if the bits are not both the same, then the corresponding bit in the result is set to 1. Otherwise, it is 0.
/// </remarks>
public class Instruction_8xy3 : CpuInstruction
{
  public Instruction_8xy3(DecodedInstruction decodedInstruction) : base(decodedInstruction)
  {
    Description = $"Set V{Decoded.x:X} = V{Decoded.x:X} XOR V{Decoded.y:X}.";
    Mnemonic = $"XOR V{Decoded.x:X}, V{Decoded.y:X}";
  }

  /// <inheritdoc/>
  public override void Execute(Cpu cpu, IDisplay display, IKeyboard keyboard)
  {
    cpu.V[Decoded.x] ^= cpu.V[Decoded.y];
  }
}
