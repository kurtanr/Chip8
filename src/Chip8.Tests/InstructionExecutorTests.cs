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
    private readonly IKeyboard _keyboard = new Mock<IKeyboard>(MockBehavior.Strict).Object;

    [Test]
    public void Constructor_ArgumentValidation_Works()
    {
      Assert.DoesNotThrow(() => new InstructionExecutor(_cpu, _display, _keyboard));
      Assert.Throws<ArgumentNullException>(() => new InstructionExecutor(null, _display, _keyboard));
      Assert.Throws<ArgumentNullException>(() => new InstructionExecutor(_cpu, null, _keyboard));
      Assert.Throws<ArgumentNullException>(() => new InstructionExecutor(_cpu, _display, null));
    }

    [Test]
    public void LoadApplication_NullArgumentValidation_Works()
    {
      var instructionExecutor = new InstructionExecutor(_cpu, _display, _keyboard);
      Assert.Throws<ArgumentNullException>(() => instructionExecutor.LoadApplication(null));
    }

    [Test]
    public void LoadApplication_WithMaxSize_Works()
    {
      var maxAllowedAppSize = Cpu.MemorySizeInBytes - Cpu.MemoryAddressOfFirstInstruction;
      var application = new byte[maxAllowedAppSize];
      application[0] = 0xAB;
      application[application.Length - 1] = 0xCD;

      var cpu = new Cpu();
      var instructionExecutor = new InstructionExecutor(cpu, _display, _keyboard);

      Assert.DoesNotThrow(() => instructionExecutor.LoadApplication(application));
      Assert.That(cpu.Memory[Cpu.MemoryAddressOfFirstInstruction], Is.EqualTo(0xAB));
      Assert.That(cpu.Memory[Cpu.MemorySizeInBytes - 1], Is.EqualTo(0xCD));
    }

    [Test]
    public void LoadApplication_WhichExceedsMaxSize_ThrowsException()
    {
      var maxAllowedAppSizeExceeded = Cpu.MemorySizeInBytes - Cpu.MemoryAddressOfFirstInstruction + 1;
      var application = new byte[maxAllowedAppSizeExceeded];

      var cpu = new Cpu();
      var instructionExecutor = new InstructionExecutor(cpu, _display, _keyboard);

      Assert.Throws<ArgumentException>(() => instructionExecutor.LoadApplication(application));
    }

    [Test]
    public void ExecutingInstruction_WithEmptyMemory_ThrowsException()
    {
      var instructionExecutor = new InstructionExecutor(_cpu, _display, _keyboard);

      Assert.Throws<InvalidOperationException>(() => instructionExecutor.ExecuteSingleInstruction());
    }

    [Test]
    public void ExecutingInstruction_WhichDoesNotModifyPc_IncreasesPcBy2()
    {
      var cpu = new Cpu();
      cpu.Memory[Cpu.MemoryAddressOfFirstInstruction + 1] = 0xE0; // CLS instruction

      Assert.That(cpu.PC, Is.EqualTo(Cpu.MemoryAddressOfFirstInstruction));

      var instructionExecutor = new InstructionExecutor(cpu, _display, _keyboard);
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

      var instructionExecutor = new InstructionExecutor(cpu, _display, _keyboard);
      var instruction = instructionExecutor.ExecuteSingleInstruction();

      Assert.That(instruction, Is.InstanceOf<Instruction_00EE>());
      Assert.That(cpu.PC, Is.EqualTo(Cpu.MemoryAddressOfFirstInstruction));
    }
  }
}
