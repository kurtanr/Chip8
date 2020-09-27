using Chip8.Instructions;
using NUnit.Framework;

namespace Chip8.Tests.Instructions
{
  [TestFixture]
  public class Instruction_Fx07Tests : BaseInstructionTests
  {
    [Test]
    public void Executing_Instruction_Fx07_WorksAsExpected()
    {
      var cpu = new Cpu { DT = 0x12 };
      var decodedInstruction = new DecodedInstruction(0xFA07);

      var instruction = new Instruction_Fx07(decodedInstruction);
      instruction.Execute(cpu, MockedDisplay, MockedKeyboard);

      Assert.That(cpu.V[decodedInstruction.x], Is.EqualTo(0x12));
      Assert.That(instruction.Mnemonic, Is.EqualTo($"LD V{decodedInstruction.x:X}, DT"));
    }
  }
}
