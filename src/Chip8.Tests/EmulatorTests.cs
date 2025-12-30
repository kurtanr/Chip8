using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Threading.Tasks;

namespace Chip8.Tests;

public class EmulatorTests
{
  private Cpu _cpu;
  private IDisplay _display;
  private IKeyboard _keyboard;
  private ISound _sound;
  private Emulator _emulator;

  private readonly byte[] _applicationWhichClearsScreenInLoop = new byte[] { 0x00, 0xE0, 0x12, 0x00 };

  [SetUp]
  public void Setup()
  {
    _cpu = new Cpu();
    _display = new Mock<IDisplay>(MockBehavior.Loose).Object;
    _keyboard = new Mock<IKeyboard>(MockBehavior.Strict).Object;
    _sound = new Mock<ISound>(MockBehavior.Loose).Object;
    _emulator = new Emulator(_cpu, _display, _keyboard, _sound);
  }

  [TearDown]
  public void TearDown()
  {
    _emulator.Dispose();
  }

  [Test]
  public void Constructor_ArgumentValidation_Works()
  {
    Assert.DoesNotThrow(() => new Emulator(_cpu, _display, _keyboard, _sound));
    Assert.Throws<ArgumentNullException>(() => new Emulator(null, _display, _keyboard, _sound));
    Assert.Throws<ArgumentNullException>(() => new Emulator(_cpu, null, _keyboard, _sound));
    Assert.Throws<ArgumentNullException>(() => new Emulator(_cpu, _display, null, _sound));
    Assert.Throws<ArgumentNullException>(() => new Emulator(_cpu, _display, _keyboard, null));
  }

  #region Reset

  [Test]
  public void Reset_WithLoadedApplication_Works()
  {
    var displayMock = new Mock<IDisplay>(MockBehavior.Strict);
    displayMock.Setup(x => x.Clear());
    displayMock.Setup(x => x.RenderIfDirty());

    var soundMock = new Mock<ISound>(MockBehavior.Strict);
    soundMock.Setup(x => x.Stop());

    var emulator = new Emulator(_cpu, displayMock.Object, _keyboard, soundMock.Object);
    emulator.LoadApplication(_applicationWhichClearsScreenInLoop);
    displayMock.Verify(x => x.Clear(), Times.Once);

    emulator.ExecuteSingleCycle();
    displayMock.Verify(x => x.Clear(), Times.Exactly(2));

    emulator.Reset();
    displayMock.Verify(x => x.Clear(), Times.Exactly(3));
    soundMock.Verify(x => x.Stop(), Times.Once);

    Assert.That(_cpu.PC, Is.EqualTo(Cpu.MemoryAddressOfFirstInstruction));
    Assert.That(emulator.IsApplicationLoaded, Is.False);
    Assert.That(emulator.IsApplicationRunning, Is.False);
    Assert.That(emulator.IsApplicationPaused, Is.False);
  }

  #endregion

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

  [Test]
  public async Task RunContinueApplication_DecrementsDelayAndSoundTimer()
  {
    _emulator.LoadApplication(_applicationWhichClearsScreenInLoop);
    _cpu.DT = 10;
    _cpu.ST = 10;

    _emulator.RunContinueApplication();

    await Task.Delay(200).ConfigureAwait(false);

    _emulator.PauseApplication();

    Assert.That(_cpu.DT, Is.LessThan(10));
    Assert.That(_cpu.ST, Is.LessThan(10));

    _emulator.StopApplication();
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
  public async Task StopApplication_WithPausedApplication_Works(int waitInMs)
  {
    _emulator.LoadApplication(_applicationWhichClearsScreenInLoop);
    _emulator.RunContinueApplication();

    await Task.Delay(waitInMs).ConfigureAwait(false);
    Assert.That(_emulator.IsApplicationRunning, Is.True);
    Assert.That(_emulator.IsApplicationPaused, Is.False);

    _emulator.PauseApplication();

    await Task.Delay(waitInMs).ConfigureAwait(false);
    Assert.That(_emulator.IsApplicationRunning, Is.False);
    Assert.That(_emulator.IsApplicationPaused, Is.True);

    _emulator.StopApplication();

    Assert.That(_emulator.IsApplicationRunning, Is.False);
    Assert.That(_emulator.IsApplicationPaused, Is.False);
  }

  [Test]
  public void StopApplication_WithApplicationWhoseSingleCycleWasExecuted_Works()
  {
    _emulator.LoadApplication(_applicationWhichClearsScreenInLoop);
    _emulator.ExecuteSingleCycle();

    Assert.That(_emulator.IsApplicationRunning, Is.False);
    Assert.That(_emulator.IsApplicationPaused, Is.True);

    _emulator.StopApplication();

    Assert.That(_emulator.IsApplicationRunning, Is.False);
    Assert.That(_emulator.IsApplicationPaused, Is.False);
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
    Assert.That(_emulator.IsApplicationPaused, Is.True);
  }

  #endregion

  #region GetValueOfCpuRegisters

  [Test]
  public void GetValueOfCpuRegisters_ReturnsCorrectPcValue()
  {
    _emulator.LoadApplication(_applicationWhichClearsScreenInLoop);

    var valueOfRegisters = _emulator.GetValueOfCpuRegisters();

    Assert.That(valueOfRegisters, Contains.Substring(
      $"{nameof(Cpu.PC)} = 0x{Cpu.MemoryAddressOfFirstInstruction:X} ({Cpu.MemoryAddressOfFirstInstruction})"));

    Assert.That(valueOfRegisters, Contains.Substring("V0 = 0x0   (0)"));
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

  #region GetInstructionsPerSecond and GetFramesPerSecond

  [Test]
  public async Task GetIpsFps_WithRunningApplication_ReturnsExpectedValues()
  {
    _emulator.LoadApplication(_applicationWhichClearsScreenInLoop);
    _emulator.RunContinueApplication();

    await Task.Delay(1100).ConfigureAwait(false);

    var ips = _emulator.GetInstructionsPerSecond();
    var fps = _emulator.GetFramesPerSecond();

    _emulator.StopApplication();

    Assert.That(ips, Is.GreaterThan(0));
    Assert.That(ips, Is.LessThanOrEqualTo(600));

    Assert.That(fps, Is.GreaterThan(0));
    Assert.That(fps, Is.LessThanOrEqualTo(70));
  }

  [Test]
  public void GetIpsFps_WithoutRunningApplication_ReturnsZero()
  {
    var ips = _emulator.GetInstructionsPerSecond();
    var fps = _emulator.GetFramesPerSecond();

    Assert.That(ips, Is.EqualTo(0));
    Assert.That(fps, Is.EqualTo(0));
  }

  #endregion
}
