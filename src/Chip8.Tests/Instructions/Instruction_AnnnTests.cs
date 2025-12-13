using Chip8.Instructions;
using NUnit.Framework;

namespace Chip8.Tests.Instructions;

[TestFixture]
public class Instruction_AnnnTests : BaseInstructionTests
{
  [Test]
  public void Executing_Instruction_Annn_WorksAsExpected()
  {
    var cpu = new Cpu();
    var decodedInstruction = new DecodedInstruction(0xA123);

    var instruction = new Instruction_Annn(decodedInstruction);
    instruction.Execute(cpu, MockedDisplay, MockedKeyboard);

    Assert.That(cpu.I, Is.EqualTo(decodedInstruction.nnn));
    Assert.That(instruction.Mnemonic, Is.EqualTo($"LD I, 0x{decodedInstruction.nnn:X}"));
  }
}
