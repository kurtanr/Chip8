namespace Chip8.Instructions;

/// <summary>
/// Call subroutine at nnn.
/// </summary>
/// <remarks>
/// The interpreter increments the stack pointer, then puts the current PC on the top of the stack. The PC is then set to nnn.
/// </remarks>
public class Instruction_2nnn : CpuInstruction
{
  public Instruction_2nnn(DecodedInstruction decodedInstruction) : base(decodedInstruction)
  {
    Description = $"Call subroutine at 0x{Decoded.nnn:X}.";
    Mnemonic = $"CALL 0x{Decoded.nnn:X}";
  }

  /// <inheritdoc/>
  public override void Execute(Cpu cpu, IDisplay display, IKeyboard keyboard)
  {
    cpu.Stack.Push((ushort)(cpu.PC + 2));
    cpu.PC = Decoded.nnn;
  }
}
