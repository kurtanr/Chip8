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
    private readonly byte _x, _kk;

    public Instruction_3xkk(ushort instructionCode, byte x, byte kk) : base(instructionCode)
    {
      _description = $"Skip next instruction if V{x:X} = 0x{kk:X}.";
      _mnemonic = $"SE V{x:X}, 0x{kk:X}";
      _x = x;
      _kk = kk;
    }

    /// <inheritdoc/>
    public override string Description => _description;

    /// <inheritdoc/>
    public override string Mnemonic => _mnemonic;

    /// <inheritdoc/>
    public override void Execute(Cpu cpu, IDisplay display)
    {
      if(cpu.V[_x] == _kk)
      {
        cpu.PC += 2;
      }
    }
  }
}
