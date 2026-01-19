using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;

namespace Chip8.UI.Wpf;

/// <summary>
/// WPF implementation of Chip-8 sound using Windows Beep API.
/// </summary>
internal sealed class Chip8NAudioSound : ISound, IDisposable
{
  private readonly IWavePlayer _wavePlayer;
  private readonly SignalGenerator _signalGenerator;

  public Chip8NAudioSound()
  {
    _wavePlayer = new WaveOutEvent();
    _signalGenerator = new SignalGenerator(44100, 1)
    {
      Frequency = 440, // A4 note
      Type = SignalGeneratorType.Sin
    };
    _wavePlayer.Init(_signalGenerator);
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
    _wavePlayer?.Dispose();
  }
}
