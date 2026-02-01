using Chip8.Instructions;
using NUnit.Framework;

namespace Chip8.Tests.Instructions;

[TestFixture]
public class Instruction_2nnnTests : BaseInstructionTests
{
  [Test]
  public void Executing_Instruction_2nnn_WorksAsExpected()
  {
    var cpu = new Cpu();
    cpu.Stack.Push((ushort)(Cpu.MemoryAddressOfFirstInstruction + 10));
    var decodedInstruction = new DecodedInstruction(0x2468);

    var instruction = new Instruction_2nnn(decodedInstruction);
    instruction.Execute(cpu, MockedDisplay, MockedKeyboard);

    Assert.That(cpu.Stack.Peek(), Is.EqualTo(Cpu.MemoryAddressOfFirstInstruction + 2));
    Assert.That(cpu.PC, Is.EqualTo(decodedInstruction.nnn));
    Assert.That(instruction.Mnemonic, Is.EqualTo($"CALL 0x{decodedInstruction.nnn:X}"));
  }
}
