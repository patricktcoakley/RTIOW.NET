using System.Drawing;

namespace RTIOW.Canvas;

public sealed record PpmCanvas : IDisposable
{
    private readonly StreamWriter _streamWriter;

    public PpmCanvas(int imageWidth, int imageHeight, string path)
    {
        _streamWriter = new StreamWriter(path, false);
        _streamWriter.WriteLine($"P3\n{imageWidth} {imageHeight} 255");
    }

    public PpmCanvas(int imageWidth, int imageHeight) : this(imageWidth, imageHeight,
        Path.Combine(Environment.CurrentDirectory, "image.ppm"))
    {
    }

    public void Dispose() => _streamWriter.Dispose();

    public void WriteColor(Color color) => _streamWriter.WriteLine($"{color.R} {color.G} {color.B}");
}
