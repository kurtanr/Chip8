using Chip8.Instructions;
using Moq;
using NUnit.Framework;

namespace Chip8.Tests.Instructions
{
  [TestFixture]
  public class Instruction_00EETests
  {
    [Test]
    public void Executing_Instruction_00EE_WorksAsExpected()
    {
      var cpu = new Cpu();
      var display = new Mock<IDisplay>(MockBehavior.Strict).Object;
      cpu.Stack.Push(Cpu.MemoryAddressOfFirstInstruction + 10);

      var instruction = new Instruction_00EE(new DecodedInstruction(0x00EE));
      instruction.Execute(cpu, display);

      Assert.That(cpu.PC, Is.EqualTo(Cpu.MemoryAddressOfFirstInstruction + 10));
      Assert.That(cpu.Stack, Is.Empty);
      Assert.That(instruction.Mnemonic, Is.EqualTo("RET"));
    }
  }
}
