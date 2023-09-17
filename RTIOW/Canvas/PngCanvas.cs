using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace RTIOW.Canvas;

public sealed record PngCanvas : IDisposable
{
    private readonly Image<Rgba32> _buffer;

    public PngCanvas(int width, int height)
    {
        _buffer = new Image<Rgba32>(width, height);
    }

    public void Dispose() =>
        _buffer.Dispose();

    public void Draw(int x, int y, Color color) =>
        _buffer[x, _buffer.Height - y - 1] = new Rgba32(color.R, color.G, color.B);

    public void View() =>
        _buffer.Save("out.png");
}
