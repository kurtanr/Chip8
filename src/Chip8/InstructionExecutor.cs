using Chip8.Instructions;
using System;

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
    /// If executed instruction is not a RET/JP/CALL instruction, this method increments <see cref="Cpu.PC"/> by 2 after instruction execution.
    /// </summary>
    /// <returns>Instruction that was executed.</returns>
    /// <exception cref="InvalidOperationException">Attempting to execute <see cref="UndefinedInstruction"/>.</exception>
    public CpuInstruction ExecuteSingleInstruction()
    {
      var decodedInstruction = _instructionDecoder.Decode(_cpu.Memory[_cpu.PC], _cpu.Memory[_cpu.PC + 1]);
      var cpuInstruction = _instructionDecoder.GetCpuInstruction(decodedInstruction);

      cpuInstruction.Execute(_cpu, _display);

      if (!(cpuInstruction is Instruction_00EE || cpuInstruction is Instruction_1nnn || cpuInstruction is Instruction_2nnn))
      {
        _cpu.PC += 2;
      }

      return cpuInstruction;
    }
  }
}
