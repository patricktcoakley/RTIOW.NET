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
            byte.Clamp((byte)(r * 255), 0, 255),
            byte.Clamp((byte)(g * 255), 0, 255),
            byte.Clamp((byte)(b * 255), 0, 255)
        );
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

    public static Vector3 RandomInUnitDisk()
    {
        while (true)
        {
            var p = new Vector3(System.Random.Shared.NextSingle(-1.0f, 1.0f), System.Random.Shared.NextSingle(-1.0f, 1.0f), 0.0f);
            if (!(p.LengthSquared() >= 1.0f))
            {
                return p;
            }
        }
    }

    public static bool NearZero(this Vector3 v)
    {
        const double s = 1e-8;
        return MathF.Abs(v.X) < s && MathF.Abs(v.Y) < s && MathF.Abs(v.Z) < s;
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
        var cosTheta = MathF.Min(Vector3.Dot(-uv, n), 1.0f);
        var rOutPerp = etaiOverEtat * (uv + cosTheta * n);
        var rOutParallel = -MathF.Sqrt(MathF.Abs(1.0f - rOutPerp.LengthSquared())) * n;
        return rOutPerp + rOutParallel;
    }
}
