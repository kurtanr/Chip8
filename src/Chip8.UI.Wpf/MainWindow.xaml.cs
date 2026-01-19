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

  public MainWindow()
  {
    InitializeComponent();

    var cpu = new Cpu(true);
    var display = new BitmapDisplay(DisplayGrid);
    var keyboard = new Chip8Keyboard();
    var sound = new Chip8NAudioSound();
    var emulator = new Emulator(cpu, display, keyboard, sound);

    var mainViewModel = new MainViewModel(emulator,
      new DialogFileInteraction(), new DialogBuildInteraction());
    DataContext = mainViewModel;

    _timer = new DispatcherTimer();
    _timer.Interval = TimeSpan.FromSeconds(1);
    _timer.Tick += (s, e) =>
    {
      mainViewModel.FpsIpsStats = $"IPS: {emulator.GetInstructionsPerSecond()}, FPS: {emulator.GetFramesPerSecond()}";
    };
    _timer.Start();

    // Subscribe to keyboard events
    KeyDown += (s, e) => keyboard.OnKeyDown(e);
    KeyUp += (s, e) => keyboard.OnKeyUp(e);
  }
}
