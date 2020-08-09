using System;

namespace Chip8.Instructions
{
  /// <summary>
  /// Base Chip-8 CPU instruction.
  /// </summary>
  public abstract class CpuInstruction
  {
    /// <summary>
    /// Decoded 16 bit identifier of the instruction.
    /// </summary>
    public DecodedInstruction Decoded { get; }

    /// <summary>
    /// Description of what the instruction does.
    /// </summary>
    public abstract string Description { get; }

    /// <summary>
    /// Symbolic name of the instruction.
    /// </summary>
    public abstract string Mnemonic { get; }

    /// <summary>
    /// Executes instruction using <paramref name="cpu"/> and <paramref name="display"/>.
    /// </summary>
    /// <param name="cpu">CPU used to execute the instruction.</param>
    /// <param name="display">Display used to execute the instruction.</param>
    public abstract void Execute(Cpu cpu, IDisplay display);

    protected CpuInstruction(DecodedInstruction decodedInstruction)
    {
      Decoded = decodedInstruction;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
      return $"{Mnemonic.PadRight(18)} // {Description}{Environment.NewLine}";
    }
  }
}
