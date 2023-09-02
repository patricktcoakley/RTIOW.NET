using System.Numerics;
using RTIOW.Hittable;
using RTIOW.Math;

namespace RTIOW.Material;

public record Dialectric(float IndexOfRefraction) : IMaterial
{
    public bool Scatter(Ray ray, HitRecord hr, ref Vector3 attenuation, ref Ray scattered)
    {
        attenuation = new Vector3(1.0f, 1.0f, 1.0f);
        var refractionRatio = hr.FrontFace ? 1.0f / IndexOfRefraction : IndexOfRefraction;
        var unitDirection = Vector3.Normalize(ray.Direction);
        var cosTheta = System.Math.Min(Vector3.Dot(-unitDirection, hr.Normal), 1.0f);
        var sinTheta = System.Math.Sqrt(1.0f - cosTheta * cosTheta);

        var cantRefract = refractionRatio * sinTheta > 1.0f;
        var direction = cantRefract || Reflectance(cosTheta, refractionRatio) > Random.Shared.NextSingle()
            ? Vector3.Reflect(unitDirection, hr.Normal)
            : Vector3Extensions.Refract(unitDirection, hr.Normal, refractionRatio);

        scattered = new Ray(hr.Point, direction);
        return true;
    }

    private float Reflectance(float cos, float refIdx)
    {
        var r0 = (1.0f - refIdx) / (1.0f + refIdx);
        r0 *= r0;
        return r0 + (1.0f - r0) * (float)System.Math.Pow(1.0f - cos, 5.0f);
    }
}
