namespace Chip8.Instructions;

/// <summary>
/// Skip next instruction if Vx != Vy.
/// </summary>
/// <remarks>
/// The values of Vx and Vy are compared, and if they are not equal, the program counter is increased by 2.
/// </remarks>
public class Instruction_9xy0 : CpuInstruction
{
  public Instruction_9xy0(DecodedInstruction decodedInstruction) : base(decodedInstruction)
  {
    Description = $"Skip next instruction if V{Decoded.x:X} != V{Decoded.y:X}.";
    Mnemonic = $"SNE V{Decoded.x:X}, V{Decoded.y:X}";
  }

  /// <inheritdoc/>
  public override void Execute(Cpu cpu, IDisplay display, IKeyboard keyboard)
  {
    if (cpu.V[Decoded.x] != cpu.V[Decoded.y])
    {
      cpu.PC += 2;
    }
  }
}
