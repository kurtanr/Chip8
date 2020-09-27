using Chip8.Instructions;
using NUnit.Framework;
using System;

namespace Chip8.Tests.Instructions
{
  [TestFixture]
  public class Instruction_Fx29Tests : BaseInstructionTests
  {
    [Test]
    public void Executing_Instruction_Fx29_WorksAsExpected()
    {
      var cpu = new Cpu();
      var decodedInstruction = new DecodedInstruction(0xFA29);
      cpu.V[decodedInstruction.x] = 0x3;

      var instruction = new Instruction_Fx29(decodedInstruction);
      instruction.Execute(cpu, MockedDisplay, MockedKeyboard);

      Assert.That(cpu.I, Is.EqualTo(15));
      Assert.That(instruction.Mnemonic, Is.EqualTo($"LD F, V{decodedInstruction.x:X}"));
    }

    [Test]
    public void Executing_Instruction_Fx29_WithVxValue_GreatherThan0xF_ThrowsException()
    {
      var cpu = new Cpu();
      cpu.V[0xA] = 0x10;
      var decodedInstruction = new DecodedInstruction(0xFA29);

      var instruction = new Instruction_Fx29(decodedInstruction);
      Assert.Throws<InvalidOperationException>(() => instruction.Execute(cpu, MockedDisplay, MockedKeyboard));
    }
  }
}
