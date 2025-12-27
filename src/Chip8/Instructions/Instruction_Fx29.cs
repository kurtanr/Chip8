namespace Chip8.Instructions;

/// <summary>
/// Set I = location of sprite for digit Vx.
/// </summary>
/// <remarks>
/// The value of I is set to the location for the hexadecimal sprite corresponding to the value of Vx.
/// Only the lower 4 bits of Vx are used as the hex digit index.
/// </remarks>
public class Instruction_Fx29 : CpuInstruction
{
  public Instruction_Fx29(DecodedInstruction decodedInstruction) : base(decodedInstruction)
  {
    Description = $"Set I = address of sprite for value of V{Decoded.x:X}.";
    Mnemonic = $"LD F, V{Decoded.x:X}";
  }

  /// <inheritdoc/>
  public override void Execute(Cpu cpu, IDisplay display, IKeyboard keyboard)
  {
    var vxValue = cpu.V[Decoded.x];
    var hexDigit = vxValue & 0xF;

    // Fonts are stored at Cpu.FontMemoryAddress; each digit sprite is 5 bytes
    cpu.I = (ushort)(Cpu.FontMemoryAddress + hexDigit * 5);
  }
}
