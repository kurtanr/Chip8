namespace Chip8;

/// <summary>
/// Abstraction of Chip-8 sound.
/// </summary>
public interface ISound
{
  /// <summary>
  /// Starts playing the sound.
  /// </summary>
  void Play();

  /// <summary>
  /// Stops playing the sound.
  /// </summary>
  void Stop();
}