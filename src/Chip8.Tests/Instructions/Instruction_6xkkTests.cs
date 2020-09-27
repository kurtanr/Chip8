using Chip8.Instructions;
using NUnit.Framework;

namespace Chip8.Tests.Instructions
{
  [TestFixture]
  public class Instruction_6xkkTests : BaseInstructionTests
  {
    [Test]
    public void Executing_Instruction_6xkk_WorksAsExpected()
    {
      var cpu = new Cpu();
      var decodedInstruction = new DecodedInstruction(0x6123);

      var instruction = new Instruction_6xkk(decodedInstruction);
      instruction.Execute(cpu, MockedDisplay, MockedKeyboard);

      Assert.That(cpu.V[decodedInstruction.x], Is.EqualTo(decodedInstruction.kk));
      Assert.That(instruction.Mnemonic, Is.EqualTo($"LD V{decodedInstruction.x:X}, 0x{decodedInstruction.kk:X}"));
    }
  }
}
