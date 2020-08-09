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
    private readonly string _description, _mnemonic;

    public Instruction_1nnn(DecodedInstruction decodedInstruction) : base(decodedInstruction)
    {
      _description = $"Jump to location 0x{Decoded.nnn:X}.";
      _mnemonic = $"JP 0x{Decoded.nnn:X}";
    }

    /// <inheritdoc/>
    public override string Description => _description;

    /// <inheritdoc/>
    public override string Mnemonic => _mnemonic;

    /// <inheritdoc/>
    public override void Execute(Cpu cpu, IDisplay display)
    {
      cpu.PC = Decoded.nnn;
    }
  }
}
