using System;

namespace Chip8.Instructions
{
  /// <summary>
  /// Skip next instruction if key with the value of Vx is not pressed.
  /// </summary>
  /// <remarks>
  /// Checks the keyboard, and if the key corresponding to the value of Vx is currently in the up position,
  /// PC is increased by 2.
  /// </remarks>
  public class Instruction_ExA1 : CpuInstruction
  {
    public Instruction_ExA1(DecodedInstruction decodedInstruction) : base(decodedInstruction)
    {
      Description = $"Skip next instruction if key with the value of V{Decoded.x:X} is not pressed.";
      Mnemonic = $"SKNP V{Decoded.x:X}";
    }

    /// <inheritdoc/>
    public override void Execute(Cpu cpu, IDisplay display, IKeyboard keyboard)
    {
      var pressedKey = keyboard.GetPressedKey();
      if (cpu.V[Decoded.x] != pressedKey)
      {
        cpu.PC += 2;
      }
    }
  }
}
