namespace Chip8.Instructions
{
  /// <summary>
  /// Call subroutine at nnn.
  /// </summary>
  /// <remarks>
  /// The interpreter increments the stack pointer, then puts the current PC on the top of the stack. The PC is then set to nnn.
  /// </remarks>
  public class Instruction_2nnn : CpuInstruction
  {
    private readonly string _description, _mnemonic;

    public Instruction_2nnn(DecodedInstruction decodedInstruction) : base(decodedInstruction)
    {
      _description = $"Call subroutine at 0x{Decoded.nnn:X}.";
      _mnemonic = $"CALL 0x{Decoded.nnn:X}";
    }

    /// <inheritdoc/>
    public override string Description => _description;

    /// <inheritdoc/>
    public override string Mnemonic => _mnemonic;

    /// <inheritdoc/>
    public override void Execute(Cpu cpu, IDisplay display)
    {
      cpu.Stack.Push(cpu.PC);
      cpu.PC = Decoded.nnn;
    }
  }
}
