using Chip8.Instructions;
using Moq;
using NUnit.Framework;

namespace Chip8.Tests.Instructions
{
  [TestFixture]
  public class Instruction_2nnnTests
  {
    [Test]
    public void Executing_Instruction_2nnn_WorksAsExpected()
    {
      var cpu = new Cpu();
      var display = new Mock<IDisplay>(MockBehavior.Strict).Object;
      cpu.Stack.Push(Cpu.MemoryAddressOfFirstInstruction + 10);
      var decodedInstruction = new DecodedInstruction(0x2468);

      var instruction = new Instruction_2nnn(decodedInstruction);
      instruction.Execute(cpu, display);

      Assert.That(cpu.Stack.Peek(), Is.EqualTo(Cpu.MemoryAddressOfFirstInstruction));
      Assert.That(cpu.PC, Is.EqualTo(decodedInstruction.nnn));
      Assert.That(instruction.Mnemonic, Is.EqualTo($"CALL 0x{decodedInstruction.nnn:X}"));
    }
  }
}
