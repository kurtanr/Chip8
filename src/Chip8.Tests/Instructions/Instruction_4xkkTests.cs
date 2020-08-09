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
      var decodedInstruction = new DecodedInstruction(0x4468);

      if (vxEqualToKk)
      {
        cpu.V[decodedInstruction.x] = decodedInstruction.kk;
      }

      var instruction = new Instruction_4xkk(decodedInstruction);
      instruction.Execute(cpu, display);

      var pcOffset = (vxEqualToKk == true) ? 0 : 2;
      Assert.That(cpu.PC, Is.EqualTo(Cpu.MemoryAddressOfFirstInstruction + pcOffset));
      Assert.That(instruction.Mnemonic, Is.EqualTo($"SNE V{decodedInstruction.x:X}, 0x{decodedInstruction.kk:X}"));
    }
  }
}
