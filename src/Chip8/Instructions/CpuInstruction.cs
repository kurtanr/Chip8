namespace Chip8.Instructions;

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
  public string Description { get; protected set; }

  /// <summary>
  /// Symbolic name of the instruction.
  /// </summary>
  public string Mnemonic { get; protected set; }

  /// <summary>
  /// Executes instruction using <paramref name="cpu"/> and <paramref name="display"/>.
  /// Checks <paramref name="keyboard"/> for user input.
  /// </summary>
  /// <param name="cpu">CPU used to execute the instruction.</param>
  /// <param name="display">Display used to execute the instruction.</param>
  /// <param name="keyboard">Chip8 keyboard abstraction which is queried for user input.</param>
  public abstract void Execute(Cpu cpu, IDisplay display, IKeyboard keyboard);

  protected CpuInstruction(DecodedInstruction decodedInstruction)
  {
    Decoded = decodedInstruction;
  }

  /// <inheritdoc/>
  public override string ToString()
  {
    return Mnemonic;
  }

  public virtual string ToStringWithDescription()
  {
    return $"{Mnemonic,-18} // {Description}";
  }
}
