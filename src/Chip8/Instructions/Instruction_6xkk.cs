namespace Chip8.Instructions
{
  /// <summary>
  /// Set Vx = kk.
  /// </summary>
  /// <remarks>
  /// The interpreter puts the value kk into register Vx.
  /// </remarks>
  public class Instruction_6xkk : CpuInstruction
  {
    private readonly string _description, _mnemonic;

    public Instruction_6xkk(DecodedInstruction decodedInstruction) : base(decodedInstruction)
    {
      _description = $"Set V{Decoded.x:X} = 0x{Decoded.kk:X}.";
      _mnemonic = $"LD V{Decoded.x:X}, 0x{Decoded.kk:X}";
    }

    /// <inheritdoc/>
    public override string Description => _description;

    /// <inheritdoc/>
    public override string Mnemonic => _mnemonic;

    /// <inheritdoc/>
    public override void Execute(Cpu cpu, IDisplay display)
    {
      cpu.V[Decoded.x] = Decoded.kk;
    }
  }
}
