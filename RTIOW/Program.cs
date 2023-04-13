using System.Numerics;
using RTIOW;
using static RTIOW.Vector3Extensions;
using Random = System.Random;

const float aspectRatio = 16.0f / 9.0f;
const int imageWidth = 400;
const int imageHeight = (int)(imageWidth / aspectRatio);
const int samplesPerPixel = 100;
const int maxDepth = 50;

var world = new HittableList(
    new Sphere(centerZ: -1.0f, radius: 0.5f),
    new Sphere(centerY: -100.5f, centerZ: -1.0f, radius: 100f)
);

var camera = new Camera();

using var ppmFile = new PpmCanvas(imageWidth, imageHeight);

for (var y = imageHeight - 1; y >= 0; --y)
{
    for (var x = 0; x < imageWidth; ++x)
    {
        var pixel = new Vector3();
        for (var s = 0; s < samplesPerPixel; ++s)
        {
            var u = (x + Random.Shared.NextSingle()) / (imageWidth - 1);
            var v = (y + Random.Shared.NextSingle()) / (imageHeight - 1);
            var ray = camera.Ray(u, v);
            pixel += HitColor(ray, world, maxDepth);
        }

        ppmFile.WriteColor(pixel.ToColor(samplesPerPixel));
    }
}

Vector3 HitColor(Ray ray, IHittable hittable, int depth)
{
    if (depth <= 0)
    {
        return Vector3.Zero;
    }

    var hr = new HitRecord();
    if (hittable.Hit(ray, 0.001f, float.PositiveInfinity, ref hr))
    {
        var target = hr.Point + RandomInHemisphere(hr.Normal);
        return 0.5f * HitColor(new Ray(hr.Point, target - hr.Point), world, depth - 1);
    }

    var unitDirection = Vector3.Normalize(ray.Direction);
    var t = 0.5f * (unitDirection.Y + 1.0f);
    return (1.0f - t) * Vector3.One + t * new Vector3(0.5f, 0.7f, 1.0f);
}
