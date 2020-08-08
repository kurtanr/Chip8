using Chip8.Instructions;
using Moq;
using NUnit.Framework;

namespace Chip8.Tests.Instructions
{
  [TestFixture]
  public class Instruction_4xkkTests
  {
    [TestCase(true)]
    [TestCase(false)]
    public void Executing_Instruction_4xkk_WorksAsExpected(bool vxEqualToKk)
    {
      var cpu = new Cpu();
      var display = new Mock<IDisplay>(MockBehavior.Strict).Object;
      const byte x = 0x4;
      const byte kk = 0x68;

      if (vxEqualToKk)
      {
        cpu.V[x] = kk;
      }

      ushort instructionCode = (0x3 << 12) | (x << 8) | kk;
      var instruction = new Instruction_4xkk(instructionCode, x, kk);
      instruction.Execute(cpu, display);

      var pcOffset = (vxEqualToKk == true) ? 0 : 2;
      Assert.That(cpu.PC, Is.EqualTo(Cpu.MemoryAddressOfFirstInstruction + pcOffset));
      Assert.That(instruction.Mnemonic, Is.EqualTo($"SNE V{x:X}, 0x{kk:X}"));
    }
  }
}
