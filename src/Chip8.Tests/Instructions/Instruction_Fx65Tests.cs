using Chip8.Instructions;
using NUnit.Framework;
using System;

namespace Chip8.Tests.Instructions;

[TestFixture]
public class Instruction_Fx65Tests : BaseInstructionTests
{
  [TestCase((ushort)0xF065, (ushort)0x240)]
  [TestCase((ushort)0xF365, (ushort)0x340)]
  [TestCase((ushort)0xFF65, (ushort)0x440)]
  public void Executing_Instruction_Fx65_WorksAsExpected(ushort instructionCode, ushort initialI)
  {
    var cpu = new Cpu();
    var decodedInstruction = new DecodedInstruction(instructionCode);
    cpu.I = initialI;

    for (byte i = 0; i <= 0xF; i++)
    {
      cpu.Memory[i + initialI] = i;
    }

    var instruction = new Instruction_Fx65(decodedInstruction);
    instruction.Execute(cpu, MockedDisplay, MockedKeyboard);

    Assert.That(cpu.I, Is.EqualTo(initialI + decodedInstruction.x + 1));
    for (byte i = 0; i <= 0xF; i++)
    {
      var expectedValue = (i <= decodedInstruction.x) ? i : (byte)0;
      Assert.That(cpu.V[i], Is.EqualTo(expectedValue));
    }
    Assert.That(instruction.Mnemonic, Is.EqualTo($"LD V{decodedInstruction.x:X}, [I]"));
  }

  [TestCase((ushort)0xFA65, (ushort)0xFFA)]
  public void ReadFromMemory_OutsideValidMemoryRange_ThrowsException(ushort instructionCode, ushort initialI)
  {
    var cpu = new Cpu { I = initialI };
    var decodedInstruction = new DecodedInstruction(instructionCode);

    var instruction = new Instruction_Fx65(decodedInstruction);
    Assert.Throws<InvalidOperationException>(() => instruction.Execute(cpu, MockedDisplay, MockedKeyboard));
  }
}
