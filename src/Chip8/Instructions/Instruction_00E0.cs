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

    public override string Description => "Clear the display.";

    public override string Mnemonic => "CLS";

    public override void Execute(Cpu cpu, IDisplay display)
    {
      display.Clear();
    }
  }
}
