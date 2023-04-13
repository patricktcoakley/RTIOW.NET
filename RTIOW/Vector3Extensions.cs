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

    public static Vector3 Random(Random random) =>
        new(
            random.NextSingle(),
            random.NextSingle(),
            random.NextSingle()
        );

    public static Vector3 Random(Random random, float min, float max) =>
        new(
            random.NextSingle(min, max),
            random.NextSingle(min, max),
            random.NextSingle(min, max)
        );

    public static Vector3 RandomInUnitSphere(Random random)
    {
        while (true)
        {
            var p = Random(random, -1f, 1f);
            if (p.LengthSquared() >= 1)
            {
                continue;
            }

            return p;
        }
    }

    public static Vector3 RandomUnitVector(Random random) => Vector3.Normalize(RandomInUnitSphere(random));

    public static Vector3 RandomInHemisphere(Vector3 normal, Random random)
    {
        var inUnitSphere = RandomInUnitSphere(random);
        return Vector3.Dot(inUnitSphere, normal) > 0 ? inUnitSphere : -inUnitSphere;
    }
}
