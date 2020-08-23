using Chip8.Instructions;
using Moq;
using NUnit.Framework;

namespace Chip8.Tests.Instructions
{
  [TestFixture]
  public class Instruction_Fx15Tests
  {
    [Test]
    public void Executing_Instruction_Fx15_WorksAsExpected()
    {
      var cpu = new Cpu();
      var display = new Mock<IDisplay>(MockBehavior.Strict).Object;
      var decodedInstruction = new DecodedInstruction(0xFA15);
      cpu.V[decodedInstruction.x] = 0x13;

      var instruction = new Instruction_Fx15(decodedInstruction);
      instruction.Execute(cpu, display);

      Assert.That(cpu.DT, Is.EqualTo(0x13));
      Assert.That(instruction.Mnemonic, Is.EqualTo($"LD DT, V{decodedInstruction.x:X}"));
    }
  }
}
