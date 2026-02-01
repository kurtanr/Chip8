using System;

namespace Chip8.Instructions;

/// <summary>
/// Set Vx = random byte AND kk.
/// </summary>
/// <remarks>
/// The interpreter generates a random number from 0 to 255, which is then ANDed with the value kk.
/// The results are stored in Vx. See instruction <see cref="Instruction_8xy2"/> for more information on AND.
/// </remarks>
public class Instruction_Cxkk : CpuInstruction
{
  public Instruction_Cxkk(DecodedInstruction decodedInstruction) : base(decodedInstruction)
  {
    Description = $"Set V{Decoded.x:X} = random byte AND 0x{Decoded.kk:X}.";
    Mnemonic = $"RND V{Decoded.x:X}, 0x{Decoded.kk:X}";
  }

  /// <inheritdoc/>
  public override void Execute(Cpu cpu, IDisplay display, IKeyboard keyboard)
  {
    var random = new Random();
    var number = (byte)random.Next(0, 256);
    cpu.V[Decoded.x] = (byte)(number & Decoded.kk);
  }
}
