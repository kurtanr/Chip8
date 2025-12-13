using Chip8.Instructions;
using NUnit.Framework;

namespace Chip8.Tests.Instructions;

[TestFixture]
public class Instruction_3xkkTests : BaseInstructionTests
{
  [TestCase(true)]
  [TestCase(false)]
  public void Executing_Instruction_3xkk_WorksAsExpected(bool vxEqualToKk)
  {
    var cpu = new Cpu();
    var decodedInstruction = new DecodedInstruction(0x3468);

    if (vxEqualToKk)
    {
      cpu.V[decodedInstruction.x] = decodedInstruction.kk;
    }

    var instruction = new Instruction_3xkk(decodedInstruction);
    instruction.Execute(cpu, MockedDisplay, MockedKeyboard);

    var pcOffset = (vxEqualToKk == true) ? 2 : 0;
    Assert.That(cpu.PC, Is.EqualTo(Cpu.MemoryAddressOfFirstInstruction + pcOffset));
    Assert.That(instruction.Mnemonic, Is.EqualTo($"SE V{decodedInstruction.x:X}, 0x{decodedInstruction.kk:X}"));
  }
}
