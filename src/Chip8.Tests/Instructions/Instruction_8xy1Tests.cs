using Chip8.Instructions;
using NUnit.Framework;

namespace Chip8.Tests.Instructions;

[TestFixture]
public class Instruction_8xy1Tests : BaseInstructionTests
{
  [TestCase(true)]
  [TestCase(false)]
  public void Executing_Instruction_8xy1_WorksAsExpected(bool allowQuirks)
  {
    var cpu = new Cpu(allowQuirks);
    var decodedInstruction = new DecodedInstruction(0x8AB1);
    const byte value1 = 0xAB;
    const byte value2 = 0xBC;
    cpu.V[decodedInstruction.x] = value1;
    cpu.V[decodedInstruction.y] = value2;
    cpu.V[0x0F] = 0x01;

    var instruction = new Instruction_8xy1(decodedInstruction);
    instruction.Execute(cpu, MockedDisplay, MockedKeyboard);

    Assert.That(cpu.V[decodedInstruction.x], Is.EqualTo(value1 | value2));
    Assert.That(cpu.V[0xF], Is.EqualTo(allowQuirks ? 0x00 : 0x01));
    Assert.That(instruction.Mnemonic, Is.EqualTo($"OR V{decodedInstruction.x:X}, V{decodedInstruction.y:X}"));
  }
}
