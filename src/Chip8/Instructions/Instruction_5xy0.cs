namespace Chip8.Instructions;

/// <summary>
/// Skip next instruction if Vx = Vy.
/// </summary>
/// <remarks>
/// The interpreter compares register Vx to register Vy, and if they are equal, increments the program counter by 2.
/// </remarks>
public class Instruction_5xy0 : CpuInstruction
{
  public Instruction_5xy0(DecodedInstruction decodedInstruction) : base(decodedInstruction)
  {
    Description = $"Skip next instruction if V{Decoded.x:X} = V{Decoded.y:X}.";
    Mnemonic = $"SE V{Decoded.x:X}, V{Decoded.y:X}";
  }

  /// <inheritdoc/>
  public override void Execute(Cpu cpu, IDisplay display, IKeyboard keyboard)
  {
    if (cpu.V[Decoded.x] == cpu.V[Decoded.y])
    {
      cpu.PC += 2;
    }
  }
}
