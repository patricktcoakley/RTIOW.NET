using System.Drawing;
using System.Numerics;

namespace RTIOW;

public static class Vector3Extensions
{
    public static Color ToColor(this Vector3 v, int samplesPerPixel)
    {
        var scale = 1.0f / samplesPerPixel;
        var r = MathF.Sqrt(v.X * scale);
        var g = MathF.Sqrt(v.Y * scale);
        var b = MathF.Sqrt(v.Z * scale);

        return Color.FromArgb(
            (int)(256 * Single.Clamp(r, 0.0f, 0.999f)),
            (int)(256 * Single.Clamp(g, 0.0f, 0.999f)),
            (int)(256 * Single.Clamp(b, 0.0f, 0.999f)));
    }

    public static Vector3 Random() =>
        new(
            System.Random.Shared.NextSingle(),
            System.Random.Shared.NextSingle(),
            System.Random.Shared.NextSingle()
        );

    public static Vector3 Random(float min, float max) =>
        new(
            System.Random.Shared.NextSingle(min, max),
            System.Random.Shared.NextSingle(min, max),
            System.Random.Shared.NextSingle(min, max)
        );

    public static Vector3 RandomInUnitSphere()
    {
        while (true)
        {
            var p = Random(-1f, 1f);
            if (p.LengthSquared() >= 1)
            {
                continue;
            }

            return p;
        }
    }

    public static Vector3 RandomUnitVector() => Vector3.Normalize(RandomInUnitSphere());

    public static Vector3 RandomInHemisphere(Vector3 normal)
    {
        var inUnitSphere = RandomInUnitSphere();
        return Vector3.Dot(inUnitSphere, normal) > 0 ? inUnitSphere : -inUnitSphere;
    }
}
