using Chip8.Instructions;
using Moq;
using NUnit.Framework;

namespace Chip8.Tests.Instructions
{
  [TestFixture]
  public class Instruction_5xy0Tests
  {
    [TestCase(true)]
    [TestCase(false)]
    public void Executing_Instruction_5xy0_WorksAsExpected(bool vxEqualToVy)
    {
      var cpu = new Cpu();
      var display = new Mock<IDisplay>(MockBehavior.Strict).Object;
      var decodedInstruction = new DecodedInstruction(0x5460);

      cpu.V[decodedInstruction.x] = 0x1;
      cpu.V[decodedInstruction.y] = vxEqualToVy ? (byte)0x1 : (byte)0x2;

      var instruction = new Instruction_5xy0(decodedInstruction);
      instruction.Execute(cpu, display);

      var pcOffset = (vxEqualToVy == true) ? 2 : 0;
      Assert.That(cpu.PC, Is.EqualTo(Cpu.MemoryAddressOfFirstInstruction + pcOffset));
      Assert.That(instruction.Mnemonic, Is.EqualTo($"SE V{decodedInstruction.x:X}, V{decodedInstruction.y:X}"));
    }
  }
}
