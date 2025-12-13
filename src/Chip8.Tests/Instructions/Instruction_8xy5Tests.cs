using Chip8.Instructions;
using NUnit.Framework;
using System;

namespace Chip8.Tests.Instructions;

[TestFixture]
public class Instruction_8xy5Tests : BaseInstructionTests
{
  [TestCase(0xFE, 0x01, 0xFD, 1)]
  [TestCase(0x01, 0xFE, 0xFD, 0)]
  [TestCase(0x02, 0x02, 0x00, 1)]
  public void Executing_Instruction_8xy5_WorksAsExpected(byte value1, byte value2, byte expectedResult, byte expectedVF)
  {
    var cpu = new Cpu();
    var decodedInstruction = new DecodedInstruction(0x8AB5);
    cpu.V[decodedInstruction.x] = value1;
    cpu.V[decodedInstruction.y] = value2;

    var instruction = new Instruction_8xy5(decodedInstruction);
    instruction.Execute(cpu, MockedDisplay, MockedKeyboard);

    Assert.That(cpu.V[decodedInstruction.x], Is.EqualTo(expectedResult));
    Assert.That(cpu.V[0xF], Is.EqualTo(expectedVF));
    Assert.That(instruction.Mnemonic, Is.EqualTo($"SUB V{decodedInstruction.x:X}, V{decodedInstruction.y:X}"));
  }

  [Test]
  public void Executing_Instruction_8xy5_WithVx_SetToVF_ThrowsException()
  {
    var cpu = new Cpu();
    var decodedInstruction = new DecodedInstruction(0x8FB5);

    var instruction = new Instruction_8xy5(decodedInstruction);
    Assert.Throws<InvalidOperationException>(() => instruction.Execute(cpu, MockedDisplay, MockedKeyboard));
  }
}
