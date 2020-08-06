using Chip8.Instructions;
using Moq;
using NUnit.Framework;
using System;

namespace Chip8.Tests
{
  [TestFixture]
  public class InstructionExecutorTests
  {
    private readonly Cpu _cpu = new Cpu();
    private readonly IDisplay _display = new Mock<IDisplay>(MockBehavior.Loose).Object;
    private readonly InstructionDecoder _instructionDecoder = new InstructionDecoder();

    [Test]
    public void ArgumentValidation_Works()
    {
      Assert.DoesNotThrow(() => new InstructionExecutor(_cpu, _display, _instructionDecoder));
      Assert.Throws<ArgumentNullException>(() => new InstructionExecutor(null, _display, _instructionDecoder));
      Assert.Throws<ArgumentNullException>(() => new InstructionExecutor(_cpu, null, _instructionDecoder));
      Assert.Throws<ArgumentNullException>(() => new InstructionExecutor(_cpu, _display, null));
    }

    [Test]
    public void ExecutingInstruction_WithEmptyMemory_ThrowsException()
    {
      var instructionExecutor = new InstructionExecutor(_cpu, _display, _instructionDecoder);

      Assert.Throws<InvalidOperationException>(() => instructionExecutor.ExecuteSingleInstruction());
    }

    [Test]
    public void ExecutingInstruction_WhichDoesNotModifyPc_IncreasesPcBy2()
    {
      var cpu = new Cpu();
      cpu.Memory[Cpu.MemoryAddressOfFirstInstruction + 1] = 0xE0; // CLS instruction

      Assert.That(cpu.PC, Is.EqualTo(Cpu.MemoryAddressOfFirstInstruction));

      var instructionExecutor = new InstructionExecutor(cpu, _display, _instructionDecoder);
      var instruction = instructionExecutor.ExecuteSingleInstruction();

      Assert.That(instruction, Is.InstanceOf<Instruction_00E0>());
      Assert.That(cpu.PC, Is.EqualTo(Cpu.MemoryAddressOfFirstInstruction + 2));
    }

    [Test]
    public void ExecutingInstruction_WhichModifiesPc_DoesNotAdditionallyIncreasePcBy2()
    {
      var cpu = new Cpu();
      cpu.Stack.Push(Cpu.MemoryAddressOfFirstInstruction);
      cpu.Memory[Cpu.MemoryAddressOfFirstInstruction + 1] = 0xEE; // RET instruction

      Assert.That(cpu.PC, Is.EqualTo(Cpu.MemoryAddressOfFirstInstruction));

      var instructionExecutor = new InstructionExecutor(cpu, _display, _instructionDecoder);
      var instruction = instructionExecutor.ExecuteSingleInstruction();

      Assert.That(instruction, Is.InstanceOf<Instruction_00EE>());
      Assert.That(cpu.PC, Is.EqualTo(Cpu.MemoryAddressOfFirstInstruction));
    }
  }
}
