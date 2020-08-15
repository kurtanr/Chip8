using Chip8.Instructions;

namespace Chip8
{
  /// <summary>
  /// Decodes memory content into instances of <see cref="DecodedInstruction"/> and <see cref="CpuInstruction"/>.
  /// </summary>
  public class InstructionDecoder
  {
    /// <summary>
    /// Decodes two bytes into an instance of <see cref="DecodedInstruction"/>.
    /// </summary>
    /// <param name="mostSignificant">Most significant byte of instruction (first one).</param>
    /// <param name="leastSignificant">Least significant byte of instruction (second one).</param>
    /// <returns>Instance of <see cref="DecodedInstruction"/>.</returns>
    public DecodedInstruction Decode(byte mostSignificant, byte leastSignificant)
    {
      // All instructions are 2 bytes long and are stored most-significant-byte first.
      var instructionCode = (ushort)((mostSignificant << 8) | leastSignificant);
      return new DecodedInstruction(instructionCode);
    }

    /// <summary>
    /// Converts instance of <see cref="DecodedInstruction"/> into an instance of <see cref="CpuInstruction"/>.
    /// </summary>
    /// <param name="decodedInstruction">Decoded 2 bytes of memory.</param>
    /// <returns>Concrete Cpu instruction or <see cref="UndefinedInstruction"/> if instruction cannot be determined.</returns>
    public CpuInstruction GetCpuInstruction(DecodedInstruction decodedInstruction)
    {
      CpuInstruction cpuInstruction = null;

      switch (decodedInstruction.OpCode)
      {
        case 0x0:
          if (decodedInstruction.kk == 0xE0)
          {
            cpuInstruction = new Instruction_00E0(decodedInstruction);
          }
          else if (decodedInstruction.kk == 0xEE)
          {
            cpuInstruction = new Instruction_00EE(decodedInstruction);
          }
          break;
        case 0x1:
          cpuInstruction = new Instruction_1nnn(decodedInstruction);
          break;
        case 0x2:
          cpuInstruction = new Instruction_2nnn(decodedInstruction);
          break;
        case 0x3:
          cpuInstruction = new Instruction_3xkk(decodedInstruction);
          break;
        case 0x4:
          cpuInstruction = new Instruction_4xkk(decodedInstruction);
          break;
        case 0x5:
          if (decodedInstruction.n == 0x0)
          {
            cpuInstruction = new Instruction_5xy0(decodedInstruction);
          }
          break;
        case 0x6:
          cpuInstruction = new Instruction_6xkk(decodedInstruction);
          break;
        case 0x7:
          cpuInstruction = new Instruction_7xkk(decodedInstruction);
          break;
        case 0x8:
          {
            switch (decodedInstruction.n)
            {
              case 0x0:
                cpuInstruction = new Instruction_8xy0(decodedInstruction);
                break;
              case 0x1:
                cpuInstruction = new Instruction_8xy1(decodedInstruction);
                break;
              case 0x2:
                cpuInstruction = new Instruction_8xy2(decodedInstruction);
                break;
              case 0x3:
                cpuInstruction = new Instruction_8xy3(decodedInstruction);
                break;
              case 0x4:
                cpuInstruction = new Instruction_8xy4(decodedInstruction);
                break;
              case 0x5:
                cpuInstruction = new Instruction_8xy5(decodedInstruction);
                break;
              case 0x6:
                cpuInstruction = new Instruction_8xy6(decodedInstruction);
                break;
              case 0x7:
                cpuInstruction = new Instruction_8xy7(decodedInstruction);
                break;
              case 0xE:
                cpuInstruction = new Instruction_8xyE(decodedInstruction);
                break;
              default:
                cpuInstruction = new UndefinedInstruction(decodedInstruction);
                break;
            }
          }
          break;
        case 0x9:
          if (decodedInstruction.n == 0x0)
          {
            cpuInstruction = new Instruction_9xy0(decodedInstruction);
          }
          break;
        case 0xA:
          cpuInstruction = new Instruction_Annn(decodedInstruction);
          break;
        case 0xB:
          cpuInstruction = new Instruction_Bnnn(decodedInstruction);
          break;
        case 0xC:
          cpuInstruction = new Instruction_Cxkk(decodedInstruction);
          break;
        case 0xF:
          {
            switch (decodedInstruction.kk)
            {
              case 0x55:
                cpuInstruction = new Instruction_Fx55(decodedInstruction);
                break;
              default:
                cpuInstruction = new UndefinedInstruction(decodedInstruction);
                break;
            }
            break;
          }
        // TODO: add other instructions
        default:
          cpuInstruction = new UndefinedInstruction(decodedInstruction);
          break;
      }

      return cpuInstruction ?? new UndefinedInstruction(decodedInstruction);
    }
  }
}
