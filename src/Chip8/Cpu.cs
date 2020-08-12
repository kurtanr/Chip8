﻿using System;
using System.Collections.Generic;

namespace Chip8
{
  /// <summary>
  /// Chip-8 CPU.
  /// </summary>
  public class Cpu
  {
    public static readonly ushort MemorySizeInBytes = 4096;
    public static readonly ushort MaxStackDepth = 16;
    public static readonly ushort MemoryAddressOfFirstInstruction = 0x200;
    public static readonly ushort MemoryAddressOfLastInstruction = 0xFFF - 1;

    #region Fields and properties

    /// <summary>
    /// 16 general purpose 8-bit registers.
    /// </summary>
    public readonly byte[] V = new byte[16];

    /// <summary>
    /// 16-bit index register.
    /// </summary>
    public ushort I { get; set; }

    private ushort _pc;

    /// <summary>
    /// 16-bit program counter.
    /// Default value is <see cref="MemoryAddressOfFirstInstruction"/>.
    /// Value must be in range [<see cref="MemoryAddressOfFirstInstruction"/>, <see cref="MemoryAddressOfLastInstruction"/>]
    /// </summary>
    public ushort PC
    {
      get { return _pc; }
      set
      {
        if (value < MemoryAddressOfFirstInstruction || value > MemoryAddressOfLastInstruction)
        {
          throw new InvalidOperationException($"Attempting to set program counter to {value} which is outside of allowed range of [{MemoryAddressOfFirstInstruction},{MemoryAddressOfLastInstruction}].");
        }
        _pc = value;
      }
    }

    /// <summary>
    /// Stack containing a maximum of 16 16-bit values.
    /// </summary>
    public readonly StackWithMaxDepth Stack = new StackWithMaxDepth();

    /// <summary>
    /// 8-bit delay timer.
    /// Decrements by 1 at a rate of 60Hz.
    /// When DT reaches 0, timer deactivates.
    /// </summary>
    public byte DT { get; set; }

    /// <summary>
    /// 8-bit sound timer.
    /// Decrements by 1 at a rate of 60Hz.
    /// As long as ST's value is greater than zero, the Chip-8 buzzer will sound.
    /// Sound produced by the Chip-8 interpreter has only one tone.
    /// When ST reaches 0, timer and sound deactivate.
    /// </summary>
    public byte ST { get; set; }

    /// <summary>
    /// 4096 bytes of memory.
    /// Programs start at <see cref="MemoryAddressOfFirstInstruction"/>, which is the starting value of <see cref="PC"/>.
    /// </summary>
    public readonly byte[] Memory = new byte[MemorySizeInBytes];

    #endregion

    #region Constructor and methods

    public Cpu()
    {
      Reset();
    }

    /// <summary>
    /// Resets all the CPU registers to the default value.
    /// </summary>
    public void Reset()
    {
      Stack.Clear();

      for (int i = 0; i < V.Length; i++)
      {
        V[i] = 0;
      }

      PC = 0x200;
      I = 0;
      DT = ST = 0;

      for (int i = 0; i < MemorySizeInBytes; i++)
      {
        Memory[i] = 0;
      }
    }

    #endregion

    #region StackWithMaxDepth implementation

    /// <summary>
    /// Stack with maximum depth set to <see cref="MaxStackDepth"/>.
    /// </summary>
    public class StackWithMaxDepth : Stack<ushort>
    {
      public StackWithMaxDepth() : base((int)MaxStackDepth)
      {
      }

      /// <summary>
      /// Inserts an object at the top of the stack.
      /// </summary>
      /// <param name="item">The object to push onto the stack.</param>
      /// <exception cref="InvalidOperationException">Exceeded maximum stack depth.</exception>
      public new void Push(ushort item)
      {
        if(Count >= MaxStackDepth)
        {
          throw new InvalidOperationException($"Exceeded maximum stack depth of {MaxStackDepth}.");
        }
        base.Push(item);
      }
    }

    #endregion
  }
}
