namespace Chip8.Instructions;

/// <summary>
/// Clear the display.
/// </summary>
public class Instruction_00E0 : CpuInstruction
{
  public Instruction_00E0(DecodedInstruction decodedInstruction) : base(decodedInstruction)
  {
    Description = "Clear the display.";
    Mnemonic = "CLS";
  }

  /// <inheritdoc/>
  public override void Execute(Cpu cpu, IDisplay display, IKeyboard keyboard)
  {
    display.Clear();
  }
}
