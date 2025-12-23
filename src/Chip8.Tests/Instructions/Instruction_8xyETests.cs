using Chip8.Instructions;
using NUnit.Framework;
using System;

namespace Chip8.Tests.Instructions;

[TestFixture]
public class Instruction_8xyETests : BaseInstructionTests
{
  [TestCase(0x01, 0x02, 0x00)]
  [TestCase(0xFF, 0xFE, 0x01)]
  public void Executing_Instruction_8xyE_WorksAsExpected(
    byte vy, byte expectedResult, byte expectedVF)
  {
    var cpu = new Cpu();
    var decodedInstruction = new DecodedInstruction(0x8ABE);
    cpu.V[decodedInstruction.y] = vy;

    var instruction = new Instruction_8xyE(decodedInstruction);
    instruction.Execute(cpu, MockedDisplay, MockedKeyboard);

    Assert.That(cpu.V[decodedInstruction.x], Is.EqualTo(expectedResult));
    Assert.That(cpu.V[0xF], Is.EqualTo(expectedVF));
    Assert.That(instruction.Mnemonic, Is.EqualTo($"SHL V{decodedInstruction.x:X}, V{decodedInstruction.y:X}"));
  }

  [Test]
  public void Executing_Instruction_8xyE_WithVx_SetToVF_WithQuirksNotAllowed_ThrowsException()
  {
    var cpu = new Cpu();
    var decodedInstruction = new DecodedInstruction(0x8FBE);

    var instruction = new Instruction_8xyE(decodedInstruction);
    Assert.Throws<InvalidOperationException>(() => instruction.Execute(cpu, MockedDisplay, MockedKeyboard));
  }

  [TestCase(0x01, 0x00, 0x00)]
  [TestCase(0xFF, 0x01, 0x01)]
  public void Executing_Instruction_8xyE_WithVx_SetToVF_WithQuirksAllowed_WorksAsExpected(
    byte vy, byte expectedResult, byte expectedVF)
  {
    var cpu = new Cpu(true);
    var decodedInstruction = new DecodedInstruction(0x8FBE);
    cpu.V[decodedInstruction.y] = vy;

    var instruction = new Instruction_8xyE(decodedInstruction);
    instruction.Execute(cpu, MockedDisplay, MockedKeyboard);

    Assert.That(cpu.V[decodedInstruction.x], Is.EqualTo(expectedResult));
    Assert.That(cpu.V[0xF], Is.EqualTo(expectedVF));
  }
}
