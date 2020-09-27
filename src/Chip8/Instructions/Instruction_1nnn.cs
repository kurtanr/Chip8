namespace Chip8.Instructions
{
  /// <summary>
  /// Jump to location nnn.
  /// </summary>
  /// <remarks>
  /// The interpreter sets the program counter to nnn.
  /// </remarks>
  public class Instruction_1nnn : CpuInstruction
  {
    public Instruction_1nnn(DecodedInstruction decodedInstruction) : base(decodedInstruction)
    {
      Description = $"Jump to location 0x{Decoded.nnn:X}.";
      Mnemonic = $"JP 0x{Decoded.nnn:X}";
    }

    /// <inheritdoc/>
    public override void Execute(Cpu cpu, IDisplay display, IKeyboard keyboard)
    {
      cpu.PC = Decoded.nnn;
    }
  }
}
