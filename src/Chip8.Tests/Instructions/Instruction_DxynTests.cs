using Chip8.Instructions;
using Moq;
using NUnit.Framework;

namespace Chip8.Tests.Instructions
{
  [TestFixture]
  public class Instruction_DxynTests : BaseInstructionTests
  {
    [Test]
    public void Executing_Instruction_Dxyn_WorksAsExpected()
    {
      const byte xCoordinate = 0x5, yCoordinate = 0x6;
      var cpu = new Cpu();
      cpu.V[2] = xCoordinate;
      cpu.V[3] = yCoordinate;
      cpu.I = 0x300;
      cpu.Memory[cpu.I] = 0x1;

      var decodedInstruction = new DecodedInstruction(0xD231);

      // Expected execution result:
      // - decodedInstruction.n bytes (1) are read from memory, starting at location I (0x300)
      // - those bytes are written to screen, starting at location (V2, V3) = 5,6
      // - since value at 0x300 is 0x1, and 1 is the least significant bit of that byte, then location where 
      //   pixel will be set is: xCoordinate + 7 (e.g. for value of 2, it would be xCoordinate + 6)
      // - re-executing the same command will reset the pixel and set VF register to 1
      var mockSequence = new MockSequence();
      var displayMock = new Mock<IDisplay>(MockBehavior.Strict);

      // First execution
      for(byte i = 0; i < 8; i++)
      {
        displayMock.InSequence(mockSequence).Setup(x => x.GetPixel(
          (byte)(xCoordinate + i), yCoordinate)).Returns(false);
      }
      displayMock.InSequence(mockSequence).Setup(x => x.SetPixel(xCoordinate + 7, yCoordinate));

      // Second execution
      for (byte i = 0; i < 8; i++)
      {
        displayMock.InSequence(mockSequence).Setup(x => x.GetPixel(
          (byte)(xCoordinate + i), yCoordinate)).Returns(i == 7);
      }
      displayMock.InSequence(mockSequence).Setup(x => x.ClearPixel(xCoordinate + 7, yCoordinate));

      var display = displayMock.Object;
      var instruction = new Instruction_Dxyn(decodedInstruction);

      instruction.Execute(cpu, display, MockedKeyboard);
      Assert.That(cpu.V[0xF], Is.EqualTo(0));

      instruction.Execute(cpu, display, MockedKeyboard);
      Assert.That(cpu.V[0xF], Is.EqualTo(1));

      displayMock.VerifyAll();
      Assert.That(instruction.Mnemonic, Is.EqualTo($"DRW V{decodedInstruction.x:X}, V{decodedInstruction.y:X}, {decodedInstruction.n:X}"));
    }

    [Test]
    public void Executing_Instruction_Dxy0_DoesNotUseDisplay()
    {
      var cpu = new Cpu();
      var decodedInstruction = new DecodedInstruction(0xD230);

      var instruction = new Instruction_Dxyn(decodedInstruction);
      instruction.Execute(cpu, MockedDisplay, MockedKeyboard);
    }
  }
}
