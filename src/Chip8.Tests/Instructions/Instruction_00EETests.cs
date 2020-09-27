using Chip8.Instructions;
using NUnit.Framework;

namespace Chip8.Tests.Instructions
{
  [TestFixture]
  public class Instruction_00EETests : BaseInstructionTests
  {
    [Test]
    public void Executing_Instruction_00EE_WorksAsExpected()
    {
      var cpu = new Cpu();
      cpu.Stack.Push((ushort)(Cpu.MemoryAddressOfFirstInstruction + 10));

      var instruction = new Instruction_00EE(new DecodedInstruction(0x00EE));
      instruction.Execute(cpu, MockedDisplay, MockedKeyboard);

      Assert.That(cpu.PC, Is.EqualTo(Cpu.MemoryAddressOfFirstInstruction + 10));
      Assert.That(cpu.Stack, Is.Empty);
      Assert.That(instruction.Mnemonic, Is.EqualTo("RET"));
    }
  }
}
