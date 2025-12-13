using Chip8.Instructions;
using System;
using System.Globalization;
using System.Linq;

namespace Chip8;

/// <summary>
/// Converts string representations of Chip-8 instructions into instances of <see cref="CpuInstruction"/>.
/// </summary>
public class InstructionEncoder
{
  /// <summary>
  /// Converts string representation of a Chip-8 instruction into instance of <see cref="CpuInstruction"/>
  /// </summary>
  /// <param name="instructionAsString">String representation of an instruction (see <see cref="CpuInstruction.Mnemonic"/>).</param>
  /// <returns>Instance of <see cref="CpuInstruction"/>.</returns>
  public CpuInstruction GetCpuInstruction(string instructionAsString)
  {
    var instruction = instructionAsString.Trim().ToLowerInvariant();

    var decodedInstruction = instruction switch
    {
      // Instruction_00E0
      "cls" =>
        new DecodedInstruction(0x00E0),

      // Instruction_00EE
      "ret" =>
        new DecodedInstruction(0x00EE),

      // Instruction_1nnn
      _ when instruction.StartsWith("jp 0x") =>
        new DecodedInstruction((ushort)(0x1000 | GetNnn(instruction))),

      // Instruction_2nnn
      _ when instruction.StartsWith("call 0x") =>
        new DecodedInstruction((ushort)(0x2000 | GetNnn(instruction))),

      // Instruction_3xkk
      _ when instruction.StartsWith("se v") && instruction.Contains("0x") =>
        new DecodedInstruction((ushort)(0x3000 | GetXkk(instruction))),

      // Instruction_4xkk
      _ when instruction.StartsWith("sne v") && instruction.Contains("0x") =>
        new DecodedInstruction((ushort)(0x4000 | GetXkk(instruction))),

      // Instruction_5xy0
      _ when instruction.StartsWith("se v") && !instruction.Contains("0x") =>
        new DecodedInstruction((ushort)(0x5000 | GetXy(instruction))),

      // Instruction_6xkk
      _ when instruction.StartsWith("ld v") && instruction.Contains("0x") =>
        new DecodedInstruction((ushort)(0x6000 | GetXkk(instruction))),

      // Instruction_7xkk
      _ when instruction.StartsWith("add v") && instruction.Contains("0x") =>
        new DecodedInstruction((ushort)(0x7000 | GetXkk(instruction))),

      // Instruction_8xy0
      _ when instruction.StartsWith("ld v") && instruction.Count(x => x.Equals('v')) == 2 =>
        new DecodedInstruction((ushort)(0x8000 | GetXy(instruction))),

      // Instruction_8xy1
      _ when instruction.StartsWith("or v") =>
        new DecodedInstruction((ushort)(0x8001 | GetXy(instruction))),

      // Instruction_8xy2
      _ when instruction.StartsWith("and v") =>
        new DecodedInstruction((ushort)(0x8002 | GetXy(instruction))),

      // Instruction_8xy3
      _ when instruction.StartsWith("xor v") =>
        new DecodedInstruction((ushort)(0x8003 | GetXy(instruction))),

      // Instruction_8xy4
      _ when instruction.StartsWith("add v") && !instruction.Contains("0x") =>
        new DecodedInstruction((ushort)(0x8004 | GetXy(instruction))),

      // Instruction_8xy5
      _ when instruction.StartsWith("sub v") =>
        new DecodedInstruction((ushort)(0x8005 | GetXy(instruction))),

      // Instruction_8xy6
      _ when instruction.StartsWith("shr v") =>
        new DecodedInstruction((ushort)(0x8006 | GetXy(instruction))),

      // Instruction_8xy7
      _ when instruction.StartsWith("subn v") =>
        new DecodedInstruction((ushort)(0x8007 | GetXy(instruction))),

      // Instruction_8xyE
      _ when instruction.StartsWith("shl v") =>
        new DecodedInstruction((ushort)(0x800E | GetXy(instruction))),

      // Instruction_9xy0
      _ when instruction.StartsWith("sne v") && !instruction.Contains("0x") =>
        new DecodedInstruction((ushort)(0x9000 | GetXy(instruction))),

      // Instruction_Annn
      _ when instruction.StartsWith("ld i") =>
        new DecodedInstruction((ushort)(0xA000 | GetNnn(instruction))),

      // Instruction_Bnnn
      _ when instruction.StartsWith("jp v0") =>
        new DecodedInstruction((ushort)(0xB000 | GetNnn(instruction))),

      // Instruction_Cxkk
      _ when instruction.StartsWith("rnd v") && instruction.Contains("0x") =>
        new DecodedInstruction((ushort)(0xC000 | GetXkk(instruction))),

      // Instruction_Dxyn
      _ when instruction.StartsWith("drw") =>
        new DecodedInstruction((ushort)(0xD000 | GetXyn(instruction))),

      // Instruction_Ex9E
      _ when instruction.StartsWith("skp") =>
        new DecodedInstruction((ushort)(0xE09E | GetX(instruction))),

      // Instruction_ExA1
      _ when instruction.StartsWith("sknp") =>
        new DecodedInstruction((ushort)(0xE0A1 | GetX(instruction))),

      // Instruction_Fx07
      _ when instruction.StartsWith("ld v") && instruction.Contains("dt") =>
        new DecodedInstruction((ushort)(0xF007 | GetX(instruction))),

      // Instruction_Fx0A
      _ when instruction.StartsWith("ld v") && instruction.Contains("k") =>
        new DecodedInstruction((ushort)(0xF00A | GetX(instruction))),

      // Instruction_Fx15
      _ when instruction.StartsWith("ld dt") =>
        new DecodedInstruction((ushort)(0xF015 | GetX(instruction))),

      // Instruction_Fx18
      _ when instruction.StartsWith("ld st") =>
        new DecodedInstruction((ushort)(0xF018 | GetX(instruction))),

      // Instruction_Fx1E
      _ when instruction.StartsWith("add i") =>
        new DecodedInstruction((ushort)(0xF01E | GetX(instruction))),

      // Instruction_Fx29
      _ when instruction.StartsWith("ld f") =>
        new DecodedInstruction((ushort)(0xF029 | GetX(instruction))),

      // Instruction_Fx33
      _ when instruction.StartsWith("ld b") =>
        new DecodedInstruction((ushort)(0xF033 | GetX(instruction))),

      // Instruction_Fx55
      _ when instruction.StartsWith("ld [i]") =>
        new DecodedInstruction((ushort)(0xF055 | GetX(instruction))),

      // Instruction_Fx65
      _ when instruction.StartsWith("ld v") && instruction.Contains("[i]") =>
        new DecodedInstruction((ushort)(0xF065 | GetX(instruction))),

      // UndefinedInstruction
      _ when instruction.StartsWith("0x") =>
        new DecodedInstruction(GetNnn(instruction)),

      // e.g. empty line
      _ => new DecodedInstruction(0x0000)
    };

    var instructionDecoder = new InstructionDecoder();
    return instructionDecoder.GetCpuInstruction(decodedInstruction);
  }

  /// <summary>
  /// Applies to instruction in the form of: "OPERATION 0xnnn"<br></br>
  /// Where OPERATION can be one of: jp, call, ld i, jp v0
  /// </summary>
  private ushort GetNnn(string instruction)
  {
    return ParseAddressUShort(instruction);
  }

  /// <summary>
  /// Applies to instruction in the form of: "OPERATION vx[0-f], 0xkk"<br></br>
  /// Where OPERATION can be one of: se, sne, ld, add, rnd
  /// </summary>
  private ushort GetXkk(string instruction)
  {
    byte x = ParseRegister(instruction);
    byte kk = ParseAddress(instruction);

    return (ushort)(x << 8 | kk);
  }

  /// <summary>
  /// Applies to instruction in the form of: "OPERATION vx[0-f], vy[0-f]"<br></br>
  /// Where OPERATION can be one of: se, ld, or, and, xor, add, sub, shr, subn, shl, sne
  /// </summary>
  private ushort GetXy(string instruction)
  {
    byte x = ParseRegister(instruction);
    byte y = ParseLastRegister(instruction);

    return (ushort)(x << 8 | y << 4);
  }

  /// <summary>
  /// Applies to Instruction_Dxyn: "DRW vx[0-f], vy[0-f], 0xn"
  /// </summary>
  private ushort GetXyn(string instruction)
  {
    byte x = ParseRegister(instruction);
    byte y = ParseLastRegister(instruction);
    byte n = ParseAddress(instruction);

    return (ushort)(x << 8 | y << 4 | n);
  }

  /// <summary>
  /// Applies to instructions in the form of: "OPERATION ... vx[0-f]"
  /// </summary>
  private ushort GetX(string instruction)
  {
    var x = ParseRegister(instruction);
    return (ushort)(x << 8);
  }

  private byte ParseRegister(string instruction)
  {
    return byte.Parse(instruction.Substring(instruction.IndexOf("v", StringComparison.InvariantCulture) + 1, 1),
      NumberStyles.HexNumber, CultureInfo.InvariantCulture);
  }

  private byte ParseLastRegister(string instruction)
  {
    return byte.Parse(instruction.Substring(instruction.LastIndexOf("v", StringComparison.InvariantCulture) + 1, 1),
      NumberStyles.HexNumber, CultureInfo.InvariantCulture);
  }

  private byte ParseAddress(string instruction)
  {
    return byte.Parse(instruction.Substring(instruction.IndexOf("0x", StringComparison.InvariantCulture) + 2),
      NumberStyles.HexNumber, CultureInfo.InvariantCulture);
  }

  private ushort ParseAddressUShort(string instruction)
  {
    return ushort.Parse(instruction.Substring(instruction.IndexOf("0x", StringComparison.InvariantCulture) + 2),
      NumberStyles.HexNumber, CultureInfo.InvariantCulture);
  }
}
