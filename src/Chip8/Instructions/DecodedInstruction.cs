using System;

namespace Chip8.Instructions;

/// <summary>
/// Instruction decoded from 2 bytes of memory.
/// </summary>
public readonly struct DecodedInstruction : IEquatable<DecodedInstruction>
{
  /// <summary>
  /// Entire instruction - 2 bytes long and stored most-significant-byte first.
  /// </summary>
  public readonly ushort InstructionCode;

  /// <summary>
  /// nnn or addr - A 12 - bit value, the lowest 12 bits of the instruction.
  /// </summary>
  public readonly ushort nnn;

  /// <summary>
  /// n or nibble - A 4-bit value, the lowest 4 bits of the instruction.
  /// </summary>
  public readonly byte n;

  /// <summary>
  /// x - A 4-bit value, the lower 4 bits of the high byte of the instruction.
  /// </summary>
  public readonly byte x;

  /// <summary>
  /// y - A 4-bit value, the upper 4 bits of the low byte of the instruction.
  /// </summary>
  public readonly byte y;

  /// <summary>
  /// kk or byte - An 8-bit value, the lowest 8 bits of the instruction.
  /// </summary>
  public readonly byte kk;

  /// <summary>
  /// A 4-bit value, the upper 4 bits of the high byte of the instruction.
  /// </summary>
  public readonly byte OpCode;

  public DecodedInstruction(ushort instructionCode)
  {
    InstructionCode = instructionCode;
    nnn = (ushort)(InstructionCode & 0xFFF);
    n = (byte)(InstructionCode & 0xF);
    x = (byte)((InstructionCode & 0xF00) >> 8);
    y = (byte)((InstructionCode & 0xF0) >> 4);
    kk = (byte)(InstructionCode & 0xFF);
    OpCode = (byte)((InstructionCode & 0xF000) >> 12);
  }

  /// <inheritdoc/>
  public bool Equals(DecodedInstruction other)
  {
    return InstructionCode == other.InstructionCode;
  }

  public override bool Equals(object obj)
  {
    return obj is DecodedInstruction other && Equals(other);
  }

  public override int GetHashCode()
  {
    return InstructionCode.GetHashCode();
  }

  public static bool operator ==(DecodedInstruction left, DecodedInstruction right)
  {
    return left.Equals(right);
  }

  public static bool operator !=(DecodedInstruction left, DecodedInstruction right)
  {
    return !left.Equals(right);
  }
}
