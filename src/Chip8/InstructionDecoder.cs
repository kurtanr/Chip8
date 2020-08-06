using Chip8.Instructions;

namespace Chip8
{
  /// <summary>
  /// Decodes memory content into instances of <see cref="CpuInstruction"/>.
  /// </summary>
  public class InstructionDecoder
  {
    /// <summary>
    /// Decodes two bytes into an instance of <see cref="CpuInstruction"/>.
    /// </summary>
    /// <param name="mostSignificant">Most significant byte of instruction (first one).</param>
    /// <param name="leastSignificant">Least significant byte of instruction (second one).</param>
    /// <returns>Decoded instruction or <see cref="UndefinedInstruction"/> if instruction cannot be decoded.</returns>
    public CpuInstruction Decode(byte mostSignificant, byte leastSignificant)
    {
      // All instructions are 2 bytes long and are stored most-significant-byte first.
      ushort instructionCode = (ushort)((mostSignificant << 8) | leastSignificant);

      // nnn or addr - A 12 - bit value, the lowest 12 bits of the instruction
      ushort nnn = (ushort)(instructionCode & 0xFFF);

      // n or nibble - A 4-bit value, the lowest 4 bits of the instruction
      byte n = (byte)(instructionCode & 0xF);

      // x - A 4-bit value, the lower 4 bits of the high byte of the instruction
      byte x = (byte)((instructionCode & 0xF00) >> 8);

      // y - A 4-bit value, the upper 4 bits of the low byte of the instruction
      byte y = (byte)((instructionCode & 0xF0) >> 4);

      // kk or byte - An 8-bit value, the lowest 8 bits of the instruction
      byte kk = (byte)(instructionCode & 0xFF);

      // A 4-bit value, the upper 4 bits of the high byte of the instruction
      byte opCode = (byte)((instructionCode & 0xF000) >> 12);

      CpuInstruction cpuInstruction = null;

      switch (opCode)
      {
        case 0x0:
          if (kk == 0xE0)
          {
            cpuInstruction = new Instruction_00E0(instructionCode);
          }
          else if (kk == 0xEE)
          {
            cpuInstruction = new Instruction_00EE(instructionCode);
          }
          break;
        case 0x1:
          cpuInstruction = new Instruction_1nnn(instructionCode, nnn);
          break;
        case 0x2:
          cpuInstruction = new Instruction_2nnn(instructionCode, nnn);
          break;
        // TODO: add other instructions
      }

      return cpuInstruction ?? new UndefinedInstruction(instructionCode);
    }
  }
}
