using Chip8.Instructions;
using Moq;
using NUnit.Framework;

namespace Chip8.Tests.Instructions
{
  [TestFixture]
  public class Instruction_1nnnTests
  {
    [Test]
    public void Executing_Instruction_1nnn_WorksAsExpected()
    {
      var cpu = new Cpu();
      var display = new Mock<IDisplay>(MockBehavior.Strict).Object;
      cpu.Stack.Push(Cpu.MemoryAddressOfFirstInstruction + 10);
      ushort nnn = 0x0246;

      var instruction = new Instruction_1nnn(0x1246, nnn);
      instruction.Execute(cpu, display);

      Assert.That(cpu.PC, Is.EqualTo(nnn));
      Assert.That(instruction.Mnemonic, Is.EqualTo($"JP 0x{nnn:X}"));
    }
  }
}
