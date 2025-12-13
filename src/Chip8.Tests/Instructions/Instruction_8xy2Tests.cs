using Chip8.Instructions;
using NUnit.Framework;

namespace Chip8.Tests.Instructions;

[TestFixture]
public class Instruction_8xy2Tests : BaseInstructionTests
{
  [Test]
  public void Executing_Instruction_8xy2_WorksAsExpected()
  {
    var cpu = new Cpu();
    var decodedInstruction = new DecodedInstruction(0x8AB2);
    const byte value1 = 0xCD;
    const byte value2 = 0xEF;
    cpu.V[decodedInstruction.x] = value1;
    cpu.V[decodedInstruction.y] = value2;

    var instruction = new Instruction_8xy2(decodedInstruction);
    instruction.Execute(cpu, MockedDisplay, MockedKeyboard);

    Assert.That(cpu.V[decodedInstruction.x], Is.EqualTo(value1 & value2));
    Assert.That(instruction.Mnemonic, Is.EqualTo($"AND V{decodedInstruction.x:X}, V{decodedInstruction.y:X}"));
  }
}
