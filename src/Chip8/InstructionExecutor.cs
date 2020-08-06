using Chip8.Instructions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chip8
{
  /// <summary>
  /// Executes CPU instructions.
  /// </summary>
  public class InstructionExecutor
  {
    private readonly Cpu _cpu;
    private readonly IDisplay _display;
    private readonly InstructionDecoder _instructionDecoder;
    private readonly IEnumerable<Type> _instructionsWhichModifyPc = new List<Type>
    { 
      typeof(Instruction_00EE),
      typeof(Instruction_1nnn),
      typeof(Instruction_2nnn)
      // TODO: add other instructions which modify PC
    };

    public InstructionExecutor(Cpu cpu, IDisplay display, InstructionDecoder instructionDecoder)
    {
      if (cpu == null)
      {
        throw new ArgumentNullException(nameof(cpu));
      }
      if (display == null)
      {
        throw new ArgumentNullException(nameof(display)); 
      }
      if (instructionDecoder == null)
      {
        throw new ArgumentNullException(nameof(instructionDecoder));
      }

      _cpu = cpu;
      _display = display;
      _instructionDecoder = instructionDecoder;
    }

    /// <summary>
    /// Decodes and executes instruction from memory location to which <see cref="Cpu.PC"/> is pointing.
    /// If executed instruction does not modify <see cref="Cpu.PC"/>, this method increments <see cref="Cpu.PC"/> by 2.
    /// </summary>
    /// <returns>Instruction that was executed.</returns>
    /// <exception cref="InvalidOperationException">Attempting to execute <see cref="UndefinedInstruction"/>.</exception>
    public CpuInstruction ExecuteSingleInstruction()
    {
      var instruction = _instructionDecoder.Decode(_cpu.Memory[_cpu.PC], _cpu.Memory[_cpu.PC + 1]);
      instruction.Execute(_cpu, _display);

      if (!_instructionsWhichModifyPc.Contains(instruction.GetType()))
      {
        _cpu.PC += 2;
      }

      return instruction;
    }
  }
}
