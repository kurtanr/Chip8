using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace Chip8.UI.Cli;

/// <summary>
/// Console implementation of <see cref="ISound"/> using NAudio.
/// </summary>
internal sealed class ConsoleSound : ISound, IDisposable
{
  private readonly IWavePlayer _wavePlayer;
  private bool _isDisposed;

  public ConsoleSound()
  {
    _wavePlayer = new WaveOutEvent();
    var signalGenerator = new SignalGenerator(44100, 1)
    {
      Frequency = 440, // A4 note
      Type = SignalGeneratorType.Sin
    };
    _wavePlayer.Init(signalGenerator);
  }

  public void Play()
  {
    if (_wavePlayer.PlaybackState != PlaybackState.Playing)
    {
      _wavePlayer.Play();
    }
  }

  public void Stop()
  {
    if (_wavePlayer.PlaybackState == PlaybackState.Playing)
    {
      _wavePlayer.Stop();
    }
  }

  public void Dispose()
  {
    if (_isDisposed)
    {
      return;
    }
    _wavePlayer.Dispose();
    _isDisposed = true;
  }
}
