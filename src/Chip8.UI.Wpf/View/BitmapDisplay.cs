using Chip8;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Chip8.UI.Wpf.View;

public sealed class BitmapDisplay : IDisplay
{
  private const int ScreenWidth = 64;
  private const int ScreenHeight = 32;
  private const int PixelSize = 10;

  private const UInt32 LightPixelColor = 0xFFF0F8FF;
  private const UInt32 DarkPixelColor = 0xFFA9A9A9;

  // For usage see: https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.imaging.writeablebitmap
  private readonly WriteableBitmap _bitmap;

  // Logical frame-buffer (Chip-8 state screen)
  private readonly bool[,] _pixelBuffer = new bool[ScreenWidth, ScreenHeight];
  private readonly Lock _bufferLock = new();

  // Indicates that the frame-buffer has changed since last render
  private volatile bool _dirty;

  // Prevents multiple render requests from queuing up
  private volatile bool _renderPending;

  public BitmapDisplay(Grid grid)
  {
    var image = new Image
    {
      Stretch = Stretch.None,
      HorizontalAlignment = HorizontalAlignment.Center,
      VerticalAlignment = VerticalAlignment.Center
    };

    RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.NearestNeighbor);
    RenderOptions.SetEdgeMode(image, EdgeMode.Aliased);

    _bitmap = new WriteableBitmap(
      ScreenWidth * PixelSize,
      ScreenHeight * PixelSize,
      96,
      96,
      PixelFormats.Bgr32,
      null);

    image.Source = _bitmap;
    grid.Children.Add(image);

    Clear();

    _dirty = true;
    RenderIfDirty();
  }

  #region IDisplay implementation (logical operations only)

  public void Clear()
  {
    lock (_bufferLock)
    {
      for (int x = 0; x < ScreenWidth; x++)
      {
        for (int y = 0; y < ScreenHeight; y++)
        {
          _pixelBuffer[x, y] = false;
        }
      }
    }

    _dirty = true;
  }

  public void ClearPixel(byte x, byte y)
  {
    lock (_bufferLock)
    {
      _pixelBuffer[x, y] = false;
    }

    _dirty = true;
  }

  public bool GetPixel(byte x, byte y)
  {
    lock (_bufferLock)
    {
      return _pixelBuffer[x, y];
    }
  }

  public void SetPixel(byte x, byte y)
  {
    lock (_bufferLock)
    {
      _pixelBuffer[x, y] = true;
    }

    _dirty = true;
  }

  #endregion

  #region IDisplay.RenderIfDirty implementation (called explicitly by emulator at 60 Hz)

  public void RenderIfDirty()
  {
    // Render only if dirty
    if (!_dirty)
    {
      return;
    }

    var dispatcher = _bitmap.Dispatcher;

    // Drop frame if dispatcher is shutting down
    if (dispatcher.HasShutdownStarted || dispatcher.HasShutdownFinished)
    {
      return;
    }

    // Ensure rendering happens on UI thread
    if (!dispatcher.CheckAccess())
    {
      // If a render is already pending, skip this request to avoid queue buildup
      if (_renderPending)
      {
        return;
      }

      _renderPending = true;
      dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(RenderIfDirty));
      return;
    }

    _renderPending = false;

    // Check dirty again - it may have been cleared by another render
    if (!_dirty)
    {
      return;
    }

    // Snapshot frame-buffer ONCE
    bool[,] snapshot;
    lock (_bufferLock)
    {
      snapshot = (bool[,])_pixelBuffer.Clone();
      _dirty = false;
    }

    _bitmap.Lock();

    // Stride is the number of bytes per row in the bitmap
    // (PixelWidth * 4 bytes per pixel for Bgr32 format)
    int stride = _bitmap.PixelWidth * 4;

    // Buffer to hold the entire bitmap data as a flat byte array
    byte[] buffer = new byte[_bitmap.PixelHeight * stride];

    for (int y = 0; y < ScreenHeight; y++)
    {
      for (int x = 0; x < ScreenWidth; x++)
      {
        int color = snapshot[x, y]
          ? unchecked((int)LightPixelColor)
          : unchecked((int)DarkPixelColor);

        // Base offsets for the scaled pixel block in the bitmap

        // Starting column offset for the x position (PixelSize pixels * 4 bytes each)
        int baseX = x * PixelSize * 4;

        // Starting row offset for the y position (PixelSize pixels high)
        int baseY = y * PixelSize;

        for (int px = 0; px < PixelSize; px++)
        {
          for (int py = 0; py < PixelSize; py++)
          {
            // Calculate the index in the buffer for the current pixel
            // (row * stride + column offset + pixel offset)
            int pixelIndex = (baseY + py) * stride + baseX + px * 4;
            buffer[pixelIndex] = (byte)(color & 0xFF);              // B
            buffer[pixelIndex + 1] = (byte)((color >> 8) & 0xFF);   // G
            buffer[pixelIndex + 2] = (byte)((color >> 16) & 0xFF);  // R
            buffer[pixelIndex + 3] = (byte)((color >> 24) & 0xFF);  // A
          }
        }
      }
    }

    _bitmap.WritePixels(
      new Int32Rect(0, 0, _bitmap.PixelWidth, _bitmap.PixelHeight),
      buffer,
      stride,
      0);

    _bitmap.Unlock();
  }

  #endregion
}