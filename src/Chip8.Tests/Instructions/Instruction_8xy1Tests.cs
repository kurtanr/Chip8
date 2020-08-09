using Chip8.Instructions;
using Moq;
using NUnit.Framework;

namespace Chip8.Tests.Instructions
{
  [TestFixture]
  public class Instruction_8xy1Tests
  {
    [Test]
    public void Executing_Instruction_8xy1_WorksAsExpected()
    {
      var cpu = new Cpu();
      var display = new Mock<IDisplay>(MockBehavior.Strict).Object;
      var decodedInstruction = new DecodedInstruction(0x8AB1);
      const byte value1 = 0xAB;
      const byte value2 = 0xBC;
      cpu.V[decodedInstruction.x] = value1;
      cpu.V[decodedInstruction.y] = value2;

      var instruction = new Instruction_8xy1(decodedInstruction);
      instruction.Execute(cpu, display);

      Assert.That(cpu.V[decodedInstruction.x], Is.EqualTo(value1 | value2));
      Assert.That(instruction.Mnemonic, Is.EqualTo($"OR V{decodedInstruction.x:X}, V{decodedInstruction.y:X}"));
    }
  }
}
