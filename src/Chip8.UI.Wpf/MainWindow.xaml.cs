using System;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Win32;
using Chip8.UI.Wpf.Interaction;
using Chip8.UI.Wpf.View;
using Chip8.UI.Wpf.ViewModel;

namespace Chip8.UI.Wpf;

public partial class MainWindow : Window
{
  private readonly DispatcherTimer _timer;
  private readonly Chip8NAudioSound _sound;
  private readonly Emulator _emulator;
  private readonly MainViewModel _mainViewModel;

  public MainWindow()
  {
    InitializeComponent();

    var cpu = new Cpu(true);
    var display = new BitmapDisplay(DisplayGrid);
    var keyboard = new Chip8Keyboard();
    _sound = new Chip8NAudioSound();
    _emulator = new Emulator(cpu, display, keyboard, _sound);

    _mainViewModel = new MainViewModel(_emulator,
      new DialogFileInteraction(), new DialogBuildInteraction());
    DataContext = _mainViewModel;

    _timer = new DispatcherTimer();
    _timer.Interval = TimeSpan.FromSeconds(1);
    _timer.Tick += (_, _) =>
    {
      _mainViewModel.FpsIpsStats = $"IPS: {_emulator.GetInstructionsPerSecond()}, FPS: {_emulator.GetFramesPerSecond()}";
    };
    _timer.Start();

    // Subscribe to keyboard events
    KeyDown += (_, e) => keyboard.OnKeyDown(e);
    KeyUp += (_, e) => keyboard.OnKeyUp(e);

    // Subscribe to system theme changes
    SystemEvents.UserPreferenceChanged += OnUserPreferenceChanged;
  }

  private void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
  {
    if (e.Category == UserPreferenceCategory.General)
    {
      _mainViewModel.NotifyThemeChanged();
    }
  }

  protected override void OnClosed(EventArgs e)
  {
    _timer.Stop();
    _sound.Dispose();
    _emulator.Dispose();

    SystemEvents.UserPreferenceChanged -= OnUserPreferenceChanged;

    base.OnClosed(e);
  }
}
