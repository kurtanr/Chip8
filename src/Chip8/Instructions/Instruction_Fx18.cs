namespace Chip8.Instructions
{
  /// <summary>
  /// Set sound timer = Vx.
  /// </summary>
  /// <remarks>
  /// ST is set equal to the value of Vx.
  /// </remarks>
  public class Instruction_Fx18 : CpuInstruction
  {
    public Instruction_Fx18(DecodedInstruction decodedInstruction) : base(decodedInstruction)
    {
      Description = $"Set ST = V{Decoded.x:X}.";
      Mnemonic = $"LD ST, V{Decoded.x:X}";
    }

    /// <inheritdoc/>
    public override void Execute(Cpu cpu, IDisplay display)
    {
      cpu.ST = cpu.V[Decoded.x];
    }
  }
}
