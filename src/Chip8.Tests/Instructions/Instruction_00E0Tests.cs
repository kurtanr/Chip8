using Chip8.Instructions;
using Moq;
using NUnit.Framework;

namespace Chip8.Tests.Instructions
{
  [TestFixture]
  public class Instruction_00E0Tests
  {
    [Test]
    public void Executing_Instruction_00E0_WorksAsExpected()
    {
      var cpu = new Cpu();
      var displayMock = new Mock<IDisplay>(MockBehavior.Strict);
      displayMock.Setup(x => x.Clear());

      var instruction = new Instruction_00E0(new DecodedInstruction(0x00E0));
      instruction.Execute(cpu, displayMock.Object);

      displayMock.Verify(x => x.Clear(), Times.Once);
      Assert.That(instruction.Mnemonic, Is.EqualTo("CLS"));
      Assert.That(instruction.ToString(), Is.EqualTo("CLS"));
      Assert.That(instruction.ToStringWithDescription(), Is.EqualTo($"{"CLS".PadRight(18)} // Clear the display."));
    }
  }
}
