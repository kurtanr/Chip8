namespace Chip8.Instructions
{
  /// <summary>
  /// Clear the display.
  /// </summary>
  public class Instruction_00E0 : CpuInstruction
  {
    public Instruction_00E0(ushort instructionCode) : base(instructionCode)
    {
    }

    /// <inheritdoc/>
    public override string Description => "Clear the display.";

    /// <inheritdoc/>
    public override string Mnemonic => "CLS";

    /// <inheritdoc/>
    public override void Execute(Cpu cpu, IDisplay display)
    {
      display.Clear();
    }
  }
}
