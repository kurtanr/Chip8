using Chip8.Instructions;
using NUnit.Framework;
using System;

namespace Chip8.Tests.Instructions;

[TestFixture]
public class Instruction_8xy5Tests : BaseInstructionTests
{
  [TestCase(0xFE, 0x01, 0xFD, 0x01)]
  [TestCase(0x01, 0x02, 0xFF, 0x00)]
  [TestCase(0x02, 0x02, 0x00, 0x01)]
  public void Executing_Instruction_8xy5_WorksAsExpected(
    byte vx, byte vy, byte expectedResult, byte expectedVF)
  {
    var cpu = new Cpu();
    var decodedInstruction = new DecodedInstruction(0x8AB5);
    cpu.V[decodedInstruction.x] = vx;
    cpu.V[decodedInstruction.y] = vy;

    var instruction = new Instruction_8xy5(decodedInstruction);
    instruction.Execute(cpu, MockedDisplay, MockedKeyboard);

    Assert.That(cpu.V[decodedInstruction.x], Is.EqualTo(expectedResult));
    Assert.That(cpu.V[0xF], Is.EqualTo(expectedVF));
    Assert.That(instruction.Mnemonic, Is.EqualTo($"SUB V{decodedInstruction.x:X}, V{decodedInstruction.y:X}"));
  }

  [Test]
  public void Executing_Instruction_8xy5_WithVx_SetToVF_WithQuirksNotAllowed_ThrowsException()
  {
    var cpu = new Cpu();
    var decodedInstruction = new DecodedInstruction(0x8FB5);

    var instruction = new Instruction_8xy5(decodedInstruction);
    Assert.Throws<InvalidOperationException>(() => instruction.Execute(cpu, MockedDisplay, MockedKeyboard));
  }

  [TestCase(0xFE, 0x01, 0x01, 0x01)]
  [TestCase(0x01, 0xFE, 0x00, 0x00)]
  [TestCase(0x02, 0x02, 0x01, 0x01)]
  public void Executing_Instruction_8xy5_WithVx_SetToVF_WithQuirksAllowed_WorksAsExpected(
    byte vx, byte vy, byte expectedResult, byte expectedVF)
  {
    var cpu = new Cpu(true);
    var decodedInstruction = new DecodedInstruction(0x8FB5);
    cpu.V[decodedInstruction.x] = vx;
    cpu.V[decodedInstruction.y] = vy;

    var instruction = new Instruction_8xy5(decodedInstruction);
    instruction.Execute(cpu, MockedDisplay, MockedKeyboard);

    Assert.That(cpu.V[decodedInstruction.x], Is.EqualTo(expectedResult));
    Assert.That(cpu.V[0xF], Is.EqualTo(expectedResult));
  }
}
