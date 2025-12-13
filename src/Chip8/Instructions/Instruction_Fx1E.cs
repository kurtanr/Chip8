using System;

namespace Chip8.Instructions;

/// <summary>
/// Set I = I + Vx.
/// </summary>
/// <remarks>
/// The values of I and Vx are added, and the results are stored in I.
/// </remarks>
public class Instruction_Fx1E : CpuInstruction
{
  public Instruction_Fx1E(DecodedInstruction decodedInstruction) : base(decodedInstruction)
  {
    Description = $"Set I = I + V{Decoded.x:X}.";
    Mnemonic = $"ADD I, V{Decoded.x:X}";
  }

  /// <inheritdoc/>
  public override void Execute(Cpu cpu, IDisplay display, IKeyboard keyboard)
  {
    var address = (ushort)(cpu.I + cpu.V[Decoded.x]);
    var maxAddress = Cpu.MemorySizeInBytes - 1;

    if (address > maxAddress)
    {
      throw new InvalidOperationException($"Attempting to set I register to 0x{cpu.I:X}." +
        $"Highest accessible memory address is 0x{maxAddress:X}.");
    }

    cpu.I = address;
  }
}
