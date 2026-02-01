namespace Chip8.Instructions;

/// <summary>
/// Set Vx = kk.
/// </summary>
/// <remarks>
/// The interpreter puts the value kk into register Vx.
/// </remarks>
public class Instruction_6xkk : CpuInstruction
{
  public Instruction_6xkk(DecodedInstruction decodedInstruction) : base(decodedInstruction)
  {
    Description = $"Set V{Decoded.x:X} = 0x{Decoded.kk:X}.";
    Mnemonic = $"LD V{Decoded.x:X}, 0x{Decoded.kk:X}";
  }

  /// <inheritdoc/>
  public override void Execute(Cpu cpu, IDisplay display, IKeyboard keyboard)
  {
    cpu.V[Decoded.x] = Decoded.kk;
  }
}
