using Chip8.Instructions;
using Moq;
using NUnit.Framework;

namespace Chip8.Tests.Instructions;

[TestFixture]
public class Instruction_ExA1Tests : BaseInstructionTests
{
  [Test]
  public void Executing_Instruction_ExA1_WorksAsExpected()
  {
    var decodedInstruction = new DecodedInstruction(0xE2A1);
    var cpu = new Cpu();
    cpu.V[2] = 0x2;

    var mockSequence = new MockSequence();
    var keyboardMock = new Mock<IKeyboard>(MockBehavior.Strict);
    keyboardMock.InSequence(mockSequence).Setup(x => x.GetPressedKey()).Returns((byte?)null);
    keyboardMock.InSequence(mockSequence).Setup(x => x.GetPressedKey()).Returns(1);
    keyboardMock.InSequence(mockSequence).Setup(x => x.GetPressedKey()).Returns(2);
    var keyboard = keyboardMock.Object;

    var instruction = new Instruction_ExA1(decodedInstruction);
    Assert.That(instruction.Mnemonic, Is.EqualTo($"SKNP V{decodedInstruction.x:X}"));

    var oldPC = cpu.PC;
    instruction.Execute(cpu, MockedDisplay, keyboard);
    Assert.That(cpu.PC, Is.EqualTo(oldPC + 2));

    oldPC = cpu.PC;
    instruction.Execute(cpu, MockedDisplay, keyboard);
    Assert.That(cpu.PC, Is.EqualTo(oldPC + 2));

    oldPC = cpu.PC;
    instruction.Execute(cpu, MockedDisplay, keyboard);
    Assert.That(cpu.PC, Is.EqualTo(oldPC));

    keyboardMock.Verify(x => x.GetPressedKey(), Times.Exactly(3));
  }
}
