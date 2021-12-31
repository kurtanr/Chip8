using Chip8.Instructions;
using NUnit.Framework;
using System;

namespace Chip8.Tests
{
  [TestFixture]
  public class InstructionEncoderTests
  {
    private readonly InstructionEncoder _instructionEncoder = new ();

    [Test]
    public void Instruction_00E0_IsCorrectlyEncoded()
    {
      const string mnemonic = "CLS";
      var cpuInstruction = _instructionEncoder.GetCpuInstruction(mnemonic);

      VerifyInstruction<Instruction_00E0>(cpuInstruction, mnemonic);
    }

    [Test]
    public void Instruction_00EE_IsCorrectlyEncoded()
    {
      const string mnemonic = "RET";
      var cpuInstruction = _instructionEncoder.GetCpuInstruction(mnemonic);

      VerifyInstruction<Instruction_00EE>(cpuInstruction, mnemonic);
    }

    [TestCase("JP 0x248", (ushort)0x248)]
    [TestCase("JP 0xABB", (ushort)0xABB)]
    public void Instruction_1nnn_IsCorrectlyEncoded(string mnemonic, ushort expectedNNN)
    {
      VerifyNnnInstruction(mnemonic, expectedNNN, typeof(Instruction_1nnn));
    }

    [TestCase("CALL 0x468", (ushort)0x468)]
    [TestCase("CALL 0x4FB", (ushort)0x4FB)]
    public void Instruction_2nnn_IsCorrectlyEncoded(string mnemonic, ushort expectedNNN)
    {
      VerifyNnnInstruction(mnemonic, expectedNNN, typeof(Instruction_2nnn));
    }

    [TestCase("SE V4, 0x68", 0x4, 0x68)]
    [TestCase("SE VF, 0xA", 0xF, 0xA)]
    public void Instruction_3xkk_IsCorrectlyEncoded(string mnemonic, byte expectedX, byte expectedKK)
    {
      VerifyXkkInstruction(mnemonic, expectedX, expectedKK, typeof(Instruction_3xkk));
    }

    [TestCase("SNE V4, 0x68", 0x4, 0x68)]
    [TestCase("SNE VF, 0xA", 0xF, 0xA)]
    public void Instruction_4xkk_IsCorrectlyEncoded(string mnemonic, byte expectedX, byte expectedKK)
    {
      VerifyXkkInstruction(mnemonic, expectedX, expectedKK, typeof(Instruction_4xkk));
    }

    [TestCase("SE V4, V6", 0x4, 0x6)]
    [TestCase("SE VA, VF", 0xA, 0xF)]
    public void Instruction_5xy0_IsCorrectlyEncoded(string mnemonic, byte expectedX, byte expectedY)
    {
      VerifyXyInstruction(mnemonic, expectedX, expectedY, typeof(Instruction_5xy0));
    }

    [TestCase("LD V4, 0x68", 0x4, 0x68)]
    [TestCase("LD VF, 0xA", 0xF, 0xA)]
    public void Instruction_6xkk_IsCorrectlyEncoded(string mnemonic, byte expectedX, byte expectedKK)
    {
      VerifyXkkInstruction(mnemonic, expectedX, expectedKK, typeof(Instruction_6xkk));
    }

    [TestCase("ADD V4, 0x68", 0x4, 0x68)]
    [TestCase("ADD VF, 0xA", 0xF, 0xA)]
    public void Instruction_7xkk_IsCorrectlyEncoded(string mnemonic, byte expectedX, byte expectedKK)
    {
      VerifyXkkInstruction(mnemonic, expectedX, expectedKK, typeof(Instruction_7xkk));
    }

    [TestCase("LD V4, V6", 0x4, 0x6)]
    [TestCase("LD VA, VF", 0xA, 0xF)]
    public void Instruction_8xy0_IsCorrectlyEncoded(string mnemonic, byte expectedX, byte expectedY)
    {
      VerifyXyInstruction(mnemonic, expectedX, expectedY, typeof(Instruction_8xy0));
    }

    [TestCase("OR V4, V6", 0x4, 0x6)]
    [TestCase("OR VA, VF", 0xA, 0xF)]
    public void Instruction_8xy1_IsCorrectlyEncoded(string mnemonic, byte expectedX, byte expectedY)
    {
      VerifyXyInstruction(mnemonic, expectedX, expectedY, typeof(Instruction_8xy1));
    }

    [TestCase("AND V4, V6", 0x4, 0x6)]
    [TestCase("AND VA, VF", 0xA, 0xF)]
    public void Instruction_8xy2_IsCorrectlyEncoded(string mnemonic, byte expectedX, byte expectedY)
    {
      VerifyXyInstruction(mnemonic, expectedX, expectedY, typeof(Instruction_8xy2));
    }

    [TestCase("XOR V4, V6", 0x4, 0x6)]
    [TestCase("XOR VA, VF", 0xA, 0xF)]
    public void Instruction_8xy3_IsCorrectlyEncoded(string mnemonic, byte expectedX, byte expectedY)
    {
      VerifyXyInstruction(mnemonic, expectedX, expectedY, typeof(Instruction_8xy3));
    }

    [TestCase("ADD V4, V6", 0x4, 0x6)]
    [TestCase("ADD VA, VF", 0xA, 0xF)]
    public void Instruction_8xy4_IsCorrectlyEncoded(string mnemonic, byte expectedX, byte expectedY)
    {
      VerifyXyInstruction(mnemonic, expectedX, expectedY, typeof(Instruction_8xy4));
    }

    [TestCase("SUB V4, V6", 0x4, 0x6)]
    [TestCase("SUB VA, VF", 0xA, 0xF)]
    public void Instruction_8xy5_IsCorrectlyEncoded(string mnemonic, byte expectedX, byte expectedY)
    {
      VerifyXyInstruction(mnemonic, expectedX, expectedY, typeof(Instruction_8xy5));
    }

    [TestCase("SHR V4, V6", 0x4, 0x6)]
    [TestCase("SHR VA, VF", 0xA, 0xF)]
    public void Instruction_8xy6_IsCorrectlyEncoded(string mnemonic, byte expectedX, byte expectedY)
    {
      VerifyXyInstruction(mnemonic, expectedX, expectedY, typeof(Instruction_8xy6));
    }

    [TestCase("SUBN V4, V6", 0x4, 0x6)]
    [TestCase("SUBN VA, VF", 0xA, 0xF)]
    public void Instruction_8xy7_IsCorrectlyEncoded(string mnemonic, byte expectedX, byte expectedY)
    {
      VerifyXyInstruction(mnemonic, expectedX, expectedY, typeof(Instruction_8xy7));
    }

    [TestCase("SHL V4, V6", 0x4, 0x6)]
    [TestCase("SHL VA, VF", 0xA, 0xF)]
    public void Instruction_8xyE_IsCorrectlyEncoded(string mnemonic, byte expectedX, byte expectedY)
    {
      VerifyXyInstruction(mnemonic, expectedX, expectedY, typeof(Instruction_8xyE));
    }

    [TestCase("SNE V4, V6", 0x4, 0x6)]
    [TestCase("SNE VA, VF", 0xA, 0xF)]
    public void Instruction_9xy0_IsCorrectlyEncoded(string mnemonic, byte expectedX, byte expectedY)
    {
      VerifyXyInstruction(mnemonic, expectedX, expectedY, typeof(Instruction_9xy0));
    }

    [TestCase("LD I, 0x468", (ushort)0x468)]
    [TestCase("LD I, 0x4FB", (ushort)0x4FB)]
    public void Instruction_Annn_IsCorrectlyEncoded(string mnemonic, ushort expectedNNN)
    {
      VerifyNnnInstruction(mnemonic, expectedNNN, typeof(Instruction_Annn));
    }

    [TestCase("JP V0, 0x468", (ushort)0x468)]
    [TestCase("JP V0, 0x4FB", (ushort)0x4FB)]
    public void Instruction_Bnnn_IsCorrectlyEncoded(string mnemonic, ushort expectedNNN)
    {
      VerifyNnnInstruction(mnemonic, expectedNNN, typeof(Instruction_Bnnn));
    }

    [TestCase("RND V4, 0x68", 0x4, 0x68)]
    [TestCase("RND VF, 0xA", 0xF, 0xA)]
    public void Instruction_Cxkk_IsCorrectlyEncoded(string mnemonic, byte expectedX, byte expectedKK)
    {
      VerifyXkkInstruction(mnemonic, expectedX, expectedKK, typeof(Instruction_Cxkk));
    }

    [TestCase("DRW V1, V2, 0x3", 0x1, 0x2, 0x3)]
    [TestCase("DRW VA, VB, 0xC", 0xA, 0xB, 0xC)]
    public void Instruction_Dxyn_IsCorrectlyEncoded(string mnemonic, byte expectedX, byte expectedY, byte expectedN)
    {
      var cpuInstruction = _instructionEncoder.GetCpuInstruction(mnemonic);

      VerifyInstruction<Instruction_Dxyn>(cpuInstruction, mnemonic);

      Assert.That(cpuInstruction.Decoded.x, Is.EqualTo(expectedX));
      Assert.That(cpuInstruction.Decoded.y, Is.EqualTo(expectedY));
      Assert.That(cpuInstruction.Decoded.n, Is.EqualTo(expectedN));
    }

    [TestCase("SKP V0", 0x0)]
    [TestCase("SKP VA", 0xA)]
    public void Instruction_Ex9E_IsCorrectlyEncoded(string mnemonic, byte expectedX)
    {
      VerifyXInstruction(mnemonic, expectedX, typeof(Instruction_Ex9E));
    }

    [TestCase("SKNP V0", 0x0)]
    [TestCase("SKNP VA", 0xA)]
    public void Instruction_ExA1_IsCorrectlyEncoded(string mnemonic, byte expectedX)
    {
      VerifyXInstruction(mnemonic, expectedX, typeof(Instruction_ExA1));
    }

    //Assert.That(instructionEncoder.GetCpuInstruction(new DecodedInstruction(0xFA07)), Is.InstanceOf<Instruction_Fx07>());
    //Assert.That(instructionEncoder.GetCpuInstruction(new DecodedInstruction(0xF10A)), Is.InstanceOf<Instruction_Fx0A>());
    //Assert.That(instructionEncoder.GetCpuInstruction(new DecodedInstruction(0xFA15)), Is.InstanceOf<Instruction_Fx15>());
    //Assert.That(instructionEncoder.GetCpuInstruction(new DecodedInstruction(0xFA18)), Is.InstanceOf<Instruction_Fx18>());
    //Assert.That(instructionEncoder.GetCpuInstruction(new DecodedInstruction(0xFA1E)), Is.InstanceOf<Instruction_Fx1E>());
    //Assert.That(instructionEncoder.GetCpuInstruction(new DecodedInstruction(0xFA29)), Is.InstanceOf<Instruction_Fx29>());
    //Assert.That(instructionEncoder.GetCpuInstruction(new DecodedInstruction(0xFA33)), Is.InstanceOf<Instruction_Fx33>());
    //Assert.That(instructionEncoder.GetCpuInstruction(new DecodedInstruction(0xF155)), Is.InstanceOf<Instruction_Fx55>());
    //Assert.That(instructionEncoder.GetCpuInstruction(new DecodedInstruction(0xF165)), Is.InstanceOf<Instruction_Fx65>());

    /*[Test]
    public void UndefinedCpuInstructions_AreDecodedAsUndefinedInstructions()
    {
      var instructionDecoder = new InstructionDecoder();

      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0x0000)), Is.InstanceOf<UndefinedInstruction>());
      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0x5468)), Is.InstanceOf<UndefinedInstruction>());
      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0x8468)), Is.InstanceOf<UndefinedInstruction>());
      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0x9468)), Is.InstanceOf<UndefinedInstruction>());
      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0xE101)), Is.InstanceOf<UndefinedInstruction>());
      Assert.That(instructionDecoder.GetCpuInstruction(new DecodedInstruction(0xF1FF)), Is.InstanceOf<UndefinedInstruction>());
    }*/

    private void VerifyNnnInstruction(string mnemonic, ushort expectedNnn, Type expectedInstructionType)
    {
      var cpuInstruction = _instructionEncoder.GetCpuInstruction(mnemonic);

      VerifyInstruction(cpuInstruction, mnemonic, expectedInstructionType);

      Assert.That(cpuInstruction.Decoded.nnn, Is.EqualTo(expectedNnn));
    }

    private void VerifyXkkInstruction(string mnemonic, byte expectedX, byte expectedKK, Type expectedInstructionType)
    {
      var cpuInstruction = _instructionEncoder.GetCpuInstruction(mnemonic);

      VerifyInstruction(cpuInstruction, mnemonic, expectedInstructionType);

      Assert.That(cpuInstruction.Decoded.x, Is.EqualTo(expectedX));
      Assert.That(cpuInstruction.Decoded.kk, Is.EqualTo(expectedKK));
    }

    private void VerifyXyInstruction(string mnemonic, byte expectedX, byte expectedY, Type expectedInstructionType)
    {
      var cpuInstruction = _instructionEncoder.GetCpuInstruction(mnemonic);

      VerifyInstruction(cpuInstruction, mnemonic, expectedInstructionType);

      Assert.That(cpuInstruction.Decoded.x, Is.EqualTo(expectedX));
      Assert.That(cpuInstruction.Decoded.y, Is.EqualTo(expectedY));
    }

    private void VerifyXInstruction(string mnemonic, byte expectedX, Type expectedInstructionType)
    {
      var cpuInstruction = _instructionEncoder.GetCpuInstruction(mnemonic);

      VerifyInstruction(cpuInstruction, mnemonic, expectedInstructionType);

      Assert.That(cpuInstruction.Decoded.x, Is.EqualTo(expectedX));
    }

    private void VerifyInstruction<T>(CpuInstruction cpuInstruction, string mnemonic) where T : CpuInstruction
    {
      Assert.That(cpuInstruction, Is.InstanceOf<T>());
      Assert.That(cpuInstruction.Mnemonic, Is.EqualTo(mnemonic));
    }

    private void VerifyInstruction(CpuInstruction cpuInstruction, string mnemonic, Type instructionType)
    {
      Assert.That(cpuInstruction, Is.InstanceOf(instructionType));
      Assert.That(cpuInstruction.Mnemonic, Is.EqualTo(mnemonic));
    }
  }
}
