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

      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0x5460)), Is.InstanceOf<Instruction_5xy0>());

      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0x6123)), Is.InstanceOf<Instruction_6xkk>());

      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0x7123)), Is.InstanceOf<Instruction_7xkk>());

      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0x8AB0)), Is.InstanceOf<Instruction_8xy0>());
      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0x8AB1)), Is.InstanceOf<Instruction_8xy1>());
      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0x8AB2)), Is.InstanceOf<Instruction_8xy2>());
      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0x8AB3)), Is.InstanceOf<Instruction_8xy3>());
      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0x8AB4)), Is.InstanceOf<Instruction_8xy4>());
      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0x8AB5)), Is.InstanceOf<Instruction_8xy5>());
      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0x8AB6)), Is.InstanceOf<Instruction_8xy6>());
      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0x8AB7)), Is.InstanceOf<Instruction_8xy7>());
      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0x8ABE)), Is.InstanceOf<Instruction_8xyE>());

      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0x9460)), Is.InstanceOf<Instruction_9xy0>());

      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0xA123)), Is.InstanceOf<Instruction_Annn>());

      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0xB123)), Is.InstanceOf<Instruction_Bnnn>());

      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0xC123)), Is.InstanceOf<Instruction_Cxkk>());

      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0xD123)), Is.InstanceOf<Instruction_Dxyn>());

      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0xFA07)), Is.InstanceOf<Instruction_Fx07>());
      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0xFA15)), Is.InstanceOf<Instruction_Fx15>());
      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0xFA18)), Is.InstanceOf<Instruction_Fx18>());
      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0xFA1E)), Is.InstanceOf<Instruction_Fx1E>());
      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0xFA29)), Is.InstanceOf<Instruction_Fx29>());
      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0xFA33)), Is.InstanceOf<Instruction_Fx33>());
      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0xF155)), Is.InstanceOf<Instruction_Fx55>());
      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0xF165)), Is.InstanceOf<Instruction_Fx65>());

      // TODO: add other instructions
    }

    [Test]
    public void UndefinedCpuInstructions_AreDecodedAsUndefinedInstructions()
    {
      var instructionDecoder = new InstructionDecoder();

      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0x0000)), Is.InstanceOf<UndefinedInstruction>());
      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0x5468)), Is.InstanceOf<UndefinedInstruction>());
      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0x8468)), Is.InstanceOf<UndefinedInstruction>());
      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0x9468)), Is.InstanceOf<UndefinedInstruction>());
      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0xF1FF)), Is.InstanceOf<UndefinedInstruction>());
    }
  }
}
