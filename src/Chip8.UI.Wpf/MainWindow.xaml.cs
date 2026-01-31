using System;
using System.Windows;
using System.Windows.Threading;
using Chip8.UI.Wpf.Interaction;
using Chip8.UI.Wpf.View;
using Chip8.UI.Wpf.ViewModel;

namespace Chip8.UI.Wpf;

public partial class MainWindow : Window
{
  private readonly DispatcherTimer _timer;
  private readonly Chip8NAudioSound _sound;
  private readonly Emulator _emulator;

  public MainWindow()
  {
    InitializeComponent();

    var cpu = new Cpu(true);
    var display = new BitmapDisplay(DisplayGrid);
    var keyboard = new Chip8Keyboard();
    _sound = new Chip8NAudioSound();
    _emulator = new Emulator(cpu, display, keyboard, _sound);

    var mainViewModel = new MainViewModel(_emulator,
      new DialogFileInteraction(), new DialogBuildInteraction());
    DataContext = mainViewModel;

    _timer = new DispatcherTimer();
    _timer.Interval = TimeSpan.FromSeconds(1);
    _timer.Tick += (_, _) =>
    {
      mainViewModel.FpsIpsStats = $"IPS: {_emulator.GetInstructionsPerSecond()}, FPS: {_emulator.GetFramesPerSecond()}";
    };
    _timer.Start();

    // Subscribe to keyboard events
    KeyDown += (_, e) => keyboard.OnKeyDown(e);
    KeyUp += (_, e) => keyboard.OnKeyUp(e);
  }

  protected override void OnClosed(EventArgs e)
  {
    _timer.Stop();
    _sound.Dispose();
    _emulator.Dispose();
    base.OnClosed(e);
  }
}
