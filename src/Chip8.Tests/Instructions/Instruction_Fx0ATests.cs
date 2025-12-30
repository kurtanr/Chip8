using Chip8.Instructions;
using Moq;
using NUnit.Framework;

namespace Chip8.Tests.Instructions;

[TestFixture]
public class Instruction_Fx0ATests : BaseInstructionTests
{
  [TestCase(0x3)]
  [TestCase(null)]
  public void Executing_Instruction_Fx0A_WorksAsExpected(byte? pressedKey)
  {
    var decodedInstruction = new DecodedInstruction(0xF10A);
    var cpu = new Cpu { PC = (ushort)(Cpu.MemoryAddressOfFirstInstruction ) };

    var keyboardMock = new Mock<IKeyboard>(MockBehavior.Strict);
    keyboardMock.Setup(x => x.WaitForKeyPressAndRelease()).Returns(pressedKey);

    var keyboard = keyboardMock.Object;

    var instruction = new Instruction_Fx0A(decodedInstruction);
    Assert.That(instruction.Mnemonic, Is.EqualTo($"LD V{decodedInstruction.x:X}, K"));

    var oldPC = cpu.PC;
    var expectedPC = (pressedKey == null ? oldPC : (ushort)(oldPC + 2));
    var expectedVx = (pressedKey == null ? 0 : pressedKey);

    instruction.Execute(cpu, MockedDisplay, keyboard);

    Assert.That(cpu.PC, Is.EqualTo(expectedPC));
    Assert.That(cpu.V[decodedInstruction.x], Is.EqualTo(expectedVx));
  }
}
