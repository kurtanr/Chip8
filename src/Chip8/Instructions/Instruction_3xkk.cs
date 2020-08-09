namespace Chip8.Instructions
{
  /// <summary>
  /// Skip next instruction if Vx = kk.
  /// </summary>
  /// <remarks>
  /// The interpreter compares register Vx to kk, and if they are equal, increments the program counter by 2.
  /// </remarks>
  public class Instruction_3xkk : CpuInstruction
  {
    private readonly string _description, _mnemonic;

    public Instruction_3xkk(DecodedInstruction decodedInstruction) : base(decodedInstruction)
    {
      _description = $"Skip next instruction if V{Decoded.x:X} = 0x{Decoded.kk:X}.";
      _mnemonic = $"SE V{Decoded.x:X}, 0x{Decoded.kk:X}";
    }

    /// <inheritdoc/>
    public override string Description => _description;

    /// <inheritdoc/>
    public override string Mnemonic => _mnemonic;

    /// <inheritdoc/>
    public override void Execute(Cpu cpu, IDisplay display)
    {
      if(cpu.V[Decoded.x] == Decoded.kk)
      {
        cpu.PC += 2;
      }
    }
  }
}
