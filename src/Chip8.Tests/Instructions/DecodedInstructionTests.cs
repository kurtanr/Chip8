using Chip8.Instructions;
using NUnit.Framework;

namespace Chip8.Tests.Instructions;

[TestFixture]
public class DecodedInstructionTests
{
  [Test]
  public void Equals_WithDecodedInstruction_ReturnsTrue_WhenInstructionCodesMatch()
  {
    var instruction1 = new DecodedInstruction(0x1234);
    var instruction2 = new DecodedInstruction(0x1234);

    Assert.That(instruction1.Equals(instruction2), Is.True);
  }

  [Test]
  public void Equals_WithDecodedInstruction_ReturnsFalse_WhenInstructionCodesDontMatch()
  {
    var instruction1 = new DecodedInstruction(0x1234);
    var instruction2 = new DecodedInstruction(0x1235);

    Assert.That(instruction1.Equals(instruction2), Is.False);
  }

  [Test]
  public void EqualityOperator_ReturnsTrue_WhenInstructionCodesMatch()
  {
    var instruction1 = new DecodedInstruction(0x1234);
    var instruction2 = new DecodedInstruction(0x1234);

    Assert.That(instruction1 == instruction2, Is.True);
  }

  [Test]
  public void InequalityOperator_ReturnsTrue_WhenInstructionCodesDontMatch()
  {
    var instruction1 = new DecodedInstruction(0x1234);
    var instruction2 = new DecodedInstruction(0x1235);

    Assert.That(instruction1 != instruction2, Is.True);
  }

  [Test]
  public void Equals_WithObject_ReturnsTrue_WhenInstructionCodesMatch()
  {
    var instruction1 = new DecodedInstruction(0x1234);
    object instruction2 = new DecodedInstruction(0x1234);

    Assert.That(instruction1.Equals(instruction2), Is.True);
  }

  [Test]
  public void Equals_WithObject_ReturnsFalse_WhenInstructionCodesDontMatch()
  {
    var instruction1 = new DecodedInstruction(0x1234);
    object instruction2 = new DecodedInstruction(0x1235);

    Assert.That(instruction1.Equals(instruction2), Is.False);
  }

  [Test]
  public void Equals_WithObject_ReturnsFalse_WhenObjectIsNull()
  {
    var instruction = new DecodedInstruction(0x1234);

    Assert.That(instruction.Equals(null), Is.False);
  }

  [Test]
  public void Equals_WithObject_ReturnsFalse_WhenObjectIsOfDifferentType()
  {
    var instruction = new DecodedInstruction(0x1234);
    object other = "not an instruction";

    Assert.That(instruction.Equals(other), Is.False);
  }

  [Test]
  public void GetHashCode_ReturnsEqualHashCodes_ForEqualInstructions()
  {
    var instruction1 = new DecodedInstruction(0x1234);
    var instruction2 = new DecodedInstruction(0x1234);

    Assert.That(instruction1.GetHashCode(), Is.EqualTo(instruction2.GetHashCode()));
  }

  [Test]
  public void GetHashCode_ReturnsDifferentHashCodes_ForDifferentInstructions()
  {
    var instruction1 = new DecodedInstruction(0x1234);
    var instruction2 = new DecodedInstruction(0x1235);

    Assert.That(instruction1.GetHashCode(), Is.Not.EqualTo(instruction2.GetHashCode()));
  }
}
