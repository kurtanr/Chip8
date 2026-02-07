using System.Text;
using Chip8;
using Chip8.UI.Cli;

if (args.Length != 1)
{
  Console.WriteLine("Usage: Chip8.UI.Cli <path-to-chip8-binary>");
  return 1;
}

string filePath = args[0];

if (!File.Exists(filePath))
{
  Console.Error.WriteLine($"Error: File not found: {filePath}");
  return 1;
}

byte[] application = File.ReadAllBytes(filePath);

using var exitSignal = new ManualResetEventSlim();

Console.CancelKeyPress += (_, e) =>
{
  e.Cancel = true;
  exitSignal.Set();
};

Console.OutputEncoding = Encoding.UTF8;
Console.CursorVisible = false;

// For details on ANSI escape codes, see:
// - https://en.wikipedia.org/wiki/ANSI_escape_code
// - https://learn.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences
// - https://learn.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences#cursor-visibility
// \x1b = ESC character
// ESC[?25l - Hide cursor
// ESC[97;40m - Set text color to bright white (97) and background to black (40)
// ESC[2J - Clear the entire screen
Console.Write("\x1b[?25l\x1b[97;40m\x1b[2J");

try
{
  var cpu = new Cpu(true);
  var display = new ConsoleDisplay();
  using var keyboard = new ConsoleKeyboard();
  using var sound = new ConsoleSound();
  using var emulator = new Emulator(cpu, display, keyboard, sound);

  emulator.LoadApplication(application);
  emulator.RunContinueApplication();

  while (!exitSignal.IsSet && !keyboard.IsEscapePressed)
  {
    Thread.Sleep(50);
  }
}
finally
{
  // ESC[?25h - Show cursor
  // ESC[0m - Reset all attributes (colors, etc.) to defaults
  Console.Write("\x1b[?25h\x1b[0m"); // Show cursor, reset attributes
  Console.CursorVisible = true;
}

return 0;
