const int imageWidth = 256;
const int imageHeight = 256;

using var ppmFile = new StreamWriter(Path.Combine(Environment.CurrentDirectory, "image.ppm"));

var header = $"""
P3
{imageWidth} {imageHeight} 255
""";

ppmFile.WriteLine(header);

for (var j = imageHeight - 1; j >= 0; --j)
{
    for (var i = 0; i < imageWidth; ++i)
    {
        var r = (float)i / (imageWidth - 1);
        var g = (float)j / (imageHeight - 1);
        const float b = 0.25f;

        var ir = (int)(255.999f * r);
        var ig = (int)(255.999f * g);
        const int ib = (int)(255.999f * b);

        ppmFile.WriteLine($"{ir} {ig} {ib}");
    }
}