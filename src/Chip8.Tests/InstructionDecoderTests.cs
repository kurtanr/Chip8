using Chip8.Instructions;
using NUnit.Framework;

namespace Chip8.Tests
{
  [TestFixture]
  public class InstructionDecoderTests
  {
    [Test]
    public void DefinedInstructions_AreCorrectlyDecoded()
    {
      var instructionDecoder = new InstructionDecoder();

      Assert.That(instructionDecoder.Decode(0x00, 0xE0), Is.InstanceOf<Instruction_00E0>());
      Assert.That(instructionDecoder.Decode(0x00, 0xEE), Is.InstanceOf<Instruction_00EE>());

      Assert.That(instructionDecoder.Decode(0x10, 0x12), Is.InstanceOf<Instruction_1nnn>());

      Assert.That(instructionDecoder.Decode(0x20, 0x12), Is.InstanceOf<Instruction_2nnn>());

      // TODO: add other instructions
    }

    [Test]
    public void UndefinedInstruction_IsCorrectlyDecoded()
    {
      var instructionDecoder = new InstructionDecoder();

      Assert.That(instructionDecoder.Decode(0x00, 0x00), Is.InstanceOf<UndefinedInstruction>());
    }
  }
}
