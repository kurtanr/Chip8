using Chip8.Instructions;
using Moq;
using NUnit.Framework;

namespace Chip8.Tests.Instructions
{
  [TestFixture]
  public class Instruction_8xy3Tests
  {
    [Test]
    public void Executing_Instruction_8xy3_WorksAsExpected()
    {
      var cpu = new Cpu();
      var display = new Mock<IDisplay>(MockBehavior.Strict).Object;
      var decodedInstruction = new DecodedInstruction(0x8AB3);
      const byte value1 = 0x1C;
      const byte value2 = 0xF4;
      cpu.V[decodedInstruction.x] = value1;
      cpu.V[decodedInstruction.y] = value2;

      var instruction = new Instruction_8xy3(decodedInstruction);
      instruction.Execute(cpu, display);

      Assert.That(cpu.V[decodedInstruction.x], Is.EqualTo(value1 ^ value2));
      Assert.That(instruction.Mnemonic, Is.EqualTo($"XOR V{decodedInstruction.x:X}, V{decodedInstruction.y:X}"));
    }
  }
}
