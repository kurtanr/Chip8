﻿using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

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

    #region RunContinueApplication

    [Test]
    public void RunContinueApplication_WhichIsNotLoaded_ThrowsException()
    {
      Assert.Throws<InvalidOperationException>(() => _emulator.RunContinueApplication());
    }

    [TestCase(0)]
    [TestCase(100)]
    public async Task RunContinueApplication_WithLoadedApplication_SetsIsApplicationRunningToTrue(int waitInMs)
    {
      _emulator.LoadApplication(_applicationWhichClearsScreenInLoop);
      _emulator.RunContinueApplication();

      await Task.Delay(waitInMs).ConfigureAwait(false);

      Assert.That(_emulator.IsApplicationRunning, Is.True);
    }

    [Test]
    public void RunContinueApplication_MultipleTimes_WithLoadedApplication_Works()
    {
      _emulator.LoadApplication(_applicationWhichClearsScreenInLoop);
      _emulator.RunContinueApplication();

      Assert.DoesNotThrow(() => _emulator.RunContinueApplication());
    }

    #endregion

    #region PauseApplication

    [Test]
    public void PauseApplication_WhichIsNotLoaded_ThrowsException()
    {
      Assert.Throws<InvalidOperationException>(() => _emulator.PauseApplication());
    }

    [Test]
    public void PauseApplication_WhichWasNeverStarted_ThrowsException()
    {
      _emulator.LoadApplication(_applicationWhichClearsScreenInLoop);

      Assert.Throws<InvalidOperationException>(() => _emulator.PauseApplication());
    }

    [TestCase(0)]
    [TestCase(100)]
    public async Task PauseApplication_WithRunningApplication_SetsIsApplicationRunningToFalse(int waitInMs)
    {
      _emulator.LoadApplication(_applicationWhichClearsScreenInLoop);
      _emulator.RunContinueApplication();

      await Task.Delay(waitInMs).ConfigureAwait(false);
      Assert.That(_emulator.IsApplicationRunning, Is.True);

      _emulator.PauseApplication();

      await Task.Delay(waitInMs).ConfigureAwait(false);
      Assert.That(_emulator.IsApplicationRunning, Is.False);

      _emulator.RunContinueApplication();

      await Task.Delay(waitInMs).ConfigureAwait(false);
      Assert.That(_emulator.IsApplicationRunning, Is.True);
    }

    [Test]
    public void PauseApplication_MultipleTimes_WithRunningApplication_Works()
    {
      _emulator.LoadApplication(_applicationWhichClearsScreenInLoop);
      _emulator.RunContinueApplication();
      _emulator.PauseApplication();

      Assert.DoesNotThrow(() => _emulator.PauseApplication());
    }

    #endregion

    #region StopApplication

    [Test]
    public void StopApplication_WhichIsNotLoaded_ThrowsException()
    {
      Assert.Throws<InvalidOperationException>(() => _emulator.StopApplication());
    }

    [Test]
    public void StopApplication_WhichWasNeverStarted_ThrowsException()
    {
      _emulator.LoadApplication(_applicationWhichClearsScreenInLoop);

      Assert.Throws<InvalidOperationException>(() => _emulator.StopApplication());
    }

    [TestCase(0)]
    [TestCase(100)]
    public async Task StopApplication_WithRunningApplication_SetsIsApplicationRunningToFalse(int waitInMs)
    {
      _emulator.LoadApplication(_applicationWhichClearsScreenInLoop);
      _emulator.RunContinueApplication();

      await Task.Delay(waitInMs).ConfigureAwait(false);
      Assert.That(_emulator.IsApplicationRunning, Is.True);

      _emulator.StopApplication();

      await Task.Delay(waitInMs).ConfigureAwait(false);
      Assert.That(_emulator.IsApplicationRunning, Is.False);

      _emulator.RunContinueApplication();

      await Task.Delay(waitInMs).ConfigureAwait(false);
      Assert.That(_emulator.IsApplicationRunning, Is.True);
    }

    [Test]
    public void StopApplication_MultipleTimes_WithRunningApplication_Works()
    {
      _emulator.LoadApplication(_applicationWhichClearsScreenInLoop);
      _emulator.RunContinueApplication();
      _emulator.StopApplication();

      Assert.DoesNotThrow(() => _emulator.StopApplication());
    }

    #endregion

    #region GetApplication

    [Test]
    public void GetApplication_WithLoadedApplication_Works()
    {
      _emulator.LoadApplication(_applicationWhichClearsScreenInLoop);

      var application = _emulator.GetApplication();

      CollectionAssert.AreEqual(new byte[] { 0x00, 0xE0, 0x12 }, application);
    }

    [Test]
    public void GetApplication_WithNoApplicationLoaded_ReturnsSingleZeroByte()
    {
      var application = _emulator.GetApplication();

      Assert.That(application.Length, Is.EqualTo(1));
      Assert.That(application[0], Is.EqualTo(0));
    }

    #endregion

    #region ExecuteSingleCycle

    [Test]
    public void ExecuteSingleCycle_WithApplicationWhichIsNotLoaded_ThrowsException()
    {
      Assert.Throws<InvalidOperationException>(() => _emulator.ExecuteSingleCycle());
    }

    [Test]
    public void ExecuteSingleCycle_WithApplicationWhichIsRunning_ThrowsException()
    {
      _emulator.LoadApplication(_applicationWhichClearsScreenInLoop);
      _emulator.RunContinueApplication();

      Assert.Throws<InvalidOperationException>(() => _emulator.ExecuteSingleCycle());
    }

    [Test]
    public void ExecuteSingleCycle_WithValidApplication_Works()
    {
      _emulator.LoadApplication(_applicationWhichClearsScreenInLoop);
      _emulator.ExecuteSingleCycle();

      Assert.That(_cpu.PC, Is.EqualTo(Cpu.MemoryAddressOfFirstInstruction + 2));
      Assert.That(_emulator.IsApplicationRunning, Is.False);
    }

    #endregion

    #region Dispose

    [Test]
    public void Dispose_WithPausedApplication_Works()
    {
      _emulator.LoadApplication(_applicationWhichClearsScreenInLoop);
      _emulator.RunContinueApplication();
      _emulator.PauseApplication();
      _emulator.Dispose();

      Assert.That(_emulator.IsApplicationRunning, Is.False);
    }

    [Test]
    public void Dispose_WithStoppedApplication_Works()
    {
      _emulator.LoadApplication(_applicationWhichClearsScreenInLoop);
      _emulator.RunContinueApplication();
      _emulator.StopApplication();
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
