using Chip8.Instructions;
using Moq;
using NUnit.Framework;

namespace Chip8.Tests.Instructions;

[TestFixture]
public class Instruction_Fx0ATests : BaseInstructionTests
{
  [Test]
  public void Executing_Instruction_Fx0A_WorksAsExpected()
  {
    var decodedInstruction = new DecodedInstruction(0xF10A);
    var cpu = new Cpu { PC = (ushort)(Cpu.MemoryAddressOfFirstInstruction ) };
    byte pressedKey = 0x3;

    var keyboardMock = new Mock<IKeyboard>(MockBehavior.Strict);
    keyboardMock.Setup(x => x.WaitForKeyPress()).Returns(pressedKey);

    var keyboard = keyboardMock.Object;

    var instruction = new Instruction_Fx0A(decodedInstruction);
    Assert.That(instruction.Mnemonic, Is.EqualTo($"LD V{decodedInstruction.x:X}, K"));

    var oldPC = cpu.PC;
    instruction.Execute(cpu, MockedDisplay, keyboard);

    Assert.That(cpu.PC, Is.EqualTo(oldPC));
    Assert.That(cpu.V[decodedInstruction.x], Is.EqualTo(pressedKey));
  }
}
