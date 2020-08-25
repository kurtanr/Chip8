using Chip8.Instructions;
using Moq;
using NUnit.Framework;
using System;

namespace Chip8.Tests.Instructions
{
  [TestFixture]
  public class Instruction_Fx1ETests
  {
    [TestCase(0xFF, (ushort)0xF00, (ushort)0xFFF)]
    public void Executing_Instruction_Fx1E_WorksAsExpected(byte vxValue, ushort iValue, ushort expectedIValue)
    {
      var cpu = new Cpu { I = iValue };
      var display = new Mock<IDisplay>(MockBehavior.Strict).Object;
      var decodedInstruction = new DecodedInstruction(0xFA1E);
      cpu.V[decodedInstruction.x] = vxValue;

      var instruction = new Instruction_Fx1E(decodedInstruction);
      instruction.Execute(cpu, display);

      Assert.That(cpu.I, Is.EqualTo(expectedIValue));
      Assert.That(instruction.Mnemonic, Is.EqualTo($"ADD I, V{decodedInstruction.x:X}"));
    }

    [TestCase(0xFF, (ushort)0xF01)]
    public void Setting_IRegister_OutsideValidMemoryRange_ThrowsException(byte vxValue, ushort iValue)
    {
      var cpu = new Cpu { I = iValue };
      var display = new Mock<IDisplay>(MockBehavior.Strict).Object;
      var decodedInstruction = new DecodedInstruction(0xFA1E);
      cpu.V[decodedInstruction.x] = vxValue;

      var instruction = new Instruction_Fx1E(decodedInstruction);
      Assert.Throws<InvalidOperationException>(() => instruction.Execute(cpu, display));
    }
  }
}
