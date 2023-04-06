using System.Numerics;
using RTIOW;

const float aspectRatio = 16.0f / 9.0f;
const int imageWidth = 400;
const int imageHeight = (int)(imageWidth / aspectRatio);
const float viewportHeight = 2.0f;
const float viewportWidth = aspectRatio * viewportHeight;
const float focalLength = 1.0f;

var origin = new Vector3(viewportWidth);
var horizontal = new Vector3 { X = viewportWidth };
var vertical = new Vector3 { Y = viewportHeight };
var lowerLeftCorner = origin - horizontal / 2 - vertical / 2 - new Vector3 { Z = focalLength };

using var ppmFile = new Ppm(imageWidth, imageHeight);

for (var j = imageHeight - 1; j >= 0; --j)
{
    for (var i = 0; i < imageWidth; ++i)
    {
        var u = (float)i / (imageWidth - 1);
        var v = (float)j / (imageHeight - 1);
        var ray = new Ray(origin, lowerLeftCorner + u * horizontal + v * vertical - origin);
        ppmFile.WriteColor(ray.Color);
    }
}
