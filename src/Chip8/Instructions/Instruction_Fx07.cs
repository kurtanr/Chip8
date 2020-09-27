namespace Chip8.Instructions
{
  /// <summary>
  /// Set Vx = delay timer value.
  /// </summary>
  /// <remarks>
  /// The value of DT is placed into Vx.
  /// </remarks>
  public class Instruction_Fx07 : CpuInstruction
  {
    public Instruction_Fx07(DecodedInstruction decodedInstruction) : base(decodedInstruction)
    {
      Description = $"Set V{Decoded.x:X} = DT.";
      Mnemonic = $"LD V{Decoded.x:X}, DT";
    }

    /// <inheritdoc/>
    public override void Execute(Cpu cpu, IDisplay display, IKeyboard keyboard)
    {
      cpu.V[Decoded.x] = cpu.DT;
    }
  }
}
