using Chip8.Instructions;
using Moq;
using NUnit.Framework;

namespace Chip8.Tests.Instructions
{
  [TestFixture]
  public class Instruction_8xy0Tests
  {
    [Test]
    public void Executing_Instruction_8xy0_WorksAsExpected()
    {
      var cpu = new Cpu();
      var display = new Mock<IDisplay>(MockBehavior.Strict).Object;
      var decodedInstruction = new DecodedInstruction(0x8AB0);
      const byte value = 0xAB;
      cpu.V[decodedInstruction.y] = value;

      var instruction = new Instruction_8xy0(decodedInstruction);
      instruction.Execute(cpu, display);

      Assert.That(cpu.V[decodedInstruction.x], Is.EqualTo(value));
      Assert.That(cpu.V[decodedInstruction.y], Is.EqualTo(value));
      Assert.That(instruction.Mnemonic, Is.EqualTo($"LD V{decodedInstruction.x:X}, V{decodedInstruction.y:X}"));
    }
  }
}
