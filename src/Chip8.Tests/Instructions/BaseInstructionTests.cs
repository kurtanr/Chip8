using Moq;

namespace Chip8.Tests.Instructions;

public abstract class BaseInstructionTests
{
  protected readonly IDisplay MockedDisplay = new Mock<IDisplay>(MockBehavior.Strict).Object;
  protected readonly IKeyboard MockedKeyboard = new Mock<IKeyboard>(MockBehavior.Strict).Object;
}
