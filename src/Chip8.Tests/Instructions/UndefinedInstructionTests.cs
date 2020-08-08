using Chip8.Instructions;
using Moq;
using NUnit.Framework;
using System;

namespace Chip8.Tests.Instructions
{
  [TestFixture]
  public class UndefinedInstructionTests
  {
    [Test]
    public void UndefinedInstruction_IsCorrectlyInitialized()
    {
      const ushort instructionCode = 0xE012;
      var undefinedInstruction = new UndefinedInstruction(instructionCode);

      Assert.That(undefinedInstruction.Description, Is.EqualTo(string.Empty));
      Assert.That(undefinedInstruction.InstructionCode, Is.EqualTo(instructionCode));
      Assert.That(undefinedInstruction.Mnemonic, Is.EqualTo($"0x{instructionCode:X4}"));
      Assert.That(undefinedInstruction.ToString(), Is.EqualTo(undefinedInstruction.Mnemonic));
    }

    [Test]
    public void ExecutingUndefinedInstruction_ThrowsException()
    {
      var cpu = new Cpu();
      var display = new Mock<IDisplay>(MockBehavior.Strict).Object;

      Assert.Throws<InvalidOperationException>(() => new UndefinedInstruction(0x0).Execute(cpu, display));
    }
  }
}
