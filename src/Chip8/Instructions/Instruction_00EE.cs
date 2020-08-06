namespace Chip8.Instructions
{
  /// <summary>
  /// Return from a subroutine.
  /// </summary>
  /// <remarks>
  /// The interpreter sets the program counter to the address at the top of the stack, then subtracts 1 from the stack pointer.
  /// </remarks>
  public class Instruction_00EE : CpuInstruction
  {
    public Instruction_00EE(ushort instructionCode) : base(instructionCode)
    {
    }

    public override string Description => "Return from a subroutine.";

    public override string Mnemonic => "RET";

    public override void Execute(Cpu cpu, IDisplay display)
    {
      cpu.PC = cpu.Stack.Pop();
    }
  }
}
