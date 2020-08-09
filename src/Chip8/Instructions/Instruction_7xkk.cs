namespace Chip8.Instructions
{
  /// <summary>
  /// Set Vx = Vx + kk.
  /// </summary>
  /// <remarks>
  /// Adds the value kk to the value of register Vx, then stores the result in Vx.
  /// </remarks>
  public class Instruction_7xkk : CpuInstruction
  {
    private readonly string _description, _mnemonic;

    public Instruction_7xkk(DecodedInstruction decodedInstruction) : base(decodedInstruction)
    {
      _description = $"Set V{Decoded.x:X} = V{Decoded.x:X} + 0x{Decoded.kk:X}.";
      _mnemonic = $"ADD V{Decoded.x:X}, 0x{Decoded.kk:X}";
    }

    /// <inheritdoc/>
    public override string Description => _description;

    /// <inheritdoc/>
    public override string Mnemonic => _mnemonic;

    /// <inheritdoc/>
    public override void Execute(Cpu cpu, IDisplay display)
    {
      // TODO: what about overflow?
      cpu.V[Decoded.x] += Decoded.kk;
    }
  }
}
