using Chip8.Instructions;
using Moq;
using NUnit.Framework;

namespace Chip8.Tests.Instructions
{
  [TestFixture]
  public class Instruction_6xkkTests
  {
    [Test]
    public void Executing_Instruction_6xkk_WorksAsExpected()
    {
      var cpu = new Cpu();
      var display = new Mock<IDisplay>(MockBehavior.Strict).Object;
      var decodedInstruction = new DecodedInstruction(0x6123);

      var instruction = new Instruction_6xkk(decodedInstruction);
      instruction.Execute(cpu, display);

      Assert.That(cpu.V[decodedInstruction.x], Is.EqualTo(decodedInstruction.kk));
      Assert.That(instruction.Mnemonic, Is.EqualTo($"LD V{decodedInstruction.x:X}, 0x{decodedInstruction.kk:X}"));
    }
  }
}
