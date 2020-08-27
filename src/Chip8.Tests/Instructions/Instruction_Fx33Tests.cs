using Chip8.Instructions;
using Moq;
using NUnit.Framework;
using System;

namespace Chip8.Tests.Instructions
{
  [TestFixture]
  public class Instruction_Fx33Tests
  {
    [TestCase(0xFE, (ushort)0xABC, 2, 5, 4)]
    public void Executing_Instruction_Fx33_WorksAsExpected(byte vx, ushort initialIValue, byte expectedHundreds, byte expectedTens, byte expectedOnes)
    {
      var cpu = new Cpu { I = initialIValue };
      var display = new Mock<IDisplay>(MockBehavior.Strict).Object;
      var decodedInstruction = new DecodedInstruction(0xFA33);
      cpu.V[decodedInstruction.x] = vx;

      var instruction = new Instruction_Fx33(decodedInstruction);
      instruction.Execute(cpu, display);

      Assert.That(cpu.Memory[initialIValue], Is.EqualTo(expectedHundreds));
      Assert.That(cpu.Memory[initialIValue + 1], Is.EqualTo(expectedTens));
      Assert.That(cpu.Memory[initialIValue + 2], Is.EqualTo(expectedOnes));
      Assert.That(cpu.I, Is.EqualTo(initialIValue + 2));
      Assert.That(instruction.Mnemonic, Is.EqualTo($"LD B, V{decodedInstruction.x:X}"));
    }

    [TestCase((ushort)0xFFE)]
    public void WriteToMemory_OutsideValidMemoryRange_ThrowsException(ushort initialI)
    {
      var cpu = new Cpu { I = initialI };
      var display = new Mock<IDisplay>(MockBehavior.Strict).Object;
      var decodedInstruction = new DecodedInstruction(0xFA33);

      var instruction = new Instruction_Fx33(decodedInstruction);
      Assert.Throws<InvalidOperationException>(() => instruction.Execute(cpu, display));
    }
  }
}
