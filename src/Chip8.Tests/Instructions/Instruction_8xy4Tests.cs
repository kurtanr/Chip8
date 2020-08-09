using Chip8.Instructions;
using Moq;
using NUnit.Framework;
using System;

namespace Chip8.Tests.Instructions
{
  [TestFixture]
  public class Instruction_8xy4Tests
  {
    [TestCase(0xFE, 0x01, 0xFF, 0)]
    [TestCase(0xFE, 0x02, 0x00, 1)]
    [TestCase(0x1C, 0xF4, 0x10, 1)]
    public void Executing_Instruction_8xy4_WorksAsExpected(byte value1, byte value2, byte expectedSum, byte expectedCarry)
    {
      var cpu = new Cpu();
      var display = new Mock<IDisplay>(MockBehavior.Strict).Object;
      var decodedInstruction = new DecodedInstruction(0x8AB4);
      cpu.V[decodedInstruction.x] = value1;
      cpu.V[decodedInstruction.y] = value2;

      var instruction = new Instruction_8xy4(decodedInstruction);
      instruction.Execute(cpu, display);

      Assert.That(cpu.V[decodedInstruction.x], Is.EqualTo(expectedSum));
      Assert.That(cpu.V[0xF], Is.EqualTo(expectedCarry));
      Assert.That(instruction.Mnemonic, Is.EqualTo($"ADD V{decodedInstruction.x:X}, V{decodedInstruction.y:X}"));
    }

    [Test]
    public void Executing_Instruction_8xy4_WithVx_SetToVF_ThrowsException()
    {
      var cpu = new Cpu();
      var display = new Mock<IDisplay>(MockBehavior.Strict).Object;
      var decodedInstruction = new DecodedInstruction(0x8FB4);

      var instruction = new Instruction_8xy4(decodedInstruction);
      Assert.Throws<InvalidOperationException>(() => instruction.Execute(cpu, display));
    }
  }
}
