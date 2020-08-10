using Chip8.Instructions;
using NUnit.Framework;

namespace Chip8.Tests.Instructions
{
  [TestFixture]
  public class DecodedInstructionTests
  {
    [Test]
    public void DecodedInstruction_IsEquatable()
    {
      var instruction1 = new DecodedInstruction(0x1234);
      var instruction2 = new DecodedInstruction(0x1234);
      var instruction3 = new DecodedInstruction(0x1235);

      Assert.That(instruction1, Is.EqualTo(instruction2));
      Assert.That(instruction1, Is.Not.EqualTo(instruction3));
    }
  }
}
