using Chip8.Instructions;
using Moq;
using NUnit.Framework;
using System;

namespace Chip8.Tests.Instructions
{
  [TestFixture]
  public class Instruction_BnnnTests
  {
    [TestCase((ushort)0xB200, 0x2, (ushort)0x202)]
    public void Executing_Instruction_Bnnn_WorksAsExpected(ushort instructionCode, byte v0, ushort expectedPcValue)
    {
      var cpu = new Cpu();
      var display = new Mock<IDisplay>(MockBehavior.Strict).Object;
      var decodedInstruction = new DecodedInstruction(instructionCode);
      cpu.V[0] = v0;

      var instruction = new Instruction_Bnnn(decodedInstruction);
      instruction.Execute(cpu, display);

      Assert.That(cpu.PC, Is.EqualTo(expectedPcValue));
      Assert.That(instruction.Mnemonic, Is.EqualTo($"JP V0, 0x{decodedInstruction.nnn:X}"));
    }

    [TestCase((ushort)0xBFFE, 0x2)]
    [TestCase((ushort)0xBFFE, 0xFF)]
    public void Jumping_OutsideValidMemoryRange_ThrowsException(ushort instructionCode, byte v0)
    {
      var cpu = new Cpu();
      var display = new Mock<IDisplay>(MockBehavior.Strict).Object;
      var decodedInstruction = new DecodedInstruction(instructionCode);
      cpu.V[0] = v0;

      var instruction = new Instruction_Bnnn(decodedInstruction);
      Assert.Throws<InvalidOperationException>(() => instruction.Execute(cpu, display));
    }
  }
}
