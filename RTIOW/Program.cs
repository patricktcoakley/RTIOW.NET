using System.Drawing;
using System.Numerics;
using RTIOW;

const float aspectRatio = 16.0f / 9.0f;
const int imageWidth = 400;
const int imageHeight = (int)(imageWidth / aspectRatio);
const float viewportHeight = 2.0f;
const float viewportWidth = aspectRatio * viewportHeight;
const float focalLength = 1.0f;

var origin = Vector3.Zero;
var horizontal = new Vector3 { X = viewportWidth };
var vertical = new Vector3 { Y = viewportHeight };
var lowerLeftCorner = origin - horizontal / 2 - vertical / 2 - new Vector3 { Z = focalLength };

var world = new HittableList(new Sphere(centerZ: -1.0f, radius: 0.5f),
    new Sphere(centerY: -100.5f, centerZ: -1.0f, radius: 100f)
);

using var ppmFile = new PpmCanvas(imageWidth, imageHeight);

for (var y = imageHeight - 1; y >= 0; --y)
{
    for (var x = 0; x < imageWidth; ++x)
    {
        var u = (float)x / (imageWidth - 1);
        var v = (float)y / (imageHeight - 1);
        var ray = new Ray(origin, lowerLeftCorner + u * horizontal + v * vertical - origin);
        ppmFile.WriteColor(HitColor(ray, world));
    }
}

Color HitColor(Ray ray, IHittable hittable)
{
    var tempHitRecord = new HitRecord();
    if (hittable.Hit(ray, 0.0f, float.MaxValue, ref tempHitRecord))
    {
        return (0.5f * (tempHitRecord.Normal + new Vector3(1.0f, 1.0f, 1.0f))).ToColor();
    }

    var unitDirection = Vector3.Normalize(ray.Direction);
    var t = 0.5f * (unitDirection.Y + 1.0f);
    return ((1.0f - t) * new Vector3(1.0f, 1.0f, 1.0f) + t * new Vector3(0.5f, 0.7f, 1.0f)).ToColor();
}
