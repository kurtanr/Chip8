﻿using Chip8.Instructions;
using Moq;
using NUnit.Framework;
using System;

namespace Chip8.Tests.Instructions
{
  [TestFixture]
  public class Instruction_Fx55Tests
  {
    [TestCase((ushort)0xFF55, (ushort)0x240)]
    [TestCase((ushort)0xF355, (ushort)0x300)]
    public void Executing_Instruction_Fx55_WorksAsExpected(ushort instructionCode, ushort initialI)
    {
      var cpu = new Cpu();
      var display = new Mock<IDisplay>(MockBehavior.Strict).Object;
      var decodedInstruction = new DecodedInstruction(instructionCode);
      cpu.I = initialI;

      for(byte i = 0; i <= 0xF; i++)
      {
        cpu.V[i] = i;
      }

      var instruction = new Instruction_Fx55(decodedInstruction);
      instruction.Execute(cpu, display);

      Assert.That(cpu.I, Is.EqualTo(initialI + decodedInstruction.x + 1));
      for (byte i = 0; i <= 0xF; i++)
      {
        byte expectedValue = (i <= decodedInstruction.x) ? (byte)i : (byte)0;
        Assert.That(cpu.Memory[i + initialI], Is.EqualTo(expectedValue));
      }
      Assert.That(instruction.Mnemonic, Is.EqualTo($"LD [I], V{decodedInstruction.x:X}"));
    }

    [TestCase((ushort)0xFA55, (ushort)0xFFA)]
    public void WriteToMemory_OutsideValidMemoryRange_ThrowsException(ushort instructionCode, ushort initialI)
    {
      var cpu = new Cpu { I = initialI };
      var display = new Mock<IDisplay>(MockBehavior.Strict).Object;
      var decodedInstruction = new DecodedInstruction(instructionCode);

      var instruction = new Instruction_Fx55(decodedInstruction);
      Assert.Throws<InvalidOperationException>(() => instruction.Execute(cpu, display));
    }
  }
}
