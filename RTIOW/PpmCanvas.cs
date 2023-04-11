using System.Drawing;

namespace RTIOW;

public sealed class PpmCanvas : ICanvas, IDisposable
{
    private readonly StreamWriter _streamWriter;

    public PpmCanvas(int imageWidth, int imageHeight, string path)
    {
        _streamWriter = new StreamWriter(path) { AutoFlush = true };

        _streamWriter.WriteLine($"P3\n{imageWidth} {imageHeight} 255");
    }

    public PpmCanvas(int imageWidth, int imageHeight) : this(imageWidth, imageHeight,
        Path.Combine(Environment.CurrentDirectory, "image.ppm"))
    {
    }

    public void WriteColor(Color color) => _streamWriter.WriteLine($"{color.R} {color.G} {color.B}");

    public void Dispose() => _streamWriter.Dispose();
}
