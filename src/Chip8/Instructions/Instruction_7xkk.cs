namespace Chip8.Instructions;

/// <summary>
/// Set Vx = Vx + kk.
/// </summary>
/// <remarks>
/// Adds the value kk to the value of register Vx, then stores the result in Vx.
/// </remarks>
public class Instruction_7xkk : CpuInstruction
{
  public Instruction_7xkk(DecodedInstruction decodedInstruction) : base(decodedInstruction)
  {
    Description = $"Set V{Decoded.x:X} = V{Decoded.x:X} + 0x{Decoded.kk:X}.";
    Mnemonic = $"ADD V{Decoded.x:X}, 0x{Decoded.kk:X}";
  }

  /// <inheritdoc/>
  public override void Execute(Cpu cpu, IDisplay display, IKeyboard keyboard)
  {
    // Vx is an 8-bit register and if the sum is over 255 (0xFF), it will be reduced by 256 (0x100) to fit.
    // Unlike in Instruction_8xy4, VF will not be set to carry flag.
    cpu.V[Decoded.x] += Decoded.kk;
  }
}
