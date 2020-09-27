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
    private readonly IKeyboard _keyboard;
    private readonly InstructionDecoder _instructionDecoder;

    public InstructionExecutor(Cpu cpu, IDisplay display, IKeyboard keyboard, InstructionDecoder instructionDecoder)
    {
      _cpu = cpu ?? throw new ArgumentNullException(nameof(cpu));
      _display = display ?? throw new ArgumentNullException(nameof(display));
      _keyboard = keyboard ?? throw new ArgumentNullException(nameof(keyboard));
      _instructionDecoder = instructionDecoder ?? throw new ArgumentNullException(nameof(instructionDecoder));
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

      cpuInstruction.Execute(_cpu, _display, _keyboard);

      if (!(cpuInstruction is Instruction_00EE || cpuInstruction is Instruction_1nnn || 
            cpuInstruction is Instruction_2nnn || cpuInstruction is Instruction_Bnnn))
      {
        _cpu.PC += 2;
      }

      return cpuInstruction;
    }
  }
}
