using Moq;
using NUnit.Framework;
using System;

namespace Chip8.Tests
{
  public class EmulatorTests
  {
    private Cpu _cpu;
    private IDisplay _display;
    private IKeyboard _keyboard;
    private Emulator _emulator;

    private readonly byte[] _applicationWhichClearsScreenInLoop = new byte[] { 0x00, 0xE0, 0x12, 0x00 };

    [SetUp]
    public void Setup()
    {
      _cpu = new Cpu();
      _display = new Mock<IDisplay>(MockBehavior.Loose).Object;
      _keyboard = new Mock<IKeyboard>(MockBehavior.Strict).Object;
      _emulator = new Emulator(_cpu, _display, _keyboard);
    }

    [TearDown]
    public void TearDown()
    {
      _emulator.Dispose();
    }

    [Test]
    public void Constructor_ArgumentValidation_Works()
    {
      Assert.DoesNotThrow(() => new Emulator(_cpu, _display, _keyboard));
      Assert.Throws<ArgumentNullException>(() => new Emulator(null, _display, _keyboard));
      Assert.Throws<ArgumentNullException>(() => new Emulator(_cpu, null, _keyboard));
      Assert.Throws<ArgumentNullException>(() => new Emulator(_cpu, _display, null));
    }

    #region LoadApplication

    [Test]
    public void LoadApplication_NullArgumentValidation_Works()
    {
      Assert.Throws<ArgumentNullException>(() => _emulator.LoadApplication(null));
    }

    [Test]
    public void LoadApplication_WithMaxSize_Works()
    {
      var maxAllowedAppSize = Cpu.MemorySizeInBytes - Cpu.MemoryAddressOfFirstInstruction;
      var application = new byte[maxAllowedAppSize];
      application[0] = 0xAB;
      application[application.Length - 1] = 0xCD;

      Assert.DoesNotThrow(() => _emulator.LoadApplication(application));
      Assert.That(_cpu.Memory[Cpu.MemoryAddressOfFirstInstruction], Is.EqualTo(0xAB));
      Assert.That(_cpu.Memory[Cpu.MemorySizeInBytes - 1], Is.EqualTo(0xCD));
    }

    [Test]
    public void LoadApplication_WhichExceedsMaxSize_ThrowsException()
    {
      var maxAllowedAppSizeExceeded = Cpu.MemorySizeInBytes - Cpu.MemoryAddressOfFirstInstruction + 1;
      var application = new byte[maxAllowedAppSizeExceeded];

      Assert.Throws<ArgumentException>(() => _emulator.LoadApplication(application));
    }

    #endregion

    #region RunApplication

    [Test]
    public void RunApplication_WhichIsNotLoaded_ThrowsException()
    {
      var maxAllowedAppSizeExceeded = Cpu.MemorySizeInBytes - Cpu.MemoryAddressOfFirstInstruction + 1;
      var application = new byte[maxAllowedAppSizeExceeded];

      Assert.Throws<InvalidOperationException>(() => _emulator.RunApplication());
    }

    [Test]
    public void RunApplication_WithValidApplication_SetsIsApplicationRunning()
    {
      _emulator.LoadApplication(_applicationWhichClearsScreenInLoop);
      _emulator.RunApplication();

      Assert.That(_emulator.IsApplicationRunning, Is.True);
    }

    [Test]
    public void RunApplication_MultipleTimes_WithValidApplication_Works()
    {
      _emulator.LoadApplication(_applicationWhichClearsScreenInLoop);
      _emulator.RunApplication();

      Assert.DoesNotThrow(() => _emulator.RunApplication());
    }

    #endregion

    #region Dispose

    [Test]
    public void Dispose_WithRunningApplication_Works()
    {
      _emulator.LoadApplication(_applicationWhichClearsScreenInLoop);
      _emulator.RunApplication();
      _emulator.Dispose();

      Assert.That(_emulator.IsApplicationRunning, Is.False);
    }

    [Test]
    public void Dispose_WithLoadedButNeverStartedApplication_Works()
    {
      _emulator.LoadApplication(_applicationWhichClearsScreenInLoop);
      _emulator.Dispose();

      Assert.That(_emulator.IsApplicationRunning, Is.False);
    }

    #endregion
  }
}
