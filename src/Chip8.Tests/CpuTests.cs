using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;

namespace Chip8.Tests;

[TestFixture]
public class CpuTests
{
  [Test]
  public void Reset_CorrectlyResets_RegistersTimersAndProgramCounter()
  {
    var cpu = new Cpu
    {
      DT = 1,
      I = 1,
      PC = (ushort)(Cpu.MemoryAddressOfFirstInstruction + 6),
      ST = 1
    };
    for (byte i = 0; i < cpu.V.Length; i++)
    {
      cpu.V[i] = i;
    }

    cpu.Reset();

    Assert.That(cpu.DT, Is.EqualTo(0));
    Assert.That(cpu.I, Is.EqualTo(0));
    Assert.That(cpu.PC, Is.EqualTo(Cpu.MemoryAddressOfFirstInstruction));
    Assert.That(cpu.ST, Is.EqualTo(0));
    CollectionAssert.AreEqual(new byte[cpu.V.Length], cpu.V);
  }

  [Test]
  public void Reset_CorrectlyResets_StackAndMemory()
  {
    var cpu = new Cpu();
    cpu.Stack.Push(1);
    cpu.Memory[0] = 1;
    cpu.Memory[Cpu.MemorySizeInBytes - 1] = 1;

    cpu.Reset();

    Assert.That(cpu.Stack, Is.Empty);
    CollectionAssert.AreEqual(new ushort[Cpu.MemorySizeInBytes], cpu.Memory);
  }

  [Test]
  public void PushToFullStack_ThrowsException()
  {
    var cpu = new Cpu();

    for (ushort i = 0; i < Cpu.MaxStackDepth; i++)
    {
      cpu.Stack.Push(i);
    }

    Assert.That(cpu.Stack.Count, Is.EqualTo(Cpu.MaxStackDepth));
    Assert.Throws<InvalidOperationException>(() => cpu.Stack.Push(1));
  }

  [Test]
  public void PopFromEmptyStack_ThrowsException()
  {
    var cpu = new Cpu();

    Assert.Throws<InvalidOperationException>(() => cpu.Stack.Pop());
  }

  [Test]
  public void SettingProgramCounterOutsideOfValidRange_ThrowsException()
  {
    var cpu = new Cpu();

    Assert.DoesNotThrow(() => cpu.PC = Cpu.MemoryAddressOfFirstInstruction);
    Assert.DoesNotThrow(() => cpu.PC = Cpu.MemoryAddressOfLastInstruction);

    Assert.Throws<InvalidOperationException>(() => cpu.PC = (ushort)(Cpu.MemoryAddressOfFirstInstruction - 1));
    Assert.Throws<InvalidOperationException>(() => cpu.PC = (ushort)(Cpu.MemoryAddressOfLastInstruction + 1));
  }
}
