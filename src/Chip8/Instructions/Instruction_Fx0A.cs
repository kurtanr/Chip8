namespace Chip8.Instructions;

/// <summary>
/// Wait for a key press, store the value of the key in Vx.
/// </summary>
/// <remarks>
/// All execution stops until a key is pressed, then the value of that key is stored in Vx.
/// Since it decrements <see cref="Cpu.PC"/> by 2 if key has not been pressed,
/// cannot be executed as first instruction of the program.
/// </remarks>
public class Instruction_Fx0A : CpuInstruction
{
  public Instruction_Fx0A(DecodedInstruction decodedInstruction) : base(decodedInstruction)
  {
    Description = $"Wait for a key press, store the value of the key in V{Decoded.x:X}.";
    Mnemonic = $"LD V{Decoded.x:X}, K";
  }

  /// <inheritdoc/>
  public override void Execute(Cpu cpu, IDisplay display, IKeyboard keyboard)
  {
    var pressedKey = keyboard.GetPressedKey();
    if (pressedKey != null)
    {
      cpu.V[Decoded.x] = (byte)pressedKey;
    }
    else
    {
      cpu.PC -= 2;
    }
  }
}
