using System;

namespace Chip8.Instructions;

/// <summary>
/// Store BCD representation of Vx in memory locations I, I+1, and I+2.
/// </summary>
/// <remarks>
/// The interpreter takes the decimal value of Vx, and places the hundreds digit in memory at location in I,
/// the tens digit at location I+1, and the ones digit at location I+2.
/// </remarks>
public class Instruction_Fx33 : CpuInstruction
{
  public Instruction_Fx33(DecodedInstruction decodedInstruction) : base(decodedInstruction)
  {
    Description = $"Set [I, I+1, I+2] = BCD(V{Decoded.x:X}).";
    Mnemonic = $"LD B, V{Decoded.x:X}";
  }

  /// <inheritdoc/>
  public override void Execute(Cpu cpu, IDisplay display, IKeyboard keyboard)
  {
    var maxAddress = Cpu.MemorySizeInBytes - 1;

    if ((cpu.I + 2) > maxAddress)
    {
      throw new InvalidOperationException($"Attempting to write to memory address I (0x{cpu.I:X})." +
        $"Highest accessible memory address is 0x{maxAddress:X}.");
    }

    var hundreds = cpu.V[Decoded.x] / 100;
    var tens = (cpu.V[Decoded.x] - hundreds * 100) / 10;
    var ones = cpu.V[Decoded.x] % 10;

    cpu.Memory[cpu.I++] = (byte)hundreds;
    cpu.Memory[cpu.I++] = (byte)tens;
    cpu.Memory[cpu.I] = (byte)ones;
  }
}
