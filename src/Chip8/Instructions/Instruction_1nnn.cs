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
    private readonly ushort _nnn;

    public Instruction_1nnn(ushort instructionCode, ushort nnn) : base(instructionCode)
    {
      _description = $"Jump to location 0x{nnn:X}.";
      _mnemonic = $"JP 0x{nnn:X}";
      _nnn = nnn;
    }

    /// <inheritdoc/>
    public override string Description => _description;

    /// <inheritdoc/>
    public override string Mnemonic => _mnemonic;

    /// <inheritdoc/>
    public override void Execute(Cpu cpu, IDisplay display)
    {
      cpu.PC = _nnn;
    }
  }
}
