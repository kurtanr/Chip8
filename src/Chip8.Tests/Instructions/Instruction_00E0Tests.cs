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

      new Instruction_00E0(0x00E0).Execute(cpu, displayMock.Object);

      displayMock.Verify(x => x.Clear(), Times.Once);
    }
  }
}
