﻿using Chip8.Instructions;
using Moq;
using NUnit.Framework;
using System;

namespace Chip8.Tests.Instructions
{
  [TestFixture]
  public class Instruction_8xy6Tests
  {
    [TestCase(0x03, 0x01, 1)]
    [TestCase(0x02, 0x01, 0)]
    public void Executing_Instruction_8xy6_WorksAsExpected(byte vy, byte expectedResult, byte expectedVF)
    {
      var cpu = new Cpu();
      var display = new Mock<IDisplay>(MockBehavior.Strict).Object;
      var decodedInstruction = new DecodedInstruction(0x8AB6);
      cpu.V[decodedInstruction.y] = vy;

      var instruction = new Instruction_8xy6(decodedInstruction);
      instruction.Execute(cpu, display);

      Assert.That(cpu.V[decodedInstruction.x], Is.EqualTo(expectedResult));
      Assert.That(cpu.V[0xF], Is.EqualTo(expectedVF));
      Assert.That(instruction.Mnemonic, Is.EqualTo($"SHR V{decodedInstruction.x:X}, V{decodedInstruction.y:X}"));
    }

    [Test]
    public void Executing_Instruction_8xy6_WithVx_SetToVF_ThrowsException()
    {
      var cpu = new Cpu();
      var display = new Mock<IDisplay>(MockBehavior.Strict).Object;
      var decodedInstruction = new DecodedInstruction(0x8FB6);

      var instruction = new Instruction_8xy6(decodedInstruction);
      Assert.Throws<InvalidOperationException>(() => instruction.Execute(cpu, display));
    }
  }
}
