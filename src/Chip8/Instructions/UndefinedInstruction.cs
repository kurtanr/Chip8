﻿using System;

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
    }

    /// <inheritdoc/>
    public override string Description => string.Empty;

    /// <inheritdoc/>
    public override string Mnemonic => $"0x{Decoded.InstructionCode:X4}";

    /// <inheritdoc/>
    public override void Execute(Cpu cpu, IDisplay display)
    {
      throw new InvalidOperationException($"Attempting to execute {nameof(UndefinedInstruction)}.");
    }

    /// <inheritdoc/>
    public override string ToString()
    {
      return Mnemonic;
    }
  }
}
