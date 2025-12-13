using System;

namespace Chip8.Instructions;

/// <summary>
/// Read registers V0 through Vx from memory starting at location I.
/// </summary>
/// <remarks>
/// The interpreter reads values from memory starting at location I into registers V0 through Vx.
/// I is set to I + X + 1 after operation.
/// </remarks>
public class Instruction_Fx65 : CpuInstruction
{
  public Instruction_Fx65(DecodedInstruction decodedInstruction) : base(decodedInstruction)
  {
    if (Decoded.x == 0)
    {
      Description = $"Fill V0 with value from address I, I+=1.";
    }
    else
    {
      Description = $"Fill V0..V{Decoded.x:X} with values starting from address I, I+={Decoded.x + 1}.";
    }
    Mnemonic = $"LD V{Decoded.x:X}, [I]";
  }

  /// <inheritdoc/>
  public override void Execute(Cpu cpu, IDisplay display, IKeyboard keyboard)
  {
    var maxAddress = Cpu.MemorySizeInBytes - 1;

    for (byte i = 0; i <= Decoded.x; i++)
    {
      if (cpu.I > maxAddress)
      {
        throw new InvalidOperationException($"Attempting to read from memory address I (0x{cpu.I:X})." +
          $"Highest accessible memory address is 0x{maxAddress:X}.");
      }

      cpu.V[i] = cpu.Memory[cpu.I++];
    }
  }
}
