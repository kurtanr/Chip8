using Chip8.Instructions;
using NUnit.Framework;

namespace Chip8.Tests.Instructions;

[TestFixture]
public class Instruction_Fx18Tests : BaseInstructionTests
{
  [Test]
  public void Executing_Instruction_Fx18_WorksAsExpected()
  {
    var cpu = new Cpu();
    var decodedInstruction = new DecodedInstruction(0xFA18);
    cpu.V[decodedInstruction.x] = 0x13;

    var instruction = new Instruction_Fx18(decodedInstruction);
    instruction.Execute(cpu, MockedDisplay, MockedKeyboard);

    Assert.That(cpu.ST, Is.EqualTo(0x13));
    Assert.That(instruction.Mnemonic, Is.EqualTo($"LD ST, V{decodedInstruction.x:X}"));
  }
}
