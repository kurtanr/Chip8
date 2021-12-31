﻿using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Chip8.Tests
{
  public class CompilationAndDecompilationOfSampleApplications
  {
    [Test]
    [Ignore("Executed manually")]
    public void LoadingThenRetrievingApplicationInEmulator_ReturnsSameByteArray()
    {
      var directory = new DirectoryInfo(@"..\..\..\..\..\games");
      foreach(var file in Directory.EnumerateFiles(directory.FullName).Where(x => !x.EndsWith(".c8")))
      {
        var originalGame = File.ReadAllBytes(file);

        var emulator = new Emulator(new Cpu(), new Mock<IDisplay>().Object, new Mock<IKeyboard>().Object);
        emulator.LoadApplication(originalGame);

        var gameFromEmulator = emulator.GetApplication();

        CollectionAssert.AreEqual(originalGame, gameFromEmulator, $"File: {file}");
        //File.WriteAllBytes(file, gameFromEmulator);
      }
    }

    [Test]
    [Ignore("Executed manually")]
    public void DecompiledApplication_IsSameAs_AlreadyDecompiledApplication()
    {
      var directory = new DirectoryInfo(@"..\..\..\..\..\games");
      foreach (var file in Directory.EnumerateFiles(directory.FullName).Where(x => !x.EndsWith(".c8")))
      {
        var application = File.ReadAllBytes(file);
        var instructionDecoder = new InstructionDecoder();

        var instructions = new List<string>();
        for (int i = 0; i < application.Length - 1; i += 2)
        {
          var instruction = GetInstruction(application[i], application[i+1], instructionDecoder);
          instructions.Add(instruction);
        }

        // if length of the application is not an even number, that means that the last instruction
        // was skipped (in the preceding for loop) because it did not start on an even address
        if(application.Length % 2 != 0)
        {
          var instruction = GetInstruction(application[application.Length - 1], 0x0, instructionDecoder);
          instructions.Add(instruction);
        }

        var codeFilePath = $"{file}.c8";
        var alreadyDecompiledApplication = File.ReadAllLines(codeFilePath);
        CollectionAssert.AreEqual(alreadyDecompiledApplication, instructions, $"File: {codeFilePath}");
        //File.WriteAllLines(codeFilePath, instructions);
      }
    }

    private string GetInstruction(byte msb, byte lsb, InstructionDecoder instructionDecoder)
    {
      var decodedInstruction = instructionDecoder.Decode(msb, lsb);
      var cpuInstruction = instructionDecoder.GetCpuInstruction(decodedInstruction);
      return cpuInstruction.Mnemonic;
    }
  }
}
