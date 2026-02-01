namespace Chip8.Instructions;

/// <summary>
/// Skip next instruction if key with the value of Vx is pressed.
/// </summary>
/// <remarks>
/// Checks the keyboard, and if the key corresponding to the value of Vx is currently in the down position,
/// PC is increased by 2.
/// </remarks>
public class Instruction_Ex9E : CpuInstruction
{
  public Instruction_Ex9E(DecodedInstruction decodedInstruction) : base(decodedInstruction)
  {
    Description = $"Skip next instruction if key with the value of V{Decoded.x:X} is pressed.";
    Mnemonic = $"SKP V{Decoded.x:X}";
  }

  /// <inheritdoc/>
  public override void Execute(Cpu cpu, IDisplay display, IKeyboard keyboard)
  {
    if (keyboard.IsKeyDown(cpu.V[Decoded.x]))
    {
      cpu.PC += 2;
    }
  }
}
