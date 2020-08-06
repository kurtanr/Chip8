using System;

namespace Chip8.Instructions
{
  /// <summary>
  /// Undefined instruction.
  /// Represents data in memory.
  /// </summary>
  public class UndefinedInstruction : CpuInstruction
  {
    public UndefinedInstruction(ushort instructionCode) : base(instructionCode)
    {
    }

    public override string Description => string.Empty;

    public override string Mnemonic => $"0x{InstructionCode:X4}";

    public override void Execute(Cpu cpu, IDisplay display)
    {
      throw new InvalidOperationException($"Attempting to execute {nameof(UndefinedInstruction)}.");
    }

    public override string ToString()
    {
      return Mnemonic;
    }
  }
}
