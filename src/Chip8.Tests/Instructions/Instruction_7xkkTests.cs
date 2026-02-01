using Chip8.Instructions;
using NUnit.Framework;

namespace Chip8.Tests.Instructions;

[TestFixture]
public class Instruction_7xkkTests : BaseInstructionTests
{
  [TestCase((ushort)0x7123, 0x38, 0x5B)]
  [TestCase((ushort)0x7123, 0xFE, 0x21)]
  public void Executing_Instruction_7xkk_WorksAsExpected(ushort instructionCode, byte oldVxValue, byte expectedVxValue)
  {
    var cpu = new Cpu();
    var decodedInstruction = new DecodedInstruction(instructionCode);
    cpu.V[decodedInstruction.x] = oldVxValue;

    var instruction = new Instruction_7xkk(decodedInstruction);
    instruction.Execute(cpu, MockedDisplay, MockedKeyboard);

    Assert.That(cpu.V[decodedInstruction.x], Is.EqualTo(expectedVxValue));
    Assert.That(instruction.Mnemonic, Is.EqualTo($"ADD V{decodedInstruction.x:X}, 0x{decodedInstruction.kk:X}"));
  }
}
