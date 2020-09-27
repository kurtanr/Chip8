using System;

namespace Chip8.Instructions
{
  /// <summary>
  /// Undefined instruction.
  /// Represents data in memory.
  /// </summary>
  public class UndefinedInstruction : CpuInstruction
  {
    public UndefinedInstruction(DecodedInstruction decodedInstruction) : base(decodedInstruction)
    {
      Description = string.Empty;
      Mnemonic = $"0x{Decoded.InstructionCode:X4}";
    }

    /// <inheritdoc/>
    public override void Execute(Cpu cpu, IDisplay display, IKeyboard keyboard)
    {
      throw new InvalidOperationException($"Attempting to execute {nameof(UndefinedInstruction)}: {Mnemonic}.");
    }

    /// <inheritdoc/>
    public override string ToStringWithDescription()
    {
      return ToString();
    }
  }
}
