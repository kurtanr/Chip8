using Chip8.Instructions;
using System.Linq;

namespace Chip8
{
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

        // UndefinedInstruction
        _ when instruction.StartsWith("0x") =>
          new DecodedInstruction(GetNnn(instruction)),

        // TODO: throw exception?
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
      return ushort.Parse(instruction.Substring(instruction.IndexOf("0x") + 2), System.Globalization.NumberStyles.HexNumber);
    }

    /// <summary>
    /// Applies to instruction in the form of: "OPERATION vx[0-f], 0xkk"<br></br>
    /// Where OPERATION can be one of: se, sne, ld, add, rnd
    /// </summary>
    private ushort GetXkk(string instruction)
    {
      byte x = byte.Parse(instruction.Substring(instruction.IndexOf("v") + 1, 1), System.Globalization.NumberStyles.HexNumber);
      byte kk = byte.Parse(instruction.Substring(instruction.IndexOf("0x") + 2), System.Globalization.NumberStyles.HexNumber);

      return (ushort)(x << 8 | kk);
    }

    /// <summary>
    /// Applies to instruction in the form of: "OPERATION vx[0-f], vy[0-f]"<br></br>
    /// Where OPERATION can be one of: se, ld, or, and, xor, add, sub, shr, subn, shl, sne
    /// </summary>
    private ushort GetXy(string instruction)
    {
      byte x = byte.Parse(instruction.Substring(instruction.IndexOf("v") + 1, 1), System.Globalization.NumberStyles.HexNumber);
      byte y = byte.Parse(instruction.Substring(instruction.LastIndexOf("v") + 1, 1), System.Globalization.NumberStyles.HexNumber);

      return (ushort)(x << 8 | y << 4);
    }
  }
}
