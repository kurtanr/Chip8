using System;

namespace Chip8.Instructions
{
  /// <summary>
  /// Store registers V0 through Vx in memory starting at location I.
  /// </summary>
  /// <remarks>
  /// The interpreter copies the values of registers V0 through Vx into memory, starting at the address in I.
  /// I is set to I + X + 1 after operation.
  /// </remarks>
  public class Instruction_Fx55 : CpuInstruction
  {
    public Instruction_Fx55(DecodedInstruction decodedInstruction) : base(decodedInstruction)
    {
      if(Decoded.x == 0)
      {
        Description = $"Store V0 at address I, I+=1.";
      }
      else
      {
        Description = $"Store V0..V{Decoded.x:X} starting at address I, I+={Decoded.x + 1}";
      }
      Mnemonic = $"LD [I], V{Decoded.x:X}";
    }

    /// <inheritdoc/>
    public override void Execute(Cpu cpu, IDisplay display)
    {
      var maxAddress = Cpu.MemorySizeInBytes - 1;

      for (int i = 0; i<= Decoded.x; i++)
      {
        if(cpu.I > maxAddress)
        {
          throw new InvalidOperationException($"Attempting to write at memory address I (0x{cpu.I:X})." +
            $"Highest accessible memory address is 0x{maxAddress:X}.");
        }

        cpu.Memory[cpu.I++] = cpu.V[i];
      }
    }
  }
}
