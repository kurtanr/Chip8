namespace Chip8.Instructions;

/// <summary>
/// Set delay timer = Vx.
/// </summary>
/// <remarks>
/// DT is set equal to the value of Vx.
/// </remarks>
public class Instruction_Fx15 : CpuInstruction
{
  public Instruction_Fx15(DecodedInstruction decodedInstruction) : base(decodedInstruction)
  {
    Description = $"Set DT = V{Decoded.x:X}.";
    Mnemonic = $"LD DT, V{Decoded.x:X}";
  }

  /// <inheritdoc/>
  public override void Execute(Cpu cpu, IDisplay display, IKeyboard keyboard)
  {
    cpu.DT = cpu.V[Decoded.x];
  }
}
