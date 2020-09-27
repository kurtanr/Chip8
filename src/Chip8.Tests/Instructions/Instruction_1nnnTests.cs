using Chip8.Instructions;
using NUnit.Framework;

namespace Chip8.Tests.Instructions
{
  [TestFixture]
  public class Instruction_1nnnTests : BaseInstructionTests
  {
    [Test]
    public void Executing_Instruction_1nnn_WorksAsExpected()
    {
      var cpu = new Cpu();
      cpu.Stack.Push((ushort)(Cpu.MemoryAddressOfFirstInstruction + 10));
      var decodedInstruction = new DecodedInstruction(0x1246);

      var instruction = new Instruction_1nnn(decodedInstruction);
      instruction.Execute(cpu, MockedDisplay, MockedKeyboard);

      Assert.That(cpu.PC, Is.EqualTo(decodedInstruction.nnn));
      Assert.That(instruction.Mnemonic, Is.EqualTo($"JP 0x{decodedInstruction.nnn:X}"));
    }
  }
}
