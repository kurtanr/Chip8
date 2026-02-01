namespace Chip8.Instructions;

/// <summary>
/// Set Vx = Vy.
/// </summary>
/// <remarks>
/// Stores the value of register Vy in register Vx.
/// </remarks>
public class Instruction_8xy0 : CpuInstruction
{
  public Instruction_8xy0(DecodedInstruction decodedInstruction) : base(decodedInstruction)
  {
    Description = $"Set V{Decoded.x:X} = V{Decoded.y:X}.";
    Mnemonic = $"LD V{Decoded.x:X}, V{Decoded.y:X}";
  }

  /// <inheritdoc/>
  public override void Execute(Cpu cpu, IDisplay display, IKeyboard keyboard)
  {
    cpu.V[Decoded.x] = cpu.V[Decoded.y];
  }
}
