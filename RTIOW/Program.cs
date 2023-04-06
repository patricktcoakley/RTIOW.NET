using System.Numerics;
using RTIOW;

const int imageWidth = 256;
const int imageHeight = 256;

using var ppmFile = new Ppm(imageWidth, imageHeight);

for (var j = imageHeight - 1; j >= 0; --j)
{
    for (var i = 0; i < imageWidth; ++i)
    {
        var color = new Vector3((float)i / (imageWidth - 1), (float)j / (imageHeight - 1), 0.25f);
        ppmFile.WriteColor(color.ToColor());
    }
}