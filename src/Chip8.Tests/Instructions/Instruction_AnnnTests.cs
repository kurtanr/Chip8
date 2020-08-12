using Chip8.Instructions;
using Moq;
using NUnit.Framework;

namespace Chip8.Tests.Instructions
{
  [TestFixture]
  public class Instruction_AnnnTests
  {
    [Test]
    public void Executing_Instruction_Annn_WorksAsExpected()
    {
      var cpu = new Cpu();
      var display = new Mock<IDisplay>(MockBehavior.Strict).Object;
      var decodedInstruction = new DecodedInstruction(0xA123);

      var instruction = new Instruction_Annn(decodedInstruction);
      instruction.Execute(cpu, display);

      Assert.That(cpu.I, Is.EqualTo(decodedInstruction.nnn));
      Assert.That(instruction.Mnemonic, Is.EqualTo($"LD I, 0x{decodedInstruction.nnn:X}"));
    }
  }
}
