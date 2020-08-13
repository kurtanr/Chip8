using Chip8.Instructions;
using Moq;
using NUnit.Framework;
using System;

namespace Chip8.Tests.Instructions
{
  [TestFixture]
  public class Instruction_CxkkTests
  {
    [TestCase((ushort)0xC101, 0x01)]
    [TestCase((ushort)0xC123, 0x23)]
    public void Executing_Instruction_Cxkk_WorksAsExpected(ushort instructionCode, byte expectedMaxValue)
    {
      var cpu = new Cpu();
      var display = new Mock<IDisplay>(MockBehavior.Strict).Object;
      var decodedInstruction = new DecodedInstruction(instructionCode);

      var instruction = new Instruction_Cxkk(decodedInstruction);
      instruction.Execute(cpu, display);

      Assert.That(cpu.V[decodedInstruction.x], Is.LessThanOrEqualTo(decodedInstruction.kk));
      Assert.That(instruction.Mnemonic, Is.EqualTo($"RND V{decodedInstruction.x:X}, 0x{decodedInstruction.kk:X}"));
    }
  }
}
