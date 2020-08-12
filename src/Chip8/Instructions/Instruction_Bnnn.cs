namespace Chip8.Instructions
{
  /// <summary>
  /// Jump to location nnn + V0.
  /// </summary>
  /// <remarks>
  /// The program counter is set to nnn plus the value of V0.
  /// </remarks>
  public class Instruction_Bnnn : CpuInstruction
  {
    public Instruction_Bnnn(DecodedInstruction decodedInstruction) : base(decodedInstruction)
    {
      Description = $"Jump to location 0x{Decoded.nnn:X} + V0.";
      Mnemonic = $"JP V0, 0x{Decoded.nnn:X}";
    }

    /// <inheritdoc/>
    public override void Execute(Cpu cpu, IDisplay display)
    {
      cpu.PC = (ushort)(Decoded.nnn + cpu.V[0]);
    }
  }
}
