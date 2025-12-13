using Chip8.Instructions;
using Moq;
using NUnit.Framework;

namespace Chip8.Tests.Instructions;

[TestFixture]
public class Instruction_Ex9ETests : BaseInstructionTests
{
  [Test]
  public void Executing_Instruction_Ex9E_WorksAsExpected()
  {
    var decodedInstruction = new DecodedInstruction(0xE19E);
    var cpu = new Cpu();
    cpu.V[1] = 0x2;

    var mockSequence = new MockSequence();
    var keyboardMock = new Mock<IKeyboard>(MockBehavior.Strict);
    keyboardMock.InSequence(mockSequence).Setup(x => x.GetPressedKey()).Returns((byte?)null);
    keyboardMock.InSequence(mockSequence).Setup(x => x.GetPressedKey()).Returns(1);
    keyboardMock.InSequence(mockSequence).Setup(x => x.GetPressedKey()).Returns(2);
    var keyboard = keyboardMock.Object;

    var instruction = new Instruction_Ex9E(decodedInstruction);
    Assert.That(instruction.Mnemonic, Is.EqualTo($"SKP V{decodedInstruction.x:X}"));

    var oldPC = cpu.PC;
    instruction.Execute(cpu, MockedDisplay, keyboard);
    Assert.That(cpu.PC, Is.EqualTo(oldPC));

    instruction.Execute(cpu, MockedDisplay, keyboard);
    Assert.That(cpu.PC, Is.EqualTo(oldPC));

    instruction.Execute(cpu, MockedDisplay, keyboard);
    Assert.That(cpu.PC, Is.EqualTo(oldPC + 2));

    keyboardMock.Verify(x => x.GetPressedKey(), Times.Exactly(3));
  }
}
