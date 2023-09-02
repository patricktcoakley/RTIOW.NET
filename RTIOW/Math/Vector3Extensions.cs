using System.Drawing;
using System.Numerics;

namespace RTIOW.Math;

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

    public static bool NearZero(this Vector3 v)
    {
        const double s = 1e-8;
        return System.Math.Abs(v.X) < s && System.Math.Abs(v.Y) < s && System.Math.Abs(v.Z) < s;
    }

    public static Vector3 Reflect(Vector3 lhs, Vector3 rhs) => lhs - 2.0f * Vector3.Dot(lhs, rhs) * rhs;

    public static Vector3 RandomUnitVector() => Vector3.Normalize(RandomInUnitSphere());

    public static Vector3 RandomInHemisphere(Vector3 normal)
    {
        var inUnitSphere = RandomInUnitSphere();
        return Vector3.Dot(inUnitSphere, normal) > 0 ? inUnitSphere : -inUnitSphere;
    }

    public static Vector3 Refract(Vector3 uv, Vector3 n, float etaiOverEtat)
    {
        var cosTheta = System.Math.Min(Vector3.Dot(-uv, n), 1.0f);
        var rOutPerp = etaiOverEtat * (uv + cosTheta * n);
        var rOutParallel = (float)-System.Math.Sqrt(
            System.Math.Abs(1.0f - rOutPerp.LengthSquared())
        ) * n;

        return rOutPerp + rOutParallel;
    }
}
