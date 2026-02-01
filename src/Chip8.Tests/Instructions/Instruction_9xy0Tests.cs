using Chip8.Instructions;
using NUnit.Framework;

namespace Chip8.Tests.Instructions;

[TestFixture]
public class Instruction_9xy0Tests : BaseInstructionTests
{
  [TestCase(true)]
  [TestCase(false)]
  public void Executing_Instruction_9xy0_WorksAsExpected(bool vxEqualToVy)
  {
    var cpu = new Cpu();
    var decodedInstruction = new DecodedInstruction(0x9460);

    cpu.V[decodedInstruction.x] = 0x1;
    cpu.V[decodedInstruction.y] = vxEqualToVy ? (byte)0x1 : (byte)0x2;

    var instruction = new Instruction_9xy0(decodedInstruction);
    instruction.Execute(cpu, MockedDisplay, MockedKeyboard);

    var pcOffset = vxEqualToVy ? 0 : 2;
    Assert.That(cpu.PC, Is.EqualTo(Cpu.MemoryAddressOfFirstInstruction + pcOffset));
    Assert.That(instruction.Mnemonic, Is.EqualTo($"SNE V{decodedInstruction.x:X}, V{decodedInstruction.y:X}"));
  }
}
