using Chip8.Instructions;
using Moq;
using NUnit.Framework;

namespace Chip8.Tests.Instructions;

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
    // - executing the same command again will set the pixel and reset VF register to 0
    var mockSequence = new MockSequence();
    var displayMock = new Mock<IDisplay>(MockBehavior.Strict);

    // First execution
    for (byte i = 0; i < 8; i++)
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

    // Third execution
    for (byte i = 0; i < 8; i++)
    {
      displayMock.InSequence(mockSequence).Setup(x => x.GetPixel(
        (byte)(xCoordinate + i), yCoordinate)).Returns(false);
    }
    displayMock.InSequence(mockSequence).Setup(x => x.SetPixel(xCoordinate + 7, yCoordinate));

    var display = displayMock.Object;
    var instruction = new Instruction_Dxyn(decodedInstruction);

    instruction.Execute(cpu, display, MockedKeyboard);
    Assert.That(cpu.V[0xF], Is.EqualTo(0));

    instruction.Execute(cpu, display, MockedKeyboard);
    Assert.That(cpu.V[0xF], Is.EqualTo(1));

    instruction.Execute(cpu, display, MockedKeyboard);
    Assert.That(cpu.V[0xF], Is.EqualTo(0));

    displayMock.VerifyAll();
    Assert.That(instruction.Mnemonic, Is.EqualTo($"DRW V{decodedInstruction.x:X}, V{decodedInstruction.y:X}, 0x{decodedInstruction.n:X}"));
  }

  [Test]
  public void Executing_Instruction_Dxyn_WithWrapping_WorksCorrectly()
  {
    var cpu = new Cpu();
    cpu.V[0] = 62; // Near right edge
    cpu.V[1] = 31; // Bottom edge
    cpu.I = 0x300;
    cpu.Memory[cpu.I] = 0xFF; // All bits set

    var decodedInstruction = new DecodedInstruction(0xD011); // 1 byte sprite
    var displayMock = new Mock<IDisplay>(MockBehavior.Strict);

    // Should wrap around X: pixels at x=62,63 then x=0,1
    // Y doesn't wrap because we only have 1 byte, so y stays at 31
    for (byte i = 0; i < 8; i++)
    {
      var expectedX = (byte)((62 + i) % 64);
      displayMock.Setup(x => x.GetPixel(expectedX, 31)).Returns(false);
      displayMock.Setup(x => x.SetPixel(expectedX, 31));
    }

    var instruction = new Instruction_Dxyn(decodedInstruction);
    instruction.Execute(cpu, displayMock.Object, new Mock<IKeyboard>().Object);

    displayMock.VerifyAll();
  }

  [Test]
  public void Executing_Instruction_Dxyn_WithMultipleBytes_WorksCorrectly()
  {
    var cpu = new Cpu();
    cpu.V[0] = 0;
    cpu.V[1] = 0;
    cpu.I = 0x300;
    cpu.Memory[0x300] = 0xF0; // 1111 0000, 240
    cpu.Memory[0x301] = 0x90; // 1001 0000, 144

    var decodedInstruction = new DecodedInstruction(0xD012); // 2 bytes
    var displayMock = new Mock<IDisplay>(MockBehavior.Strict);

    // First byte at y=0
    for (byte i = 0; i < 8; i++)
    {
      displayMock.Setup(x => x.GetPixel(i, 0)).Returns(false);
      if (i < 4)
      {
        displayMock.Setup(x => x.SetPixel(i, 0));
      }
    }

    // Second byte at y=1
    for (byte i = 0; i < 8; i++)
    {
      displayMock.Setup(x => x.GetPixel(i, 1)).Returns(false);
      if (i == 0 || i == 3)
      {
        displayMock.Setup(x => x.SetPixel(i, 1));
      }
    }

    var instruction = new Instruction_Dxyn(decodedInstruction);
    instruction.Execute(cpu, displayMock.Object, new Mock<IKeyboard>().Object);

    displayMock.VerifyAll();
  }

  [Test]
  public void Executing_Instruction_Dxy0_DoesNotUseDisplay()
  {
    var cpu = new Cpu();
    var decodedInstruction = new DecodedInstruction(0xD230);

    var instruction = new Instruction_Dxyn(decodedInstruction);
    instruction.Execute(cpu, MockedDisplay, MockedKeyboard);
  }

  [Test]
  public void Instruction_Dxy0_CorrectlySets_NibbleAsHex_InMnemonic()
  {
    var decodedInstruction = new DecodedInstruction(0xDABC);
    var instruction = new Instruction_Dxyn(decodedInstruction);

    Assert.That(instruction.Decoded.x, Is.EqualTo(0xA));
    Assert.That(instruction.Decoded.y, Is.EqualTo(0xB));
    Assert.That(instruction.Decoded.n, Is.EqualTo(0xC));
    Assert.That(instruction.Mnemonic, Is.EqualTo("DRW VA, VB, 0xC"));
  }
}
