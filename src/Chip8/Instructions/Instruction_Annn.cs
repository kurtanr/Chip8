namespace Chip8.Instructions;

/// <summary>
/// Set I = nnn.
/// </summary>
/// <remarks>
/// The value of register I is set to nnn.
/// </remarks>
public class Instruction_Annn : CpuInstruction
{
  public Instruction_Annn(DecodedInstruction decodedInstruction) : base(decodedInstruction)
  {
    Description = $"Set I = 0x{Decoded.nnn:X}.";
    Mnemonic = $"LD I, 0x{Decoded.nnn:X}";
  }

  /// <inheritdoc/>
  public override void Execute(Cpu cpu, IDisplay display, IKeyboard keyboard)
  {
    cpu.I = Decoded.nnn;
  }
}
