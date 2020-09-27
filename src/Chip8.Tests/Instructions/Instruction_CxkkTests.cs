using Chip8.Instructions;
using NUnit.Framework;

namespace Chip8.Tests.Instructions
{
  [TestFixture]
  public class Instruction_CxkkTests : BaseInstructionTests
  {
    [TestCase((ushort)0xC101)]
    [TestCase((ushort)0xC123)]
    public void Executing_Instruction_Cxkk_WorksAsExpected(ushort instructionCode)
    {
      var cpu = new Cpu();
      var decodedInstruction = new DecodedInstruction(instructionCode);

      var instruction = new Instruction_Cxkk(decodedInstruction);
      instruction.Execute(cpu, MockedDisplay, MockedKeyboard);

      Assert.That(cpu.V[decodedInstruction.x], Is.LessThanOrEqualTo(decodedInstruction.kk));
      Assert.That(instruction.Mnemonic, Is.EqualTo($"RND V{decodedInstruction.x:X}, 0x{decodedInstruction.kk:X}"));
    }
  }
}
