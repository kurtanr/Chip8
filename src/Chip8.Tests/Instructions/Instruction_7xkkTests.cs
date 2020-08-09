using Chip8.Instructions;
using Moq;
using NUnit.Framework;

namespace Chip8.Tests.Instructions
{
  [TestFixture]
  public class Instruction_7xkkTests
  {
    [Test]
    public void Executing_Instruction_7xkk_WorksAsExpected()
    {
      var cpu = new Cpu();
      var display = new Mock<IDisplay>(MockBehavior.Strict).Object;
      var decodedInstruction = new DecodedInstruction(0x7123);
      const byte oldVxValue = 0x38;
      cpu.V[decodedInstruction.x] = oldVxValue;

      var instruction = new Instruction_7xkk(decodedInstruction);
      instruction.Execute(cpu, display);

      Assert.That(cpu.V[decodedInstruction.x], Is.EqualTo(oldVxValue + decodedInstruction.kk));
      Assert.That(instruction.Mnemonic, Is.EqualTo($"ADD V{decodedInstruction.x:X}, 0x{decodedInstruction.kk:X}"));
    }
  }
}
