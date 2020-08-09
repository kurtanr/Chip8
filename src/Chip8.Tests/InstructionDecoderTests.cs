using Chip8.Instructions;
using NUnit.Framework;

namespace Chip8.Tests
{
  [TestFixture]
  public class InstructionDecoderTests
  {
    [Test]
    public void Decode_Works()
    {
      var instructionDecoder = new InstructionDecoder();
      var decodedInstruction = instructionDecoder.Decode(0x12, 0x34);

      Assert.That(decodedInstruction.InstructionCode, Is.EqualTo(0x1234));
      Assert.That(decodedInstruction.nnn, Is.EqualTo(0x234));
      Assert.That(decodedInstruction.n, Is.EqualTo(0x4));
      Assert.That(decodedInstruction.x, Is.EqualTo(0x2));
      Assert.That(decodedInstruction.y, Is.EqualTo(0x3));
      Assert.That(decodedInstruction.kk, Is.EqualTo(0x34));
      Assert.That(decodedInstruction.OpCode, Is.EqualTo(0x1));
    }

    [Test]
    public void DefinedCpuInstructions_AreCorrectlyDecoded()
    {
      var instructionDecoder = new InstructionDecoder();

      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0x00E0)), Is.InstanceOf<Instruction_00E0>());
      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0x00EE)), Is.InstanceOf<Instruction_00EE>());

      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0x1248)), Is.InstanceOf<Instruction_1nnn>());

      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0x2468)), Is.InstanceOf<Instruction_2nnn>());

      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0x3468)), Is.InstanceOf<Instruction_3xkk>());

      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0x4468)), Is.InstanceOf<Instruction_4xkk>());

      // TODO: add other instructions
    }

    [Test]
    public void UndefinedCpuInstruction_IsCorrectlyDecoded()
    {
      var instructionDecoder = new InstructionDecoder();

      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0x0000)), Is.InstanceOf<UndefinedInstruction>());
    }
  }
}
